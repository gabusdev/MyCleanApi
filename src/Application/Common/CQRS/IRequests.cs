namespace Application.Common.CQRS;

// This is just a shorthand to make it a bit easier to diferentiate Commands from Queries

/// <summary>
/// Represents a Command no Response
/// </summary>
public interface ICommand : ICommand<Unit> { }
/// <summary>
/// Represents a Command with a Response
/// </summary>
/// <typeparam name="TResponse">Type of the Response</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
/// <summary>
/// Represents a Query with no Response
/// </summary>
public interface IQuery : IQuery<Unit> { }
/// <summary>
/// Represents a Query with Response
/// </summary>
/// <typeparam name="TResponse">Type of the Response</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}


