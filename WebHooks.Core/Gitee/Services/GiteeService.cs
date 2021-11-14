using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Models.Gitee;

namespace WebHooks.Core.Gitee.Services
{
    public class GiteeService : IGiteeService
    {
        public void HandlePushEvent(PushWebHook webHook)
        {
            throw new NotImplementedException();
        }
    }
}
