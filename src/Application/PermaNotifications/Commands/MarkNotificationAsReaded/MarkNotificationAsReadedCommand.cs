using Microsoft.Extensions.Localization;

namespace Application.PermaNotifications.Commands.MarkNotificationAsReaded
{
    public class MarkNotificationAsReadedCommand : ICommand
    {
        public string NotificationID { get; set; } = null!;

        public class MarkNotificationAsReadedCommandHandler : ICommandHandler<MarkNotificationAsReadedCommand, Unit>
        {
            private readonly IPermaNotificationService _notificationService;
            private readonly ICurrentUserService _currentUserService;
            private readonly IStringLocalizer<MarkNotificationAsReadedCommandHandler> _localizer;
            public MarkNotificationAsReadedCommandHandler(IPermaNotificationService notificationService, ICurrentUserService currentUserService,
                IStringLocalizer<MarkNotificationAsReadedCommandHandler> localizer)
            {
                _currentUserService = currentUserService;
                _notificationService = notificationService;
                _localizer = localizer;
            }
            public async Task<Unit> Handle(MarkNotificationAsReadedCommand request, CancellationToken cancellationToken)
            {
                var current = _currentUserService.GetUserId();
                if (current == string.Empty)
                {
                    throw new ForbiddenException(_localizer["identity.notallowed"]);
                }
                
                await _notificationService.SetNotificationAsReaded(current, request.NotificationID);
                
                return Unit.Value;
            }
        }
    }
}
