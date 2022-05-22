using Application.Common.Events;
using Application.Common.Interfaces;
using Infrastructure.Common.Services;

namespace Infrastructure.Common;

internal static class Startup
{
    public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration config)
    {
        return services
                .AddTransient<ICurrentUserService, CurrentUserService>()
                .AddTransient<IHttpContextService, HttpContextService>()
                .AddTransient<ISerializerService, NewtonSoftService>()
                .AddTransient<IDateTimeService, DateTimeService>()
                .AddScoped<IDomainEventService, DomainEventService>();
    }
}

