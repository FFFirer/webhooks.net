using WebHooks.Data.DbContexts;
using WebHooks.Data.Gitee.Repository;
using WebHooks.Data.Repositories;

namespace WebHooks.Data.Gitee
{
    public class GiteeConfigRepository : Repository<GiteeWebhookConfig, int>, IGiteeConfigRepository
    {
        public GiteeConfigRepository(WebHooksDataConext context) : base(context)
        {
        }
    }
}
