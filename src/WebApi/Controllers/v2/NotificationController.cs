using Application.UserNotifications.Commands.SendUserNotificationCommand;
using Application.UserNotifications.Queries.GetUnreadedNotificationsByUserId;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2
{
    [ApiVersion("2")]
    public class NotificationController : VersionedApiController
    {
        [HttpGet("{userId}")]
        [MustHavePermission(ApiAction.Search, ApiResource.Notifications)]
        public async Task<ActionResult> NotificationsTest(string userId)
        {
            var response = await Mediator.Send(new GetUnreadedNotificationsByUserIdQuery { UserId = userId });
            return Ok(new { notifications = response });
        }

        [HttpPost]
        [MustHavePermission(ApiAction.Create, ApiResource.Notifications)]
        public async Task<ActionResult> NotificationsTest(SendNotificationCommand command)
        {
            var id = await Mediator.Send(command);

            return Ok(new { Id = id });
        }
    }
}
