using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Data.AdditionalWork.Git.Repositories
{
    public interface IGitConfigRepository : IRepository<GitConfig, int>
    {
    }
}
