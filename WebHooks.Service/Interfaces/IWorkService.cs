using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Service.Dtos;

namespace WebHooks.Service.Interfaces
{
    public interface IWorkService
    {
        Task<List<WorkDto>> ListAsync();
    }
}
