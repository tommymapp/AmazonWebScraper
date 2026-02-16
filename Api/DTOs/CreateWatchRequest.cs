namespace Api.DTOs;

public record CreateWatchRequest(
    string Url,
    decimal TargetPrice,
    string Email
);