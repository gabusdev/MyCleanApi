using Infrastructure.ApiVersioning;
using Infrastructure.Auth;
using Infrastructure.BackgroundJobs;
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
                .AddBackgroundJobs(config)
                .AddPersistence(config)
                .AddMailing(config)
                .AddIdentity(config)
                .AddAuth(config)
                .AddExceptionMiddleware()
                .AddJsonLocalization()
                .AddOpenApi()
                .AddMyApiVersioning()
                .AddCaching()
                .AddRateLimit();
        }
        public static IApplicationBuilder UseInfraestructure(this IApplicationBuilder app, IConfiguration config, bool development)
        {
            return app
                .UseBackgroundJobs(config)
                .UseExceptionMiddleware()
                .UseAuth()
                .UseLocalization()
                .UseOpenApi(development)
                .UseCaching()
                .UseRateLimit();
        }
    }
}