namespace Application.PermaNotifications.Queries.GetUnreadedNotificationsByUserId;

public class GetUnreadedNotificationsByUserIdQuery : IQuery<List<NotificationDto>>
{
    public string UserId { get; set; } = null!;

    public class GetUnreadedNotificationsByUserIdQueryHandler : IQueryHandler<GetUnreadedNotificationsByUserIdQuery, List<NotificationDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetUnreadedNotificationsByUserIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<NotificationDto>> Handle(GetUnreadedNotificationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userNotifications = await _uow.UserNotifications.GetOrderedByAsync(
                un => un.DestinationUserId == request.UserId && un.Readed == false,
                un => un.Notification.CreatedOn, true, "Notification");

            return userNotifications.Adapt<List<NotificationDto>>();
        }
    }
}


