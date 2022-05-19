using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Climapi.Api.AppServices.Caching
{
    public static class Startup
    {
        public static IServiceCollection AddCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddHttpCacheHeaders(expirationOption =>
            {
                expirationOption.MaxAge = 120;
                expirationOption.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
            },
            validationOption =>
            {
                validationOption.MustRevalidate = true;
            }
            );

            return services;
        }
        public static IApplicationBuilder UseCaching(this IApplicationBuilder app)
        {
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            return app;
        }
    }
}
