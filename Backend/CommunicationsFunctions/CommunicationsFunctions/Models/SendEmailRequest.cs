namespace CommunicationsFunctions.Models;

public record SendEmailRequest
{
    public required string Email { get; init; }
    public required string TemplateName { get; init; }
    public required string FirstName { get; init; }
    public required string Subject { get; init; }
}