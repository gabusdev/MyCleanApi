using Domain.Common.Contracts;

namespace Application.Common.Events;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
