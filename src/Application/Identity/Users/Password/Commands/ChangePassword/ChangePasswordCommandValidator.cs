using Microsoft.Extensions.Localization;

namespace Application.Identity.Users.Password.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator(IStringLocalizer<ChangePasswordCommandValidator> localizer)
    {
        RuleFor(p => p.Password)
            .NotEmpty();

        RuleFor(p => p.NewPassword)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(p => p.ConfirmNewPassword)
            .Equal(p => p.NewPassword)
                .WithMessage(localizer["validation.pass.missmatch"]);
    }

}

