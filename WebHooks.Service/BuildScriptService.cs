using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;
using WebHooks.Service.Interfaces;

namespace WebHooks.Service
{
    public class BuildScriptService : IBuildScriptService
    {
        private IBuildScriptRepository repository;

        public BuildScriptService(IBuildScriptRepository repository)
        {
            this.repository = repository;
        }

        public Task<BuildScript?> GetAsync(Guid workId)
        {
            return repository.GetAll().AsNoTracking()
                .Where(a => a.WorkId == workId)
                .FirstOrDefaultAsync();
        }

        public Task<List<BuildScript>> GetListAsync(Guid workId)
        {
            return repository.GetAll()
                .AsNoTracking()
                .Where(a => a.WorkId == workId)
                .ToListAsync();
        }

        public async Task SaveAsync(BuildScript script)
        {
            if(script.Id > 0)
            {
                await this.repository.UpdateAsync(script);
            }
            else
            {
                await this.repository.InsertAsync(script);
            }
        }

        public Task SaveListAsync(List<BuildScript>? scripts)
        {
            if(scripts == null)
            {
                return Task.CompletedTask;
            }

            var tasks = scripts.Select(a => repository.UpdateAsync(a, false));

            Task.WaitAll(tasks.ToArray());

            return repository.SaveChangesAsync();
        }
    }
}
