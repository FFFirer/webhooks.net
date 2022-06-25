using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;

namespace WebHooks.Service.WorkRunner
{
    /// <summary>
    /// Work运行上下文
    /// </summary>
    public abstract class AbstractWorkRunningContext
    {
        public abstract Work Work { get; set; }
    }
}
