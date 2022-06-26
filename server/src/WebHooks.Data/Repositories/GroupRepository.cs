using WebHooks.Data.DbContexts;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Data.Repositories
{
    public class GroupRepository : Repository<Group, Guid>, IGroupRepository
    {
        public GroupRepository(WebHooksDataContext context) : base(context)
        {
        }
    }
}
