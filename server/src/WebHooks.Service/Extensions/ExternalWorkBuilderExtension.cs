using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Service.WorkRunner;

namespace WebHooks.Service.Extensions
{
    /// <summary>
    /// 扩展工作构建器
    /// </summary>
    public static class ExternalWorkBuilderExtension 
    {
        /// <summary>
        /// 添加扩展运行器
        /// </summary>
        /// <typeparam name="TExternalWorkRunner"></typeparam>
        /// <param name="services"></param>
        /// <param name="name"></param>
        public static void AddExternalRunner<TExternalWorkRunner>(this IServiceCollection services, string name) where TExternalWorkRunner : class, IExternalWorkRunner
        {
            // 注入实现
            services.AddScoped<IExternalWorkRunner, TExternalWorkRunner>();

            services.AddScoped<TExternalWorkRunner>();

            services.AddExternalBuildOption(name, (services) => { return services.GetRequiredService<TExternalWorkRunner>(); });
        }

        public static void AddExternalBuildOption(this IServiceCollection services, string name, Func<IServiceProvider, IExternalWorkRunner> build)
        {
            services.Configure<ExternalWorkRunnerBuildOption>(name, options => options.BuildFrom = build);
        }
    }
}
