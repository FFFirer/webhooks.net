using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebHooks.Data.DbContexts;

namespace WebHooks.EFCore.Migrator
{
    static class Program
    {

        static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(Environment.CurrentDirectory);   // 当前工作目录
            });

            builder.ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<WebHookMigrationDbContext>();
                
                services.AddScoped<EFCoreMigrationExecutor>();
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var migrationExecutor = scope.ServiceProvider.GetRequiredService<EFCoreMigrationExecutor>();

                migrationExecutor.Run();
            }
        }
    }
}