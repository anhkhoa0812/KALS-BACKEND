namespace KALS.API.Models.User;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
}