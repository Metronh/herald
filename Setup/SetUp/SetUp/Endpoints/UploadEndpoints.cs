using Microsoft.AspNetCore.Http.HttpResults;
using SetUp.Interfaces.Services;

namespace SetUp.Endpoints;

public static class UploadEndpoints
{
    public static void AddUploadEndpoints(this WebApplication app)
    {
        app.MapGet("/SetUpProject", SetUpProject).WithOpenApi();
        app.MapGet("/UploadUsers", UploadUsers).WithOpenApi();
        app.MapGet("/UploadArticles", UploadArticles).WithOpenApi();
    }

    [EndpointName("Set up application")]
    [EndpointDescription("This endpoint uploads both users and articles to their respective databases.")]
    private static async Task<Results<Ok, ProblemHttpResult>> SetUpProject(IUploadService uploadService,
        ILogger<Program> logger)
    {
        logger.LogInformation("SetUpProject endpoint hit at {utcTime}", DateTime.UtcNow);
        await Task.WhenAll(uploadService.UploadUsers(), uploadService.UploadArticles());
        logger.LogInformation("SetUpProject endpoint completed at {utcTime}", DateTime.UtcNow);
        return TypedResults.Ok();
    }

    [EndpointName("Upload Users")]
    [EndpointDescription("This endpoint uploads users to their respective database.")]
    private static async Task<Results<Ok, ProblemHttpResult>> UploadUsers(IUploadService uploadService,
        ILogger<Program> logger)
    {
        logger.LogInformation("{endpointName} endpoint hit at {utcTime}", nameof(UploadUsers), DateTime.UtcNow);
        await uploadService.UploadUsers();
        logger.LogInformation("{endpointName}  endpoint completed at {utcTime}", nameof(UploadUsers), DateTime.UtcNow);
        return TypedResults.Ok();
    }
    
    [EndpointName("Upload Articles")]
    [EndpointDescription("This endpoint uploads articles to their respective database.")]
    private static async Task<Results<Ok, ProblemHttpResult>> UploadArticles(IUploadService uploadService,
        ILogger<Program> logger)
    {
        logger.LogInformation("{endpointName} endpoint hit at {utcTime}", nameof(UploadArticles), DateTime.UtcNow);
        await uploadService.UploadArticles();
        logger.LogInformation("{endpointName}  endpoint completed at {utcTime}", nameof(UploadArticles), DateTime.UtcNow);
        return TypedResults.Ok();
    }
}