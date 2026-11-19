public class RoomMessageDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; } = "User";
    public string Content { get; set; } = "You're not supposed to see this message.";
    public DateTime Timestamp { get; set; }
}