namespace Application.Common.Events
{
    public interface IGraphQLSubscriptionService
    {
        // Summary:
        //      Sends a Notification to the sub/pub service of GrapqhQL Subscriptions
        Task SendAsync<TTopic, TMessage>(TTopic topic, TMessage message) where TTopic : notnull;
    }
}
