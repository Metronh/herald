using Microsoft.AspNetCore.Http.HttpResults;

namespace UserService.Endpoints;

public static class HealthEndpoints
{
    public static void AddHealthEndpoints(this WebApplication app)
    {
        app.MapGet("/",HealthEndpoint);
    }

    private static async Task<Results<Ok,ProblemHttpResult>> HealthEndpoint(ILogger<Program> logger)
    {
        logger.LogInformation("Health endpoint hit at {utcTime}", DateTime.UtcNow);
        logger.LogInformation("Health endpoint completed at {utcTime}", DateTime.UtcNow);
        return TypedResults.Ok();
    }
}