using System.Text.Json;
using CommunicationsFunctions.Interfaces.Services;

using CommunicationsFunctions.Models;
using CommunicationsFunctions.Models.MassTransit;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CommunicationsFunctions.Functions;

public class SendWelcomeEmailQueueFunction
{
    private readonly ILogger<SendWelcomeEmailQueueFunction> _logger;
    private readonly ICommunicationService _communicationService;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private const string EmailQueueName = "resend-queue";
    private const string RabbitMqConnectionString = "RabbitMqConnectionString";

    public SendWelcomeEmailQueueFunction(ILogger<SendWelcomeEmailQueueFunction> logger,
        ICommunicationService communicationService, JsonSerializerOptions jsonSerializerOptions)
    {
        _logger = logger;
        _communicationService = communicationService;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    [Function("SendEmailRabbitMqTrigger")]
    public async Task Run(
        [RabbitMQTrigger(EmailQueueName, ConnectionStringSetting = RabbitMqConnectionString)]
        string emailQueueItem)
    {
        _logger.LogInformation("Processing message from {QueueName}", EmailQueueName);
        MassTransitEnvelope<SendWelcomeEmailRequest> massTransitEnvelope =
            JsonSerializer.Deserialize<MassTransitEnvelope<SendWelcomeEmailRequest>>(emailQueueItem, _jsonSerializerOptions) ??
            throw new JsonException("Failed to deserialize message from queue: result was null");

        await _communicationService.SendWelcomeEmailCommunication(massTransitEnvelope.Message);
        
        _logger.LogInformation("Successfully processed message from {QueueName}", EmailQueueName);
    }
}