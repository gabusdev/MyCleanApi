using Application;
using Hangfire;
using Infrastructure.ApiVersioning;
using Infrastructure.Auth;
using Infrastructure.BackgroundJobs;
using Infrastructure.Caching;
using Infrastructure.Common;
using Infrastructure.Cors;
using Infrastructure.CurrentUser;
using Infrastructure.FileStorage;
using Infrastructure.Identity;
using Infrastructure.Localization;
using Infrastructure.Mailing;
using Infrastructure.Mappers;
using Infrastructure.Middlewares;
using Infrastructure.OpenApi;
using Infrastructure.Persistence;
using Infrastructure.RateLimit;
using Infrastructure.ResponseCaching;
using Prometheus;

namespace Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddHttpContextAccessor()
                .AddApplication()
                .AddCustomMappers()
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
                .AddFileStorageService()
                .AddCurrentUser()
                .AddEndpointsApiExplorer();
        }
        public static IApplicationBuilder UseInfraestructure(this IApplicationBuilder app, IConfiguration config, bool development)
        {
            return app
                .UseLocalization()
                .UseFileStorage()
                .UseCustomMiddlewares()
                .UseRouting()
                .UseCorsPolicy()
                .UseAuth()
                .UseCurrentUser()
                .UseMyResponseCaching()
                .UseRateLimit()
                .UseBackgroundJobs(config)
                .UseOpenApi(development)
                .UseHttpMetrics()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHangfireDashboard("/dev/jobs");
                    endpoints.MapMetrics("/dev/metrics");
                });
        }
    }
}