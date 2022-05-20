using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Infrastructure.OpenApi
{
    public static class ApiVersioningExtension
    {
        public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services, bool headerCheck = false)
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
                o.GroupNameFormat = "'v' VVV";
            });

            return services;
        }
    }
}
