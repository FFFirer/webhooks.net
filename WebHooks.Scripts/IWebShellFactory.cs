using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Scripts
{
    public interface IWebShellFactory
    {
        IWebShell Create(WebShellStartupInfo startupInfo);
    }
}
