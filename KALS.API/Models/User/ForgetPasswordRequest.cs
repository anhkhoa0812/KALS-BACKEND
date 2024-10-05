namespace KALS.API.Models.User;

public class ForgetPasswordRequest
{
    public string Otp { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}