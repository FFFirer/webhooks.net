using System.Text;
using WebHooks.Core.Commands;

namespace WebHooks.Gitee.Helpers
{
    public class WebShellExecuteLogger : IWebShellOutput
    {
        private readonly ILogger _logger;
        public WebShellExecuteLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WebShellExecuteLogger>();
        }

        private StringBuilder Messages { get; set; } = new StringBuilder();

        public void Clear()
        {
            Messages.Clear();
        }

        public string Get()
        {
            return Messages.ToString();
        }

        public void WriteLine(object? sender, string message)
        {
            Messages.AppendLine(message);
            _logger.LogTrace($"<OUTPUT> {message}");
        }
    }
}
