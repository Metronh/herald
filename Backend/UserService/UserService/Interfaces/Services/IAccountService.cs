using UserService.Models.Request;
using UserService.Models.Response;

namespace UserService.Interfaces.Services;

public interface IAccountService
{
    public Task<CreateUserResponse> CreateUser(CreateUserRequest request, bool isAdministrator = false);
    public Task<LoginResponse> Login(LoginRequest request);
    public Task<DeactivateAccountResponse> DeactivateAccount(DeactivateAccountRequest request);
}