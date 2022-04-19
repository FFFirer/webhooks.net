using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.DbContexts;

namespace WebHooks.EntityFrameworkCore.Pgsql
{
    public static class DataContextRegister
    {
        public static IServiceCollection AddPgsqlDataContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WebHooksDataConext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            return services;
        }
    }
}
