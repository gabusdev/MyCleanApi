using Infrastructure.Auth.Jwt;
using Infrastructure.Auth.Permissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Auth;
internal static class Startup
{
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        return services
            .Configure<SecuritySettings>(config.GetSection(nameof(SecuritySettings)))
            .AddPermissions()
            .AddAuthorization(opt =>
            {
                /*
                opt.AddPolicy("AdminRights",
                    policy => policy.RequireRole(Enum.GetName(RoleEnum.Admin)!));
                */
            })
            .AddJwtAuth(config);
    }

    private static IServiceCollection AddPermissions(this IServiceCollection services) =>
        services
            .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

    internal static IApplicationBuilder UseAuth(this IApplicationBuilder app) =>
        app
            .UseAuthentication()
            .UseAuthorization();

}

