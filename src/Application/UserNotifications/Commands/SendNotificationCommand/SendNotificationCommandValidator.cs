using Application.Identity.Users;
using Microsoft.Extensions.Localization;

namespace Application.UserNotifications.Commands.SendNotificationCommand
{
    public class SendNotificationCommandValidator : AbstractValidator<SendNotificationCommand>
    {
        public SendNotificationCommandValidator(IUserService userService, IStringLocalizer<SendNotificationCommandValidator> localizer)
        {
            RuleFor(c => c.Message)
                .NotEmpty()
                .Length(1, 100);
            RuleFor(c => c.DestinationUserId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await userService.GetByIdAsync(id, ct) is not null)
                .WithMessage(localizer["identity.usernotfound"]);
        }
    }
}
