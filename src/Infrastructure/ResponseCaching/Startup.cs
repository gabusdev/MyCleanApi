using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.ResponseCaching
{
    public static class Startup
    {
        public static IServiceCollection AddMyResponseCaching(this IServiceCollection services, IConfiguration config)
        {
            services.AddResponseCaching(o =>
            {
                o.MaximumBodySize = 2048;
                o.SizeLimit = 150_000;
            });
            /*services.AddHttpCacheHeaders(expirationOption =>
            {
                expirationOption.MaxAge = 15;
                expirationOption.CacheLocation = CacheLocation.Private;
            },
            validationOption =>
            {
                validationOption.MustRevalidate = true;
            }
            );*/
            /*services.AddHttpCacheHeaders(o =>
            {
                o.NoStore = true;
            },
            v =>
            {
                v.MustRevalidate = true;
            }
            );*/
            //services.AddScoped<ApiResponseCacheAttribute>();
            return services;
        }
        public static IApplicationBuilder UseMyResponseCaching(this IApplicationBuilder app)
        {
            app.UseResponseCaching();
            //app.UseHttpCacheHeaders();

            return app;
        }
    }
}
