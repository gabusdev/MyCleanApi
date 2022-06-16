using Microsoft.Extensions.Localization;

namespace Application.Identity.Users.Commands.ToggleUserStatus
{
    public class ToggleUserStatusCommandValidator : AbstractValidator<ToggleUserStatusCommand>
    {
        public ToggleUserStatusCommandValidator(IStringLocalizer<ToggleUserStatusCommandValidator> localizer)
        {
            RuleFor(c => c.ActivateUser)
                .NotEmpty()
                .Unless(c => c.ActivateUser == false);

            RuleFor(c => c.UserId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Equal(c => c.QueryUserId)
                    .WithMessage(localizer["validation.id.missmatch"]);
        }
    }
}
