public class User
{
    public int UserId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public Guid Token { get; set; }
    public string Username { get; set; } = "";
}