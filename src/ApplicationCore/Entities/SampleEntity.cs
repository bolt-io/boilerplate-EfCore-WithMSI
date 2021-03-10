
using System;
using ApplicationCore.SharedKernel;
using ApplicationCore.SharedKernel.Abstractions;

namespace ApplicationCore.Entities
{
    public class SampleEntity : BaseEntity<Guid>, IAggregateRoot
    {
        public string? ExampleProperty { get; set; }
    }
}