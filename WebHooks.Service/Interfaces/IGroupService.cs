using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Service.Dtos;
using WebHooks.Shared.Paging;

namespace WebHooks.Service.Interfaces
{
    public interface IGroupService
    {
        Task<List<GroupDto>> ListAsync();

        Task<PagingResult<GroupDto>> PageAsync(PagingQuery pagingQuery);

        Task SaveAsync(GroupDto group);

        Task RemoveAsync(Guid id);
    }
}
