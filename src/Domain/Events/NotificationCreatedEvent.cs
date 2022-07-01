using Domain.Common.Contracts;

namespace Domain.Events
{
    public class NotificationCreatedEvent : DomainEvent
    {
        public NotificationCreatedEvent(PermaNotification item)
        {
            Notification = item;
        }

        public PermaNotification Notification { get; }
    }
}
