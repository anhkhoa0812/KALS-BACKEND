namespace KALS.API.Models.User;

public class UpdateMemberRequest
{
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Ward { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Address { get; set; }
    
    public int? ProvinceCode { get; set; }
    public int? DistrictCode { get; set; }
    public int? WardCode { get; set; }
    
    public void TrimString()
    {
        Username = Username?.Trim();
        FullName = FullName?.Trim();
        Ward = Ward?.Trim();
        Province = Province?.Trim();
        District = District?.Trim();
        Address = Address?.Trim();
    }
}