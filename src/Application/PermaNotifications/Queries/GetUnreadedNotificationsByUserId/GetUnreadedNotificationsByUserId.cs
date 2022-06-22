namespace Application.PermaNotifications.Queries.GetUnreadedNotificationsByUserId;

public class GetUnreadedNotificationsByUserIdQuery : IQuery<List<NotificationDto>>
{
    public string UserId { get; set; } = null!;

    public class GetUnreadedNotificationsByUserIdQueryHandler : IQueryHandler<GetUnreadedNotificationsByUserIdQuery, List<NotificationDto>>
    {
        private readonly IPermaNotificationService _notificationService;

        public GetUnreadedNotificationsByUserIdQueryHandler(IPermaNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<List<NotificationDto>> Handle(GetUnreadedNotificationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userNotifications = await _notificationService.GetNotifications(request.UserId);

            return userNotifications.Adapt<List<NotificationDto>>();
        }
    }
}


