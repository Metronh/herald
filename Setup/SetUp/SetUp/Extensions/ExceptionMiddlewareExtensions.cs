using SetUp.Middleware;

namespace SetUp.ExceptionMiddlewareExtensions;

public static class ExceptionMiddlewareExtensions
{
    public static void UseCustomExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}