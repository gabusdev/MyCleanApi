using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserNotifications.Queries
{
    public class NotificationDto
    {
        public string Id { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime? SendedAt { get; set; }
        public DateTime? RecivedAt { get; set; }
    }
}
