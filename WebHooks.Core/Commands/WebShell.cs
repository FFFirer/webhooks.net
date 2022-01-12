using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Core.Commands
{
    public class WebShell
    {
        private bool shouldExit { get; set; }
        private int exitCode { get; set; }

        private WebShellHost host;
        private Runspace runspace;
        private PowerShell? powershell;

        private readonly ILogger _logger;

        private object locker = new object();

        public bool ShouldExit
        {
            get { return shouldExit; }
            set { shouldExit = value; }
        }

        public WebShell(ILogger logger, IWebShellOutput? output)
        {
            this._logger = logger;
            this.host = new WebShellHost(this, output);

            var initialState = InitialSessionState.CreateDefault();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                initialState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.RemoteSigned;
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("zh-cn");

            PreLoadCommands(initialState);

            this.runspace = RunspaceFactory.CreateRunspace(this.host, initialState);
            this.runspace.Open();

            lock (locker)
            {
                this.powershell = PowerShell.Create();
            }
        }

        public int ExitCode
        {
            get { return exitCode; }
            set { exitCode = value; }
        }

        private void ExecuteHelper(string cmd, object? input)
        {
            if (string.IsNullOrEmpty(cmd))
            {
                return;
            }

            lock (this.locker)
            {
                this.powershell = PowerShell.Create();
            }

            this.powershell.Runspace = this.runspace;

            try
            {
                this.powershell.AddScript(cmd).AddParameter("ErrorAction", "Stop");

                this.powershell.AddCommand("out-default");

                foreach (var command in this.powershell.Commands.Commands)
                {
                    command.MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);
                }

                this.powershell.Invoke();
            }
            finally
            {
                lock (locker)
                {
                    this.powershell?.Dispose();
                    this.powershell = null;
                }
            }
        }

        private void ExecuteHelper(Action<PowerShell> addCmds)
        {
            lock (this.locker)
            {
                this.powershell = PowerShell.Create();
            }

            this.powershell.Runspace = this.runspace;

            try
            {
                addCmds(this.powershell);

                this.powershell.AddCommand("out-string");

                this.powershell.Commands.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);

                this.powershell.Invoke();
            }
            finally 
            {
                lock (locker)
                {
                    this.powershell?.Dispose();
                    this.powershell = null;
                }
            }
        }
        private async Task AsyncExecuteHelper(Action<PowerShell> addCmds)
        {
            lock (this.locker)
            {
                this.powershell = PowerShell.Create();
            }

            this.powershell.Runspace = this.runspace;

            try
            {
                _logger.LogDebug("开始执行命令");

                addCmds(this.powershell);

                this.powershell.AddCommand("out-string");

                foreach (var cmd in this.powershell.Commands.Commands)
                {
                    cmd.MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);
                }

                var results = await this.powershell.InvokeAsync();

                LogExecuteResults(results);
            }
            finally
            {
                lock (locker)
                {
                    this.powershell?.Dispose();
                    this.powershell = null;
                }
            }
        }
        private void ReportException(Exception e)
        {
            if (e != null)
            {
                object error;
                IContainsErrorRecord? icer = e as IContainsErrorRecord;

                if (icer != null)
                {
                    error = icer.ErrorRecord;
                }
                else
                {
                    error = (object)new ErrorRecord(e, "WebShell.ReportException", ErrorCategory.NotSpecified, null);
                }

                lock (this.locker)
                {
                    this.powershell = PowerShell.Create();
                }

                this.powershell.Runspace = this.runspace;

                try
                {
                    this.powershell.AddScript("$input").AddCommand("out-string");

                    Collection<PSObject> result;
                    PSDataCollection<object> inputCollection = new PSDataCollection<object>();
                    inputCollection.Add(error);
                    inputCollection.Complete();
                    result = this.powershell.Invoke(inputCollection);

                    if(result.Count > 0)
                    {
                        string str = result[0].BaseObject as string ?? string.Empty;
                        if (!string.IsNullOrEmpty(str))
                        {
                            this.host.UI.WriteErrorLine(str.Substring(0, str.Length - 1));

                        }
                    }
                }
                finally
                {
                    lock (this.locker)
                    {
                        this.powershell.Dispose();
                        this.powershell = null;
                    }
                }
            }
        }

        public void Execute(string cmd)
        {
            try
            {
                this.ExecuteHelper(cmd, null);
            }
            catch (RuntimeException rte)
            {

                this.ReportException(rte);
            }
        }

        public void Execute(Action<PowerShell> addCmd)
        {
            try
            {
                _logger.LogInformation("开始执行");
                this.ExecuteHelper(addCmd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行失败");
                this.ReportException(ex);
            }
            finally
            {
                _logger.LogInformation("执行结束");
                if (this.shouldExit)
                {
                    _logger.LogDebug($"退出执行({this.exitCode})");
                    throw new Exception($"退出执行({this.exitCode})");
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Executes(params string[] cmds)
        {
            if(cmds.Length <= 0)
            {
                return;
            }

            foreach (var cmd in cmds)
            {
                this.host.UI.WriteLine($"=========================");
                this.host.UI.WriteLine($"[EXECUTE CMD]>{cmd}");
                this.host.UI.WriteLine($"-------------------------");

                if (this.shouldExit)
                {
                    this.host.UI.WriteLine($"exit {exitCode}");
                    break;
                }

                this.Execute(cmd);
            }
        }
    
        public void Executes(IEnumerable<Action<PowerShell>> addCommands)
        {
            foreach (var addCmd in addCommands)
            {
                try
                {
                    this.ExecuteHelper(addCmd);
                }
                catch (Exception ex)
                {
                    this.ReportException(ex);
                }
                finally
                {
                    if (this.shouldExit)
                    {
                        throw new Exception($"退出执行({this.exitCode})");
                    }
                }
            }
        }

        private void LogExecuteResults(PSDataCollection<PSObject> results)
        {
            
        }

        private void PreLoadCommands(InitialSessionState? initialSessionState)
        {
            if (initialSessionState == null)
            {
                throw new ArgumentNullException(nameof(initialSessionState));
            }

            var contentRootDir = AppDomain.CurrentDomain.BaseDirectory;

            var scriptsDir = Path.Combine(contentRootDir, "scripts");

            if (!Directory.Exists(scriptsDir))
            {
                throw new DirectoryNotFoundException(scriptsDir);
            }

            var initScriptPath = Path.Combine(scriptsDir, "Git-Help.ps1");

            if (!File.Exists(initScriptPath))
            {
                throw new FileNotFoundException(initScriptPath);
            }

            try
            {
                var scriptsDirInfo = new DirectoryInfo(scriptsDir);

                var childDirs = scriptsDirInfo.GetDirectories();

                if (childDirs.Any())
                {
                    foreach (var childDir in childDirs)
                    {
                        _logger.LogInformation($"Import Module: {childDir.FullName}");
                        initialSessionState.ImportPSModulesFromPath(childDir.FullName);
                    }
                }

                // powershell 模块按文件夹导入
                //initialSessionState.ImportPSModulesFromPath(initScriptPath); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"导入模块失败！{initScriptPath}");
                throw;
            }
        }

    }
}
