using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common;

public class SoftAuditableEntity: SoftAuditableEntity<string> { }

public class SoftAuditableEntity<T> : IHasDomainEvent, IAuditableEntity<T>, ISoftDelete<T>
{
    public T? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public T? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
        
    public DateTime? DeletedOn { get; set; }
    public T? DeletedBy { get; set; }
        
    public List<DomainEvent> DomainEvents { get; set; } = new();

    protected SoftAuditableEntity()
    {
        CreatedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }
}
