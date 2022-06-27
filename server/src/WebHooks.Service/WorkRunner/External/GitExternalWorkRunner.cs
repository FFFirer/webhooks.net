using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebHooks.Data.AdditionalWork.Git;
using WebHooks.Service.Exceptions;
using WebHooks.Service.Git;

namespace WebHooks.Service.WorkRunner.External
{
    public class GitExternalWorkRunner : BaseExternalWorkRunner, IExternalWorkRunner
    {
        private readonly IGitConfigService _gitConfigService;

        public GitExternalWorkRunner(ILogger<GitExternalWorkRunner> logger, IGitConfigService gitConfigService) : base(logger)
        {
            this._gitConfigService = gitConfigService;
        }

        public async Task Run(AbstractWorkRunningContext context)
        {
            if (context.Work.WorkingDirectory == null)
            {
                throw new WorkRunningException($"未找到工作路径");
            }

            if (!Directory.Exists(context.Work.WorkingDirectory))
            {
                throw new WorkRunningException($"目录未创建: {context.Work.WorkingDirectory}");
            }

            var gitConfig = await _gitConfigService.GetAsync(context.Work.Id);

            if (gitConfig == null)
            {
                throw new ExternalConfigException($"没有找到配置");
            }

            // Discover是向父级目录查找
            var repositoryPath = context.Work.WorkingDirectory;

            // 判断是否存在.git文件夹
            var gitFolder = Path.Combine(context.Work.WorkingDirectory, ".git");

            var existsRepository = Directory.Exists(gitFolder);

            if (!existsRepository)
            {
                if (gitConfig.RepositoryAddress == null)
                {
                    throw new ExternalConfigException($"{nameof(GitConfig.RepositoryAddress)}为空");
                }

                if (string.IsNullOrEmpty(gitConfig.Branch))
                {
                    throw new ExternalConfigException($"{nameof(GitConfig.RepositoryAddress)}为空");
                }

                // Clone
                var cloneOptions = new CloneOptions()
                {
                    Checkout = true,
                    BranchName = gitConfig.Branch,
                    RecurseSubmodules = false,
                };

                repositoryPath = Repository.Clone(gitConfig.RepositoryAddress, context.Work.WorkingDirectory, cloneOptions);

                this._logger.LogDebug($"克隆存储库到：{repositoryPath}");
            }
            else
            {
                // Pull
                using (var repository = new Repository(repositoryPath))
                {
                    var logMessage = "";

                    var remote = repository.Network.Remotes.FirstOrDefault(r => r.Url == gitConfig.RepositoryAddress);

                    if (remote == null)
                    {
                        throw new WorkRunningException($"当前工作目录下的远程仓库地址与配置不符");
                    }

                    var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);

                    Commands.Fetch(repository, remote.Name, refSpecs, null, logMessage);

                    this._logger.LogDebug($"Fetch代码仓库：{logMessage}");

                    var friendlyBranchName = gitConfig.Branch;
                    var friendlyRemoteBranchName = $"{remote.Name}/{gitConfig.Branch}";

                    var localBranch = repository.Branches.FirstOrDefault(b => !b.IsRemote && b.RemoteName == remote.Name && b.FriendlyName == friendlyBranchName);
                    var remoteBranch = repository.Branches.FirstOrDefault(b => b.IsRemote && b.RemoteName == remote.Name && b.FriendlyName == friendlyRemoteBranchName);

                    if (localBranch == null)
                    {
                        localBranch = repository.CreateBranch(gitConfig.Branch);

                        if (remoteBranch == null)
                        {
                            throw new WorkRunningException($"不存在分支{gitConfig.Branch}");
                        }

                        // 给定的分支
                        repository.Branches.Update(localBranch, b => b.TrackedBranch = remoteBranch.CanonicalName);
                    }

                    var currentBranch = Commands.Checkout(repository, localBranch);

                    var pullOptions = new PullOptions();



                    pullOptions.MergeOptions = new MergeOptions();
                    pullOptions.MergeOptions.FailOnConflict = true;
                    pullOptions.FetchOptions = new FetchOptions();

                    pullOptions.FetchOptions.CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) => new UsernamePasswordCredentials()
                    {
                        Username = gitConfig.UserName ?? string.Empty,
                        Password = gitConfig.Password ?? string.Empty,
                    });

                    Commands.Pull(repository, new Signature(gitConfig.UserName ?? string.Empty, gitConfig.Email ?? string.Empty, new DateTimeOffset(DateTime.Now)), pullOptions);

                    this._logger.LogDebug($"检出分支: {currentBranch.FriendlyName}");
                }
            }
        }
    }
}
