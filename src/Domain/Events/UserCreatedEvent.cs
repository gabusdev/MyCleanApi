using Domain.Common.Contracts;

namespace Domain.Events;

public class UserCreatedEvent : DomainEvent
{
    public UserCreatedEvent(IApplicationUser item)
    {
        Item = item;
    }

    public IApplicationUser Item { get; }
}
