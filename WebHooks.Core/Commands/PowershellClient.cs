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

            _runspace = RunspaceFactory.CreateRunspace(_host, initialState);

            _powershell = PowerShell.Create();

            _powershell.Runspace = _runspace;
        }

        public void Dispose()
        {
            
        }

        public static PowershellClient Create(ILogger logger)
        {
            return new PowershellClient(logger);
        }

        private Runspace _runspace { get; set; }

        private PowerShell _powershell { get; set; }

        private WebHooksHost _host { get; set; }

        private WebHooksProgram _program { get; set; }

        public async Task<Tuple<int, PSDataCollection<PSObject>>> InvokeAsync(PSCommand commands)
        {
            _powershell.Commands = commands;

            var results = await _powershell.InvokeAsync();

            _powershell.Commands.Clear();
            return new(_program.ExitCode, results);
        }

        /// <summary>
        /// 预加载
        /// </summary>
        /// <param name="powershell"></param>
        private void PreLoadCommands(InitialSessionState? initialSessionState)
        {
            if(initialSessionState == null)
            {
                return;
            }

        }
    }
}
