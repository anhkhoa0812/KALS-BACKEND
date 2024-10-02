namespace KALS.API.Models.User;

public class LoginRequest
{
    public string UsernameOrPhoneNumber { get; set; }
    public string Password { get; set; }
}