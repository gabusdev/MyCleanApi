using Application.UserNotifications.Commands.SendNotificationCommand;
using Application.UserNotifications.Commands.SendNotificationToAllCommand;
using Application.UserNotifications.Queries.GetUnreadedNotificationsByUserId;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2
{
    [ApiVersion("2")]
    public class NotificationController : VersionedApiController
    {
        [HttpGet("{userId}")]
        [MustHavePermission(ApiAction.Search, ApiResource.Notifications)]
        public async Task<ActionResult> GetUnreadedNotifications(string userId)
        {
            var response = await Mediator.Send(new GetUnreadedNotificationsByUserIdQuery { UserId = userId });
            return Ok(new { notifications = response });
        }

        [HttpPost]
        [MustHavePermission(ApiAction.Create, ApiResource.Notifications)]
        public async Task<ActionResult> SendNotification(SendNotificationCommand command)
        {
            var id = await Mediator.Send(command);

            return Ok(new { Id = id });
        }

        [HttpPost("broadcast")]
        [MustHavePermission(ApiAction.Create, ApiResource.Notifications)]
        public async Task<ActionResult> BroadcastNotification(SendNotificationToAllCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
