using Mapster;
using Microsoft.EntityFrameworkCore;
using WebHooks.Data.Gitee;
using WebHooks.Data.Gitee.Repositories;
using WebHooks.Data.Repositories.Interfaces;
using WebHooks.Service.Gitee.Dtos;

namespace WebHooks.Service.Gitee
{
    public class GiteeService : IGiteeService
    {
        private readonly IGiteeConfigRepository repo;

        public GiteeService(IGiteeConfigRepository repo)
        {
            this.repo = repo;
        }

        public async Task<GiteeWebHookConfigDto?> GetConfigAsync(Guid workId)
        {
            var config = await repo.GetAll()
                .AsNoTracking().Where(a => a.WorkId == workId).FirstOrDefaultAsync();

            if(config == null)
            {
                return null;
            }

            var dto = config.Adapt<GiteeWebHookConfigDto>();

            return dto;
        }

        public Task RemoveConfigAsync(Guid workId, int id)
        {
            return repo.RemoveAsync(id);
        }

        public async Task SaveConfigAsync(GiteeWebHookConfigDto? dto)
        {
            if(dto == null)
            {
                return;
            }

            var config = dto.Adapt<GiteeWebhookConfig>();

            if(config.Id > 0)
            {
                await repo.UpdateAsync(config);
            }
            else
            {
                await repo.InsertAsync(config);
            }
        }
    }
}
