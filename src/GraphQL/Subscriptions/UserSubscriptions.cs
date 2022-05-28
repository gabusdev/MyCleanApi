using Application.Identity.Users.UserQueries;
using Domain.Entities;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Subscriptions
{
    [ExtendObjectType(OperationTypeNames.Subscription)]
    public class UserSubscriptions
    {
        [Subscribe]
        [Topic(nameof(UserCreatedEvent))]
        public UserDetailsDto BookAdded([EventMessage] UserDetailsDto user) => user;
    }
}
