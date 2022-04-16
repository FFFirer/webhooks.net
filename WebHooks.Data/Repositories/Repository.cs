using Microsoft.EntityFrameworkCore;
using WebHooks.Data.DbContexts;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Data.Repositories
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
    {
        protected readonly WebHooksDataConext _context;

        public Repository(WebHooksDataConext context)
        {
            _context = context;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetAsync(TPrimaryKey id)
        {
            return await this.GetAll().FirstOrDefaultAsync(a => a.Id!.Equals(id));
        }

        public virtual async Task<TEntity?> LoadAsync(TPrimaryKey id)
        {
            return await this.GetAll().AsNoTracking().FirstOrDefaultAsync(a => a.Id!.Equals(id));
        }
    }
}
