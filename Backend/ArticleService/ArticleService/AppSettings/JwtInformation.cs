using System.ComponentModel.DataAnnotations;

namespace ArticleService.AppSettings;

public record JwtInformation
{
    [Required]
    public required string TokenKey { get; init; }
    [Required]
    public required string Issuer { get; init; }
    [Required]
    public required string Audience { get; init; }
}