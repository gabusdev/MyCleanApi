using Infrastructure.Middlewares.ExceptionHandler;

namespace Infrastructure.Middlewares;

internal static class Startup
{
    internal static IServiceCollection AddCustomMiddlewares(this IServiceCollection services) =>
        services
            .AddScoped<ExceptionMiddleware>();
    internal static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app) =>
        app
            .UseMiddleware<ExceptionMiddleware>();
}