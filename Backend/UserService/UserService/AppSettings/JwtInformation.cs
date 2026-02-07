using System.ComponentModel.DataAnnotations;

namespace UserService.AppSettings;

public record JwtInformation
{
    [Required]
    public required string TokenKey { get; init; }
    [Required]
    public required string Issuer { get; init; }
    [Required]
    public required string Audience { get; init; }
    [Required]
    public required int ExpiryTime { get; init; }
}