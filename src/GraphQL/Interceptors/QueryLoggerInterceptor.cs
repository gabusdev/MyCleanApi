using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace GraphQL.Interceptors;

public class QueryLoggerInterceptor : DefaultHttpRequestInterceptor
{
    public override ValueTask OnCreateAsync(HttpContext context,
        IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        // To Add a Global State Property
        // requestBuilder.SetProperty("foo", "bar");
        // It can be accessed by: [GlobalState("foo")] string foo

        // If there is an error and most stop execution: throw new GraphQLException()

        // Must allways be executed

        Log.Information($"GraphQL Query executed: {requestBuilder.GetType()}");
        return base.OnCreateAsync(context, requestExecutor, requestBuilder,
            cancellationToken);
    }
}
