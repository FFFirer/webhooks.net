using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Module
{
    public interface IExternalModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}
