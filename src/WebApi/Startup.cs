using Application;
using Application.Common.Interfaces;
using Hangfire;
using Infrastructure;
using WebApi.Services;

namespace WebApi
{
    public static class Startup
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IHttpContextService, HttpContextService>();

            services.AddApplication();
            services.AddInfrastructure(config);

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            
            return services;
        }
        public static IApplicationBuilder UseConfigurations(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseRouting();
            app.UseInfraestructure(config);

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });

            return app;
        }
    }
}
