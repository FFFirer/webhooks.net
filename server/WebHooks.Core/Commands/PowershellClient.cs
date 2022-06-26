using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using System.Text;
using WebHooks.Models;

namespace WebHooks.Core.Commands
{
    /// <summary>
    /// 构建一个Powershell命令行的客户端实例
    /// </summary>
    public class PowershellClient : IShellClient, IDisposable
    {
        private object runLock = new object();
        private readonly ILogger _logger;
        private PowershellClient(ILogger logger)
        {
            _logger = logger;

            _program = new WebHooksProgram();

            _host = new WebHooksHost(_program);

            var initialState = InitialSessionState.CreateDefault();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                initialState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.RemoteSigned;
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("zh-cn");

            PreLoadCommands(initialState);

            _runspace = RunspaceFactory.CreateRunspace(_host, initialState);

            //_powershell = PowerShell.Create();

            //_powershell.Runspace = _runspace;
        }

        public void Dispose()
        {
            lock (runLock)
            {
                if(_powershell != null)
                {
                    _powershell?.Dispose();
                    _powershell = null;
                }
            }
            _runspace.Dispose();
        }

        public static PowershellClient Create(ILogger logger)
        {
            return new PowershellClient(logger);
        }

        private Runspace _runspace { get; set; }

        private PowerShell? _powershell { get; set; }

        private WebHooksHost _host { get; set; }

        private WebHooksProgram _program { get; set; }

        public async Task<Tuple<int, PSDataCollection<PSObject>>> InvokeAsync(Action<PowerShell> addCommands)
        {
            var results = new PSDataCollection<PSObject>();

            lock (runLock)
            {
                this._powershell = PowerShell.Create();
            }

            this._powershell.Runspace = this._runspace;

            try
            {
                if (_powershell == null)
                {
                    throw new NullReferenceException($"没有初始化：{nameof(_powershell)}");
                }

                if(_powershell.Runspace.RunspaceStateInfo.State != RunspaceState.Opened)
                {
                    _powershell.Runspace.Open();
                }

                addCommands(_powershell);

                _logger.LogDebug($"Commands Count: {_powershell.Commands.Commands.Count}");
                _logger.LogDebug($"Current Commands: {string.Join("\n", _powershell.Commands.Commands.Select(a => a.CommandText))}");

                foreach (var cmd in _powershell.Commands.Commands)
                {
                    cmd.MergeMyResults(PipelineResultTypes.All, PipelineResultTypes.Output);
                }

                results = await _powershell.InvokeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "命令行意外停止！");
                throw;
            }
            finally
            {
                _logger.LogDebug($"命令执行结果\n{FormatResults(results)}");

                _logger.LogDebug($"命令执行历史\n{_powershell?.HistoryString}");

                if (_powershell != null)
                {
                    _powershell?.Dispose();
                    _powershell = null;
                }
            }

            return new(_program.ExitCode, results);
        }

        /// <summary>
        /// 预加载
        /// </summary>
        /// <param name="powershell"></param>
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

        /// <summary>
        /// 格式化结果
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private string FormatResults(PSDataCollection<PSObject>? results)
        {
            if (results == null)
            {
                return string.Empty;
            }

            var resultBuilder = new StringBuilder();

            foreach (var result in results)
            {
                resultBuilder.AppendLine(result.ToString());
            }

            return resultBuilder.ToString();
        }
    }
}
