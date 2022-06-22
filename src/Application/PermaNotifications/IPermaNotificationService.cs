using Domain.Entities;
using Domain.Entities.JoinTables;
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
        Task SendNotificationToAll(string message, string? senderId = null);
        Task<IEnumerable<UserNotification>> GetNotifications(string userId, bool readed = false);
        Task SetNotificationAsReaded(string userId, string notificationId);
    }
}
