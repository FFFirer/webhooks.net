using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebHooks.Core.Commands
{
    public class WebShellOutputHelepr : IWebShellOutput
    {
        private StringBuilder _buffer => new StringBuilder();
        private readonly ILogger? _logger;

        public WebShellOutputHelepr()
        {

        }
        public WebShellOutputHelepr(ILogger logger)
        {
            this._logger = logger;
        }

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
            message = FormatMessage(message);
            _buffer.AppendLine(message);
            _logger?.LogDebug($"<OUTPUT> {message}");
        }

        private string FormatMessage(string message)
        {
            message = Regex.Replace(message, AnsiColorPattern, "");   // 去除Ansi Escape Codes
            message = message.TrimEnd('\n');
            return message;
        }

        private string AnsiColorPattern = @"\u001b(.*?)m";
    }
}
