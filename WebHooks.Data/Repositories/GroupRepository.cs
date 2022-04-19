using WebHooks.Data.DbContexts;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Data.Repositories
{
    public class GroupRepository : Repository<Group, Guid>, IGroupRepository
    {
        public GroupRepository(WebHooksDataConext context) : base(context)
        {
        }
    }
}
