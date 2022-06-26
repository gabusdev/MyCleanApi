using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Shared.Notifications;

namespace Infrastructure.Notifications;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        // await Groups.AddToGroupAsync(Context.ConnectionId, $"GroupTenant-{_currentTenant.Id}");

        await base.OnConnectedAsync();

        Log.Information("A client connected to NotificationHub: {connectionId}", Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"GroupTenant-{_currentTenant!.Id}");

        await base.OnDisconnectedAsync(exception);

        Log.Information("A client disconnected from NotificationHub: {connectionId}", Context.ConnectionId);
    }

#if DEBUG
    // Only for testing Purpouses
    public async Task TestingEchoFunction(string message)
    {
        await Clients
            .Caller
            .SendAsync("EchoTesting", message);
    }
#endif
}