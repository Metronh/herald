namespace UploadData.Interfaces.Services;

public interface IUploadService
{
    public Task UploadArticles();
    public Task UploadUsers();
    public Task CreateDatabase();
}