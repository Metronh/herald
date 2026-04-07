using UserService.Entities;

namespace UserService.Interfaces.Repository;

public interface IUserRepository
{
    public Task CreateUser(UserEntity user);
    public Task<UserEntity?> GetUserLoginDetailsByUsername(string username);
    public Task RegisterLogin(LoginSessionEntity loginSession);
    public Task UpdateLoginAttempts(UserEntity userEntity);
    public Task LockAccount(UserEntity userEntity);
    public Task SessionCleanUp();
    public Task DeactivateAccount(UserEntity userEntity);
}