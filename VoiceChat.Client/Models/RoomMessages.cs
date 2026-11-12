namespace VoiceChat.Client.Models
{
    public class RoomMessage
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Content { get; set; } = "";
        public int SenderId { get; set; }
        public string SenderName { get; set; } = "User";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}