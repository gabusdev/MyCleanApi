using Domain.Entities;
using Domain.Entities.JoinTables;

namespace Application.PermaNotifications.Commands.SendNotificationCommand
{
    public class SendNotificationCommand : ICommand<string>
    {
        public string Message { get; set; } = null!;
        public string DestinationUserId { get; set; } = null!;

        public class SendNotificationCommandHandler : ICommandHandler<SendNotificationCommand, string>
        {
            private readonly IPermaNotificationService _notificationService;
            private readonly ICurrentUserService _currentUserService;

            public SendNotificationCommandHandler(IPermaNotificationService notificationService, ICurrentUserService currentUserService)
            {
                _currentUserService = currentUserService;
                _notificationService = notificationService;
            }
            public async Task<string> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
            {
                var currentUserId = _currentUserService.GetUserId();
                if (currentUserId == null)
                {
                    throw new ForbiddenException("Dont Have Permissions to do this action");
                }
                
                var notId = await _notificationService.SendNotificationToUser(request.Message, request.DestinationUserId, currentUserId);
                
                return notId;
            }
        }
    }
}
