using WebHooks.Data.DbContexts;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Data.Repositories
{
    public class PlatformRepository : Repository<Platform, Guid>, IPlatformRepository
    {
        public PlatformRepository(WebHooksDataConext context) : base(context)
        {

        }
    }
}
