using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Infrastructure.ApiVersioning
{
    public static class Startup
    {
        public static IServiceCollection AddMyApiVersioning(this IServiceCollection services, bool headerCheck = false)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                if (headerCheck)
                    opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
            });

            return services;
        }
    }
}
