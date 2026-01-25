using Microsoft.AspNetCore.Http.HttpResults;
using SetUp.Interfaces.Services;

namespace SetUp.Endpoints;

public static class DeleteEndpoints
{
    public static void AddDeleteEndpoints(this WebApplication app)
    {
        app.MapDelete("/DeleteAllData", DeleteAllData);
        app.MapDelete("/DeleteAllArticles",DeleteAllArticles);
        app.MapDelete("/DeleteAllUsers", DeleteAllUsers);
    }
    
    [EndpointName("Deletes all data on UserDb and ArticleDB")]
    [EndpointDescription("Deletes all data on UsersDB and ArticlesDB.")]
    private static async Task<Results<Ok, ProblemHttpResult>> DeleteAllData(IDeleteService deleteService,
        ILogger<Program> logger)
    {
        logger.LogInformation("{endPointName} endpoint hit at {utcTime}", nameof(DeleteAllData),DateTime.UtcNow);
        await Task.WhenAll(deleteService.DeleteAllArticles(), deleteService.DeleteAllUsers());
        logger.LogInformation("{endPointName} endpoint completed at {utcTime}", nameof(DeleteAllData),DateTime.UtcNow);
        return TypedResults.Ok();
    }
    
    [EndpointName("Deletes all articles from articleDB")]
    [EndpointDescription("Deletes all data on ArticlesDB.")]
    private static async Task<Results<Ok, ProblemHttpResult>> DeleteAllArticles(IDeleteService deleteService, 
        ILogger<Program> logger)
    {
        logger.LogInformation("{endPointName} endpoint hit at {utcTime}", nameof(DeleteAllArticles),DateTime.UtcNow);
        await deleteService.DeleteAllArticles();
        logger.LogInformation("{endPointName} endpoint completed at {utcTime}", nameof(DeleteAllArticles),DateTime.UtcNow);
        return TypedResults.Ok();
    }
    
    [EndpointName("Deletes all users from usersDB")]
    [EndpointDescription("Deletes all data on UsersDB.")]
    private static async Task<Results<Ok, ProblemHttpResult>> DeleteAllUsers(IDeleteService deleteService, 
        ILogger<Program> logger)
    {
        logger.LogInformation("{endPointName} endpoint hit at {utcTime}", nameof(DeleteAllUsers),DateTime.UtcNow);
        await deleteService.DeleteAllUsers();
        logger.LogInformation("{endPointName} endpoint completed at {utcTime}", nameof(DeleteAllUsers),DateTime.UtcNow);
        return TypedResults.Ok();
    }
}