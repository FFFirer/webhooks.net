using Microsoft.EntityFrameworkCore;
using WebHooks.Data.Gitee;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Service.Gitee
{
    public class GiteeService : IGiteeService
    {
        private readonly IRepository<GiteeWebhookConfig, int> repo;

        public GiteeService(IRepository<GiteeWebhookConfig, int> repo)
        {
            this.repo = repo;
        }

        public Task<GiteeWebhookConfig?> GetConfigAsync(Guid workId)
        {
            return repo.GetAll().AsNoTracking().Where(a => a.WorkId == workId).FirstOrDefaultAsync();
        }

        public Task RemoveConfigAsync(int id)
        {
            return repo.RemoveAsync(id);
        }

        public Task SaveConfigAsync(GiteeWebhookConfig? config)
        {
            if(config == null)
            {
                return Task.CompletedTask;
            }

            return repo.UpdateAsync(config);
        }
    }
}
