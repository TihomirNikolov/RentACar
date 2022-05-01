using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        int Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity> SelectAsync(Expression<Func<TEntity, bool>> where = null);
        Task<IEnumerable<TEntity>> SelectAllAsync(Expression<Func<TEntity, bool>> where = null);
    }
}
