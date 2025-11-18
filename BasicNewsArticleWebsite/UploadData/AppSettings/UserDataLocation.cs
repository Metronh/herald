namespace UploadData.AppSettings;

public record UserDataLocation
{
    public required string AccountsCsv { get; init; }
};