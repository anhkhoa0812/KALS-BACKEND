namespace KALS.API.Models.Lab;

public class CreateLabRequest
{
    public string? Name { get; set; }
    public IFormFile File { get; set; }
}