using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Management.Automation;
using System.Text.Json;
using WebHooks.Core.Commands;
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

        public async Task HandlePushEventAsync(string repoKey, string xGiteeToken, string xGiteeTimestamp, string xGiteeEvent, PushWebHook webHook)
        {
            _logger.LogInformation($@"Push事件触发，{nameof(xGiteeToken)}:{xGiteeToken}，{nameof(xGiteeTimestamp)}:{xGiteeTimestamp}，{nameof(xGiteeEvent)}:{xGiteeEvent}                            , {nameof(webHook)}: {JsonSerializer.Serialize(webHook)}");

            // 对应仓库的命名配置
            var option = _pushWebHookOptionsAccessor.Get(repoKey);

            if (!CheckRequest(xGiteeToken, xGiteeTimestamp, option.Secret))
            {
                _logger.LogError($@"请求校验失败");
                return;
            }

            if (option == null)
            {
                _logger.LogError($@"没有配置对应的构建配置，repo key: {repoKey}");
                return;
            }

            var runDictionary = Path.Combine(Environment.CurrentDirectory, "tasks", repoKey);
            if (!Directory.Exists(runDictionary))
            {
                Directory.CreateDirectory(runDictionary);   // 创建工作目录
            }

            var gitConfigFolder = Path.Combine(runDictionary, ".git");

            var shell = PowershellClient.Create(_logger);

            // 拉取代码操作
            var commands = new PSCommand();

            commands.AddStatement()
                .AddCommand("Git-Pull-Branch")
                .AddParameter("Directory", runDictionary)
                .AddParameter("RepoUrl", webHook?.Repository?.CloneUrl)
                .AddParameter("Branch", GetBranch(webHook));

            var (exitCode, results) = await shell.InvokeAsync(commands);

            if (exitCode != 0)
            {
                _logger.LogWarning($"检查Git仓库时出错, {string.Join("\r\n", results.Select(a => a.ToString()).ToList())}");
                return;
            }

            // 执行step
            foreach (var step in option.Steps)
            {
                var stepCommands = new PSCommand();

                foreach (var script in step.Scripts)
                {
                    stepCommands.AddCommand(script);
                }

                var (stepExitCode, stepResults) = await shell.InvokeAsync(commands);

                if(stepExitCode != 0)
                {
                    _logger.LogWarning($"执行步骤时出错, {string.Join("\r\n", stepResults.Select(a => a.ToString()).ToList())}");

                    break;
                }
            }

            return;
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

        private string GetBranch(PushWebHook? webHook)
        {
            return webHook?.Ref?.Substring(10) ?? string.Empty;
        }
    }
}
