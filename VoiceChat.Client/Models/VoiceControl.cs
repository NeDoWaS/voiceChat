public class VoiceUser
{
    public User User { get; set; } = null!;
    public bool IsMuted { get; set; }
    public bool IsSpeaking { get; set; }
}