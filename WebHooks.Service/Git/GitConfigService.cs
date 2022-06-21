using Microsoft.EntityFrameworkCore;
using WebHooks.Data.AdditionalWork.Git;
using WebHooks.Data.AdditionalWork.Git.Repositories;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Service.Git
{
    public class GitConfigService : IGitConfigService
    {
        private readonly IGitConfigRepository repository;
        private readonly IWorkRepository workRepository;

        public GitConfigService(IGitConfigRepository repository, IWorkRepository workRepository)
        {
            this.repository = repository;
            this.workRepository = workRepository;
        }

        public async Task<GitConfig?> GetAsync(Guid workId)
        {
            if (await workRepository.ExistsAsync(workId))
            {
                throw new Exception($"no such data, {nameof(workId)}: {workId}");
            }

            return await repository.GetAll().AsNoTracking().Where(a => a.WorkId == workId).FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(Guid workId, int configId)
        {
            if(await workRepository.ExistsAsync(workId))
            {
                throw new Exception($"no such data, {nameof(workId)}: {workId}");
            }

            await repository.RemoveAsync(config => config.Id == configId && config.WorkId == workId);
        }

        public async Task SaveAsync(GitConfig? gitConfig)
        {
            if (gitConfig == null)
            {
                throw new Exception("no save data");
            }

            if (gitConfig.WorkId == Guid.Empty)
            {
                throw new Exception("no binded work");
            }

            if (gitConfig.Id > 0)
            {
                await repository.UpdateAsync(gitConfig);
            }
            else
            {
                await repository.InsertAsync(gitConfig);
            }
        }
    }
}
