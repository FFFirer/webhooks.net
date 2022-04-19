using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Xml;
using WebHooks.Data.DbContexts;

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
