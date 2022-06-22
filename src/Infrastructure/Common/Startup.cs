using Application.Common.Events;
using Application.Common.Exporters;
using Application.Common.Interfaces;
using Infrastructure.Common.Export;
using Infrastructure.Common.Services;

namespace Infrastructure.Common;

internal static class Startup
{
    public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration config)
    {
        return services
                .AddTransient<IHttpContextService, HttpContextService>()
                .AddTransient<ISerializerService, NewtonSoftService>()
                .AddTransient<IDateTimeService, DateTimeService>()
                .AddScoped<IDomainEventService, DomainEventService>()
                .AddTransient<IExcelWriter, ExcelWriter>();
    }
}

