using Microsoft.AspNetCore.SignalR;
using VoiceChat.Client.Models;

namespace VoiceChat.Client.Services
{
    public class ChatHub : Hub
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
                    new RoomMessage
                    {
                        Id = message.Id,
                        RoomId = message.RoomId,
                        Content = message.Content,
                        SenderId = message.SenderId,
                        SenderName = message.SenderName,
                        Timestamp = message.Timestamp
                    }
                );
        }
    }
}