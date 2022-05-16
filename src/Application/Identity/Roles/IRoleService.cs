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

    Task<string> CreateOrUpdateAsync(CreateOrUpdateRoleCommand request);

    List<string> GetAllPermissions();
    Task<string> UpdatePermissionsAsync(UpdateRolePermissionsCommand request, CancellationToken cancellationToken);

    Task<string> DeleteAsync(string id);
}