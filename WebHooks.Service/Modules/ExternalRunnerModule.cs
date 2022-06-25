using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Module;
using WebHooks.Service.Extensions;
using WebHooks.Service.WorkRunner;
using WebHooks.Service.WorkRunner.External;

namespace WebHooks.Service.Modules
{
    public class ExternalRunnerModule : ExternalModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // 添加扩展配置
            services.AddExternalRunner<GitExternalWorkRunner>("git");
        }
    }
}
