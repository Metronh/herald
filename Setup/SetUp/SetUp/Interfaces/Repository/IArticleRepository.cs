using SetUp.Models;

namespace SetUp.Interfaces.Repository;

public interface IArticleRepository
{
    public Task UploadArticles(List<Article> articles);
    public Task DeleteArticles();
}