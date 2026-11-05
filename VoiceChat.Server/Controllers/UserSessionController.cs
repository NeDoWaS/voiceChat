using Microsoft.AspNetCore.Mvc;
using VoiceChat.Server.Models;

namespace VoiceChat.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSessionController : ControllerBase
    {
        private readonly VoiceChatDbContext _db;

        public UserSessionController(VoiceChatDbContext db)
        {
            _db = db;
        }

        [HttpPost("createSession")]
        public IActionResult CreateSession(int userId, string sessionToken, DateTime expiresAt)
        {
            var newSession = new UserSession(userId, sessionToken, expiresAt);
            _db.UserSessions.Add(newSession);
            _db.SaveChanges();
            Console.WriteLine($"New session for user {userId} was created!");
            return Ok();
        }

        [HttpPost("updateSession")]
        public IActionResult UpdateSession(int userId, string sessionToken, DateTime expiresAt)
        {
            var existingSession = _db.UserSessions.FirstOrDefault(s => s.UserId == userId);
            if (existingSession != null)
            {
                existingSession.Token = sessionToken;
                existingSession.ExpiresAt = expiresAt;
                _db.SaveChanges();
                Console.WriteLine($"Session for user {userId} was updated!");
                return Ok();
            }
            else
            {
                var newSession = new UserSession(userId, sessionToken, expiresAt);
                _db.UserSessions.Add(newSession);
                _db.SaveChanges();
                Console.WriteLine($"New session for user {userId} was created!");
                return Ok();
            }
        }

        [HttpGet("verifySession")]
        public IActionResult VerifySession( string sessionToken)
        {
            var session = _db.UserSessions.FirstOrDefault(s => s.Token == sessionToken);
            if (session == null)
            {
                Console.WriteLine($"Session with token {sessionToken} was not found");
                return NotFound();
            }
            Console.WriteLine($"Session with token {sessionToken} was verified");
            return Ok("Session is valid");
        }
        [HttpGet("getSessionByUserId")]
        public IActionResult GetSessionByUserId(int userId)
        {
            var session = _db.UserSessions.FirstOrDefault(s => s.UserId == userId);
            if (session == null)
            {
                Console.WriteLine($"Session for user {userId} was not found");
                return NotFound();
            }
            Console.WriteLine($"Session for user {userId} was found");
            return Ok(session);
        }
    }
}