using Application.Common.Exceptions;
using Application.PermaNotifications.Queries;
using Application.PermaNotifications.Queries.GetUnreadedNotificationsByUserId;
using MediatR;
using System.Collections.Generic;
using WebApi.Controllers.v2;
using Xunit;

namespace Application.IntegrationTest.PermaNotification
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
            var controller = new NotificationController();
            var x = await controller.SetAsReaded("");

            Assert.IsType<List<NotificationDto>>(result);
            Assert.Empty(result);
        }
    }
}