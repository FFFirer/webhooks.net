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

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            this.Set().Add(entity);

            await this.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity?> LoadAsync(TPrimaryKey id)
        {
            return await this.GetAll().AsNoTracking().FirstOrDefaultAsync(a => a.Id!.Equals(id));
        }

        public async Task RemoveAsync(TPrimaryKey id)
        {
            var data = await this.GetAsync(id);

            if(data != null)
            {
                this.Set().Remove(data);
                await this.SaveChangesAsync();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public DbSet<TEntity> Set()
        {
            return _context.Set<TEntity>();
        }

        public async Task<TEntity?> UpdateAsync(TEntity? entity)
        {
            TEntity? exist = null;
            if (entity == null)
            {
                return exist;
            }

            exist = await _context.Set<TEntity>().FindAsync(entity.Id);
            if(exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(entity);
               await  _context.SaveChangesAsync();
            }
            return exist;
        }
    }
}
