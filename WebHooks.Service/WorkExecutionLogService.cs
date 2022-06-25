using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;
using WebHooks.Service.Interfaces;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Service
{
    public class WorkExecutionLogService : IWorkExecutionLogService
    {
        private readonly IWorkExecutionLogRepository _repository;

        public WorkExecutionLogService(IWorkExecutionLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<WorkExecutionLog> Create(Guid workId)
        {
            var log = new WorkExecutionLog()
            {
                WorkId = workId,
                Status = Data.Constants.WebExecutionStatus.Ready
            };

            return await _repository.InsertAsync(log);
        }

        public async Task Update(WorkExecutionLog executionLog)
        {
            await _repository.UpdateAsync(executionLog);
        }
    }
}
