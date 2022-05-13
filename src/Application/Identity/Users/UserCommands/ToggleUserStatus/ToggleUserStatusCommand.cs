using Application.Common.Exceptions;
using Application.Common.Messaging;

namespace Application.Identity.Users.UserCommands.ToggleUserStatus;

public class ToggleUserStatusRequest
{
    public bool ActivateUser { get; set; }
    public string? UserId { get; set; }
}

public class ToggleUserStatusCommand : ToggleUserStatusRequest, ICommand
{
    public string? QueryUserId { get; set; }
}

public class ToggleUserStatusCommandHandler : IdentityCommandHandler<ToggleUserStatusCommand>
{
    public ToggleUserStatusCommandHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
    {
    }

    public override async Task<Unit> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.QueryUserId != request.UserId)
            throw new ConflictException("The Id's privided do not match");

        await _userService.ToggleStatusAsync(request, cancellationToken);
        return Unit.Value;
    }
}