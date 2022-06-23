using Domain.Entities.JoinTables;

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
