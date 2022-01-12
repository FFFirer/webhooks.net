﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            this.runspace = RunspaceFactory.CreateRunspace(this.host, initialState);
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

        private async Task AsyncExecuteHelper(string cmd, object? input)
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

                this.powershell.Commands.Commands[0].MergeMyResults(PipelineResultTypes.All, PipelineResultTypes.Output);

                //if (input != null)
                //{
                //    this.powershell.In(new object[] { input });
                //}else
                //{

                //}

                await this.powershell.InvokeAsync();
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
                addCmds(this.powershell);

                this.powershell.AddCommand("out-string");

                this.powershell.Commands.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);

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

        public async Task ExecuteAsync(string cmd)
        {
            try
            {
                await this.AsyncExecuteHelper(cmd, null);
            }
            catch (RuntimeException rte)
            {

                this.ReportException(rte);
            }
        }

        public async Task ExecuteAsync(Action<PowerShell> addCmd)
        {
            try
            {
                await this.AsyncExecuteHelper(addCmd);
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



        /// <summary>
        /// 初始化
        /// </summary>
        public async Task ExecutesAsync(params string[] cmds)
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

                await this.ExecuteAsync(cmd);
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
    }
}
