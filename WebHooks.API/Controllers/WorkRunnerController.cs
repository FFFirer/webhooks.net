using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHooks.Data.Repositories.Interfaces;
using WebHooks.Service.Exceptions;
using WebHooks.Service.Interfaces;
using WebHooks.Service.WorkRunner;

namespace WebHooks.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkRunnerController : ControllerBase
    {
        private readonly IWorkRunner _runner;
        private readonly IWorkRepository _works;

        public WorkRunnerController(IWorkRunner runner, IWorkRepository workRepository)
        {
            this._runner = runner;
            this._works = workRepository;
        }

        [HttpPost("{workId}")]
        public async Task Run(Guid workId)
        {
            var work = await _works.GetAsync(workId);

            if(work == null)
            {
                throw new WorkRunningException($"没有找到对象的工作项: {workId}");
            }

            await _runner.RunAsync(work);
        }
    }
}
