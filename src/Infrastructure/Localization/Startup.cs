using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Infrastructure.Localization;

internal static class Startup
{
    public static IServiceCollection AddJsonLocalization(this IServiceCollection services)
    {
        return services.AddLocalization()
            .AddSingleton<LocalizerMiddleware>()
            .AddDistributedMemoryCache()
            .AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
    }
    internal static IApplicationBuilder UseLocalization(this IApplicationBuilder app)
    {
        var options = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"))
        };
        app.UseRequestLocalization(options);
        app.UseStaticFiles();
        app.UseMiddleware<LocalizerMiddleware>();

        return app;
    }
}
