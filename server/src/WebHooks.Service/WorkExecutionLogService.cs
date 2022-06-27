using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;
using WebHooks.Service.Interfaces;
using WebHooks.Data.Repositories.Interfaces;
using WebHooks.Service.Models;
using Microsoft.EntityFrameworkCore;
using WebHooks.Data.Constants;

namespace WebHooks.Service
{
    public class WorkExecutionLogService : IWorkExecutionLogService
    {
        private readonly IWorkExecutionLogRepository _repository;

        public WorkExecutionLogService(IWorkExecutionLogRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 将在此之前的所有非完成状态的都设置为被中止，并修改被修改时间
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="saveImmediately"></param>
        /// <returns></returns>
        public async Task CleanTimeoutAsync(Guid workId, DateTime deadline, bool saveImmediately = true)
        {
            var timeouts = _repository.GetAll()
                .Where(a =>
                        a.WorkId == workId 
                    && (a.Status != WorkExecutionStatus.Completed && a.Status != WorkExecutionStatus.Interrupeted) 
                    && a.ExecuteStartAt < deadline);

            await timeouts.ForEachAsync(a =>
            {
                a.Status = WorkExecutionStatus.Interrupeted;
                a.ModifedAt = DateTime.UtcNow;
                a.Success = false;
            });

            await _repository.SaveChangesAsync();
        }

        public async Task<WorkExecutionLog> CreateAsync(Guid workId)
        {
            var log = new WorkExecutionLog()
            {
                WorkId = workId,
                Status = Data.Constants.WorkExecutionStatus.Ready
            };

            return await _repository.InsertAsync(log);
        }

        public async Task<WorkExecutionLog?> GetDetailAsync(Guid workId, long logId)
        {
            return await _repository.GetAsync(logId);
        }

        public async Task<List<WorkExecutionLogSummary>> GetSummariesAsync(Guid workId)
        {
            return await _repository.GetAll()
                .AsNoTracking()
                .Where(a => a.WorkId == workId)
                .OrderByDescending(a => a.CreatedAt)
                .Select(log => new WorkExecutionLogSummary()
                {
                    Id = log.Id,
                    WorkId = log.WorkId,
                    ExecuteEndAt = log.ExecuteEndAt,
                    ExecuteStartAt = log.ExecuteStartAt,
                    ElapsedTime = log.ElapsedTime,
                    Status = log.Status,
                    Success = log.Success,
                })
                .ToListAsync();
        }

        public async Task UpdateAsync(WorkExecutionLog executionLog)
        {
            await _repository.UpdateAsync(executionLog);
        }
    }
}
