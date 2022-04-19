using Microsoft.Extensions.DependencyInjection;
using WebHooks.Data.Repositories;
using WebHooks.Data.Repositories.Interfaces;
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

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IWorkService, WorkService>();

            return services;
        }
    }
}
