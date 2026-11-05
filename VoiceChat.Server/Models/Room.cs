namespace VoiceChat.Server.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public string? Description { get; set; } = null;

        public Room(string name, int ownerId, string? description = null)
        {
            Name = name;
            OwnerId = ownerId;
            Description = description;
        }
    }
}