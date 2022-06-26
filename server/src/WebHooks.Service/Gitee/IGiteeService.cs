using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;
using WebHooks.Data.Gitee;
using WebHooks.Service.Gitee.Dtos;

namespace WebHooks.Service.Gitee
{
    public interface IGiteeService
    {
        Task<GiteeWebHookConfigDto?> GetConfigAsync(Guid workId);

        Task SaveConfigAsync(GiteeWebHookConfigDto? dto);

        Task RemoveConfigAsync(Guid workdId, int id);
    }
}
