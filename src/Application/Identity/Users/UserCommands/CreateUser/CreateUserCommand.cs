using Application.Common.Messaging;

namespace Application.Identity.Users.UserCommands.CreateUser;

public class CreateUserCommand : ICommand<string>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string? PhoneNumber { get; set; }
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, string>
{
    private readonly IUserService _userService;
    private readonly IHttpContextService _httpContextService;

    public CreateUserCommandHandler(IUserService userService, IHttpContextService httpContextService)
    {
        _userService = userService;
        _httpContextService = httpContextService;
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newUserId = await _userService.CreateAsync(request, _httpContextService.GetOrigin());

        return newUserId;
    }
}