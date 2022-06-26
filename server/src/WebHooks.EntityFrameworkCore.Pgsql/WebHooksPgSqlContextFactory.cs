using Microsoft.EntityFrameworkCore.Design;

namespace WebHooks.EntityFrameworkCore.Pgsql
{
    public class WebHooksPgSqlContextFactory : IDesignTimeDbContextFactory<WebHooksPgSqlContext>
    {
        public WebHooksPgSqlContext CreateDbContext(string[] args)
        {
            return new WebHooksPgSqlContext();
        }
    }
}
