﻿using Microsoft.EntityFrameworkCore;
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

        public Task<List<BuildScript>> GetListAsync(Guid workId)
        {
            return repository.GetAll()
                .AsNoTracking()
                .Where(a => a.WorkId == workId)
                .ToListAsync();
        }

        public Task SaveAsync(List<BuildScript>? scripts)
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