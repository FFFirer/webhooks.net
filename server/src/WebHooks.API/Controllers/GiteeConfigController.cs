using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebHooks.API.Models.Inputs.Gitee;
using WebHooks.Service.Gitee;
using WebHooks.Service.Gitee.Dtos;

namespace WebHooks.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GiteeConfigController : BasicController
    {
        private readonly IGiteeService _giteeService;

        public GiteeConfigController(IGiteeService giteeService)
        {
            _giteeService = giteeService;   
        }

        [HttpGet("{workId}")]
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

        [HttpPost()]
        public async Task Save(SaveGiteeWebHookConfigInput input)
        {
            var dto = input.Adapt<GiteeWebHookConfigDto>();

            await _giteeService.SaveConfigAsync(dto);
        }

        [HttpPost("{workId}/{configId}")]
        public async Task Remove(Guid workId, int configId)
        {
            await _giteeService.RemoveConfigAsync(workId, configId);
        }
    }
}
