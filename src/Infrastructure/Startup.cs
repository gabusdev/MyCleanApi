using Infrastructure.Auth;
using Infrastructure.Identity;
using Infrastructure.Middlewares;
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
                .AddPersistence(config)
                .AddIdentity(config)
                .AddAuth(config)
                .AddExceptionMiddleware();
        }
        public static IApplicationBuilder UseInfraestructure(this IApplicationBuilder app)
        {
            return app
                .UseExceptionMiddleware()
                .UseCurrentUser();
        }
    }
}