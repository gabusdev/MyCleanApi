using Application.Common.Pagination;
using Application.Identity.Users;
using Application.Identity.Users.Commands.CreateUser;
using Application.Identity.Users.Commands.DeleteUser;
using Application.Identity.Users.Commands.SetRoles;
using Application.Identity.Users.Commands.ToggleUserStatus;
using Application.Identity.Users.Password.Commands.ResetPassword;
using Application.Identity.Users.Password.Queries.ForgotPasswordQuery;
using Application.Identity.Users.Queries;
using Application.Identity.Users.Queries.GetAllPaged;
using Application.Identity.Users.Queries.GetById;
using Application.Identity.Users.Queries.GetUserRoles;
using Infrastructure.ResponseCaching;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Identity.Users.Commands.SetRoles.SetUserRolesRequest;

namespace WebApi.Controllers.neutral.Identity;

public class UserController : VersionNeutralApiController
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [MustHavePermission(ApiAction.View, ApiResource.Users)]
    [SwaggerOperation("Get Users", "Returns a Lis with All Users")]
    [ApiResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
    public async Task<ActionResult<PagedList<UserDetailsDto>>> GetListAsync([FromQuery] PaginationParams pparams, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetAllUsersPagedQuery(pparams), cancellationToken);
    }

    [HttpGet("{id}")]
    [MustHavePermission(ApiAction.View, ApiResource.Users)]
    [MustHavePermission(ApiAction.Search, ApiResource.Users)]
    [SwaggerOperation("Get User by Id", "Search for the user with given Id")]
    public async Task<ActionResult> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var user = await Mediator.Send(new GetUserByIdQuery() { UserId = id }, cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    [MustHavePermission(ApiAction.Create, ApiResource.Users)]
    [SwaggerOperation("Create User", "Creates a new User")]
    public async Task<ActionResult<string>> CreateAsync(CreateUserCommand request)
    {
        var userId = await Mediator.Send(request);
        return Created("/api/v1/users/" + userId, userId);
    }

    [HttpPost("self-register")]
    [AllowAnonymous]
    [SwaggerOperation("Register", "Self Register a New User")]
    public async Task<ActionResult<string>> SelfRegisterAsync(CreateUserCommand request)
    {
        var userId = await Mediator.Send(request);
        return Created("/api/v1/profile", userId);
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

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    [SwaggerOperation("Confirm email address for a user.", "")]
    public async Task<string> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code, CancellationToken cancellationToken)
    {
        return await _userService.ConfirmEmailAsync(userId, code, cancellationToken);
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