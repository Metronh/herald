using SetUp.Middleware;

namespace SetUp.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void UseCustomExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}