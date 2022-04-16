using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.PowerShell
{
    public interface IProcessRunner
    {
        IProcess Start(string? filePath, ProcessSettings? settings);
    }
}
