using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;
using WebHooks.Data.Gitee;

namespace WebHooks.Service.Gitee
{
    public interface IGiteeService
    {
        Task<GiteeWebhookConfig?> GetConfigAsync(Guid workId);

        Task SaveConfigAsync(GiteeWebhookConfig? config);

        Task RemoveConfigAsync(int id);
    }
}
