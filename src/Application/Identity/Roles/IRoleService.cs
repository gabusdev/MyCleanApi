using Application.Identity.Roles.Commands.CreateUpdateCommand;
using Application.Identity.Roles.Commands.UpdatePermissionsCommand;
using Application.Identity.Users.Queries;

namespace Application.Identity.Roles;

public interface IRoleService
{
    Task<bool> ExistsAsync(string roleName, string? excludeId);

    Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken);
    Task<RoleDto?> GetByIdAsync(string id);
    Task<RoleDto?> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);
    Task<IEnumerable<UserDetailsDto>> GetUsersByIdAsync(string roleId);

    Task<string> CreateOrUpdateAsync(CreateOrUpdateRoleCommand request);
    Task DeleteAsync(string id);

    List<string> GetAllPermissions();
    Task UpdatePermissionsAsync(UpdateRolePermissionsCommand request, CancellationToken cancellationToken);

}