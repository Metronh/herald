using Microsoft.AspNetCore.Http.HttpResults;

namespace ArticleService.Endpoints;

public static class HealthEndpoints
{
    public static void AddHealthEndpoint(this WebApplication app) => app.MapGet("/", Health);

    private static async Task<Results<Ok, ProblemHttpResult>> Health(ILogger<Program> logger)
    {
        logger.LogInformation("Health endpoint hit at {utcTime}", DateTime.UtcNow);
        logger.LogInformation("Health endpoint completed at {utcTime}", DateTime.UtcNow);
        return TypedResults.Ok();
    }
}