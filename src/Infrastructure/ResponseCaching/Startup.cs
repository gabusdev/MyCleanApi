namespace Infrastructure.ResponseCaching
{
    public static class Startup
    {
        public static IServiceCollection AddMyResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching(o =>
            {
                o.MaximumBodySize = 2048;
                o.SizeLimit = 150_000;
            });
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
        public static IApplicationBuilder UseMyResponseCaching(this IApplicationBuilder app)
        {
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            return app;
        }
    }
}
