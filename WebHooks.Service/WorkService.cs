using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Service.Dtos;
using WebHooks.Service.Interfaces;

namespace WebHooks.Service
{
    public class WorkService : IWorkService
    {
        public Task<List<WorkDto>> ListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
