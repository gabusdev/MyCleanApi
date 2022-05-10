using Infrastructure.Persistence;
using Infrastructure.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddPersistence(config)
                .AddIdentity(config);
        }
    }
}