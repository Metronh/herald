using SetUp.Models;

namespace SetUp.Interfaces.Repository;

public interface IUserRepository
{
    public Task UploadUsers(List<User> user);
    public Task DeleteAllUsers();
}