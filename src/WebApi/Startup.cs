using Application;
using Infrastructure;
using Infrastructure.Persistence;

namespace WebApi
{
    public static class Startup
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddApplication();
            services.AddInfrastructure(config);

            return services;
        }
        public static IApplicationBuilder UseConfigurations(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
