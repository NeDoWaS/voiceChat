namespace VoiceChat.Server.Models
{
    public class RoomMember
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public RoomMember(int roomId, int userId)
        {
            RoomId = roomId;
            UserId = userId;
        }
    }

}