using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;

namespace WebHooks.Service.Interfaces
{
    public interface IWorkExecutionLogService
    {
        Task<WorkExecutionLog> Create(Guid workId);

        Task Update(WorkExecutionLog executionLog);
    }
}
