using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.RateLimit;

internal static class Startup
{
    internal static IServiceCollection AddRateLimit(this IServiceCollection services)
    {
        services.AddMemoryCache();

        var rules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "*",
                Limit = 5,
                Period = "10s"
            },
            new RateLimitRule
            {
                Endpoint = "*",
                Limit = 20,
                Period = "1m"
            }
        };
        services.Configure<IpRateLimitOptions>(o =>
        {
            o.GeneralRules = rules;
        });

        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        services.AddHttpContextAccessor();

        return services;
    }
    internal static IApplicationBuilder UseRateLimit(this IApplicationBuilder app)
    {
        return app.UseIpRateLimiting();
    }
}