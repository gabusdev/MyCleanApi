
using Application.PermaNotifications;
using Microsoft.Extensions.Localization;

namespace Application.Identity.Users.EventHandler
{
    public class UserCreatedHandler : EventNotificationHandler<UserCreatedEvent>
    {
        private readonly IGraphQLSubscriptionService _gqlSender;
        private readonly IUserService _userService;
        private readonly IPermaNotificationService _notificationService;
        private readonly IStringLocalizer<UserCreatedHandler> _localizer;
        public UserCreatedHandler(IGraphQLSubscriptionService gqlSender, IUserService userService,
            IPermaNotificationService notificationService,
            IStringLocalizer<UserCreatedHandler> localizer)
        {
            _gqlSender = gqlSender;
            _userService = userService;
            _localizer = localizer;
            _notificationService = notificationService;
        }
        public override async Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(@event.userId, cancellationToken);

            await _notificationService.SendNotificationToUser(
                _localizer["welcomeMessage"],
                @event.userId);

            await _gqlSender.SendAsync(nameof(UserCreatedEvent), user);
        }
    }
}
