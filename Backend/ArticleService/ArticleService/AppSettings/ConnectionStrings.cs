namespace ArticleService.AppSettings;

public record ConnectionStrings
{
    public required string MongoDb { get; init; }
}