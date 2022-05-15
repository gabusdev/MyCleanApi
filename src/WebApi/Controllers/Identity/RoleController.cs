using Application.Identity.Roles;
using Application.Identity.Roles.Queries.GetAllRolesQuery;
using Application.Identity.Roles.Queries.GetPermissionsQuery;
using Application.Identity.Roles.Queries.GetRoleWithPermissionsQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseApiController
    {
        [HttpGet]
        [MustHavePermission(ApiAction.View, ApiResource.Roles)]
        public async Task<List<RoleDto>> GetAllRoles()
        {
            return await Mediator.Send(new GetAllRolesQuery());
        }

        [HttpGet("{id}")]
        [MustHavePermission(ApiAction.View, ApiResource.Roles)]
        public async Task<RoleDto> GetRoles(string id)
        {
            return await Mediator.Send(new GetRoleWithPermissionsQuery() { RoleId = id });
        }

        [HttpGet("permissions")]
        [MustHavePermission(ApiAction.View, ApiResource.Permisions)]
        public async Task<List<string>> GetAllPermissions()
        {
            return await Mediator.Send(new GetAllPermissionsQuery());
        }

        [HttpPost]
        [MustHavePermission(ApiAction.Create, ApiResource.Roles)]
        [MustHavePermission(ApiAction.Update, ApiResource.Roles)]
        public async Task<ActionResult> RegisterRole(CreateOrUpdateRoleCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("permissions")]
        [MustHavePermission(ApiAction.Update, ApiResource.Roles)]
        [MustHavePermission(ApiAction.Update, ApiResource.RoleClaims)]
        public async Task<ActionResult> UpdatePermissions(UpdateRolePermissionsCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
