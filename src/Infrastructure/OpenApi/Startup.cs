namespace Infrastructure.OpenApi
{
    public static class Startup
    {
        public static IServiceCollection ConfigureOpenApi(this IServiceCollection services)
        {
            services
                .ConfigureApiVersioning()
                .AddSwaggerGen(c =>
                    c.EnableAnnotations()
                )
                .ConfigureOptions<ConfigureSwaggerOptions>()
                .ConfigureOptions<ConfigureSwaggerUIOptions>();

            return services;
        }

        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
