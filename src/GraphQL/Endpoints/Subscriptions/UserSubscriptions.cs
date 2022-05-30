using Application.Identity.Users.UserQueries;
using Domain.Events;

namespace GraphQL.Endpoints.Subscriptions
{
    [ExtendObjectType(OperationTypeNames.Subscription)]
    public class UserSubscriptions
    {
        [Subscribe]
        [Topic(nameof(UserCreatedEvent))]
        public UserDetailsDto BookAdded([EventMessage] UserDetailsDto user) => user;
    }
}
