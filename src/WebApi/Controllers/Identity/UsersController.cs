using Application.Identity.Users.Password.Commands.ChangePassword;
using Application.Identity.Users.Password.Commands.ResetPassword;
using Application.Identity.Users.Password.Queries.ForgotPasswordQuery;
using Application.Identity.Users.UserCommands.CreateUser;
using Application.Identity.Users.UserCommands.ToggleUserStatus;
using Application.Identity.Users.UserCommands.UpdateUser;
using Application.Identity.Users.UserQueries;
using Application.Identity.Users.UserQueries.GetAll;
using Application.Identity.Users.UserQueries.GetById;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Identity;
[Route("api/[controller]")]
public class UsersController : BaseApiController
{
    [HttpGet]
    [MustHavePermission(ApiAction.View,ApiResource.Users)]
    [SwaggerOperation("Get Users", "Returns a Lis with All Users")]
    public async Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetAllUsersQuery(), cancellationToken);
    }

    [HttpGet("{id}")]
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
}