using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Notifications;

[Authorize]
public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

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

    public async Task<string> Test()
    {
        Log.Information($"Executing SgnalR Test Method for conecction {Context.ConnectionId} and " +
            $"user with id {Context.UserIdentifier}");
        await Clients.Caller.SendAsync("Test","This is Response from Server");
        return "Kk de Mono";
    }
}