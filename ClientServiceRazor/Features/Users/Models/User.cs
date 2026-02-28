namespace ClientServiceRazor.Features.Users.Models;

public class User
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}