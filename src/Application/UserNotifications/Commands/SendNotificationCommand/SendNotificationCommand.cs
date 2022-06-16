using Domain.Entities;
using Domain.Entities.JoinTables;

namespace Application.UserNotifications.Commands.SendUserNotificationCommand
{
    public class SendNotificationCommand : ICommand<string>
    {
        public string Message { get; set; } = null!;
        public string DestinationUserId { get; set; } = null!;

        public class SendNotificationCommandHandler : ICommandHandler<SendNotificationCommand, string>
        {
            private readonly IUnitOfWork _uow;
            private readonly ICurrentUserService _currentUserService;

            public SendNotificationCommandHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
            {
                _currentUserService = currentUserService;
                _uow = uow;
            }
            public async Task<string> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
            {
                var currentUserId = _currentUserService.GetUserId();
                if (currentUserId == null)
                    throw new ForbiddenException("Dont Have Permissions to do this action");

                var newNotification = new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = request.Message,
                };

                var result = await _uow.Notifications.InsertAsync(newNotification);
                if (!result)
                    throw new InternalServerException("Could not create Notification");

                var userNotification = new UserNotification
                {
                    Id = Guid.NewGuid().ToString(),
                    DestinationUserId = request.DestinationUserId,
                    NotificationId = newNotification.Id,
                    OriginUserId = currentUserId,
                    Readed = false
                };

                result = await _uow.UserNotifications.InsertAsync(userNotification);
                if (!result)
                    throw new InternalServerException("Could not create Notification");

                await _uow.CommitAsync();
                return userNotification.Id;
            }
        }
    }
}
