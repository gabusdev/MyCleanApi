namespace Domain.Common
{
    public interface IEntity : IEntity<string> { }
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}
