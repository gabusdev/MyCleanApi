
using MediatR.Pipeline;
using Serilog;

namespace Application.Common.Behaviors;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehaviour(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.GetUserId();
        string userName = string.Empty;

        if (!string.IsNullOrEmpty(userId))
        {
            userName = _currentUserService.Name ?? string.Empty;
        }

        Log.Information("Api Request: {Name} {@UserId} {@UserName}",
            requestName, userId, userName);

        return Task.CompletedTask;
    }
}
