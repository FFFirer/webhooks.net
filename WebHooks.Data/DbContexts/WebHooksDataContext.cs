using Microsoft.EntityFrameworkCore;
using WebHooks.Data.AdditionalWork.Git;
using WebHooks.Data.Entities;
using WebHooks.Data.Extensions;
using WebHooks.Data.Gitee;

namespace WebHooks.Data.DbContexts
{
    public class WebHooksDataContext : DbContext
    {
        public WebHooksDataContext() : base()
        {

        }

        public WebHooksDataContext(DbContextOptions<WebHooksDataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GiteeWebhookConfig>(builder =>
            {
                builder.Property(a => a.Authentication).HasConversion<string>();
                builder.Property(a => a.AuthenticationKey).HasJsonConversion();

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

        public virtual DbSet<GitConfig> GitConfigs => Set<GitConfig>();

        public virtual DbSet<Setting> Settings => Set<Setting>();
    }
}
