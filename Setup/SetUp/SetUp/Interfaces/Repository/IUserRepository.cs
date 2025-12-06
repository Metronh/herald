using SetUp.Models;

namespace SetUp.Interfaces.Repository;

public interface IUserRepository
{
    public Task UploadUser(User user);
    public Task UploadArticles(List<Article> articles);
}