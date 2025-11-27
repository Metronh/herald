using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ArticleService.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly string _contentTypeJson = "application/json";
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an error and this is the error {ex.Message}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = _contentTypeJson;

        var problemDetails = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Detail = "There was an internal server error.",
        };
        var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(problemDetailsJson);
    }
}