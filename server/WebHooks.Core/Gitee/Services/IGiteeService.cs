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
        /// <summary>
        /// 处理推送事件
        /// </summary>
        /// <param name="configKey">配置Key</param>
        /// <param name="xGiteeToken">Gitee Token</param>
        /// <param name="xGiteeTimestamp">Gitee Timestamp</param>
        /// <param name="xGiteeEvent">Gitee Event</param>
        /// <param name="webHook">Gitee WebHook</param>
        /// <returns></returns>
        Task HandlePushEventAsync(string configKey, string xGiteeToken, string xGiteeTimestamp, string xGiteeEvent, PushWebHook webHook);

        /// <summary>
        /// 处理推送事件，替换Powershell执行逻辑
        /// </summary>
        /// <param name="configKey"></param>
        /// <param name="xGiteeToken"></param>
        /// <param name="xGiteeTimestamp"></param>
        /// <param name="xGiteeEvent"></param>
        /// <param name="webHook"></param>
        /// <returns></returns>
        void HandlePushEventAsyncV2(string configKey, string xGiteeToken, string xGiteeTimestamp, string xGiteeEvent, PushWebHook webHook);
    }
}
