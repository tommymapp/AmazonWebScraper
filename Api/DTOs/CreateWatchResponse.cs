namespace Api.DTOs;

public class CreateWatchResponse
{
    public Guid Id { get; set; }
    public string Url {get; set;}
    public decimal TargetPrice  {get; set;}
    public string Email  {get; set;}
    public string Status  {get; set;}
}