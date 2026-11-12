using Microsoft.AspNetCore.SignalR;
namespace VoiceChat.Server.Hubs{
public class VoiceHub : Hub
{
    public async Task JoinRoom(int roomId)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            $"room-{roomId}"
        );
    }

    public async Task LeaveRoom(int roomId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            $"room-{roomId}"
        );
    }

    public async Task SendMessage(int roomId, RoomMessage message)
    {
        await Clients.Group($"room-{roomId}")
            .SendAsync(
                "ReceiveMessage",
                message
            );
    }
}
}