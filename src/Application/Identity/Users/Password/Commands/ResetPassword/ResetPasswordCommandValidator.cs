using Microsoft.Extensions.Localization;

namespace Application.Identity.Users.Password.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator(IStringLocalizer<ResetPasswordCommandValidator> localizer)
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(p => p.ConfirmNewPassword)
                .NotEmpty()
                .Equal(p => p.Password)
                    .WithMessage(localizer["validation.pass.missmatch"]); ;

            RuleFor(p => p.Token)
                .NotEmpty();
        }
    }
}
