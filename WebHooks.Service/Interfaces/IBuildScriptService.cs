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
        Task<BuildScript?> GetAsync(Guid workId);

        Task SaveAsync(BuildScript script);

        Task<List<BuildScript>> GetListAsync(Guid workId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scripts"></param>
        /// <returns></returns>
        Task SaveListAsync(List<BuildScript>? scripts);
    }
}
