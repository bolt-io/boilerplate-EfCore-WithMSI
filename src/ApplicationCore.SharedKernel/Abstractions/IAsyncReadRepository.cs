using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.SharedKernel.Abstractions
{
    public interface IAsyncReadRepository<T> where T : class, IAggregateRoot
    {
        Task<T?> GetByIdAsync(string id);
        Task<T?> GetByPrimaryKeyValuesAsync(params object[] keyValues);
        Task<IReadOnlyList<T>> ListAllAsync(int skip = 0, int take = 100);
        Task<IReadOnlyList<T>> ListWhereAsync(Expression<Func<T, bool>> criteria, int skip = 0, int take = 100);
    }
}
