using Microsoft.EntityFrameworkCore;
using Shared.Authorization;

namespace Infrastructure.Identity
{
    internal partial class UserService
    {
        public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            _ = user ?? throw new NotFoundException(_localizer["identity.usernotfound"]);

            var userRoles = await _userManager.GetRolesAsync(user);
            var permissions = new List<string>();

            foreach (var role in await _roleManager.Roles
                .Where(r => userRoles.Contains(r.Name))
                .ToListAsync(cancellationToken))
            {
                permissions.AddRange(await _db.RoleClaims
                    .Where(rc => rc.RoleId == role.Id && rc.ClaimType == ApiClaims.Permission)
                    .Select(rc => rc.ClaimValue)
                    .ToListAsync(cancellationToken));
            }

            return permissions.Distinct().ToList();
        }

        public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken)
        {
            var permissions = await GetPermissionsAsync(userId, cancellationToken);

            return permissions?.Contains(permission) ?? false;
        }
    }
}
