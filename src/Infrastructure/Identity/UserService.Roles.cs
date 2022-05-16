using Application.Identity.Users.UserCommands.SetRoles;
using Application.Identity.Users.UserQueries;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

internal partial class UserService
{
    public async Task<string> AssignRolesAsync(string userId, SetUserRolesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException("User Not Found.");

        foreach (var userRole in request.UserRoles)
        {
            // Check if Role Exists
            if (await _roleManager.FindByNameAsync(userRole.RoleName) is not null)
            {
                if (userRole.Enabled)
                {
                    if (!await _userManager.IsInRoleAsync(user, userRole.RoleName))
                    {
                        await _userManager.AddToRoleAsync(user, userRole.RoleName);
                    }
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole.RoleName);
                }
            }
        }

        return "User Roles Updated Successfully.";
    }

    public async Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken)
    {
        var userRoles = new List<UserRoleDto>();

        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException("User Not Found.");

        var roles = await _roleManager.Roles.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var role in roles)
        {
            userRoles.Add(new UserRoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Description = role.Description,
                Enabled = await _userManager.IsInRoleAsync(user, role.Name)
            });
        }

        return userRoles;
    }

    public async Task<bool> HasRoleAsync(string userId, string role, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException("User Not Found.");

        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<double> GetSecurityLevel(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException("User Not Found.");

        var roleNames = await _userManager.GetRolesAsync(user);
        var roles = new List<ApplicationRole>();

        foreach (var role in roleNames)
        {
            roles.Add(await _roleManager.FindByNameAsync(role));
        }
        var level = roles.Count > 0 ? roles.Min(r => r.SecurityLevel) : double.PositiveInfinity;

        return level;
    }
}

