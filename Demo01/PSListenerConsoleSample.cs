using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace Demo01
{
    internal class PSListenerConsoleSample
    {
        private bool shouldExit { get; set; }
        private int exitCode { get; set; }

        private MyHost myHost;
        private Runspace myRunspace;
        private PowerShell? currentPowerShell;

        private object instanceLock = new object();

        public bool ShouldExit
        {
            get { return shouldExit; }
            set { shouldExit = value; }
        }

        public int ExitCode
        {
            get { return exitCode; }
            set { exitCode = value; }
        }

        public PSListenerConsoleSample()
        {
            this.myHost = new MyHost(this);
            this.myRunspace = RunspaceFactory.CreateRunspace(this.myHost);
            this.myRunspace.Open();

            lock (instanceLock)
            {
                this.currentPowerShell = PowerShell.Create();
            }

            try
            {
                this.currentPowerShell.Runspace = this.myRunspace;

                PSCommand[] profileCommands = new PSCommand[0];

                foreach (var command in profileCommands)
                {
                    this.currentPowerShell.Commands = command;
                    this.currentPowerShell.Invoke();
                }

            }
            finally
            {
                lock (this.instanceLock)
                {
                    this.currentPowerShell.Dispose();
                    this.currentPowerShell = null;
                }
            }
        }

        private void executeHelper(string cmd, object? input)
        {
            if (string.IsNullOrEmpty(cmd))
            {
                return;
            }

            lock (this.instanceLock)
            {
                this.currentPowerShell = PowerShell.Create();
            }

            this.currentPowerShell.Runspace = this.myRunspace;

            try
            {
                this.currentPowerShell.AddScript(cmd);

                this.currentPowerShell.AddCommand("out-default");
                this.currentPowerShell.Commands.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);

                if (input != null)
                {
                    this.currentPowerShell.Invoke(new object[] { input });
                }
                else
                {
                    this.currentPowerShell.Invoke();
                }
            }
            finally
            {
                lock (this.instanceLock)
                {
                    this.currentPowerShell?.Dispose();
                    this.currentPowerShell = null;
                }
            }
        }

        private void ReportException(Exception e)
        {
            if(e != null)
            {
                object error;
                IContainsErrorRecord? icer = e as IContainsErrorRecord;
                    
                if(icer != null)
                {
                    error = icer.ErrorRecord;
                }
                else
                {
                    error = (object)new ErrorRecord(e, "Host.ReportException", ErrorCategory.NotSpecified, null);
                }


                lock (this.instanceLock)
                {
                    this.currentPowerShell = PowerShell.Create();
                }

                this.currentPowerShell.Runspace = this.myRunspace;

                try
                {
                    this.currentPowerShell.AddScript("$input").AddCommand("out-string");

                    // Do not merge errors, this function will swallow errors.
                    Collection<PSObject> result;
                    PSDataCollection<object> inputCollection = new PSDataCollection<object>();
                    inputCollection.Add(error);
                    inputCollection.Complete();
                    result = this.currentPowerShell.Invoke(inputCollection);

                    if (result.Count > 0)
                    {
                        string str = result[0].BaseObject as string ?? "";
                        if (!string.IsNullOrEmpty(str))
                        {
                            // Remove \r\n that is added by out-string.
                            this.myHost.UI.WriteErrorLine(str.Substring(0, str.Length - 2));
                        }
                    }


                }
                finally
                {
                    // Dispose of the pipeline line and set it to null, locked because currentPowerShell
                    // may be accessed by the ctrl-C handler.
                    lock (this.instanceLock)
                    {
                        this.currentPowerShell.Dispose();
                        this.currentPowerShell = null;
                    }
                }
            }
        }


        /// <summary>
        /// Basic script execution routine - any runtime exceptions are
        /// caught and passed back into the engine to display.
        /// </summary>
        /// <param name="cmd">The parameter is not used.</param>
        private void Execute(string cmd)
        {
            try
            {
                // Execute the command with no input.
                this.executeHelper(cmd, null);
            }
            catch (RuntimeException rte)
            {
                this.ReportException(rte);
            }
        }

        /// <summary>
        /// Method used to handle control-C's from the user. It calls the
        /// pipeline Stop() method to stop execution. If any exceptions occur
        /// they are printed to the console but otherwise ignored.
        /// </summary>
        /// <param name="sender">See sender property of ConsoleCancelEventHandler documentation.</param>
        /// <param name="e">See e property of ConsoleCancelEventHandler documentation</param>
        private void HandleControlC(object? sender, ConsoleCancelEventArgs e)
        {
            try
            {
                lock (this.instanceLock)
                {
                    if (this.currentPowerShell != null && this.currentPowerShell.InvocationStateInfo.State == PSInvocationState.Running)
                    {
                        this.currentPowerShell.Stop();
                    }
                }

                e.Cancel = true;
            }
            catch (Exception exception)
            {
                this.myHost.UI.WriteErrorLine(exception.ToString());
            }
        }

        /// <summary>
        /// Implements the basic listener loop. It sets up the ctrl-C handler, then
        /// reads a command from the user, executes it and repeats until the ShouldExit
        /// flag is set.
        /// </summary>
        public void Run()
        {
            // Set up the control-C handler.
            Console.CancelKeyPress += new ConsoleCancelEventHandler(this.HandleControlC);
            Console.TreatControlCAsInput = false;

            // Read commands to execute until ShouldExit is set by
            // the user calling "exit".
            while (!this.ShouldExit)
            {
                this.myHost.UI.Write(ConsoleColor.Cyan, ConsoleColor.Black, "\nPSConsoleSample: ");
                string cmd = Console.ReadLine() ?? String.Empty;
                this.Execute(cmd);
            }

            // Exit with the desired exit code that was set by exit command.
            // This is set in the host by the MyHost.SetShouldExit() implementation.
            Environment.Exit(this.ExitCode);
        }
    }
}
