using WebHooks.Data.Entities;

namespace WebHooks.Data.Gitee
{
    public class GiteeWebhookConfig : Entity<int>
    {
        public Guid WorkId { get; set; } = default!;

        public string? WebHookUrl { get; set; }

        public GiteeWebHookAuthentications Authentications { get; set; } = default!;

        public GiteeSecret Secret { get; set; } = default!;

        public GiteeSignatureKey SignatureKey { get; set; } = default!;

        public List<string> Events { get; set; } = new List<string>();
    }
}
