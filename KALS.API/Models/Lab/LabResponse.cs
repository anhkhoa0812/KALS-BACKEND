namespace KALS.API.Models.Lab;

public class LabResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
}