using Domain.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.JoinTables
{
    public class UserNotification: IEntity
    {
        public string Id { get; set; } = null!;
        public string DestinationUserId { get; set; } = null!;
        public string OriginUserId { get; set; } = null!;
        public string NotificationId { get; set; } = null!;
        public bool Readed { get; set; }
        public DateTime? ReadedOn { get; set; }

        public IApplicationUser DestinationUser { get; set; } = null!;
        public IApplicationUser OriginUser { get; set; } = null!;
        public Notification Notification { get; set; } = null!;
    }
}
