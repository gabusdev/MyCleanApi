
using Application.PermaNotifications.Commands.SendNotificationCommand;
using Microsoft.Extensions.Localization;
using Serilog;

namespace Application.Identity.Users.EventHandler
{
    public class UserCreatedHandler : EventNotificationHandler<UserCreatedEvent>
    {
        private readonly IGraphQLSubscriptionService _gqlSender;
        private readonly IUserService _userService;
        private readonly ISender _mediator;
        private readonly IStringLocalizer<UserCreatedHandler> _localizer;
        public UserCreatedHandler(IGraphQLSubscriptionService gqlSender, IUserService userService, ISender mediator,
            IStringLocalizer<UserCreatedHandler> localizer)
        {
            _gqlSender = gqlSender;
            _userService = userService;
            _mediator = mediator;
            _localizer = localizer;
        }
        public override async Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(@event.userId, cancellationToken);

            await _mediator.Send(new SendNotificationCommand {
                DestinationUserId = @event.userId,
                Message = _localizer["welcomeMessage"]
            });
            await _gqlSender.SendAsync(nameof(UserCreatedEvent), user);
        }
    }
}
