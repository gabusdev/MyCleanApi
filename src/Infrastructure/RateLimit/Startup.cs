using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using Infrastructure.Caching;
using Microsoft.AspNetCore.HttpOverrides;
using StackExchange.Redis;

namespace Infrastructure.RateLimit;

internal static class Startup
{
    internal static IServiceCollection AddRateLimit(this IServiceCollection services, IConfiguration conf, CacheSettings cacheSettings)
    {
        services.Configure<IpRateLimitOptions>(conf.GetSection("IpRateLimiting"));

        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        if (cacheSettings.UseDistributedCache)
        {
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();

            if (cacheSettings.PreferRedis)
            {
                services.AddSingleton<IConnectionMultiplexer>(provider =>
                    ConnectionMultiplexer.Connect(new ConfigurationOptions()
                    {
                        AbortOnConnectFail = true,
                        EndPoints = { cacheSettings.RedisURL }
                    })
                    );
                services.AddRedisRateLimiting();
            }
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
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        return app.UseIpRateLimiting();
    }
}