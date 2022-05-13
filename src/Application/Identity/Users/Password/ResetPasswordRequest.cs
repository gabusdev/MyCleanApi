namespace Application.Identity.Users.Password;

public class ResetPasswordRequest
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? ConfirmNewPassword { get; set; }

    public string? Token { get; set; }
}

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
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