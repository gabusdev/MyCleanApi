using Domain.Common.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
