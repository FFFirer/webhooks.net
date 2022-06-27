using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;
using WebHooks.Service.Models;

namespace WebHooks.Service.Interfaces
{
    public interface IWorkExecutionLogService
    {
        Task<List<WorkExecutionLogSummary>> GetSummariesAsync(Guid workId);

        Task<WorkExecutionLog?> GetDetailAsync(Guid workId, long logId);

        Task<WorkExecutionLog> CreateAsync(Guid workId);

        Task UpdateAsync(WorkExecutionLog executionLog);

        Task CleanTimeoutAsync(Guid workId, DateTime deadline, bool saveImmediately = true);
    }
}
