namespace UploadData.AppSettings;

public record UserDataLocation
{
    public required string UsersCsv { get; init; }
    public required string ArticlesCsv { get; init; }
};