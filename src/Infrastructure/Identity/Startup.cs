using Application.Identity.Roles;
using Application.Identity.Tokens;
using Infrastructure.Identity.Role;
using Infrastructure.Identity.Token;
using Infrastructure.Identity.User;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Identity;

public static class Startup
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config) =>
        services
            .AddTransient<IUserService, UserService>()
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IRoleService, RoleService>()
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 0;
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._+";
                })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .Services;
}