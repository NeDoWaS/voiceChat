using System.ComponentModel.DataAnnotations;
public class RoomMessage
{
    [Key]
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; } = "User";
    public string Content { get; set; } = "";
    public DateTime Timestamp { get; set; }
    public RoomMessage(int roomId, int senderId, string content)
    {
        RoomId = roomId;
        SenderId = senderId;
        Content = content;
        Timestamp = DateTime.UtcNow;
    }
}
