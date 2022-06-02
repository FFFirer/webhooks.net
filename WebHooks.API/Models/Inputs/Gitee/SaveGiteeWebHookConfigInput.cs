using WebHooks.Data.Gitee;
using WebHooks.Service.Dtos;
using WebHooks.Service.Gitee.Dtos;

namespace WebHooks.API.Models.Inputs.Gitee
{
    public class SaveGiteeWebHookConfigInput : Dto<int>
    {
        public Guid WorkId { get; set; } = default!;

        public string WebHookUrl { get; set; } = default!;

        public GiteeWebHookAuthentication Authentication { get; set; } = default!;

        public GiteeAuthenticationKey AuthenticationKey { get; set; } = default!;


        public List<string> Events { get; set; } = new List<string>();
    }
}
