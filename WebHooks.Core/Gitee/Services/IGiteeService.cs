using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Models.Gitee;

namespace WebHooks.Core.Gitee.Services
{
    public interface IGiteeService
    {
        Task HandlePushEventAsync(string configKey, string xGiteeToken, string xGiteeTimestamp, string xGiteeEvent, PushWebHook webHook);
    }
}
