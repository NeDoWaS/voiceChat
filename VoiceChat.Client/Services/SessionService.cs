namespace VoiceChat.Client.Services
{
    public class SessionService
    {
        public string? Token { get; private set; }

        public int? UserId { get; private set; }

        public string? UserName { get; private set; }

        public bool IsLoggedIn => !string.IsNullOrEmpty(Token) && UserId.HasValue;

        public void SetSession(string token, int? userId, string userName)
        {
            Token = token;
            UserId = userId;
            UserName = userName;
        }

        public void ClearSession()
        {
            Token = null;
            UserId = null;
            UserName = null;
        }
    }
}