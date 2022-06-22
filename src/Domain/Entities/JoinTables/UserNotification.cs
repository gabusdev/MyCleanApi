using Domain.Common.Contracts;

namespace Domain.Entities.JoinTables
{
    public class UserNotification : IEntity
    {
        public string Id { get; set; } = null!;
        public string DestinationUserId { get; set; } = null!;
        public string OriginUserId { get; set; } = "admin";
        public string NotificationId { get; set; } = null!;
        public bool Readed { get; set; }
        public DateTime? ReadedOn { get; set; }

        public IApplicationUser DestinationUser { get; set; } = null!;
        public IApplicationUser OriginUser { get; set; } = null!;
        public PermaNotification Notification { get; set; } = null!;
    }
}
