using Microsoft.AspNetCore.Mvc;
using VoiceChat.Server.Models;

namespace VoiceChat.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly VoiceChatDbContext _db;
        public RoomsController(VoiceChatDbContext db)
        {
            _db = db;
        }

        [HttpPost("createRoom")]
        public IActionResult CreateRoom(string name, string description, int ownerId)
        {
            var room = new Models.Room(name, ownerId, description);
            if (!_db.Rooms.Any(r => r.Name == name))
            {
                _db.Rooms.Add(room);
                _db.SaveChanges();
                Console.WriteLine($"Room {name} with description {description} was created!");
                return Ok();
            }
            else
            {
                Console.WriteLine($"Room with name {name} already exists!");
                return Conflict("Room with this name already exists!");
            }
        }


        [HttpGet("getRoom")]
        public IActionResult GetRoom(int id)
        {
            // Логика получения информации о комнате
            if (!_db.Rooms.Any(r => r.Id == id))
            {
                Console.WriteLine($"Room with id {id} not found!");
                return NotFound("Room not found!");
            }
            else
            {
                var room = _db.Rooms.FirstOrDefault(r => r.Id == id);
                Console.WriteLine($"Getting information for room with id {id}!");
                return Ok(room);
            }
        }

        [HttpGet("getAllRooms")] 
        public IActionResult GetAllRooms()
        {
            // Логика получения списка всех комнат
            Console.WriteLine("Getting list of all rooms!");

            var rooms = _db.Rooms.ToList();
            return Ok(rooms);
        }
        
        [HttpDelete("deleteById")] 
        public IActionResult DeleteRoom(int id)
        {
            var room = _db.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                Console.WriteLine($"Room with id {id} not found!");
                return NotFound("Room not found!");
            }
            else
            {
                _db.Rooms.Remove(room);
                _db.SaveChanges();
                Console.WriteLine($"Room with id {id} was deleted!");
                return Ok();
            }
        }
    }
}