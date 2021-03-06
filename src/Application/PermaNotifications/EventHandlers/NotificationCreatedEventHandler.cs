using Microsoft.Extensions.Localization;
using Shared.Notifications;

namespace Application.PermaNotifications.EventHandlers
{
    internal class NotificationCreatedEventHandler : EventNotificationHandler<NotificationCreatedEvent>
    {
        private readonly INotificationSender _sender;
        private readonly IStringLocalizer<NotificationCreatedEventHandler> _localizer;
        public NotificationCreatedEventHandler(INotificationSender sender,
            IStringLocalizer<NotificationCreatedEventHandler> localizer)
        {
            _sender = sender;
            _localizer = localizer;
        }
        public override async Task Handle(NotificationCreatedEvent @event, CancellationToken cancellationToken)
        {
            var users = @event.Notification.UserNotifications.Select(un => un.DestinationUserId).ToList();
            var notification = new BasicNotification
            {
                Message = _localizer["notifications.new"],
                Label = BasicNotification.LabelType.Information
            };
            await _sender.SendToUsersAsync(notification, users, cancellationToken);
        }
    }
}
