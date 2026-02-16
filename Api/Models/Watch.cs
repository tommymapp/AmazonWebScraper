namespace Api.Models;

public class Watch
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } = string.Empty;
    public decimal TargetPrice { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
}