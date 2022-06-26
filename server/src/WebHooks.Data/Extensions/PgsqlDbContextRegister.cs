using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebHooks.Data.DbContexts;

namespace WebHooks.Data.Extensions
{
    public static class PgsqlDbContextRegister
    {
        public static IServiceCollection AddPgsqlDataContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WebHooksDataContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            return services;
        }
    }
}
