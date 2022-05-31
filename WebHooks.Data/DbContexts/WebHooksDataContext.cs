using Microsoft.EntityFrameworkCore;
using WebHooks.Data.Entities;
using WebHooks.Data.Extensions;
using WebHooks.Data.Gitee;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GiteeWebhookConfig>(builder =>
            {
                builder.Property(a => a.Authentications).HasConversion<string>();
                builder.Property(a => a.Secret).HasJsonConversion();
                builder.Property(a => a.SignatureKey).HasJsonConversion();
                builder.Property(a => a.Events).HasJsonConversion();
            });

            modelBuilder.Entity<BuildScript>(builder =>
            {
                builder.Property(a => a.Scripts).HasJsonConversion();
            });
        }

        public virtual DbSet<Group> Groups => Set<Group>();
        public virtual DbSet<Work> Works => Set<Work>();
        public virtual DbSet<GiteeWebhookConfig> GiteeConfigs => Set<GiteeWebhookConfig>();
        public virtual DbSet<BuildScript> BuildScripts => Set<BuildScript>();
    }
}
