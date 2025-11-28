namespace UploadData.AppSettings;

public record ConnectionStrings
{
    public required string PostgresDb { get; init; }
    public required string MongoDb { get; init; }
}