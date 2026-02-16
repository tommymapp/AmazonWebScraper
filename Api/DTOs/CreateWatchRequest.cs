using System.ComponentModel.DataAnnotations;

namespace Api.DTOs;

public record CreateWatchRequest(
    [Required] [Url] 
    string Url,
    [Required] [Range(0.01, double.MaxValue)]
    decimal TargetPrice,
    [Required] [EmailAddress]
    string Email
);