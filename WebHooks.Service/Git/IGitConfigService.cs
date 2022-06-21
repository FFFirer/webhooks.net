using WebHooks.Data.AdditionalWork.Git;

namespace WebHooks.Service.Git
{
    public interface IGitConfigService
    {
        Task<GitConfig?> GetAsync(Guid workId);

        Task SaveAsync(GitConfig? gitConfig);

        Task RemoveAsync(Guid workId, int configId);
    }
}
