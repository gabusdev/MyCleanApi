using Application.Identity.Users;

namespace Application.PermaNotifications.Commands.SendNotificationToAllCommand
{
    public class SendNotificationToAllCommandValidator : AbstractValidator<SendNotificationToAllCommand>
    {
        public SendNotificationToAllCommandValidator(IUserService userService)
        {
            RuleFor(c => c.Message)
                .NotEmpty()
                .Length(1, 120);
        }
    }
}
