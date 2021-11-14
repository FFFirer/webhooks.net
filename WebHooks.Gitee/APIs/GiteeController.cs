using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebHooks.Gitee.Events;
using WebHooks.Models.Gitee;

namespace WebHooks.Gitee.APIs
{
    [Route("api/webhooks/[controller]")]
    [ApiController]
    public class GiteeController : ControllerBase
    {
        private ILogger<GiteeController> _logger;

        public GiteeController(ILogger<GiteeController> logger)
        {
            _logger = logger;
        }

        [HttpPost("push")]
        public IActionResult OnPushAsync(PushWebHook webhook)
        {
            var (xGiteeToken, xGiteeTimestamp, xGiteeEvent) = ParseGiteeHeader(HttpContext);

            if(!CheckToken(xGiteeTimestamp, xGiteeToken, ""))
            {
                _logger.LogError($"签名校验失败！token:{xGiteeToken}，timestamp:{xGiteeTimestamp}，secret:{""}");
            }

            try
            {
                // 触发事件，传入webhook信息
                EventCenter.Instance.OnGiteePushed(this, new PushEventArgs(webhook, xGiteeEvent));
            }
            catch (Exception ex)
            {
                _logger.LogError($"触发事件失败，event:{xGiteeEvent}");
            }

            return Ok();
        }

        /// <summary>
        /// 校验webhook请求
        /// </summary>
        /// <param name="webhook"></param>
        /// <returns></returns>
        private bool ValidateWebHook(HttpContext httpContext, PushWebHook webhook)
        {

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
        
        /// <summary>
        /// 校验签名
        /// </summary>
        /// <param name="xGiteeTimestamp"></param>
        /// <param name="xGiteeToken"></param>
        /// <returns></returns>
        private bool CheckToken(string xGiteeTimestamp, string xGiteeToken, string secret)
        {
            var calcTokenBytes = Helpers.GiteeHelper.CalcGiteeSign(xGiteeTimestamp, secret);

            var calcToken = Convert.ToBase64String(calcTokenBytes);

            return calcToken == xGiteeToken;
        }
    }
}
