using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace SetUp.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly string _contentTypeJson = "application/json";
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {exceptionMsg}, at {utcTime}",ex, DateTime.UtcNow);
            await HandleExceptionAsync(httpContext);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = _contentTypeJson;
        
        var problemDetails = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Detail = "There was an internal server error.",
        };
        var json = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(json);
    }
}