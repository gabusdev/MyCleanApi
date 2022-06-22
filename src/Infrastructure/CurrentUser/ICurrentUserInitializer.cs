using System.Security.Claims;

namespace Infrastructure.CurrentUser;

public interface ICurrentUserInitializer
{
    void SetCurrentUser(ClaimsPrincipal user);

    void SetCurrentUserId(string userId);
}