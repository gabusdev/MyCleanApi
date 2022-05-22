
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Users.EventHandler
{
    public class UserCreatedHandler : EventNotificationHandler<UserCreatedEvent>
    {
        public override Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken)
        {
            Log.Information($"--->Event<--- Created new User: {@event.Item.UserName}");

            return Task.CompletedTask;
        }
    }
}
