namespace Application.Common.CQRS;

public interface ICommand : ICommand<Unit> { }
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

public interface IQuery : IQuery<Unit> { }
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}


