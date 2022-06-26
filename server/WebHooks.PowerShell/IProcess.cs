using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.PowerShell
{
    public interface IProcess: IDisposable
    {
        void WaitForExit();

        bool WaitForExit(int milliseconds);

        int GetExitCode();

        IEnumerable<string> GetStandardError();

        IEnumerable<string> GetStandardOutput();

        void Kill();
    }
}
