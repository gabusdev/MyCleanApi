using Domain.Common.Contracts;

namespace Domain.Events;

public class UserCreatedEvent : DomainEvent
{
    public UserCreatedEvent(string item)
    {
        userId = item;
    }

    public string userId { get; }
}
