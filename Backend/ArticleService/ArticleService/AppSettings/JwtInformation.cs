namespace ArticleService.AppSettings;

public record JwtInformation
{
    public required string TokenKey { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
}