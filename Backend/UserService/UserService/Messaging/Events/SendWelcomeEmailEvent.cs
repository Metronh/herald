namespace UserService.Messaging.Events;

public record SendWelcomeEmailEvent
{
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public Guid UserId { get; init; }
}