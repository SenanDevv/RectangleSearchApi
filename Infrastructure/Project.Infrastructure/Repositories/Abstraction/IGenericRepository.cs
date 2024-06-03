using Project.Core.Utilities.Results;
using System.Linq.Expressions;

namespace Project.Infrastructure.Repositories.Abstraction
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAllAsNoTracking(Expression<Func<TEntity, bool>> filter = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);
        Task<Result> AddAsync(TEntity item);
        Task<Result> AddRangeAsync(ICollection<TEntity> items);
        Task<Result> UpdateAsync(TEntity item);
    }
}
