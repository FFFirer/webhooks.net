using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Models;
using WebHooks.Models.Common;
using WebHooks.Models.Gitee.Options;

namespace WebHooks.Core.Commands
{
    /// <summary>
    /// 构建一个Powershell命令行的客户端实例
    /// </summary>
    public class PowershellClient : IShellClient, IDisposable
    {
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

            _powershell = PowerShell.Create();

            _powershell.Runspace = _runspace;
        }

        public void Dispose()
        {
            _powershell?.Dispose();
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

            var input = new PSDataCollection<PSObject>();
            var output = new PSDataCollection<PSObject>();

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

                results = await _powershell.InvokeAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("命令行意外停止！", ex);
                throw;
            }
            finally
            {
                _logger.LogDebug($"命令执行输入\n{string.Join("\n", input)}");
                _logger.LogDebug($"命令执行结果\n{string.Join("\n", results)}");
                _logger.LogDebug($"命令执行输出\n{string.Join("\n", output)}");
                _logger.LogDebug($"命令执行历史\n{_powershell?.HistoryString}");

                if (_powershell != null)
                {
                    _powershell.Commands.Clear();
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

                //initialSessionState.ImportPSModulesFromPath(initScriptPath);
            }
            catch (Exception ex)
            {
                _logger.LogError($"导入模块失败！{initScriptPath}", ex);
                throw;
            }
        }
    }
}
