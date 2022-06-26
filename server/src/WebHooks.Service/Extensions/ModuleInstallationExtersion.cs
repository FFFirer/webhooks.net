using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Module;

namespace WebHooks.Service.Extensions
{
    public static class ModuleInstallationExtersion
    {
        /// <summary>
        /// 安装模块
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection InstallModule<TModule>(this IServiceCollection services) where TModule : ExternalModule, new()
        {
            var module = new TModule();
            module.ConfigureServices(services);
            return services;
        }
    }
}
