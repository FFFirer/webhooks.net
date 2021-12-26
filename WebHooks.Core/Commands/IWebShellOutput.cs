using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Core.Commands
{
    public interface IWebShellOutput
    {
        public void WriteLine(object? sender, string message);
        public string Get();
        public void Clear();
    }
}
