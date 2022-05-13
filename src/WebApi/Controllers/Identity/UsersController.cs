

using Application.Identity.Users;
using Application.Identity.Users.Password;
using Application.Identity.Users.UserCommands.CreateUser;
using Application.Identity.Users.UserCommands.ToggleUserStatus;
using Application.Identity.Users.UserCommands.UpdateUser;
using Application.Identity.Users.UserQueries;
using Application.Identity.Users.UserQueries.GetAll;
using Application.Identity.Users.UserQueries.GetById;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace WebApi.Controllers.Identity;
[Route("[controller]")]
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return Mediator.Send(new GetAllUsersQuery(), cancellationToken);
    }

    [HttpGet("{id}")]
    public Task<UserDetailsDto> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return Mediator.Send(new GetUserByIdQuery() { UserId = id }, cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateAsync(CreateUserCommand request)
    {
        var userId = await Mediator.Send(request);
        return Created("/users/"+userId,  userId);
    }

    [HttpPost("self-register")]
    public async Task<ActionResult<string>> SelfRegisterAsync(CreateUserCommand request)
    {
        var userId = await Mediator.Send(request);
        return Created("/users/" + userId, userId);
    }

    [HttpPost("{id}/toggle-status")]
    public async Task<ActionResult> ToggleStatusAsync(string id, ToggleUserStatusRequest request, CancellationToken cancellationToken)
    {
        ToggleUserStatusCommand command = request.Adapt<ToggleUserStatusCommand>();
        command.QueryUserId = id;
        
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(UpdateUserRequest request, string id)
    {
        UpdateUserCommand command= request.Adapt<UpdateUserCommand>();
        command.QueryUserId= id;
        
        await Mediator.Send(command);
        return NoContent();
    }


    private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
}