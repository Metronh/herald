using UserService.Entities;

namespace UserService.Interfaces.Repository;

public interface IUserRepository
{
    public Task CreateUser(UserEntity user);
    public Task<UserEntity?> GetUserLoginDetails(string username);
    public Task RegisterLogin(UserEntity user);
    public Task SessionCleanUp();
}