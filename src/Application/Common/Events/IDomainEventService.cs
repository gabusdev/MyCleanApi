using Domain.Common;

namespace Application.Common.Events;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
