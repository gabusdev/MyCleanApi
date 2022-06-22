using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PermaNotifications
{
    public interface IPermaNotificationService
    {
        Task<string> SendNotificationToUser(string message, string destinationId, string? senderId = null);
        Task<string> SendNotificationToAll(string message, string? senderId = null);
        Task<IEnumerable<PermaNotification>> GetUnreadedNotifications(string userId);
        Task SetNotificationAsReaded(string userId, string notificationId);
    }
}
