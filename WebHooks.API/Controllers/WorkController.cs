using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHooks.API.Models.Inputs;
using WebHooks.Data.Entities;
using WebHooks.Service.Dtos;
using WebHooks.Service.Gitee;
using WebHooks.Service.Interfaces;
using WebHooks.Shared.Paging;

namespace WebHooks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkController : BasicController
    {
        private readonly IWorkService _workService;
        private readonly IGiteeService _giteeService;
        private readonly IBuildScriptService _buildScriptService;

        public WorkController(IWorkService workService, IBuildScriptService buildScriptService, IGiteeService giteeService)

        {
            _workService = workService;
            _buildScriptService = buildScriptService;
            _giteeService = giteeService;
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
            var config = await _giteeService.GetConfigAsync(input.Id);
            if(config != null)
            {
                await _giteeService.RemoveConfigAsync(config.Id);
            }
        }

        [HttpGet("[action]/{workId}")]
        public async Task<WorkDetailDto> Detail(Guid workId)
        {
            var dto = new WorkDetailDto();
            var work = await _workService.GetAsync(workId);
            if(work == null)
            {
                return dto;
            }
            dto.Work = work;
            dto.Config = await _giteeService.GetConfigAsync(workId);
            dto.Script = await _buildScriptService.GetAsync(workId);

            return dto;
        }

        [HttpPost("[action]")]
        public async Task SaveDetail(WorkDetailDto detail)
        {
            await _workService.SaveAsync(detail.Work);
            await _giteeService.SaveConfigAsync(detail.Config);
        }

        [HttpPost("[action]")]
        public async Task SaveScripts(BuildScriptDto dto)
        {
            var data = dto.Adapt<BuildScript>();

            await _buildScriptService.SaveAsync(data);
        }
    }
}
