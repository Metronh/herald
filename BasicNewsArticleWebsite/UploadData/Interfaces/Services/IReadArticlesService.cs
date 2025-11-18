using UploadData.Models;

namespace UploadData.Interfaces.Services;

public interface IReadArticlesService
{
    public List<Article> GetArticles();
    public List<User> GetUsers();
}