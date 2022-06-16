using Application.Identity.Users;

namespace Application.UserNotifications.Commands.SendNotificationCommand
{
    public class SendNotificationCommandValidator : AbstractValidator<SendNotificationCommand>
    {
        public SendNotificationCommandValidator(IUserService userService)
        {
            RuleFor(c => c.Message)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(c => c.DestinationUserId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await userService.GetAsync(id, ct) is not null);
        }
    }
}
