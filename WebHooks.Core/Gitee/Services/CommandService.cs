using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Models.Gitee.Options;

namespace WebHooks.Core.Gitee.Services
{
    public class CommandService : ICommandService
    {
        private readonly ILogger _logger;   

        public CommandService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CommandService>();
        }

        public List<string> GetShellScripts(string @event)
        {
            throw new NotImplementedException();
        }

        public async Task RunScripts(GiteeWebHookOption option)
        {
             
        }

        public void RunShellScripts(string @event)
        {
            throw new NotImplementedException();
        }
    }
}
