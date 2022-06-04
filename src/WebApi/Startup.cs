using Application;
using GraphQL;
using Hangfire;
using Infrastructure;

namespace WebApi
{
    public static class Startup
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();

            services.AddApplication();
            services.AddInfrastructure(config);
            services.AddMyGraphQL();

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            return services;
        }
        public static IApplicationBuilder UseConfigurations(this IApplicationBuilder app, IConfiguration config, bool development)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseInfraestructure(config, development);
            app.UseMyGraphQL();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
                //endpoints.MapGraphQL().RequireAuthorization();
            });

            return app;
        }
    }
}
