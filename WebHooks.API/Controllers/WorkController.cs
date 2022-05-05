using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHooks.API.Models.Inputs;
using WebHooks.Service.Dtos;
using WebHooks.Service.Interfaces;
using WebHooks.Shared.Paging;

namespace WebHooks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkController : BasicController
    {
        private readonly IWorkService _workService;

        public WorkController(IWorkService workService)
        {
            _workService = workService;
        }

        [HttpPost("[action]")]
        public async Task<PagingResult<WorkDto>> Query(PagingInput input)
        {
            var query = input.Adapt<PagingQuery>();

            var result = await _workService.PageAsync(query);

            return result;
        }

        [HttpPost("[action]")]
        public async Task Save(WorkDto workDto)
        {
            await _workService.SaveAsync(workDto);
        }

        [HttpPost("[action]")]
        public async Task Remove(RemoveWorkInput input)
        {
            await _workService.RemoveAsync(input.Id);
        }
    }
}
