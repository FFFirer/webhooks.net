using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHooks.Data.Entities;
using WebHooks.Service.Interfaces;
using WebHooks.Service.Models;

namespace WebHooks.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkExecutionLogController : ControllerBase
    {
        private readonly IWorkExecutionLogService _executionLogService;

        public WorkExecutionLogController(IWorkExecutionLogService executionLogService)
        {
            this._executionLogService = executionLogService;
        }

        [HttpGet("{workId}")]
        public async Task<List<WorkExecutionLogSummary>> GetSummaries(Guid workId)
        {
            var summaries = await _executionLogService.GetSummariesAsync(workId);
            return summaries;
        }

        [HttpGet("{workId}/{logId}")]
        public async Task<WorkExecutionLog?> GetDetail(Guid workId, long logId)
        {
            var detail = await _executionLogService.GetDetailAsync(workId, logId);

            return detail;
        }
    }
}
