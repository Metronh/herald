namespace ArticleService.AppSettings;

public record ConnectionStrings
{
    public required string MongoDb { get; init; }
    public required string Redis { get; init; }
}