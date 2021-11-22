using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebHooks.Core.Gitee.Helpers;
using WebHooks.Models.Gitee;
using WebHooks.Models.Gitee.Options;

namespace WebHooks.Core.Gitee.Services
{
    public class GiteeService : IGiteeService
    {
        private readonly ILogger _logger;
        private readonly IOptionsSnapshot<PushWebHookOption> _pushWebHookOptionsAccessor;
        private readonly ICommandService _commandService;

        public GiteeService(ILogger<GiteeService> logger
            , IOptionsSnapshot<PushWebHookOption> pushWebHookOptionAccessor
            , ICommandService commandService)
        {
            _logger = logger;
            _pushWebHookOptionsAccessor = pushWebHookOptionAccessor;
            _commandService = commandService;
        }

        public Task HandlePushEventAsync(string repoKey, string xGiteeToken, string xGiteeTimestamp, string xGiteeEvent, PushWebHook webHook)
        {
            _logger.LogInformation($@"Push事件触发，{nameof(xGiteeToken)}:{xGiteeToken}，{nameof(xGiteeTimestamp)}:{xGiteeTimestamp}，{nameof(xGiteeEvent)}:{xGiteeEvent}                            , {nameof(webHook)}: {JsonSerializer.Serialize(webHook)}");

            // 对应仓库的命名配置
            var option = _pushWebHookOptionsAccessor.Get(repoKey);

            if (!CheckRequest(xGiteeToken, xGiteeTimestamp, option.Secret))
            {
                _logger.LogError($@"请求校验失败");
                return Task.CompletedTask;
            }

            

            return Task.CompletedTask;
        }

        /// <summary>
        /// 校验请求
        /// </summary>
        /// <param name="xGiteeToken"></param>
        /// <param name="xGiteeTimestamp"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        private bool CheckRequest(string xGiteeToken, string xGiteeTimestamp, string secret)
        {
            var fact = Convert.ToBase64String(GiteeHelper.CalcGiteeSign(xGiteeTimestamp, secret));

            return xGiteeToken == fact;
        }
    }
}
