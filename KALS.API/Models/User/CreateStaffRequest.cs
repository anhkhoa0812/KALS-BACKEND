using KALS.Domain.Enums;

namespace KALS.API.Models.User;

public class CreateStaffRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
    
    public StaffType Type { get; set; }
}