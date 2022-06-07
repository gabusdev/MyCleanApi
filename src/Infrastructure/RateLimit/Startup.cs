using AspNetCoreRateLimit;


namespace Infrastructure.RateLimit;

internal static class Startup
{
    internal static IServiceCollection AddRateLimit(this IServiceCollection services, IConfiguration conf, bool useDistributedCache = false)
    {
        services.Configure<IpRateLimitOptions>(conf.GetSection("IpRateLimiting"));

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