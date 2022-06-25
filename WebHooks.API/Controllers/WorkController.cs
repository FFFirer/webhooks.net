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
    /// <summary>
    /// 工作项控制器
    /// </summary>
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

        /// <summary>
        /// 查询，分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<PagingResult<WorkDto>> Query(PagingInput input)
        {
            var query = input.Adapt<PagingQuery>();

            var result = await _workService.PageAsync(query);

            return result;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="workDto"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task Save(WorkDto workDto)
        {
            await _workService.SaveAsync(workDto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task Remove(RemoveWorkInput input)
        {
            await _workService.RemoveAsync(input.WorkId);
            var config = await _giteeService.GetConfigAsync(input.WorkId);
            if(config != null)
            {
                await _giteeService.RemoveConfigAsync(input.WorkId, config.Id);
            }
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 保存详情
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task SaveDetail(WorkDetailDto detail)
        {
            await _workService.SaveAsync(detail.Work);
            await _giteeService.SaveConfigAsync(detail.Config);
        }

        /// <summary>
        /// 保存脚本
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task SaveScripts(BuildScriptDto dto)
        {
            var data = dto.Adapt<BuildScript>();

            await _buildScriptService.SaveAsync(data);
        }

    }
}
