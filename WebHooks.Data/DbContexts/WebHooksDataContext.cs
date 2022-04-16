using Microsoft.EntityFrameworkCore;
using WebHooks.Data.Entities;

namespace WebHooks.Data.DbContexts
{
    public class WebHooksDataConext : DbContext
    {
        public WebHooksDataConext(DbContextOptions<WebHooksDataConext> options) : base(options)
        {

        }

        public DbSet<Platform> Platforms => Set<Platform>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Work> Works => Set<Work>();
    }
}
