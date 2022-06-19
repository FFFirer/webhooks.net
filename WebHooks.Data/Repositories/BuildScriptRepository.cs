using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.DbContexts;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Data.Repositories
{
    public class BuildScriptRepository : Repository<BuildScript, int>, IBuildScriptRepository
    {
        public BuildScriptRepository(WebHooksDataContext context) : base(context)
        {
        }
    }
}
