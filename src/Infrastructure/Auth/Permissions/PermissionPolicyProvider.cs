using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Shared.Authorization;

namespace Infrastructure.Auth.Permissions;

internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
    private IHttpContextService _contextService;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options, IHttpContextService contextService)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        _contextService = contextService;
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(ApiClaims.Permission, StringComparison.OrdinalIgnoreCase))
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(policyName));
            return Task.FromResult<AuthorizationPolicy?>(policy.Build());
        }

        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        var path = _contextService.GetPath().ToLower();
        if (path.StartsWith("/api") || path.StartsWith("/files"))
            return Task.FromResult<AuthorizationPolicy?>(new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
        else 
            return Task.FromResult<AuthorizationPolicy?>(null);
    }
        
}