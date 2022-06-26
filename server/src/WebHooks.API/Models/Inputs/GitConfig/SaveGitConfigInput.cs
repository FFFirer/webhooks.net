using WebHooks.Service.Git.Dtos;

namespace WebHooks.API.Models.Inputs.GitConfig
{
    public class SaveGitConfigInput
    {
        public GitConfigDto? ToSave { get; set; }
    }
}
