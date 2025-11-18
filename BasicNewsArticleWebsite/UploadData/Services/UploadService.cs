using UploadData.Interfaces;
using UploadData.Interfaces.Services;
using UploadData.Models;

namespace UploadData.Services;

public class UploadService(IReadArticlesService readArticlesService) : IUploadService
{
    private readonly IReadArticlesService _readArticlesService = readArticlesService;

    public async Task UploadArticles()
    {
        List<Article> articles = _readArticlesService.GetArticles();
    }

    public async Task UploadUsers()
    {
        List<User> users = _readArticlesService.GetUsers();
    }

    public Task CreateDatabase()
    {
        throw new NotImplementedException();
    }
}