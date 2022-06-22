using Application.Identity.Users.Commands.UpdateUser;
using Application.Identity.Users.Password.Commands.ChangePassword;
using Application.Identity.Users.Queries.GetById;
using Application.Identity.Users.Queries.GetUserPermissions;
using Application.PermaNotifications.Queries.GetUnreadedNotificationsByUserId;
using Infrastructure.ResponseCaching;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.neutral.Personal
{
    [Authorize]
    public class ProfileController : VersionNeutralApiController
    {
        [HttpGet]
        [ApiResponseCache(Duration = 120, Location = ResponseCacheLocation.Client)]
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
        //[MustHavePermission(ApiAction.View, ApiResource.Permissions)]
        [SwaggerOperation("Get Permissions", "Get permissions of currently logged in user.")]
        public async Task<List<string>> GetPermissionsAsync(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetUserPermissionsQuery() { UserId = User.GetUserId() }, cancellationToken);
        }

        [HttpGet("notifiactions")]
        [MustHavePermission(ApiAction.View, ApiResource.Notifications)]
        [SwaggerOperation("Get Unreaded Notifications", "Get Unreaded Notifications for current User")]
        public async Task<ActionResult> GetUnreadedNotifications()
        {
            var response = await Mediator.Send(new GetUnreadedNotificationsByUserIdQuery { UserId = User.GetUserId() });
            return Ok(response);
        }
    }
}
