using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Scripts
{
    public class WebShellFactory : IWebShellFactory
    {
        public IWebShell Create(WebShellStartupInfo startupInfo)
        {
            return InternalCreate(startupInfo);
        }

        private IWebShell InternalCreate(WebShellStartupInfo startupInfo)
        {
            switch (startupInfo.WebShellType)
            {
                case WebShellTypes.PowerShell:
                    return new PowerShell.WebPowerShell(startupInfo);
                default:
                    throw new Exceptions.CreateWebShellException("不支持的Shell类型");
            }
        }
    }
}
