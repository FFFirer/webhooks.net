using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebHooks.Data.DbContexts;

namespace WebHooks.EFCore.Migrator
{
    public class WebHookMigrationDbContext : WebHooksDataContext
    {
        private readonly IConfiguration configuration;

        public WebHookMigrationDbContext(IConfiguration configuration) : base()
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Default"));
        }
    }
}
