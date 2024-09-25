namespace KALS.API.Models.User;

public class LoginRequest
{
    public string UserNameOrPhoneNumber { get; set; }
    public string Password { get; set; }
}