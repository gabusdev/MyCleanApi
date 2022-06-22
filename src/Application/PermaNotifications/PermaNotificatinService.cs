using Application.Identity.Users;
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
        private readonly IUserService _userService;
        private readonly IStringLocalizer<PermaNotificatinService> _localizer;
        public PermaNotificatinService(IUnitOfWork uow,
            IStringLocalizer<PermaNotificatinService> localizer,
            IUserService userService)
        {
            _uow = uow;
            _localizer = localizer;
            _userService = userService;
        }
        public async Task<IEnumerable<UserNotification>> GetNotifications(string userId, bool readed = false)
        {
            var userNotifications = await _uow.UserNotifications.GetOrderedByAsync(
                un => un.DestinationUserId == userId && un.Readed == readed,
                un => un.Notification.CreatedOn, true, "Notification");

            return userNotifications;
        }

        public async Task SendNotificationToAll(string message, string? senderId = null)
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

            var users = await _userService.GetAsync(new CancellationToken());

            foreach (var user in users)
            {
                var userNotification = new UserNotification
                {
                    Id = Guid.NewGuid().ToString(),
                    DestinationUserId = user.Id!,
                    NotificationId = newNotification.Id,
                    OriginUserId = senderId,
                    Readed = false
                };

                result = await _uow.UserNotifications.InsertAsync(userNotification);
                if (!result)
                {
                    throw new InternalServerException("Could not create Notification");
                }
            }

            await _uow.CommitAsync();
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
                OriginUserId = senderId,
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
