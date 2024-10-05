using KALS.Domain.Enums;

namespace KALS.API.Models.User;

public class UpdateStaffRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? FullName { get; set; }
    public StaffType? Type { get; set; }
    
    public void TrimString()
    {
        Username = Username?.Trim();
        Password = Password?.Trim();
        FullName = FullName?.Trim();
    }
}