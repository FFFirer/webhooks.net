using Mapster;
using Microsoft.EntityFrameworkCore;
using WebHooks.Data.Gitee;
using WebHooks.Data.Gitee.Repository;
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
            var config = await repo.GetAll().AsNoTracking().Where(a => a.WorkId == workId).FirstOrDefaultAsync();

            if(config == null)
            {
                return null;
            }

            var dto = config.Adapt<GiteeWebHookConfigDto>();

            return dto;
        }

        public Task RemoveConfigAsync(int id)
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

            await repo.UpdateAsync(config);
        }
    }
}
