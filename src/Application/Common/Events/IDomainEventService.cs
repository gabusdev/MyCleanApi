using Domain.Common.Contracts;

namespace Application.Common.Events;

public interface IDomainEventService
{
    /// <summary>
    /// Publishes a Domain Event to be Handled by its specific Handler
    /// </summary>
    /// <param name="domainEvent">The Event to be published</param>
    /// <returns></returns>
    Task Publish(DomainEvent domainEvent);
}
