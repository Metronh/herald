using CommunicationsFunctions.Models.Enums;

namespace CommunicationsFunctions.Models.Entities;

public record CommunicationEntity
{
    public Guid CommunicationId { get; init; }
    public Guid UserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? SentAt { get; init; }
    public required CommunicationType CommunicationType { get; init; }
    public required string CommunicationTitle { get; init; }
    public required string CommunicationAddress { get; init; }
    public required string Status { get; init; }
};