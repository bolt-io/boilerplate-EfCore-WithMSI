using ApplicationCore.SharedKernel.Abstractions;
using System;

namespace ApplicationCore.SharedKernel
{
    public abstract class BaseEntity<TId> : IBaseEntity<TId> where TId : notnull
    {
        public TId Id { get; set; } = default!;
    }
}
