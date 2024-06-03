using Microsoft.EntityFrameworkCore;
using Project.Core.Utilities.Results;
using Project.Infrastructure.DAL;
using Project.Infrastructure.Repositories.Abstraction;
using System.Linq.Expressions;

namespace Project.Infrastructure.Repositories.Implementation
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
         where TEntity : class
    {
        private readonly AppDbContext _dbContext;

        protected DbSet<TEntity> DbSet { get; set; }

        public GenericRepository(AppDbContext dbContext)
        {
            DbSet = dbContext.Set<TEntity>();
            _dbContext = dbContext;
        }

        public async Task<Result> AddAsync(TEntity item)
        {
            var result = new Result();

            try
            {
                await DbSet.AddAsync(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = ex.Message;
            }

            return result;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await DbSet.FirstOrDefaultAsync(filter);
        }

        public async Task<Result> UpdateAsync(TEntity item)
        {
            var result = new Result();

            try
            {
                DbSet.Update(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = ex.Message;
            }

            return result;
        }

        public async Task<Result> AddRangeAsync(ICollection<TEntity> items)
        {
            var result = new Result();

            try
            {
                await DbSet.AddRangeAsync(items);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = ex.Message;
            }

            return result;
        }

        public IQueryable<TEntity> GetAllAsNoTracking(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null ? DbSet.AsNoTracking() : DbSet.Where(filter).AsNoTracking();
        }
    }
}
