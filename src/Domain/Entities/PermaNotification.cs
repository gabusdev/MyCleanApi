using Domain.Common.Contracts;
using Domain.Entities.JoinTables;

namespace Domain.Entities
{
    public class PermaNotification : SoftAuditableEntity, IEntity, IAuditableEntity
    {
        public string Id { get; set; } = null!;
        public string Message { get; set; } = null!;
        public virtual ICollection<UserNotification> UserNotifications { get; set; }

        public PermaNotification()
        {
            UserNotifications = new HashSet<UserNotification>();
        }
    }
}
