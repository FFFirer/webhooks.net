using Microsoft.Extensions.DependencyInjection;
using WebHooks.Module;
using WebHooks.Service.WorkRunner;
using WebHooks.Service.Extensions;
using WebHooks.Scripts;

namespace WebHooks.Service
{
    public class WorkRunnerModule : ExternalModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IWorkRunner, WorkRunner.WorkRunner>();
            services.InstallModule<WebShellModule>();
        }
    }
}
