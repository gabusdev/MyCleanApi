using Application.Common.Interfaces;
using Application.Identity.Roles;
using Application.Identity.Roles.Commands.CreateUpdateCommand;
using Application.Identity.Roles.Commands.UpdatePermissionsCommand;
using Application.Identity.Users.UserQueries;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Shared.Authorization;

namespace Infrastructure.Identity;

internal class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly IStringLocalizer<RoleService> _localizer;

    public RoleService(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext db,
        ICurrentUser currentUser,
        IStringLocalizer<RoleService> stringLocalizer)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _db = db;
        _currentUser = currentUser;
        _localizer = stringLocalizer;
    }

    public async Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken) =>
        (await _roleManager.Roles.ToListAsync(cancellationToken))
            .Adapt<List<RoleDto>>();

    public async Task<int> GetCountAsync(CancellationToken cancellationToken) =>
        await _roleManager.Roles.CountAsync(cancellationToken);

    public async Task<bool> ExistsAsync(string roleName, string? excludeId) =>
        await _roleManager.FindByNameAsync(roleName)
            is ApplicationRole existingRole
            && existingRole.Id != excludeId;

    public async Task<RoleDto> GetByIdAsync(string id) =>
        await _db.Roles.SingleOrDefaultAsync(x => x.Id == id) is { } role
            ? role.Adapt<RoleDto>()
            : throw new NotFoundException(_localizer["entity.notfound", "Role"]);

    public async Task<RoleDto> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken)
    {
        var role = await GetByIdAsync(roleId);

        role.Permissions = await _db.RoleClaims
            .Where(c => c.RoleId == roleId && c.ClaimType == ApiClaims.Permission)
            .Select(c => c.ClaimValue)
            .ToListAsync(cancellationToken);

        return role;
    }

    public async Task CreateOrUpdateAsync(CreateOrUpdateRoleCommand request)
    {
        if (string.IsNullOrEmpty(request.Id))
        {
            // Create a new role.
            var role = new ApplicationRole(request.Name, request.Description, request.SecurityLevel);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                throw new InternalServerException(_localizer["entity.create.failed", "Role"], result.GetErrors());
            }
        }
        else
        {
            // Update an existing role.
            var role = await _roleManager.FindByIdAsync(request.Id);

            _ = role ?? throw new NotFoundException(_localizer["entity.notfound", "Role"]);

            if (ApiRoles.IsDefault(role.Name))
            {
                throw new ConflictException(_localizer["identity.notallowed"]);
            }

            role.Name = request.Name;
            role.NormalizedName = request.Name.ToUpperInvariant();
            role.Description = request.Description;
            role.SecurityLevel = request.SecurityLevel;
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                throw new InternalServerException(_localizer["entity.update.failed", role.Id], result.GetErrors());
            }
        }
    }

    public async Task UpdatePermissionsAsync(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId);
        _ = role ?? throw new NotFoundException(_localizer["entity.notfound", "Role"]);
        if (role.Name == ApiRoles.Admin)
        {
            throw new ConflictException(_localizer["identity.notallowed"]);
        }

        var currentClaims = await _roleManager.GetClaimsAsync(role);

        // Remove permissions that were previously selected
        foreach (var claim in currentClaims.Where(c => !request.Permissions.Any(p => p == c.Value)))
        {
            var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
            if (!removeResult.Succeeded)
            {
                throw new InternalServerException(_localizer["entity.delete.failed", role.Id], removeResult.GetErrors());
            }
        }

        // Add all permissions that were not previously selected
        foreach (string permission in request.Permissions.Where(c => !currentClaims.Any(p => p.Value == c)))
        {
            if (!string.IsNullOrEmpty(permission) && ApiPermissions.All.Any(p => p.Name == permission))
            {
                _db.RoleClaims.Add(new ApplicationRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = ApiClaims.Permission,
                    ClaimValue = permission,
                    CreatedBy = _currentUser.GetUserId().ToString()
                });
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public async Task DeleteAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        _ = role ?? throw new NotFoundException(_localizer["entity.notfound", "Role"]);

        if (ApiRoles.IsDefault(role.Name))
        {
            throw new ConflictException(_localizer["identity.notallowed"]);
        }

        if ((await _userManager.GetUsersInRoleAsync(role.Name)).Count > 0)
        {
            throw new ConflictException(_localizer["identity.notallowed"]);
        }

        await _roleManager.DeleteAsync(role);
    }

    public async Task<double> GetSecurityLevel(string name)
    {
        var role = await _roleManager.FindByNameAsync(name);

        return role.SecurityLevel;
    }

    public List<string> GetAllPermissions()
    {
        var apiPermissions = ApiPermissions.All.Select(ap => ap.Name);
        return apiPermissions.ToList();
    }

    public async Task<IEnumerable<UserDetailsDto>> GetUsersByIdAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        _ = role ?? throw new NotFoundException(_localizer["entity.notfound", "Role"]);
        var users = await _userManager.GetUsersInRoleAsync(role.Name);
        return users.Adapt<IEnumerable<UserDetailsDto>>();
    }
}