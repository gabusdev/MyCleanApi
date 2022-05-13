using Application.Common.Interfaces;
using Infrastructure.Auth.Jwt;
using Infrastructure.Auth.Permissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Auth;
internal static class Startup
{
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config, bool locked = false)
    {
        return services
            .AddCurrentUser()
            .AddPermissions()
            .AddAuthorization(opt =>
            {
                if (locked)
                {
                    opt.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
                }
                /*
                opt.AddPolicy("AdminRights",
                    policy => policy.RequireRole(Enum.GetName(RoleEnum.Admin)!));
                */
            })
            .AddJwtAuth(config);
    }

    internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
        app;

    private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services
            .AddScoped<ICurrentUser, CurrentUser>();

    private static IServiceCollection AddPermissions(this IServiceCollection services) =>
        services
            .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
}

