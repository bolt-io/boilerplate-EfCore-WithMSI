using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApplicationCore.SharedKernel.Abstractions;

namespace Infrastructure.EFCore.Data
{
    public class EfReadRepository<T> : IAsyncReadRepository<T> where T : class, IAggregateRoot
    {
        protected readonly DbContext _dbContext;

        public EfReadRepository(DbContext dbContext) => _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public virtual async Task<IReadOnlyList<T>> ListAllAsync(int skip = 0, int take = 100) => await _dbContext.Set<T>().AsNoTracking().IncludeNavigationPropertiesAsync(_dbContext).Skip(skip).Take(take).ToListAsync();

        public virtual Task<T?> GetByIdAsync(string id)
        {
            return GetByPrimaryKeyValuesAsync(id);
        }

        public virtual async Task<T?> GetByPrimaryKeyValuesAsync(params object[] keyValues)
        {
            var result = await _dbContext.Set<T>().FindAsync(keyValues);
            result = await result.LoadNavigationPropertiesAsync(_dbContext);

            if(!_dbContext.UseChangeTracker && result != null)
            { // Detach and do not track entity if change tracker is disabled.
                _dbContext.Entry(result).State = EntityState.Detached;
            }

            return result;
        }

        public virtual async Task<IReadOnlyList<T>> ListWhereAsync(Expression<Func<T, bool>> criteria, int skip = 0, int take = 100)
        {
            return await _dbContext.Set<T>().AsNoTracking().Skip(skip).Take(take).IncludeNavigationPropertiesAsync(_dbContext).Where(criteria).ToListAsyncSafe();
        }
    }
}
