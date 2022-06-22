using GraphQL;
using Infrastructure;

namespace WebApi
{
    public static class Startup
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddInfrastructure(config);
            services.AddMyGraphQL();

            return services;
        }
        public static IApplicationBuilder UseConfigurations(this IApplicationBuilder app, IConfiguration config, bool development)
        {
            app.UseInfraestructure(config, development);
            app.UseMyGraphQL();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL("/api/graphql")/*.RequireAuthorization()*/;
            });

            return app;
        }
    }
}
