using Application.Common.Events;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Common.Services;

public class DomainEventService : IDomainEventService
{
    private readonly IPublisher _mediator;

    public DomainEventService(IPublisher mediator)
    {
        _mediator = mediator;
    }

    public async Task Publish(DomainEvent domainEvent)
    {
        await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(
            typeof(EventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
    }
}
