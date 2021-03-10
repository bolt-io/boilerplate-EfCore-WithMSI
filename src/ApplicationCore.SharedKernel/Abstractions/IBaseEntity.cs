namespace ApplicationCore.SharedKernel.Abstractions
{
    public interface IBaseEntity<TId> where TId : notnull
    {
        public TId Id { get; set; }
    }
}