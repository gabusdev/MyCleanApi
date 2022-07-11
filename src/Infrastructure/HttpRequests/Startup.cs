using Application.Common.HttpRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HttpRequests;

internal static class Startup
{
    internal static IServiceCollection AddCustomMiddlewares(this IServiceCollection services) =>
        services
            .AddScoped<IHttpFetcher, Fetcher>();
}
