using Application.Common.Events;
using HotChocolate.Subscriptions;

namespace GraphQL.Services
{
    internal class GraphQLSubscriptionService : IGraphQLSubscriptionService
    {
        private readonly ITopicEventSender _eventSender;
        public GraphQLSubscriptionService([Service] ITopicEventSender eventSender)
        {
            _eventSender = eventSender;
        }

        public async Task SendAsync<TTopic, TMessage>(TTopic topic, TMessage message) where TTopic : notnull
        {
            await _eventSender.SendAsync(topic, message);
        }
    }
}
