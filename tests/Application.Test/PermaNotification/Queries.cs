using Application.PermaNotifications.Queries;
using Application.PermaNotifications.Queries.GetUnreadedNotificationsByUserId;
using MediatR;
using System.Collections.Generic;
using Xunit;

namespace Application.Test.PermaNotification
{
    public class Queries
    {
        public ISender Mediator { get; set; }
        public Queries(ISender mediator)
        {
            Mediator = mediator;
        }
        [Fact]
        public async void GetNotificationsReturnsNotificationDtoList()
        {
            var request = new GetUnreadedNotificationsByUserIdQuery();
            var result = await Mediator.Send(request);

            Assert.IsType<List<NotificationDto>>(result);
            Assert.Empty(result);
        }
    }
}