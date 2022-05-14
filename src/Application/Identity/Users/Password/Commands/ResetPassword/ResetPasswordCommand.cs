namespace Application.Identity.Users.Password.Commands.ResetPassword;

public class ResetPasswordCommand : ICommand
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? ConfirmNewPassword { get; set; }

    public string? Token { get; set; }

    public class ResetPasswordCommandHandler : IdentityCommandHandler<ResetPasswordCommand>
    {
        public ResetPasswordCommandHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
        {
        }

        public override async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            await _userService.ResetPasswordAsync(request);
            return Unit.Value;
        }
    }
}

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(p => p.Password)
            .NotEmpty();

        RuleFor(p => p.ConfirmNewPassword)
            .NotEmpty()
            .Equal(p => p.Password);

        RuleFor(p => p.Token)
            .NotEmpty();
    }
}