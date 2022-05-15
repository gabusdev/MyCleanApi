﻿using Application.Identity.Users.Password.Commands.ChangePassword;
using Application.Identity.Users.UserCommands.UpdateUser;
using Application.Identity.Users.UserQueries;
using Application.Identity.Users.UserQueries.GetById;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Personal
{
    [Route("api/profile")]
    [Authorize]
    [ApiController]
    public class PersonalController : BaseApiController
    {
        [HttpGet("profile")]
        [SwaggerOperation("Get Profile", "Get profile details of currently logged in user.")]
        public async Task<UserDetailsDto> GetProfileAsync(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetUserByIdQuery() { UserId = User.GetUserId() }, cancellationToken);
        }

        [HttpPut("profile")]
        [SwaggerOperation("Update Profile", "Update profile details of currently logged in user.")]
        public async Task<ActionResult> UpdateProfileAsync(UpdateUserRequest request)
        {
            UpdateUserCommand command = request.Adapt<UpdateUserCommand>();
            command.QueryUserId = User.GetUserId();

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
        [MustHavePermission(ApiAction.View,ApiResource.Permisions)]
        [SwaggerOperation("Get Permissions", "Get permissions of currently logged in user.")]
        public async Task<List<string>> GetPermissionsAsync(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetUserPermissionsQuery() { UserId=User.GetUserId() }, cancellationToken);
        }
    }
}
