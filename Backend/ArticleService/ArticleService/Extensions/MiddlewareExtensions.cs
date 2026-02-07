using ArticleService.Middleware;

namespace ArticleService.Extensions;

public static class MiddlewareExtensions
{
    public static void UseCustomExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}