using Application.PermaNotifications.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers.v2;
using Xunit;

namespace WebApi.IntegrationTest.Notification
{
    public class PermaNotifiacationTests
    {
        private readonly NotificationController _controller;
        public PermaNotifiacationTests()
        {
            _controller = new NotificationController();
        }

        [Fact]
        public async Task GetNotifiacationReturnsOk()
        {
            var result = await _controller.GetUnreadedNotifications("");
            Assert.IsType<List<NotificationDto>>(result);
        }
    }
}
