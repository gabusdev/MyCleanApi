using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
