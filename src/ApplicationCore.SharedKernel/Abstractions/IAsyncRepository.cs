using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.SharedKernel.Abstractions
{
    public interface IAsyncRepository<T> : IAsyncReadRepository<T> where T : class, IAggregateRoot
    {
        Task<T> AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
