namespace Domain.Common.Contracts;

public interface ISoftDelete : ISoftDelete<string> { }
public interface ISoftDelete<T>
{
    DateTime? DeletedOn { get; set; }
    T? DeletedBy { get; set; }
}