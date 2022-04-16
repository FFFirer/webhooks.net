using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.PowerShell
{
    /// <summary>
    /// 包装器
    /// </summary>
    public class ProcessWrapper : IProcess
    {
        private readonly Process _process;
        private readonly ConcurrentQueue<string> _consoleErrorOueue;
        private readonly ConcurrentQueue<string> _consoleOutputOueue;

        public ProcessWrapper(Process process)
        {
            _process = process;
            _consoleErrorOueue = new ConcurrentQueue<string>();
            _consoleOutputOueue = new ConcurrentQueue<string>();
        }

        public void Dispose()
        {
            _process.Dispose();
        }

        public int GetExitCode()
        {
            return _process.ExitCode;
        }

        internal void StandardErrorReceived(string? standardError)
        {
            if(standardError != null)
            {
                _consoleErrorOueue.Enqueue(standardError);
            }
        }

        public IEnumerable<string> GetStandardError()
        {
            while (!_consoleErrorOueue.IsEmpty || !_process.HasExited)
            {
                if (!_consoleErrorOueue.TryDequeue(out var line))
                {
                    continue;
                }

                yield return line;
            }
        }

        internal void StandardOutputReceived(string? standardOutput)
        {
            if(standardOutput != null)
            {
                _consoleOutputOueue.Enqueue(standardOutput);
            }
        }

        public IEnumerable<string> GetStandardOutput()
        {
            while (!_consoleOutputOueue.IsEmpty || !_process.HasExited)
            {
                if (!_consoleOutputOueue.TryDequeue(out var line))
                {
                    continue;
                }

                yield return line;
            }
        }

        public void Kill()
        {
            _process.Kill();
            _process.WaitForExit();
        }

        public void WaitForExit()
        {
            _process.WaitForExit();
        }

        public bool WaitForExit(int milliseconds)
        {
            if (_process.WaitForExit(milliseconds))
            {
                return true;
            }
            _process.Refresh();
            if (!_process.HasExited)
            {
                _process.Kill();
            }
            return false;
        }
    }
}
