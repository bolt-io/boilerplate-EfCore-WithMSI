using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.SharedKernel.Abstractions
{
    public interface IQueryableAsync<in TInput, TOutput>
    {
        Task<IEnumerable<TOutput>> QueryAsync(TInput @params = default);
    }
}
