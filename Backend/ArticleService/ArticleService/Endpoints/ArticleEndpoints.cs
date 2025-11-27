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

    private static async Task<Results<Ok<List<ArticleResponse>>, ProblemHttpResult>> GetArticle(
        GetArticlesByTitleRequest request, IArticlesService articlesService)
    {
        var result = await articlesService.GetArticleByTitle(request: request);
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok, ProblemHttpResult>> CreateArticle(CreatArticleRequest request,
        IArticlesService articlesService)
    {
        await articlesService.CreateArticle(request: request);
        return TypedResults.Ok();
    }
}