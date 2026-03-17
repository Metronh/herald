using System.Net;
using System.Text.Json;
using CommunicationsFunctions.Interfaces;
using CommunicationsFunctions.Interfaces.Services;
using CommunicationsFunctions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;

namespace CommunicationsFunctions.Functions;

public class SendEmailHttpTrigger
{
    private readonly ILogger<SendEmailHttpTrigger> _logger;
    private readonly ICommunicationService _communicationService;

    public SendEmailHttpTrigger(ILogger<SendEmailHttpTrigger> logger, ICommunicationService communicationService)
    {
        _logger = logger;
        _communicationService = communicationService;
    }

    [Function("SendWelcomeEmailHttpTrigger")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")]
        HttpRequestData req)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        SendWelcomeEmailRequest request =
            await JsonSerializer.DeserializeAsync<SendWelcomeEmailRequest>(req.Body, jsonSerializerOptions) ??
            throw new ArgumentNullException(nameof(request), "Request body is null");

        await _communicationService.SendWelcomeEmailCommunication(request);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync("Welcome to Azure Functions!");
        return response;
    }
}