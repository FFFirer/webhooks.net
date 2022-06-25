using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Service.WorkRunner
{
    public class ExternalWorkRunnerBuildOption
    {
        /// <summary>
        /// 构建扩展任务执行器
        /// </summary>
        public Func<IServiceProvider, IExternalWorkRunner>? BuildFrom { get; set; }

        public ExternalWorkRunnerBuildOption()
        {

        }

        public ExternalWorkRunnerBuildOption(Func<IServiceProvider, IExternalWorkRunner> buildFrom)
        {
            this.BuildFrom = buildFrom;
        }
    }
}
