using WebHooks.Data.DbContexts;
using WebHooks.Data.Repositories;

namespace WebHooks.Data.AdditionalWork.Git.Repositories
{
    public class GitConfigRepository : Repository<GitConfig, int>, IGitConfigRepository
    {
        public GitConfigRepository(WebHooksDataContext context): base(context)
        {

        }
    }
}
