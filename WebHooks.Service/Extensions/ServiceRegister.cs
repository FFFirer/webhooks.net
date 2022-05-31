using Microsoft.Extensions.DependencyInjection;
using WebHooks.Data.Gitee;
using WebHooks.Data.Gitee.Repository;
using WebHooks.Data.Repositories;
using WebHooks.Data.Repositories.Interfaces;
using WebHooks.Service.Gitee;
using WebHooks.Service.Interfaces;

namespace WebHooks.Service.Extensions
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddWebHooksBasicService(this IServiceCollection services)
        {
            return services.AddRepostories().AddServices();
        }

        public static IServiceCollection AddRepostories(this IServiceCollection services)
        {
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IWorkRepository, WorkRepository>();

            services.AddScoped<IBuildScriptRepository, BuildScriptRepository>();

            services.AddScoped<IGiteeConfigRepository, GiteeConfigRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IWorkService, WorkService>();

            services.AddScoped<IBuildScriptService, BuildScriptService>();
            services.AddScoped<IGiteeService, GiteeService>();

            return services;
        }
    }
}
