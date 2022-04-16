using WebHooks.Data.DbContexts;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Data.Repositories
{
    public class ProjectRepository : Repository<Project, Guid>, IProjectRepository
    {
        public ProjectRepository(WebHooksDataConext context) : base(context)
        {
        }
    }
}
