using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.OpenApi
{
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IConfiguration _config;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider,
            IConfiguration config)
        {
            _provider = provider;
            _config = config;
        }

        public void Configure(SwaggerGenOptions options)
        {
            var swaggerConfig = GetConfig();

            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateApiDescription(description, swaggerConfig));
            }

            if (swaggerConfig.Auth)
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer A23rt...\""
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            }
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private OpenApiInfo CreateApiDescription(ApiVersionDescription apiDescription, SwaggerSettings conf)
        {

            var apiInfo = new OpenApiInfo
            {
                Description = conf.Description,
                Title = conf.Title,
                Version = apiDescription.ApiVersion.ToString(),
                Contact = new OpenApiContact()
                {
                    Name = conf.Contact_Name,
                    Url = new Uri(conf.Contact_Url)
                }
            };
            if (apiDescription.IsDeprecated)
            {
                apiInfo.Description += " This API version has been deprecated.";
            }


            return apiInfo;
        }

        private SwaggerSettings GetConfig()
        {
            return new SwaggerSettings
            {
                Title = _config["OpenApi:Title"] ?? "Default Title",
                Description = _config["OpenApi:Description"] ?? "Default API Demo.",
                Version = _config["OpenApi:Version"] ?? "v1",
                Contact_Name = _config["OpenApi:Contact_Name"] ?? "Default Name",
                Contact_Url = _config["OpenApi:Contact_Url"] ?? "https://github.com/gabusdev",
                Auth = Convert.ToBoolean(_config["OpenApi:Auth"])
            };
        }
    }
}
