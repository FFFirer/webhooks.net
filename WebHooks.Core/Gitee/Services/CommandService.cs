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
        

        public List<string> GetShellScripts(string @event)
        {
            throw new NotImplementedException();
        }

        public Task RunScripts(PushWebHookOption option)
        {
            // 生成临时目录，一样的配置一样的目录
            var process = new Process();
            process.StartInfo.FileName = option.Application;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;

            

            return Task.CompletedTask; 
        }

        public void RunShellScripts(string @event)
        {
            throw new NotImplementedException();
        }
    }
}
