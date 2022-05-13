﻿using Infrastructure.Auth.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Auth;
internal static class Startup
{
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config, bool locked = false)
    {
        return services
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
}
