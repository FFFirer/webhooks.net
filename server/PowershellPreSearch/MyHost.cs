using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Host;
using System.Text;
using System.Threading.Tasks;

namespace PowershellPreSearch
{
    public class MyHost : PSHost
    {
        private MyProgram program;

        public MyHost(MyProgram program)
        {
            this.program = program;
        }

        private CultureInfo originalCulture => System.Threading.Thread.CurrentThread.CurrentUICulture;
        private CultureInfo originalUICulture => System.Threading.Thread.CurrentThread.CurrentUICulture;

        public override CultureInfo CurrentCulture
        {
            get { return this.originalCulture; }
        }

        public override CultureInfo CurrentUICulture
        {
            get { return this.originalUICulture; }
        }

        public Guid myId => Guid.NewGuid();

        public override Guid InstanceId
        {
            get { return this.myId; }
        }

        public override string Name
        {
            get
            {
                return "MyHostSample";
            }
        }

        public override PSHostUserInterface UI
        {
            get
            {
                return null;
            }
        }

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
            return;
        }

        public override void NotifyEndApplication()
        {
            return;
        }

        public override void SetShouldExit(int exitCode)
        {
            this.program.ShouldExit = true;
            this.program.ExitCode = exitCode;
        }
    }
}
