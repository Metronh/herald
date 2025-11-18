using Microsoft.AspNetCore.Http.HttpResults;
using UploadData.Interfaces;
using UploadData.Interfaces.Services;

namespace UploadData.Endpoints;

public static class UploadEndpoints
{
    public static void AddUploadEndpoints(this WebApplication app)
    {
        app.MapGet("/UploadArticles", UploadArticles).WithOpenApi();
        app.MapGet("/UploadUsers",UploadUsers).WithOpenApi();
    }

    private static async Task<Results<Ok, ProblemHttpResult>> UploadArticles(IUploadService uploadService)
    {
        await uploadService.UploadArticles();
        throw new NotImplementedException();
    }

    private static async Task<Results<Ok, ProblemHttpResult>> UploadUsers(IUploadService uploadService)
    {
        await uploadService.UploadUsers();
        throw new NotImplementedException();
    }
}