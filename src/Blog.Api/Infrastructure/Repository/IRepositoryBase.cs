using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Api.Infrastructure.Repository
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveAsync();
    }

    public interface IRepositoryBaseById<T, TId>
    {
        Task<T> GetByIdAsync(TId id);
        Task<bool> IsExistAsync(TId id);
        void SoftDelete(TId id);
    }
}
