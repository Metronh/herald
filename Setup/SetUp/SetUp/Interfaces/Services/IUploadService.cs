namespace SetUp.Interfaces.Services;

public interface IUploadService
{
    public Task UploadArticles();
    public Task UploadUsers();
}