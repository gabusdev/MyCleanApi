using Domain.Common.Contracts;
using Domain.Entities.JoinTables;

namespace Domain.Entities
{
    public class Notification : SoftAuditableEntity, IEntity
    {
        public string Id { get; set; } = null!;
        public string Message { get; set; } = null!;
        public virtual ICollection<UserNotification> UserNotifications { get; set; }

        public Notification()
        {
            UserNotifications = new HashSet<UserNotification>();
        }
    }
}
