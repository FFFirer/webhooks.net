using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using PS = System.Management.Automation.PowerShell;

namespace WebHooks.Scripts.PowerShell
{
    public class WebPowerShell : IWebShell
    {
        private bool _shouldExit { get; set; } = false;
        public bool ShoultExit
        {
            get { return _shouldExit; }
            set { _shouldExit = value; }
        }

        private int _exitCode { get; set; } = 0;
        public int ExitCode
        {
            get { return _exitCode; }
            set { _exitCode = value; }
        }

        private readonly WebShellStartupInfo _startupInfo;
        private readonly WebPowerShellHost _host;
        private readonly Runspace _runspace;
        private readonly PS _powershell;

        public string Name
        {
            get
            {
                return _startupInfo.Name;
            }
        }

        public Version Version
        {
            get
            {
                return _startupInfo.Version;
            }
        }


        public WebPowerShell(WebShellStartupInfo startupInfo)
        {
            _startupInfo = startupInfo;

            var host = new WebPowerShellHost(this);
            _host = host;

            var runspace = InitRunspace(host);
            _runspace = runspace;

            _powershell = PS.Create(runspace);
        }

        private Runspace InitRunspace(PSHost psHost)
        {
            var initialSessionState = InitialSessionState.CreateDefault();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                initialSessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Bypass;
            }

            var runspace = RunspaceFactory.CreateRunspace(psHost, initialSessionState);
            runspace.Open();

            return runspace;
        }

        public async Task<(int, PSDataCollection<PSObject>)> ExecuteAsync(string script)
        {
            var handledScript = $@"
try{{
{script}
}}
finally{{
Write-Output $LASTEXITCODE              
}}
";

            if (_powershell.Runspace.RunspaceStateInfo.State != RunspaceState.Opened)
            {
                _powershell.Runspace.Open();
            }

            _powershell.AddStatement()
                .AddCommand("Set-Location")
                .AddParameter("Path", _startupInfo.WorkingDirectory);

            await _powershell.InvokeAsync();

            _powershell.Commands.Clear();

            _powershell.AddScript(handledScript);

            _powershell.Commands.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);

            var results = await _powershell.InvokeAsync();

            int exitcode = 0;

            if (results.Count > 0)
            {
                var lastLine = results.Last().BaseObject;

                if (lastLine is int _code)
                {
                    exitcode = _code;
                    results = new PSDataCollection<PSObject>(results.Take(results.Count - 1));
                }
            }

            return (exitcode, results);
        }

        public async Task<(int, IEnumerable<string>)> ExecuteScriptAsync(string script)
        {
            var (exitCode, results) = await ExecuteAsync(script);

            return (exitCode, new List<string>());
        }

        #region 合规的Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this._powershell.Dispose();
            this._runspace.Dispose();
        }


        ~WebPowerShell()
        {
            Dispose(false);
        }

        #endregion

    }
}
