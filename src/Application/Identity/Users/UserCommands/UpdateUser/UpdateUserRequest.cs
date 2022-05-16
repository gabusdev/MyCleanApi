namespace Application.Identity.Users.UserCommands.UpdateUser;

public class UpdateUserRequest
{
    public string Id { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
}

public class UpdateUserCommand : UpdateUserRequest, ICommand
{
    public string? CurrentUserId { get; set; }
}

public class UpdateUserCommandHandler : IdentityCommandHandler<UpdateUserCommand>
{
    public UpdateUserCommandHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
    {
    }

    public override async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Id != request.CurrentUserId)
            throw new ConflictException("The Id's privided do not match");
        await _userService.UpdateAsync(request, request.Id);

        return Unit.Value;
    }
}