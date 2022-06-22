using Microsoft.Extensions.Localization;

namespace Application.PermaNotifications.Commands.MarkNotificationAsReaded
{
    public class MarkNotificationAsReadedCommand : ICommand
    {
        public string NotificationID { get; set; } = null!;

        public class MarkNotificationAsReadedCommandHandler : ICommandHandler<MarkNotificationAsReadedCommand, Unit>
        {
            private readonly IUnitOfWork _uow;
            private readonly ICurrentUserService _currentUserService;
            private readonly IStringLocalizer<MarkNotificationAsReadedCommandHandler> _localizer;
            public MarkNotificationAsReadedCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService,
                IStringLocalizer<MarkNotificationAsReadedCommandHandler> localizer)
            {
                _currentUserService = currentUserService;
                _uow = unitOfWork;
                _localizer = localizer;
            }
            public async Task<Unit> Handle(MarkNotificationAsReadedCommand request, CancellationToken cancellationToken)
            {
                var current = _currentUserService.GetUserId();
                if (current == string.Empty)
                {
                    throw new ForbiddenException(_localizer["identity.notallowed"]);
                }

                var userUnreadedNotifications =
                    await _uow.UserNotifications.GetAsync(filter:
                        un => un.DestinationUserId == current
                        && un.Readed == false);

                var notification = userUnreadedNotifications.FirstOrDefault(n => n.Id == request.NotificationID);

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

                return Unit.Value;
            }
        }
    }
}
