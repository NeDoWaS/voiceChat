namespace VoiceChat.Server.Models
{
    public class UserSession    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public DateTime ExpiresAt { get; set; }

        public UserSession(int userId, string token, DateTime expiresAt)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
        }
    }
}