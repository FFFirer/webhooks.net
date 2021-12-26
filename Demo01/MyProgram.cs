
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo01
{
    internal class MyProgram
    {
        private bool shouldExit { get; set; }
        private int exitCode { get; set; }
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
    }
}
