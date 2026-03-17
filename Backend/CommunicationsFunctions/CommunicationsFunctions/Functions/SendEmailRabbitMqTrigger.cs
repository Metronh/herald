using CommunicationsFunctions.AppSettings;
using CommunicationsFunctions.Messaging.Events;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CommunicationsFunctions.Functions;

public class SendEmailRabbitMqTrigger
{
    private readonly ILogger<SendEmailRabbitMqTrigger> _logger;
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private const string EmailQueueName = "Welcome Email Queue";

    public SendEmailRabbitMqTrigger(ILogger<SendEmailRabbitMqTrigger> logger,
        MessageBrokerSettings messageBrokerSettings)
    {
        _logger = logger;
        _messageBrokerSettings = messageBrokerSettings;
    }

    [Function("SendEmailRabbitMqTrigger")]
    public void Run(
        [RabbitMQTrigger(EmailQueueName, ConnectionStringSetting = "amqp://guest:guest@localhost:5672")]
        SendWelcomeEmailEvent myQueueItem)
    {
        _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
    }
}