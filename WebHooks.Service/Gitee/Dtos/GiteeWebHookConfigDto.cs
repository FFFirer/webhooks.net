using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Gitee;
using WebHooks.Service.Dtos;

namespace WebHooks.Service.Gitee.Dtos
{
    public class GiteeWebHookConfigDto : Dto<int>
    {
        public Guid WorkId { get; set; } = default!;

        public string WebHookUrl { get; set; } = default!;

        public GiteeWebHookAuthentication Authentication { get; set; } = default!;

        public GiteeAuthenticationKey AuthenticationKey { get; set; } = default!;


        public List<string> Events { get; set; } = new List<string>();
    }
}
