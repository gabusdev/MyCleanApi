using Application.Common.Interfaces;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.AspNetCore.Subscriptions.Messages;
using HotChocolate.Execution;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Interceptors;
public class SocketSubscriptionInterceptor : DefaultSocketSessionInterceptor
{
    private readonly ICurrentUserService current;
    public SocketSubscriptionInterceptor([Service] ICurrentUserService current)
    {
        this.current = current;
    }
    public override ValueTask<ConnectionStatus> OnConnectAsync(
        ISocketConnection connection, InitializeConnectionMessage message,
        CancellationToken cancellationToken)
    {
        var userName = current.Name ?? "Anonymous";
        Log.Information($"GraphQL Socket Subcription started by {userName}");
        return base.OnConnectAsync(connection, message, cancellationToken);
    }

    public override ValueTask OnRequestAsync(ISocketConnection connection,
        IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        return base.OnRequestAsync(connection, requestBuilder,
            cancellationToken);
    }

    public override ValueTask OnCloseAsync(ISocketConnection connection,
        CancellationToken cancellationToken)
    {
        var userName = current.Name ?? "Anonymous";
        Log.Information($"GraphQL Socket Subcription closed by {userName}");
        return base.OnCloseAsync(connection, cancellationToken);
    }
}
