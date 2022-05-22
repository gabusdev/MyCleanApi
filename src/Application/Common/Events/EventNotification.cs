using Domain.Common;

namespace Application.Common.Events;

public class EventNotification<TEvent> : INotification
    where TEvent : DomainEvent
{
    public EventNotification(TEvent @event) => Event = @event;

    public TEvent Event { get; }
}