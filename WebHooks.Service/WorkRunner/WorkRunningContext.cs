using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;

namespace WebHooks.Service.WorkRunner
{
    public class WorkRunningContext : AbstractWorkRunningContext
    {
        public WorkRunningContext(Work work)
        {
            this.Work = work;
        }   

        public override Work Work { get; set; }
    }
}
