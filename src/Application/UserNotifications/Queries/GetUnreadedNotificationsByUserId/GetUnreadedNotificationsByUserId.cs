using Application.Common.Persistence;
using Application.Common.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserNotifications.Queries.GetUnreadedNotifications;

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
            throw new Exception();
            /*var source = await _uow.UserNotifications.GetAsync();
            source.First()
            return*/
        }
    }
}


