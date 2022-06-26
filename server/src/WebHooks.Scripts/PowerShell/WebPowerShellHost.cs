using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Host;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Scripts.PowerShell
{
    public class WebPowerShellHost : PSHost
    {
        private readonly WebPowerShell _shell;

        public WebPowerShellHost(WebPowerShell shell)
        {
            this._shell = shell;
            this.Name = shell.Name;
            this.Version = shell.Version;
            this.UI = new WebPowerShellUserInterface();
        }

        public override CultureInfo CurrentCulture => CultureInfo.CurrentCulture;

        public override CultureInfo CurrentUICulture => CultureInfo.CurrentUICulture;

        public override Guid InstanceId => Guid.NewGuid();

        public override string Name { get; }

        public override PSHostUserInterface UI { get; }

        public override Version Version { get; }

        public override void EnterNestedPrompt()
        {
            
        }

        public override void ExitNestedPrompt()
        {
            
        }

        public override void NotifyBeginApplication()
        {
            
        }

        public override void NotifyEndApplication()
        {
            
        }

        public override void SetShouldExit(int exitCode)
        {
            this._shell.ExitCode = exitCode;
            if(exitCode > 0)
            {
                this._shell.ShoultExit = true;
            }
        }
    }
}
