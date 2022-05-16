using Application.Identity.Roles.Commands.CreateUpdateCommand;
using Application.Identity.Roles.Commands.UpdatePermissionsCommand;

namespace Application.Identity.Roles;

public interface IRoleService
{
    Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string roleName, string? excludeId);

    Task<RoleDto> GetByIdAsync(string id);

    Task<RoleDto> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken);

    Task CreateOrUpdateAsync(CreateOrUpdateRoleCommand request);

    List<string> GetAllPermissions();
    Task UpdatePermissionsAsync(UpdateRolePermissionsCommand request, CancellationToken cancellationToken);

    Task DeleteAsync(string id);
}