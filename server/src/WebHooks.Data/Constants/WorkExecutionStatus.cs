using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Data.Constants
{
    public enum WorkExecutionStatus
    {
        Ready = 1,
        Executing = 2,
        Completed = 3,
        Interrupeted = 4,   // 被中断
    }
}
