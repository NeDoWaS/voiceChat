using Microsoft.AspNetCore.Mvc;
using VoiceChat.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace VoiceChat.Server.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    
    public class RoomMessageController : ControllerBase
    {

        //db

        private readonly VoiceChatDbContext _db;
        public RoomMessageController(VoiceChatDbContext db) 
        {
        _db = db;
        }

        //

        [HttpPost("sendMessage")]
        public IActionResult SendMessage(string text, string session, int roomId)
        {
            int? userId = _db.UserSessions.FirstOrDefault(s => s.Token == session)?.UserId ?? 0;
            if (userId == null || userId == 0)
            {
                return BadRequest("Invalid session token");
            }
            var message = new RoomMessage(roomId, userId.Value, text);
            _db.RoomMessages.Add(message);
            _db.SaveChanges();
            Console.WriteLine($"Message '{text}' from user {userId.Value} was sent!");
            return Ok();
        }

        [HttpGet("getMessages")]
        public async Task<IActionResult> GetMessages(int roomId)
        {
            var messages = await (
                from message in _db.RoomMessages
                join user in _db.Users
                    on message.SenderId equals user.Id
                where message.RoomId == roomId
                select new RoomMessageDto
                {
                    Id = message.Id,
                    RoomId = message.RoomId,
                    SenderId = message.SenderId,
                    SenderName = user.Name,
                    Content = message.Content,
                    Timestamp = message.Timestamp
                }
            ).ToListAsync();

            return Ok(messages);
        }

        [HttpPost("editMessage")]
        public IActionResult EditMessage(int messageId, string newContent)
        {
            var message = _db.RoomMessages.FirstOrDefault(u => u.Id == messageId);
            if (message == null)
            {
                Console.WriteLine($"Message with an id {messageId} was not found");
                return NotFound();
            }
            message.Content = newContent;
            _db.SaveChanges();
            Console.WriteLine($"Message with an id {messageId} was edited");
            return Ok();
        }
        [HttpDelete("deleteMessage")]
        public IActionResult DeleteMessage(int messageId)
        {
            var message = _db.RoomMessages.FirstOrDefault(u => u.Id == messageId);
            if (message == null)
            {
                Console.WriteLine($"Message with an id {messageId} was not found");
                return NotFound();
            }
            _db.RoomMessages.Remove(message);
            _db.SaveChanges();
            Console.WriteLine($"Message with an id {messageId} was deleted");
            return Ok();
        }
    }
}