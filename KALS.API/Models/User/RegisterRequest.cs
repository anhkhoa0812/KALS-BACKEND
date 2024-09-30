namespace KALS.API.Models.User;

public class RegisterRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
    public string Otp { get; set; }
}