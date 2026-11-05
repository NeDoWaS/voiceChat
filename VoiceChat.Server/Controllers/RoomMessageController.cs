using Microsoft.AspNetCore.Mvc;
using VoiceChat.Server.Models;

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
        public IActionResult SendMessage(string text, int authorId, int roomId)
        {
            var message = new RoomMessage(roomId, authorId, text);
            _db.RoomMessages.Add(message);
            _db.SaveChanges();
            Console.WriteLine($"Message '{text}' from user {authorId} was sent!");
            return Ok();
        }

        [HttpGet("getMessages")]
        public IActionResult GetMessages(int roomId)
        {
            var messages = _db.RoomMessages.Where(m => m.RoomId == roomId).ToList();
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