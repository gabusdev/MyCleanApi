using Application.Identity.Users;
using Domain.Entities;
using Domain.Entities.JoinTables;

namespace Application.PermaNotifications.Commands.SendNotificationToAllCommand
{
    public class SendNotificationToAllCommand : ICommand<string>
    {
        public string Message { get; set; } = null!;

        public class SendNotificationToAllCommandHandler : ICommandHandler<SendNotificationToAllCommand, string>
        {
            private readonly IPermaNotificationService _notificationService;
            private readonly ICurrentUserService _currentUserService;

            public SendNotificationToAllCommandHandler(IPermaNotificationService notificationService, ICurrentUserService currentUserService)
            {
                _currentUserService = currentUserService;
                _notificationService = notificationService;
            }
            public async Task<string> Handle(SendNotificationToAllCommand request, CancellationToken cancellationToken)
            {
                var currentUserId = _currentUserService.GetUserId();
                if (currentUserId == null)
                {
                    throw new ForbiddenException("Dont Have Permissions to do this action");
                }
                
                await _notificationService.SendNotificationToAll(request.Message, currentUserId);
                
                return "";
            }
        }
    }
}
