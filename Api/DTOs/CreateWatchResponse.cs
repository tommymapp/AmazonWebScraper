namespace Api.DTOs;

public class CreateWatchResponse
{
    public Guid Id { get; set; }
    public string Url {get; set;}
    public double TargetPrice  {get; set;}
    public string Email  {get; set;}
}