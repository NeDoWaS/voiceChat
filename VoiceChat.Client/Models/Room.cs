using static System.Collections.IEnumerable;
namespace VoiceChat.Client.Models

{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int OwnerId { get; set; } = 0;

        public Room(string name, int ownerId, string description = "")
        {
            Name = name;
            OwnerId = ownerId;
            Description = description;
        }
    }
}