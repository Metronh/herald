namespace SetUp.Interfaces.Services;

public interface IDeleteService
{
    public Task DeleteAllArticles();
    public Task DeleteAllUsers();
}