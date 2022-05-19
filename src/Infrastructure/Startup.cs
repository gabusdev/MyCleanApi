using Climapi.Api.AppServices.Caching;
using Infrastructure.Auth;
using Infrastructure.BackgroundJobs;
using Infrastructure.Identity;
using Infrastructure.Localization;
using Infrastructure.Mailing;
using Infrastructure.Middlewares;
using Infrastructure.OpenApi;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .ConfigureOpenApi()
                .AddCaching();
        }
        public static IApplicationBuilder UseInfraestructure(this IApplicationBuilder app, IConfiguration config)
        {
            return app
                .UseBackgroundJobs(config)
                .UseExceptionMiddleware()
                .UseAuth()
                .UseLocalization()
                .UseOpenApi()
                .UseCaching();
        }
    }
}