using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;

namespace WebHooks.Service.Interfaces
{
    public interface IBuildScriptService
    {
        Task<List<BuildScript>> GetListAsync(Guid workId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scripts"></param>
        /// <returns></returns>
        Task SaveAsync(List<BuildScript>? scripts);
    }
}
