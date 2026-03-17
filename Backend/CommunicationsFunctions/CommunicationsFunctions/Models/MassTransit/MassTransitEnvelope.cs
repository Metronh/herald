namespace CommunicationsFunctions.Models.MassTransit;

public class MassTransitEnvelope<T>
{
    public required T Message { get; set; }
}