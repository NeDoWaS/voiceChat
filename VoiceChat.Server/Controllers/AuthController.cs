using Microsoft.AspNetCore.Mvc;
using VoiceChat.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VoiceChat.Server.DTos;

namespace VoiceChat.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly VoiceChatDbContext _db;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserController(
            VoiceChatDbContext db,
            IPasswordHasher<User> passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterRequest request)
        {
            var user = new User(request.Name, request.Email, "");
            user.Email = user.Email.Trim().ToLowerInvariant();

            bool emailExists = await _db.Users.AnyAsync(u => u.Email.ToLower() == user.Email);

            if (emailExists)
            {
                return Conflict(new { message = "Email is already registered." });
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _db.Users.Add(user);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                //check second time, just to be sure
                return Conflict(new { message = "Email is already registered." });
            }

            Console.WriteLine($"User {user.Name} with email {user.Email} and email {user.Email} was registered!");

            return Ok(new { message = "Registration Successfull!" });

        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginRequest>> LoginUserAsync([FromBody] LoginRequest request)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found! ");
            }
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid password!");
            }
            else
            {
                //login successful, GUID generation for session logic 

                string token = Guid.NewGuid().ToString("D");
                var expiresAt = DateTime.UtcNow.AddDays(7); // Session expires in 7 days
                var session = new UserSession(user.Id, token, expiresAt);
                _db.UserSessions.Add(session);
                await _db.SaveChangesAsync();
                return Ok(new LoginResponse
                {
                    Token = token,
                    UserName = user.Name,
                    UserId = user.Id
                });
            }
        }

        [HttpGet("getUserById")]
        public IActionResult GetUser(int id)
        {
            // Логика получения информации о пользователе
            Console.WriteLine($"Getting information for user with id {id}!");

            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            return Ok(user);
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser([FromQuery] string sessionToken)
        {
            var session = _db.UserSessions
                .FirstOrDefault(s => s.Token == sessionToken);

            if (session == null)
                return NotFound("Session not found!");

            // Use your actual expiry property name here.
            if (session.ExpiresAt <= DateTime.UtcNow)
                return Unauthorized("Session expired!");

            var user = _db.Users
                .FirstOrDefault(u => u.Id == session.UserId);

            if (user == null)
                return NotFound("User not found!");

            var profile = new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };

            return Ok(profile);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogoutUser()
        {
            var session = await GetValidSessionAsync();
            if (session == null)
            {
                return Unauthorized(new { message = "Invalid or expired session token." });
            }
            else

                _db.UserSessions.Remove(session);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Session token deleted! User logged out" });
        }
        private async Task<UserSession?> GetValidSessionAsync()
        {
            if (!Request.Headers.TryGetValue(
                "X-Session-Token", out var headerValue))
            {
                return null;
            }

            string token = headerValue.ToString();

            if (!Guid.TryParse(token, out _))
            {
                return null;
            }

            return await _db.UserSessions.FirstOrDefaultAsync(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow);
        }
    }
}