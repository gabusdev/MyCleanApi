namespace Application.Identity.Users.Password.Commands.ChangePassword;

public class ChangePasswordCommand : ICommand
{
    public string Password { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmNewPassword { get; set; } = default!;

    public class ChangePasswordRequestHandler : IdentityCommandHandler<ChangePasswordCommand>
    {
        private readonly ICurrentUserService _currentUser;
        public ChangePasswordRequestHandler(IUserService userService, IHttpContextService httpContextService, ICurrentUserService currentUser) : base(userService, httpContextService)
        {
            _currentUser = currentUser;
        }

        public async override Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await _userService.ChangePasswordAsync(request, _currentUser.GetUserId());
            return Unit.Value;
        }
    }
}

