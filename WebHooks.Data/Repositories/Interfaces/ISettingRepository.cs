using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;

namespace WebHooks.Data.Repositories.Interfaces
{
    public interface ISettingRepository : IRepository<Setting, Guid>
    {
        Task<List<Setting>> GetOneAsync(string section);

        Task<long> SaveListAsync(List<Setting> settings);

        Task<List<Setting>> GetListAsync(string section, params string[] subsections);

        Task<long> SaveOrDeleteAsync(List<Setting> toSave, List<Setting> toDelete);
    }
}
