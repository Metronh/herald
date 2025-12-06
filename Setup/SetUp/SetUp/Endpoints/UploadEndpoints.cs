using Microsoft.AspNetCore.Http.HttpResults;
using SetUp.Interfaces.Services;

namespace SetUp.Endpoints;

public static class UploadEndpoints
{
    public static void AddUploadEndpoints(this WebApplication app)
    {
        app.MapGet("/SetUpProject", SetUpProject).WithOpenApi();
    }

    [EndpointName("Set up application")]
    [EndpointDescription("This endpoint uploads both users and articles to their respective databases.")]
    private static async Task<Results<Created, ProblemHttpResult>> SetUpProject(IUploadService uploadService,
        ILogger<Program> logger)
    {
        logger.LogInformation("SetUpProject endpoint hit at {utcTime}", DateTime.UtcNow);
        await Task.WhenAll(uploadService.UploadUsers(), uploadService.UploadArticles());
        logger.LogInformation("SetUpProject endpoint completed at {utcTime}", DateTime.UtcNow);
        return TypedResults.Created();
    }
}