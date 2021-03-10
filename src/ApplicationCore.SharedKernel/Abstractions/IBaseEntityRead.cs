using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.SharedKernel.Abstractions
{
    public interface IBaseEntityRead<out TId>
    {
        public TId Id { get; }
    }
}
