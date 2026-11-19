using System.Net.Http.Json;
using VoiceChat.Client.Models;

namespace VoiceChat.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly SessionService _session;

        public ApiService(HttpClient http, SessionService session)
        {
            _http = http;
            _session = session;
        }
        //
        /*
        AUTHORIZATION
        */
        //
        public async Task<bool> RegisterUserAsync(string name, string email, string password)
        {
            var request = new RegisterRequest
            {
                Name = name,
                Email = email.ToLower(),
                Password = password
            };
            var response = await _http.PostAsJsonAsync("api/User/createUser", request);

            return response.IsSuccessStatusCode;
        }

        public async Task<UserProfile?> GetCurrentUserAsync(string sessionToken)
        {
            var response = await _http.GetAsync(
                $"api/user/me?sessionToken={Uri.EscapeDataString(sessionToken)}"
            );

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserProfile>();
        }
        //LOGIN
        public async Task<LoginResponse?> LoginUserAsync(string email, string password)
        {
            var request = new LoginRequest
            {
                Email = email.ToLower(),
                Password = password
            };
            var response = await _http.PostAsJsonAsync("api/User/login", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login failed: {error}");
                return null;
            }
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (result == null || string.IsNullOrEmpty(result.Token) || result.UserId <= 0)
            {
                Console.WriteLine("Login failed: Invalid response from server.  \n Response content: " + await response.Content.ReadAsStringAsync());
                return null;
            }
            _session.SetSession(result.Token, result.UserId, result.UserName);
            return result;
        }

        //LOGOUT 
        public async Task<bool> LogoutUserAsync()
        {
            if (string.IsNullOrWhiteSpace(_session.Token))
            {
                return false;
            }

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "api/User/logout"
            );

            request.Headers.Add("X-Session-Token", _session.Token);

            var response = await _http.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        //
        /* 
        ROOMS
        */
        //

        public async Task<Room> GetRoomByIdAsync(int roomId)
        {
            var response = await _http.GetAsync($"api/rooms/getRoom?id={roomId}");
            response.EnsureSuccessStatusCode();

            var room = await response.Content.ReadFromJsonAsync<Room>();
            return room ?? new Room("Unknown room", 0, "");
        }

        //LOAD ROOM MESSAGES
        public async Task<List<RoomMessage>> GetRoomMessagesAsync(int roomId)
        {
            var response = await _http.GetAsync($"api/roommessage/getMessages?roomId={roomId}");
            response.EnsureSuccessStatusCode();

            var messages = await response.Content.ReadFromJsonAsync<List<RoomMessage>>() ?? new List<RoomMessage>();
            return messages;
        }

        //SEND MESSAGE
        public async Task SendRoomMessageAsync(int roomId, string content)
        {
            var response = await _http.PostAsync(
                $"api/roommessage/sendMessage?text={Uri.EscapeDataString(content)}" +
                $"&authorId={_session.UserId ?? 0}" +
                $"&session={_session.Token}" +
                $"&roomId={roomId}",
                null);

            response.EnsureSuccessStatusCode();
        }
        //CREATE ROOM
        public async Task CreateRoomAsync(string name, int ownerId, string description)
        {

            var response = await _http.PostAsync(
                $"api/rooms/createRoom?name={Uri.EscapeDataString(name)}" +
                $"&ownerId={ownerId}" +
                $"&description={Uri.EscapeDataString(description)}",
                null);
            response.EnsureSuccessStatusCode();
        }
        //LOAD ROOMS (FOR HOMEPAGE
        public async Task<List<Room>> GetRoomsAsync()
        {
            var response = await _http.GetAsync("api/rooms/getAllRooms");
            response.EnsureSuccessStatusCode();

            var rooms = await response.Content.ReadFromJsonAsync<List<Room>>();
            return rooms;
        }

        //SESSION VALIDATION 
        public async Task<bool> ValidateSessionAsync()
        {
            if (string.IsNullOrWhiteSpace(_session.Token))
                return false;

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/UserSession/verifySession"
            );

            request.Headers.Add("X-Session-Token", _session.Token);

            var response = await _http.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
        public int GetUserId()
        {
            return _session.UserId ?? 0;
        }
        public string GetUserName()
        {
            return _session.UserName ?? "";
        }
        public string GetSessionToken()
        {
            return _session.Token ?? "";
        }
    }
}