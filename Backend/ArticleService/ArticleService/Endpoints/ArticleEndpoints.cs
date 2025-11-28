using ArticleService.Interfaces.Services;
using ArticleService.Models.Request;
using ArticleService.Models.Response;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ArticleService.Endpoints;

public static class ArticleEndpoints
{
    public static void RegisterGetArticleEndpoints(this WebApplication app)
    {
        app.MapPost("/GetArticleByTitle", GetArticle);
        app.MapPost("/CreateArticle", CreateArticle);
    }

    [EndpointName("Get article by title")]
    [EndpointDescription("Retrieves article by name can be phrase as well")]
    private static async Task<Results<Ok<List<ArticleResponse>>, ProblemHttpResult>> GetArticle(
        GetArticlesByTitleRequest request, IArticlesService articlesService, ILogger<Program> logger)
    {
        logger.LogInformation("GetArticleByTitle endpoint hit at {utcTime}", DateTime.UtcNow);
        var result = await articlesService.GetArticleByTitle(request: request);
        logger.LogInformation("GetArticleByTitle endpoint complete at {utcTime}", DateTime.UtcNow);
        return TypedResults.Ok(result);
    }
    [EndpointName("Create article")]
    [EndpointDescription("Creates a brand new article and stores it but can only be done by admin accounts")]
    private static async Task<Results<Ok, ProblemHttpResult>> CreateArticle(CreateArticleRequest request,
        IArticlesService articlesService, ILogger<Program> logger)
    {
        logger.LogInformation("CreateArticle endpoint hit at {utcTime}", DateTime.UtcNow);
        await articlesService.CreateArticle(request: request);
        logger.LogInformation("CreateArticle endpoint complete at {utcTime}", DateTime.UtcNow);
        return TypedResults.Ok();
    }
}