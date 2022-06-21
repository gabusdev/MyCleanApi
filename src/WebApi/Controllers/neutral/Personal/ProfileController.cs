using Application.Identity.Users.Commands.UpdateUser;
using Application.Identity.Users.Password.Commands.ChangePassword;
using Application.Identity.Users.Queries.GetById;
using Application.Identity.Users.Queries.GetUserPermissions;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.neutral.Personal
{
    [Authorize]
    public class ProfileController : VersionNeutralApiController
    {
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        [SwaggerOperation("Get Profile", "Get profile details of currently logged in user.")]
        public async Task<ActionResult> GetProfileAsync(CancellationToken cancellationToken)
        {
            var user = await Mediator.Send(new GetUserByIdQuery() { UserId = User.GetUserId() }, cancellationToken);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPut]
        [SwaggerOperation("Update Profile", "Update profile details of currently logged in user.")]
        public async Task<ActionResult> UpdateProfileAsync(UpdateUserRequest request)
        {
            UpdateUserCommand command = request.Adapt<UpdateUserCommand>();
            command.CurrentUserId = User.GetUserId();

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("change-password")]
        [SwaggerOperation("Reset password", "Change password of currently logged in user.")]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("permissions")]
        [MustHavePermission(ApiAction.View, ApiResource.Permissions)]
        [SwaggerOperation("Get Permissions", "Get permissions of currently logged in user.")]
        public async Task<List<string>> GetPermissionsAsync(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetUserPermissionsQuery() { UserId = User.GetUserId() }, cancellationToken);
        }
    }
}
