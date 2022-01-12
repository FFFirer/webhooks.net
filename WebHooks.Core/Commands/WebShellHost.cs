using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Host;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Core.Commands
{
    public class WebShellHost : PSHost
    {
        #region private
        private WebShell shellInstance;
        private CultureInfo originalCultureInfo => System.Threading.Thread.CurrentThread.CurrentCulture;
        private CultureInfo originalUICultureInfo => System.Threading.Thread.CurrentThread.CurrentUICulture;
        private Guid instanceId => Guid.NewGuid();
        private WebShellUserInterface webshellUI { get; set; } = new WebShellUserInterface();
        private IWebShellOutput output { get; set; }
        private void EmitOutput(object? sender, string message)
        {
            output?.WriteLine(sender, message);
        }
        #endregion

        public WebShellHost(WebShell webShell, IWebShellOutput? webShellOutput)
        {
            shellInstance = webShell;

            if(webShellOutput != null)
            {
                webShellOutput.WriteLine(this, "订阅事件");
                output = webShellOutput;
                webShellOutput.WriteLine(this, "订阅事件成功");
            }
            else
            {
                throw new Exception("Ouput为空");
            }

            webshellUI.OutputEventHandker += EmitOutput;
        }

        public override CultureInfo CurrentCulture => originalCultureInfo;

        public override CultureInfo CurrentUICulture => originalUICultureInfo;

        public override Guid InstanceId => instanceId;

        public override string Name => "Web Shell";

        public override PSHostUserInterface UI => webshellUI;

        public override Version Version => new Version(1, 0, 0, 0);

        public override void EnterNestedPrompt()
        {
            throw new NotImplementedException();
        }

        public override void ExitNestedPrompt()
        {
            throw new NotImplementedException();
        }

        public override void NotifyBeginApplication()
        {
            throw new NotImplementedException();
        }

        public override void NotifyEndApplication()
        {
            throw new NotImplementedException();
        }

        public override void SetShouldExit(int exitCode)
        {
            this.shellInstance.ShouldExit = true;
            this.shellInstance.ExitCode = exitCode;
        }
    }
}
