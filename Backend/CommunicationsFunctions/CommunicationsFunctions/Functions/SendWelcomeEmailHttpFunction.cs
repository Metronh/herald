using System.Net;
using System.Text.Json;
using CommunicationsFunctions.Interfaces.Services;
using CommunicationsFunctions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CommunicationsFunctions.Functions;

public class SendWelcomeEmailHttpFunction
{
    private readonly ICommunicationService _communicationService;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SendWelcomeEmailHttpFunction(ICommunicationService communicationService, JsonSerializerOptions jsonSerializerOptions)
    {
        _communicationService = communicationService;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    [Function("SendWelcomeEmailHttpTrigger")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")]
        HttpRequestData req)
    {
        SendWelcomeEmailRequest request =
            await JsonSerializer.DeserializeAsync<SendWelcomeEmailRequest>(req.Body, _jsonSerializerOptions) ??
            throw new ArgumentNullException(nameof(request), "Request body is null");

        await _communicationService.SendWelcomeEmailCommunication(request);

        var response = req.CreateResponse(HttpStatusCode.OK);
        return response;
    }
    
    
}