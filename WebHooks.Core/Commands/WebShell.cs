using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
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
            this.runspace = RunspaceFactory.CreateRunspace(this.host);
            this.runspace.Open();

            lock (locker)
            {
                this.powershell = PowerShell.Create();
            }

            try
            {

            }
            catch (Exception)
            {

                throw;
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
                this.powershell.AddScript(cmd);

                this.powershell.AddCommand("out-default");

                this.powershell.Commands.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);

                if (input != null)
                {
                    this.powershell.Invoke(new object[] { input });
                }else
                {
                    this.powershell.Invoke();
                }
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
    }
}
