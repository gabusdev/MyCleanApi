using System.Security.Claims;

namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? Name { get; }

    string GetUserId();

    string? GetUserEmail();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();
}