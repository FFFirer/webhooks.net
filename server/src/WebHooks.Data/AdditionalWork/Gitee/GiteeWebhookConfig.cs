using WebHooks.Data.Entities;

namespace WebHooks.Data.Gitee
{
    /// <summary>
    /// Gitee WebHook 配置
    /// </summary>
    public class GiteeWebhookConfig : Entity<int>
    {
        /// <summary>
        /// 关联的工作项
        /// </summary>
        public Guid WorkId { get; set; } = default!;

        /// <summary>
        /// 监听的地址
        /// </summary>
        public string? WebHookUrl { get; set; }

        /// <summary>
        /// 工作目录
        /// </summary>
        public string? WorkDirectory { get; set; }

        /// <summary>
        /// 身份验证方式
        /// </summary>
        public GiteeWebHookAuthentication Authentication { get; set; } = default!;

        /// <summary>
        /// 身份验证密码
        /// </summary>
        public GiteeAuthenticationKey AuthenticationKey { get; set; } = default!;

        /// <summary>
        /// 触发的事件
        /// </summary>
        public List<string> Events { get; set; } = new List<string>();
    }
}
