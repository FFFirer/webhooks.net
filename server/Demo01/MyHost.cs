using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Host;
using System.Text;
using System.Threading.Tasks;

namespace Demo01
{
    internal class MyHost : PSHost
    {
        private PSListenerConsoleSample _program;
        private CultureInfo originalCultureInfo => System.Threading.Thread.CurrentThread.CurrentCulture;
        private CultureInfo originalUICultureInfo => System.Threading.Thread.CurrentThread.CurrentUICulture;
        private Guid myGuid => Guid.NewGuid();
        private MyHostUserInterface myHostUserInterface => new MyHostUserInterface();

        public override CultureInfo CurrentCulture => this.originalCultureInfo;

        public override CultureInfo CurrentUICulture => this.originalUICultureInfo;

        public override Guid InstanceId => this.myGuid;

        public MyHost(PSListenerConsoleSample program)
        {
            _program = program;
        }

        public override string Name => "PSDemo";

        public override PSHostUserInterface UI => this.myHostUserInterface;

        public override Version Version => new Version(1,0,0,0);

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
            return;
        }

        public override void NotifyEndApplication()
        {
            return;
        }

        public override void SetShouldExit(int exitCode)
        {
            this._program.ShouldExit = true;
            this._program.ExitCode = exitCode;
        }
    }
}
