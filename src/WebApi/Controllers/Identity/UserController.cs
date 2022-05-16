using Application.Identity.Users.Password.Commands.ResetPassword;
using Application.Identity.Users.Password.Queries.ForgotPasswordQuery;
using Application.Identity.Users.UserCommands.CreateUser;
using Application.Identity.Users.UserCommands.DeleteUser;
using Application.Identity.Users.UserCommands.SetRoles;
using Application.Identity.Users.UserCommands.ToggleUserStatus;
using Application.Identity.Users.UserQueries;
using Application.Identity.Users.UserQueries.GetAll;
using Application.Identity.Users.UserQueries.GetById;
using Application.Identity.Users.UserQueries.GetUserRoles;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Identity.Users.UserCommands.SetRoles.SetUserRolesRequest;

namespace WebApi.Controllers.Identity;
[Route("api/[controller]")]
public class UserController : BaseApiController
{
    [HttpGet]
    [MustHavePermission(ApiAction.View, ApiResource.Users)]
    [SwaggerOperation("Get Users", "Returns a Lis with All Users")]
    public async Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetAllUsersQuery(), cancellationToken);
    }

    [HttpGet("{id}")]
    [MustHavePermission(ApiAction.View, ApiResource.Users)]
    [MustHavePermission(ApiAction.Search, ApiResource.Users)]
    [SwaggerOperation("Get User by Id", "Search for the user with given Id")]
    public async Task<UserDetailsDto> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetUserByIdQuery() { UserId = id }, cancellationToken);
    }

    [HttpPost]
    [MustHavePermission(ApiAction.Create, ApiResource.Users)]
    [SwaggerOperation("Create User", "Creates a new User")]
    public async Task<ActionResult<string>> CreateAsync(CreateUserCommand request)
    {
        var userId = await Mediator.Send(request);
        return Created("/users/" + userId, userId);
    }

    [HttpPost("self-register")]
    [AllowAnonymous]
    [SwaggerOperation("Register", "Self Register a New User")]
    public async Task<ActionResult<string>> SelfRegisterAsync(CreateUserCommand request)
    {
        var userId = await Mediator.Send(request);
        return Created("/users/" + userId, userId);
    }

    [HttpPost("{id}/toggle-status")]
    [MustHavePermission(ApiAction.Update, ApiResource.Users)]
    [SwaggerOperation("Toggle Status", "Toggles the Active Status of a User")]
    public async Task<ActionResult> ToggleStatusAsync(string id, ToggleUserStatusRequest request, CancellationToken cancellationToken)
    {
        ToggleUserStatusCommand command = request.Adapt<ToggleUserStatusCommand>();
        command.QueryUserId = id;

        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    [MustHavePermission(ApiAction.Delete, ApiResource.Users)]
    [SwaggerOperation("Delete User", "Deletes an User With Id given.")]
    public async Task<ActionResult> DeleteAsync(DeleteUserCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{id}/roles")]
    [MustHavePermission(ApiAction.View, ApiResource.UserRoles)]
    [SwaggerOperation("Get Roles", "Get a user's roles.")]
    public async Task<List<UserRoleDto>> GetRolesAsync(string id, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetUserRolesQuery() { UserId = id }, cancellationToken);
    }

    [HttpPost("{id}/roles")]
    [MustHavePermission(ApiAction.Update, ApiResource.UserRoles)]
    [SwaggerOperation("Set Roles", "Update a user's assigned roles.")]
    public async Task<ActionResult> AssignRolesAsync(string id, SetUserRolesRequest request, CancellationToken cancellationToken)
    {
        SetUserRolesCommand command = request.Adapt<SetUserRolesCommand>();
        command.UserId = id;
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [SwaggerOperation("Forgot Password", "Request a pasword reset token for a user.")]
    public async Task<string> ForgotPasswordAsync(ForgotPasswordQuery request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [SwaggerOperation("Reset password.", "Reset a user's password.")]
    public async Task<ActionResult> ResetPasswordAsync(ResetPasswordCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }




    /*
     * This Endpoint is used depending on Politics of the Api Owner
     * There is already in Personal Controllers
     * 
    [HttpPut("{id}")]
    [MustHavePermission(ApiAction.Update, ApiResource.Users)]
    [SwaggerOperation("Update User","Updates a User with given Id.")]
    public async Task<ActionResult> UpdateAsync(UpdateUserRequest request, string id)
    {
        UpdateUserCommand command = request.Adapt<UpdateUserCommand>();
        command.QueryUserId = id;

        await Mediator.Send(command);
        return NoContent();
    }
    */
}