using Infrastructure.Persistence;
using Infrastructure.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Infrastructure.Middlewares;
using Infrastructure.Auth;

namespace Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration config)
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
                .UseExceptionMiddleware();
        }
    }
}