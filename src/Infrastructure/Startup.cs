using Infrastructure.ApiVersioning;
using Infrastructure.Auth;
using Infrastructure.BackgroundJobs;
using Infrastructure.Caching;
using Infrastructure.Common;
using Infrastructure.Cors;
using Infrastructure.FileStorage;
using Infrastructure.Identity;
using Infrastructure.Localization;
using Infrastructure.Mailing;
using Infrastructure.Middlewares;
using Infrastructure.OpenApi;
using Infrastructure.Persistence;
using Infrastructure.RateLimit;
using Infrastructure.ResponseCaching;

namespace Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddCommon(config)
                .AddCaching(config)
                .AddMyResponseCaching(config)
                .AddBackgroundJobs(config)
                .AddPersistence(config)
                .AddMailing(config)
                .AddIdentity(config)
                .AddAuth(config)
                .AddCustomMiddlewares()
                .AddJsonLocalization()
                .AddOpenApi()
                .AddMyApiVersioning()
                .AddCorsPolicy(config)
                .AddFileStorageService();
        }
        public static IApplicationBuilder UseInfraestructure(this IApplicationBuilder app, IConfiguration config, bool development)
        {
            return app
                .UseCustomMiddlewares()
                .UseCorsPolicy()
                .UseMyResponseCaching()
                .UseRateLimit()
                .UseLocalization()
                .UseBackgroundJobs(config)
                .UseFileStorage()
                .UseAuth()
                .UseOpenApi(development);
        }
    }
}