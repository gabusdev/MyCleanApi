using AspNetCoreRateLimit;


namespace Infrastructure.RateLimit;

internal static class Startup
{
    internal static IServiceCollection AddRateLimit(this IServiceCollection services, bool useDistributedCache)
    {
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

        List<string> endpointWhiteList = new()
        {
            "*:/graphql/*"
        };
        services.Configure<IpRateLimitOptions>(o =>
        {
            o.GeneralRules = rules;
            o.EndpointWhitelist = endpointWhiteList;
        });

        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        if (useDistributedCache)
        {
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
        }
        else
        {
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        }

        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        return services;
    }
    internal static IApplicationBuilder UseRateLimit(this IApplicationBuilder app)
    {
        return app.UseIpRateLimiting();
    }
}