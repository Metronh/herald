namespace UserService.Interfaces.Services;

public interface ITokenService
{
    public string GenerateToken(Guid id, string email, bool administrator);
}