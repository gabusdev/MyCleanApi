using Domain.Common.Contracts;

namespace Application.Common.Events;

/// <summary>
/// Represents a MediatR Notification with a Domain Event to be Handled
/// </summary>
/// <typeparam name="TEvent">Type of the Event</typeparam>
public class EventNotification<TEvent> : INotification
    where TEvent : DomainEvent
{
    public EventNotification(TEvent @event) => Event = @event;

    public TEvent Event { get; }
}