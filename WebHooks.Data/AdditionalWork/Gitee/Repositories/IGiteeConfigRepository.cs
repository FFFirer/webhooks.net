using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Data.Gitee.Repositories
{
    public interface IGiteeConfigRepository : IRepository<GiteeWebhookConfig, int>
    {
        
    }
}
