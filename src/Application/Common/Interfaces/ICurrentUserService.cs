using System.Security.Claims;

namespace Application.Common.Interfaces;

// Interface To Access Data from Current User

public interface ICurrentUserService
{
    string? Name { get; }

    string GetUserId();

    string? GetUserEmail();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();
}