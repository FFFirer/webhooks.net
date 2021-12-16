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
        private readonly IOptionsSnapshot<GiteeWebHookOption> _giteeOptions;
        private readonly ICommandService _commandService;

        public GiteeService(ILogger<GiteeService> logger
            , IOptionsSnapshot<GiteeWebHookOption> namedGiteeOptions
            , ICommandService commandService)
        {
            _logger = logger;
            _giteeOptions = namedGiteeOptions;
            _commandService = commandService;
        }

        public async Task HandlePushEventAsync(string repoKey, string xGiteeToken, string xGiteeTimestamp, string xGiteeEvent, PushWebHook webHook)
        {
            _logger.LogInformation($@"Push事件触发，{nameof(xGiteeToken)}:{xGiteeToken}，{nameof(xGiteeTimestamp)}:{xGiteeTimestamp}，{nameof(xGiteeEvent)}:{xGiteeEvent}, {nameof(webHook)}: {JsonSerializer.Serialize(webHook)}");

            // 对应仓库的命名配置
            var option = _giteeOptions.Get(repoKey);

            _logger.LogDebug($"配置[{repoKey}], 内容[{JsonSerializer.Serialize(option)}]");

            if (option == null)
            {
                _logger.LogError($@"没有配置对应的构建配置，repo key: {repoKey}");
                return;
            }

            if (!CheckRequest(xGiteeToken, xGiteeTimestamp, option.Secret))
            {
                _logger.LogError($@"请求校验失败");
                return;
            }

            if (!option.Events.ContainsKey(xGiteeEvent))
            {
                _logger.LogWarning($"{xGiteeEvent}, 未配置事件触发条件: {string.Join(",", option.Events.Keys)}");
                return;
            }

            var runDictionary = Path.Combine(Environment.CurrentDirectory, "tasks", option.Platform, repoKey);
            if (!Directory.Exists(runDictionary))
            {
                Directory.CreateDirectory(runDictionary);   // 创建工作目录
            }

            var gitConfigFolder = Path.Combine(runDictionary, ".git");

            var shell = PowershellClient.Create(_logger);

            // 拉取代码操作
            var pullBranch = (PowerShell shell) =>
            {
                shell.AddStatement()
                .AddCommand("Get-GitBranch")
                .AddParameter("Directory", runDictionary)
                .AddParameter("RepoUrl", webHook?.Repository?.CloneUrl)
                .AddParameter("Branch", GetBranch(webHook));
            };

            var (exitCode, results) = await shell.InvokeAsync(pullBranch);

            _logger.LogDebug($"ExitCode: {exitCode}");

            if (exitCode != 0)
            {
                _logger.LogWarning($"检查Git仓库时出错, {string.Join("\r\n", results.Select(a => a.ToString()).ToList())}");
                return;
            }

            _logger.LogDebug($"触发实现Steps: {string.Join(",", option.Events.Keys)}");

            // 执行step
            foreach (var step in option.Events[xGiteeEvent])
            {
                var stepCommands = new PSCommand();

                var executeScripts = (PowerShell shell) =>
                {
                    foreach (var script in step.Scripts)
                    {
                        _logger.LogDebug($"添加脚本：{script}");
                        shell.AddStatement().AddScript(script);
                    }
                };
                
                var (stepExitCode, stepResults) = await shell.InvokeAsync(executeScripts);

                _logger.LogDebug($"StepExitCode: {stepExitCode}");

                if (stepExitCode != 0)
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
            if(xGiteeToken == secret)
            {
                // 密码校验
                return true;
            }

            // 签名校验
            var fact = Convert.ToBase64String(GiteeHelper.CalcGiteeSign(xGiteeTimestamp, secret));

            _logger.LogDebug($"时间戳[{xGiteeTimestamp}], 请求Token[{xGiteeToken}], 私钥[{secret}], 计算出Token[{fact}]");

            return xGiteeToken == fact;
        }

        private string GetBranch(PushWebHook? webHook)
        {
            return webHook?.Ref?.Substring(10) ?? string.Empty;
        }
    }
}
