using Microsoft.Extensions.DependencyInjection;

namespace WebHooks.Module
{
    public class ExternalModule : IExternalModule
    {
        /// <summary>
        /// 服务配置
        /// </summary>
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services) { }
    }
}