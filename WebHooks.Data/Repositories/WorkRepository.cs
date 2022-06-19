using WebHooks.Data.DbContexts;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Data.Repositories
{
    public class WorkRepository : Repository<Work, Guid>, IWorkRepository
    {
        public WorkRepository(WebHooksDataContext context) : base(context)
        {
        }
    }
}
