using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebHooks.API.Models.Inputs.GitConfig;
using WebHooks.Data.AdditionalWork.Git;
using WebHooks.Service.Git;
using WebHooks.Service.Git.Dtos;

namespace WebHooks.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GitConfigController : BasicController
    {
        private readonly IGitConfigService _gitConfigService;

        public GitConfigController(IGitConfigService gitConfigService)
        {
            this._gitConfigService = gitConfigService;
        }

        [HttpGet("{workId}")]
        public async Task<GitConfigDto> Get(Guid workId)
        {
            var config = await _gitConfigService.GetAsync(workId);

            if(config == null)
            {
                config = new GitConfig();
                config.WorkId = workId;
            }

            var dto = config.Adapt<GitConfigDto>();

            return dto;
        }

        [HttpPost()]
        public async Task Save(SaveGitConfigInput input)
        {
            var config = input.ToSave?.Adapt<GitConfig>();

            await _gitConfigService.SaveAsync(config);
        }
    }
}
