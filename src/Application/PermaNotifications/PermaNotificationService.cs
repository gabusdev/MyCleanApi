using Application.Identity.Users;
using Domain.Entities;
using Domain.Entities.JoinTables;
using Microsoft.Extensions.Localization;

namespace Application.PermaNotifications
{
    internal class PermaNotificationService : IPermaNotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<PermaNotificationService> _localizer;
        public PermaNotificationService(IUnitOfWork uow,
            IStringLocalizer<PermaNotificationService> localizer,
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

            var newNotification = CreatePermaNotification(message);
            
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
            var newNotification = CreatePermaNotification(message);

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

        private static PermaNotification CreatePermaNotification(string message)
        {
            
            var not =  new PermaNotification
            {
                Id = Guid.NewGuid().ToString(),
                Message = message,
            };

            not.DomainEvents.Add(new NotificationCreatedEvent(not));

            return not;
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
