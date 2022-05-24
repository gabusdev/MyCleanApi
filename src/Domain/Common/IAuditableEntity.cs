namespace Domain.Common;

public interface IAuditableEntity : IAuditableEntity<string> { }

public interface IAuditableEntity<T>
{
    public T? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public T? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}