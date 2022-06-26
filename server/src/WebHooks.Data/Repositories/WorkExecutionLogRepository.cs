using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;
using WebHooks.Data.DbContexts;

namespace WebHooks.Data.Repositories
{
    public class WorkExecutionLogRepository : Repository<WorkExecutionLog, long>, IWorkExecutionLogRepository
    {
        public WorkExecutionLogRepository(WebHooksDataContext context): base(context) { }
    }
}
