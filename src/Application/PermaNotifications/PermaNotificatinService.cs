using Domain.Entities;
using Domain.Entities.JoinTables;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PermaNotifications
{
    internal class PermaNotificatinService : IPermaNotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IStringLocalizer<PermaNotificatinService> _localizer;
        public PermaNotificatinService(IUnitOfWork uow,
            IStringLocalizer<PermaNotificatinService> localizer)
        {
            _uow = uow;
            _localizer = localizer;
        }
        public Task<IEnumerable<PermaNotification>> GetUnreadedNotifications(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> SendNotificationToAll(string message, string? senderId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SendNotificationToUser(string message, string destinationId, string? senderId = null)
        {
            var newNotification = new PermaNotification
            {
                Id = Guid.NewGuid().ToString(),
                Message = message,
            };

            var result = await _uow.Notifications.InsertAsync(newNotification);
            if (!result)
            {
                throw new InternalServerException("Could not create Notification");
            }

            var userNotification = new UserNotification
            {
                Id = Guid.NewGuid().ToString(),
                DestinationUserId = destinationId,
                NotificationId = newNotification.Id,
                OriginUserId = senderId ?? "system",
                Readed = false
            };

            result = await _uow.UserNotifications.InsertAsync(userNotification);
            if (!result)
            {
                throw new InternalServerException("Could not create Notification");
            }

            await _uow.CommitAsync();

            return userNotification.Id;
        }

        public async Task SetNotificationAsReaded(string userId, string notificationId)
        {
            var userUnreadedNotifications =
                    await _uow.UserNotifications.GetAsync(filter:
                        un => un.DestinationUserId == userId
                        && un.Readed == false);

            var notification = userUnreadedNotifications.FirstOrDefault(n => n.Id == notificationId);

            if (notification is not null)
            {
                notification.Readed = true;
                notification.ReadedOn = DateTime.Now;
                _uow.UserNotifications.Update(notification);
                await _uow.CommitAsync();
            }
            else
            {
                throw new NotFoundException(_localizer["entity.notfound", "Notification"]);
            }
        }
    }
}
