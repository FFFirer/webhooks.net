using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;

namespace WebHooks.Service.WorkRunner
{
    public interface IWorkRunner
    {
        Task RunAsync(Work work);
    }
}
