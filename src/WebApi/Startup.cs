using Application;
using Application.Common.Interfaces;
using Infrastructure;
using WebApi.Services;

namespace WebApi
{
    public static class Startup
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddTransient<IHttpContextService, HttpContextService>();

            services.AddApplication();
            services.AddInfrastructure(config);

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
                c.EnableAnnotations()
            );

            return services;
        }
        public static IApplicationBuilder UseConfigurations(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseInfraestructure();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
