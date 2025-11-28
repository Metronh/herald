using Microsoft.AspNetCore.Http.HttpResults;

namespace UploadData.Endpoints;

public static class HealthEndpoints
{
    public static void AddHealthEndpoints(this WebApplication app)
    {
        app.MapGet("/",Health);
    }
    [EndpointName("Health Endpoint")]
    [EndpointDescription("Checks the application is up and running")]
    private static async Task<Results<Ok, ProblemHttpResult>> Health(ILogger<Program> log)
    {
        log.LogInformation("Health probe hit at {time}", DateTime.UtcNow);
        log.LogInformation("Health probe completed at {time}", DateTime.UtcNow);
        return TypedResults.Ok();
    }
}