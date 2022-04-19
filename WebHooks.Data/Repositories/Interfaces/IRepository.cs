using Microsoft.EntityFrameworkCore;
using WebHooks.Data.Entities;

namespace WebHooks.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        DbSet<TEntity> Set();

        /// <summary>
        /// 获取IQueryable<TEntity>
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 根据主键获取一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> GetAsync(TPrimaryKey id);

        /// <summary>
        /// 根据主键获取一条记录，AsNoTracking
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> LoadAsync(TPrimaryKey id);

        Task<int> SaveChangesAsync();
    }
}
