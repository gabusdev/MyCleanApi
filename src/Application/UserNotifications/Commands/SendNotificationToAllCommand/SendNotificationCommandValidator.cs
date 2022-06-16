using Application.Identity.Users;

namespace Application.UserNotifications.Commands.SendUserNotificationCommand
{
    public class SendNotificationToAllCommandValidator : AbstractValidator<SendNotificationToAllCommand>
    {
        public SendNotificationToAllCommandValidator(IUserService userService)
        {
            RuleFor(c => c.Message)
                .NotEmpty()
                .Length(1, 120);
            RuleFor(c => c.DestinationUserId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await userService.GetAsync(id, ct) is not null);
        }
    }
}
