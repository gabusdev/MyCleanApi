using Application.Common.Interfaces;

namespace Infrastructure.CurrentUser;

internal static class Startup
{
    internal static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services
            .AddScoped<CurrentUserMiddleware>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUserService>());
    internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
        app.UseMiddleware<CurrentUserMiddleware>();
}
