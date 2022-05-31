using WebHooks.Data.Entities;

namespace WebHooks.Data.Gitee
{
    public class GiteeWebhookConfig : Entity<int>
    {
        public Guid WorkId { get; set; } = default!;

        public string? WebHookUrl { get; set; }

        public GiteeWebHookAuthentication Authentication { get; set; } = default!;

        public GiteeAuthenticationKey AuthenticationKey { get; set; } = default!;


        public List<string> Events { get; set; } = new List<string>();
    }
}
