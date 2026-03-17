namespace CommunicationsFunctions.AppSettings;

public record ResendSettings
{
    public required string FromAddress { get; init; }
    public required string ApiKey { get; init; }
}