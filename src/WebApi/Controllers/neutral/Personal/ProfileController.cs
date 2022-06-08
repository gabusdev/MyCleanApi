using Application.Identity.Users.Password.Commands.ChangePassword;
using Application.Identity.Users.UserCommands.UpdateUser;
using Application.Identity.Users.UserQueries;
using Application.Identity.Users.UserQueries.GetById;
using Application.Identity.Users.UserQueries.GetUserPermissions;
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
        public async Task<UserDetailsDto> GetProfileAsync(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetUserByIdQuery() { UserId = User.GetUserId() }, cancellationToken);
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
        [MustHavePermission(ApiAction.View, ApiResource.Permisions)]
        [SwaggerOperation("Get Permissions", "Get permissions of currently logged in user.")]
        public async Task<List<string>> GetPermissionsAsync(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetUserPermissionsQuery() { UserId = User.GetUserId() }, cancellationToken);
        }
    }
}
