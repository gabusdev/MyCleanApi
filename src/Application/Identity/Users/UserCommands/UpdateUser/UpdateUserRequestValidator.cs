using Microsoft.Extensions.Localization;

namespace Application.Identity.Users.UserCommands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(IUserService userService, IStringLocalizer<UpdateUserCommandValidator> localizer)
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .Equal(p => p.CurrentUserId)
                .WithMessage(localizer["validation.id.missmatch"]);

        RuleFor(p => p.FirstName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(p => p.LastName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(u => u.UserName).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(6)
            .MustAsync(async (user, name, _) => !await userService.ExistsWithNameAsync(name, user.Id))
                .WithMessage((_, name) => localizer["validation.username.used", name]);

        RuleFor(p => p.Email)
            .NotEmpty()
            .EmailAddress()
                .WithMessage("Invalid Email Address.")
            .MustAsync(async (user, email, _) => !await userService.ExistsWithEmailAsync(email, user.Id))
                .WithMessage((_, email) => localizer["validation.email.used", email]);

        RuleFor(p => p.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (user, phone, _) => !await userService.ExistsWithPhoneNumberAsync(phone!, user.Id))
                .WithMessage((_, phone) => localizer["validation.phone.used", phone!])
                .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));
    }
}