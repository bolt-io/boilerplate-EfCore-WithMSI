using System.Threading.Tasks;
using ApplicationCore.SharedKernel.Abstractions;

namespace Infrastructure.EFCore.Data
{
    public class EfRepository<T> : EfReadRepository<T>, IAsyncRepository<T> where T : class, IAggregateRoot
    {
        public EfRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public virtual Task UpdateAsync(T entity)
        {
            _dbContext.Update(entity); // Use update here as setting EntityState.Modified doesn't track value types.
            return _dbContext.SaveChangesAsync();
        }

        public virtual Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return _dbContext.SaveChangesAsync();
        }


    }
}
