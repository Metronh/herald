using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using UploadData.Interfaces;
using UploadData.Interfaces.Services;

namespace UploadData.Endpoints;

public static class UploadEndpoints
{
    public static void AddUploadEndpoints(this WebApplication app)
    {
        app.MapGet("/SetUpProject",SetUpProject).WithOpenApi();
    }
    

    private static async Task<Results<Ok, ProblemHttpResult>> SetUpProject(IUploadService uploadService)
    {
        try
        {
            await Task.WhenAll(uploadService.UploadUsers(),uploadService.UploadArticles());
            return TypedResults.Ok();
        }
        catch
        {
            Console.WriteLine("Error here");
            return TypedResults.Problem(statusCode:404);
        }
    }
}