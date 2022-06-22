namespace Domain.Common.Contracts
{
    public interface IEntity : IEntity<string> { }
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}
