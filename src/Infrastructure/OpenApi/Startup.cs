namespace Infrastructure.OpenApi
{
    public static class Startup
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            services
                .AddSwaggerGen(c =>
                    c.EnableAnnotations()
                )
                .ConfigureOptions<ConfigureSwaggerOptions>()
                .ConfigureOptions<ConfigureSwaggerUiOptions>();

            return services;
        }

        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app, bool development)
        {
            if (development)
                app
                    .UseSwagger()
                    .UseSwaggerUI();

            return app;
        }
    }
}
