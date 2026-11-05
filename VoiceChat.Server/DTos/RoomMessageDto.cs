using Microsoft.AspNetCore.Http.HttpResults;

public class RoomMessageDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime dateSent {get; set;} = DateTime.UtcNow;
}