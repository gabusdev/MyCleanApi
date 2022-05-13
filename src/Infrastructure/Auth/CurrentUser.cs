using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Authorization;
using System.Security.Claims;

namespace Infrastructure.Auth;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccesor;
    
    private ClaimsPrincipal? _user;

    public CurrentUser(IHttpContextAccessor httpContextAccesor)
    {
        _httpContextAccesor = httpContextAccesor;
        _user = _httpContextAccesor.HttpContext?.User;
    }

    public string? Name => _user?.Identity?.Name;

    private string _userId = string.Empty;

    public string GetUserId() =>
        IsAuthenticated()
            ? _user?.GetUserId() ?? string.Empty
            : _userId;

    public string? GetUserEmail() =>
        IsAuthenticated()
            ? _user!.GetEmail()
            : string.Empty;

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated is true;

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) is true;

    public IEnumerable<Claim>? GetUserClaims() =>
        _user?.Claims;

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(string userId)
    {
        if (_userId != string.Empty)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(userId))
        {
            _userId = userId;
        }
    }
}