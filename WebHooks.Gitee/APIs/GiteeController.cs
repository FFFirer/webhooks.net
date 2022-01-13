using Microsoft.AspNetCore.Mvc;
using WebHooks.Core.Gitee.Services;
using WebHooks.Models.Gitee;

namespace WebHooks.Gitee.APIs
{
    [Route("api/webhooks/[controller]")]
    [ApiController]
    public class GiteeController : ControllerBase
    {
        private readonly ILogger<GiteeController> _logger;
        private readonly IGiteeService _giteeService;

        public GiteeController(ILogger<GiteeController> logger
            , IGiteeService giteeService, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _giteeService = giteeService;
        }

        [HttpPost("push/{repoKey}")]
        public IActionResult OnPushAsync([FromRoute]string repoKey, PushWebHook webhook)
        {
            try
            {
                var (xGiteeToken, xGiteeTimestamp, xGiteeEvent) = ParseGiteeHeader(HttpContext);

                _giteeService.HandlePushEventAsync(repoKey, xGiteeToken, xGiteeTimestamp, xGiteeEvent, webhook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理失败");
            }

            return Ok();
        }

        /// <summary>
        /// 校验Http头部
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>
        /// X-Gitee-Token, X-Gitee-Timestamp，X-Gitee-Event
        /// </returns>
        private Tuple<string, string, string> ParseGiteeHeader(HttpContext httpContext)
        {
            var contentType = HttpContext.Request.ContentType;

            if (string.IsNullOrEmpty(contentType) || contentType != "application/json")
            {
                throw new InvalidDataException($"数据格式不正确: {contentType ?? string.Empty}");
            }

            var userAgents = HttpContext.Request.Headers.UserAgent.ToList();

            if(!userAgents.Any(u => u.Equals("git-oschina-hook")))
            {
                throw new InvalidDataException($"User Agent不正确：{string.Join(";", userAgents)}");
            }

            string xGiteeToken;

            if (httpContext.Request.Headers.ContainsKey("X-Gitee-Token"))
            {
                xGiteeToken = httpContext.Request.Headers["X-Gitee-Token"];
            }
            else
            {
                throw new InvalidDataException($"缺少头部信息：X-Gitee-Token");
            }

            string xGiteeTimestamp;

            // 头部时间戳
            if (httpContext.Request.Headers.ContainsKey("X-Gitee-Timestamp"))
            {
                xGiteeTimestamp = httpContext.Request.Headers["X-Gitee-Timestamp"];
            }
            else
            {
                throw new InvalidDataException($"缺少头部信息：X-Gitee-Timestamp");
            }

            string xGiteeEvent;

            if (httpContext.Request.Headers.ContainsKey("X-Gitee-Event"))
            {
                xGiteeEvent = httpContext.Request.Headers["X-Gitee-Event"];
            }
            else
            {
                throw new InvalidDataException($"缺少头部信息：X-Gitee-Event");
            }

            return new Tuple<string, string, string>(xGiteeToken, xGiteeTimestamp, xGiteeEvent);
        }
    }
}
