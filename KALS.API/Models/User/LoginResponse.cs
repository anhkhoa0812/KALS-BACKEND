namespace KALS.API.Models.User;

public class LoginResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}