using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ArticleService.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly string _contentTypeJson = "application/json";
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
    

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError("Error: {exceptionMsg}, at {utcTime}", exception.Message, DateTime.UtcNow);
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