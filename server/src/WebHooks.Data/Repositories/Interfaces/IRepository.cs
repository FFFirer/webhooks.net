using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;
using WebHooks.Data.Entities;

namespace WebHooks.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        DatabaseFacade Database { get; }

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
        /// 根据主键移除一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveAsync(TPrimaryKey id);

        /// <summary>
        /// 删除，条件
        /// </summary>
        /// <returns></returns>
        Task RemoveAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键获取一条记录，AsNoTracking
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> LoadAsync(TPrimaryKey id);

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity?> UpdateAsync(TEntity? entity, bool saveImmediately = true);

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(TPrimaryKey id);
    }
}
