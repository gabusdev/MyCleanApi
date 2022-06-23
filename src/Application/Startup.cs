using Application.Common.Behaviors;
using Application.PermaNotifications;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services
                .AddMediatR(assembly)
                .AddValidatorsFromAssembly(assembly)
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
                .AddScoped<IPermaNotificationService, PermaNotificatinService>();

            return services;
        }
    }
}