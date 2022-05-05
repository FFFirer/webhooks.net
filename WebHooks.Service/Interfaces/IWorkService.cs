using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Service.Dtos;
using WebHooks.Shared.Paging;

namespace WebHooks.Service.Interfaces
{
    public interface IWorkService
    {
        Task<List<WorkDto>> ListAsync();

        Task<PagingResult<WorkDto>> PageAsync(PagingQuery pagingQuery);

        Task SaveAsync(WorkDto workDto);

        Task RemoveAsync(Guid id);
    }
}
