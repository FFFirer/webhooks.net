using Microsoft.EntityFrameworkCore;
using WebHooks.Data.Entities;

namespace WebHooks.Data.DbContexts
{
    public class WebHooksDataConext : DbContext
    {
        public WebHooksDataConext() : base()
        {

        }

        public WebHooksDataConext(DbContextOptions<WebHooksDataConext> options) : base(options)
        {

        }

        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Work> Works => Set<Work>();
    }
}
