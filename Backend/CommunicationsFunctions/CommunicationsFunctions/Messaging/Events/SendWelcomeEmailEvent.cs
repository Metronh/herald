namespace CommunicationsFunctions.Messaging.Events;

public record SendWelcomeEmailEvent
{
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required Guid UserId { get; init; }
};