using Microsoft.EntityFrameworkCore;

namespace WebHooks.EntityFrameworkCore.Pgsql
{
    public class WebHooksPgSqlContext : WebHooksDataConext
    {
        public WebHooksPgSqlContext(DbContextOptions<WebHooksDataConext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}