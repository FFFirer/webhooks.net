using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Module;
using WebHooks.Service.WorkRunner;

namespace WebHooks.Service
{
    public class WorkRunnerModule : ExternalModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IWorkRunner, WorkRunner.WorkRunner>();
        }
    }
}
