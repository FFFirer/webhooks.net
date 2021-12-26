using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Core.Commands
{
    public class WebShellOutputHelepr : IWebShellOutput
    {
        private StringBuilder _buffer => new StringBuilder();

        public void Clear()
        {
            _buffer.Clear();
        }

        public string Get()
        {
            return _buffer.ToString();
        }

        public void WriteLine(object? sender, string message)
        {
            _buffer.AppendLine(message);
        }
    }
}
