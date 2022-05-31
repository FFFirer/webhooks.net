using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHooks.API.Models.Inputs.Gitee;
using WebHooks.Data.Gitee;
using WebHooks.Service.Gitee;
using WebHooks.Service.Gitee.Dtos;

namespace WebHooks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiteeConfigController : BasicController
    {
        private readonly IGiteeService _giteeService;

        public GiteeConfigController(IGiteeService giteeService)
        {
            _giteeService = giteeService;   
        }

        [HttpGet("[action]/{workId}/{configId}")]
        public async Task<GiteeWebHookConfigDto> Get(Guid workId)
        {
            var config = await _giteeService.GetConfigAsync(workId);

            if(config == null)
            {
                config = new GiteeWebHookConfigDto();
                config.WorkId = workId;
            }

            var dto = config.Adapt<GiteeWebHookConfigDto>();

            return dto;
        }

        [HttpPost("[action]")]
        public async Task Save(SaveGiteeWebHookConfigInput input)
        {
            var dto = input.Adapt<GiteeWebHookConfigDto>();

            await _giteeService.SaveConfigAsync(dto);
        }
    }
}
