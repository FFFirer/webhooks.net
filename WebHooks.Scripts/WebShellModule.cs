using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Module;

namespace WebHooks.Scripts
{
    public class WebShellModule : ExternalModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IWebShellFactory, WebShellFactory>();
        }
    }
}
