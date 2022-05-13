

using Application.Identity.Users;
using Application.Identity.Users.Password;
using Application.Identity.Users.UserCommands;
using Application.Identity.Users.UserCommands.CreateUser;
using Application.Identity.Users.UserCommands.ToggleUserStatus;
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
        return _userService.GetListAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    public Task<UserDetailsDto> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _userService.GetAsync(id, cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateAsync(CreateUserCommand request)
    {
        var userId = await Mediator.Send(request);
        return Created("/users/"+userId,  userId);
    }

    [HttpPost("self-register")]
    public Task<string> SelfRegisterAsync(CreateUserCommand request)
    {
        return _userService.CreateAsync(request, GetOriginFromRequest());
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
        await _userService.UpdateAsync(request, id);
        return NoContent();
    }


    private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
}