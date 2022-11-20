using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using uCondo.Domain.Entities;

namespace uCondo.Domain.Repositories
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : Base
    {
        Task Delete(TEntity entity);
        Task Delete(List<TEntity> entity);
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate = null, string[] includes = null);
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate = null, string[] includes = null, Int32 pageNumber = 1, Int32 pageSize = 10, bool orderByDesc = true);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate, string[] includes = null);
        Task Create(TEntity entity);
        Task Create(List<TEntity> entity);
        Task Update(TEntity entity);
        Task Update(List<TEntity> entity);
        Task<Int32> Count();
        Task Save();
    }
}
