using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UploadData.Extensions;

public static class ExceptionMiddlewareExtension
{
    private static readonly string ContentTypeJson = "application/json";

    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = ContentTypeJson;

                var contextFeat = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeat is not null)
                {
                    Console.WriteLine($"There was an error this is the exeception: {contextFeat.Error}");
                    var problemDetails = new ProblemDetails
                    {
                        Status = context.Response.StatusCode,
                        Detail = "There was an internal server error.",
                    };
                    var json = JsonSerializer.Serialize(problemDetails);
                    await context.Response.WriteAsync(json);
                }
            });
        });
    }
}