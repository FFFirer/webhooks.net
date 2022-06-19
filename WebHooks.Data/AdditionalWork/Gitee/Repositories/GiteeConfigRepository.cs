using WebHooks.Data.DbContexts;
using WebHooks.Data.Gitee.Repositories;
using WebHooks.Data.Repositories;

namespace WebHooks.Data.Gitee
{
    public class GiteeConfigRepository : Repository<GiteeWebhookConfig, int>, IGiteeConfigRepository
    {
        public GiteeConfigRepository(WebHooksDataContext context) : base(context)
        {

        }
    }
}
