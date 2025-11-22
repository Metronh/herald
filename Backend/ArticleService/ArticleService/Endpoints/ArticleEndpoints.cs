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
        try
        {
            var result = await articlesService.GetArticleByTitle(request: request);
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return TypedResults.Problem(statusCode:500);
        }
    }

    private static async Task<Results<Ok, ProblemHttpResult>> CreateArticle(CreatArticleRequest request,
        IArticlesService articlesService)
    {
        try
        {
            await articlesService.CreateArticle(request: request);
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return TypedResults.Problem(statusCode: 500);
        }
    }
}