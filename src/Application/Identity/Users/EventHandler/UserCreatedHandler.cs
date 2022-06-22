
using Serilog;

namespace Application.Identity.Users.EventHandler
{
    public class UserCreatedHandler : EventNotificationHandler<UserCreatedEvent>
    {
        private readonly IGraphQLSubscriptionService _gqlSender;
        private readonly IUserService _userService;
        public UserCreatedHandler(IGraphQLSubscriptionService gqlSender, IUserService userService)
        {
            _gqlSender = gqlSender;
            _userService = userService;
        }
        public override async Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken)
        {
            Log.Information($"--->Event<--- Created new User: {@event.Item.UserName}");

            var user = await _userService.GetByIdAsync(@event.Item.Id, cancellationToken);
            await _gqlSender.SendAsync(nameof(UserCreatedEvent), user);
        }
    }
}
