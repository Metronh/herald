using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UploadData.Middleware;

namespace UploadData.Extensions;

public static class ExceptionMiddlewareExtension
{
    public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandling>();
    }
}