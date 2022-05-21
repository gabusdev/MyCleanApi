using Application.Identity.Roles;
using Application.Identity.Roles.Commands.CreateUpdateCommand;
using Application.Identity.Roles.Commands.UpdatePermissionsCommand;
using Application.Identity.Roles.Queries.GetAllRolesQuery;
using Application.Identity.Roles.Queries.GetPermissionsQuery;
using Application.Identity.Roles.Queries.GetRoleWithPermissionsQuery;
using Application.Identity.Roles.Queries.GetUsersByRoleQuery;
using Application.Identity.Users.UserQueries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1.Identity
{
    public class RoleController : VersionNeutralApiController
    {
        [HttpGet]
        [MustHavePermission(ApiAction.View, ApiResource.Roles)]
        [SwaggerOperation("Get Roles", "Returns a List with All Roles.")]
        public async Task<List<RoleDto>> GetAllRoles()
        {
            return await Mediator.Send(new GetAllRolesQuery());
        }

        [HttpGet("{id}")]
        [MustHavePermission(ApiAction.View, ApiResource.Roles)]
        [MustHavePermission(ApiAction.Search, ApiResource.Roles)]
        [SwaggerOperation("Get Role Info", "Returns the Info About a specified role with its Permissions.")]
        public async Task<RoleDto> GetRoles(string id)
        {
            return await Mediator.Send(new GetRoleWithPermissionsQuery() { RoleId = id });
        }

        [HttpGet("{id}/users")]
        [MustHavePermission(ApiAction.View, ApiResource.Roles)]
        [MustHavePermission(ApiAction.View, ApiResource.Users)]
        [MustHavePermission(ApiAction.View, ApiResource.UserRoles)]
        [SwaggerOperation("Get Users by Role", "Returns a List with all Users within a Role.")]
        public async Task<List<UserDetailsDto>> GetUsersbyRoles(string id)
        {
            return await Mediator.Send(new GetUsersByRoleQuery() { RoleId = id });
        }

        [HttpGet("permissions")]
        [SwaggerOperation("Get All Permissions", "Returns a List with All Permissions to use by Roles.")]
        [MustHavePermission(ApiAction.View, ApiResource.Permisions)]
        public async Task<List<string>> GetAllPermissions()
        {
            return await Mediator.Send(new GetAllPermissionsQuery());
        }

        [HttpPost]
        [MustHavePermission(ApiAction.Create, ApiResource.Roles)]
        [MustHavePermission(ApiAction.Update, ApiResource.Roles)]
        [SwaggerOperation("Register or Update Role", "Updates a Role with the specified Id. Not Id provided creates one.")]
        public async Task<ActionResult> RegisterRole(CreateOrUpdateRoleCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("permissions")]
        [MustHavePermission(ApiAction.Update, ApiResource.RoleClaims)]
        [SwaggerOperation("Set Role Permissions", "Sets the Specified Permissions to a Role.")]
        public async Task<ActionResult> UpdatePermissions(UpdateRolePermissionsCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
