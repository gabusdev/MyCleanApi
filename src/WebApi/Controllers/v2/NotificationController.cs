using Application.PermaNotifications.Commands.MarkNotificationAsReaded;
using Application.PermaNotifications.Commands.SendNotificationCommand;
using Application.PermaNotifications.Commands.SendNotificationToAllCommand;
using Application.PermaNotifications.Queries.GetUnreadedNotificationsByUserId;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2
{
    [ApiVersion("2")]
    public class NotificationController : VersionedApiController
    {
        [HttpGet("user/{userId}")]
        [MustHavePermission(ApiAction.Search, ApiResource.Notifications)]
        [SwaggerOperation("Get User Unreaded Notifications", "Gets Unreaded Notifications for specified user")]
        public async Task<ActionResult> GetUnreadedNotifications(string userId)
        {
            var response = await Mediator.Send(new GetUnreadedNotificationsByUserIdQuery { UserId = userId });
            return Ok(new { notifications = response });
        }

        [HttpPost]
        [MustHavePermission(ApiAction.Create, ApiResource.Notifications)]
        [SwaggerOperation("Create Notification", "Creates a Notification for specified User")]
        public async Task<ActionResult> SendNotification(SendNotificationCommand command)
        {
            var id = await Mediator.Send(command);

            return Ok(new { Id = id });
        }

        [HttpPost("broadcast")]
        [MustHavePermission(ApiAction.Create, ApiResource.Notifications)]
        [SwaggerOperation("Broadcast Notification", "Sends a Notification to All Users")]
        public async Task<ActionResult> BroadcastNotification(SendNotificationToAllCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}/readed")]
        [MustHavePermission(ApiAction.View, ApiResource.Notifications)]
        [SwaggerOperation("Set Notification as Readed", "Set Notification with given Id as Readed")]
        public async Task<ActionResult> SetAsReaded(string id)
        {
            await Mediator.Send(new MarkNotificationAsReadedCommand { NotificationID = id });
            return NoContent();
        }
    }
}
