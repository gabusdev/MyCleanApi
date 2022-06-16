namespace Application.Common.Events;

// This Interface is for using for Hot Chocolate Graphql Subscriptions system

public interface IGraphQLSubscriptionService
{      
    /// <summary>
    ///     Sends a Notification to the sub/pub service of GrapqhQL Subscriptions
    /// </summary>
    /// <typeparam name="TTopic">Type of the Trigger</typeparam>
    /// <typeparam name="TMessage">Type of the Response Message</typeparam>
    /// <param name="topic">The Trigger Object</param>
    /// <param name="message">The Response Objcet</param>
    /// <returns></returns>
    Task SendAsync<TTopic, TMessage>(TTopic topic, TMessage message) where TTopic : notnull;
}
