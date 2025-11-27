using ArticleService.Middleware;

namespace ArticleService.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void UseCustomExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}