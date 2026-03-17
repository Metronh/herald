namespace CommunicationsFunctions.Models;

public record SendWelcomeEmailRequest
{
    public Guid CommunicationId { get; init; } = Guid.NewGuid();
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public Guid UserId { get; init; }
}