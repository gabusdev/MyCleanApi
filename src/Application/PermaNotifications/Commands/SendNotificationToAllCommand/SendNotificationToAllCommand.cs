﻿using Application.Identity.Users;
using Domain.Entities;
using Domain.Entities.JoinTables;

namespace Application.PermaNotifications.Commands.SendNotificationToAllCommand
{
    public class SendNotificationToAllCommand : ICommand<string>
    {
        public string Message { get; set; } = null!;

        public class SendNotificationToAllCommandHandler : ICommandHandler<SendNotificationToAllCommand, string>
        {
            private readonly IUnitOfWork _uow;
            private readonly ICurrentUserService _currentUserService;
            private readonly IUserService _userService;

            public SendNotificationToAllCommandHandler(IUnitOfWork uow, ICurrentUserService currentUserService, IUserService userService)
            {
                _currentUserService = currentUserService;
                _uow = uow;
                _userService = userService;
            }
            public async Task<string> Handle(SendNotificationToAllCommand request, CancellationToken cancellationToken)
            {
                var currentUserId = _currentUserService.GetUserId();
                if (currentUserId == null)
                {
                    throw new ForbiddenException("Dont Have Permissions to do this action");
                }

                var newNotification = new PermaNotification
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = request.Message,
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
                        OriginUserId = currentUserId,
                        Readed = false
                    };

                    result = await _uow.UserNotifications.InsertAsync(userNotification);
                    if (!result)
                    {
                        throw new InternalServerException("Could not create Notification");
                    }
                }

                await _uow.CommitAsync();
                return "";
            }
        }
    }
}
