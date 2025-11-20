namespace UploadData.AppSettings;

public record CsvLocations
{
    public required string UsersCsv { get; init; }
    public required string ArticlesCsv { get; init; }
};