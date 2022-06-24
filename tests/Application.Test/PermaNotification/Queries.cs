using Application.Common.Exceptions;
using Application.PermaNotifications.Queries;
using Application.PermaNotifications.Queries.GetUnreadedNotificationsByUserId;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApi.Controllers.v2;
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
            var controller = new NotificationController();
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var x = await controller.SetAsReaded("123");

            Assert.IsType<List<NotificationDto>>(result);
            Assert.Empty(result);
        }
    }
}