using Dapper;
using UserService.Entities;
using UserService.Interfaces.Database;
using UserService.Interfaces.Repository;

namespace UserService.Repository;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<UserRepository> _logger;
    
    public UserRepository(IDbConnectionFactory connectionFactory, ILogger<UserRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task CreateUser(UserEntity user)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(CreateUser), DateTime.UtcNow);
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(
            """
            INSERT INTO users (id, username, email, first_name, last_name, administrator, password) 
            VALUES 
                (@Id, @Username, @Email, @FirstName, @LastName, @Administrator, @Password)
            """, user
        );

        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(UserRepository), nameof(CreateUser), DateTime.UtcNow);
    }

    public async Task<UserEntity?> GetUserLoginDetailsByUsername(string username)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(GetUserLoginDetailsByUsername), DateTime.UtcNow);

        using var connection = await _connectionFactory.CreateConnectionAsync();


        var user = await connection.QueryFirstOrDefaultAsync<UserEntity>("""
                                                                         SELECT * FROM users WHERE username = @Username
                                                                         """, new { Username = username });


        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(UserRepository), nameof(GetUserLoginDetailsByUsername), DateTime.UtcNow);
        return user;
    }

    public async Task RegisterLogin(LoginSessionEntity loginSession)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(RegisterLogin), DateTime.UtcNow);
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("""
                                            INSERT INTO login_sessions (login_session_id, user_id, login_time, logout_time) 
                                            VALUES (@LoginSessionId, @UserId, @LoginTime, @LogoutTime)
                                            """, loginSession);
    }

    public async Task UpdateLoginAttempts(UserEntity userEntity)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(UpdateLoginAttempts), DateTime.UtcNow);
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("""
                                      UPDATE users 
                                      SET failed_login_attempts = @FailedLoginAttempts
                                      WHERE id = @Id
                                      """, userEntity);
    }

    public async Task LockAccount(UserEntity userEntity)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(LockAccount), DateTime.UtcNow);
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("""
                                      UPDATE users
                                      SET locked_out = true
                                      WHERE id = @Id
                                      """,  userEntity);
    }

    public async Task SessionCleanUp()
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(SessionCleanUp), DateTime.UtcNow);
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("""
                                            UPDATE login_sessions 
                                            SET session_active = false 
                                            WHERE logout_time < @CurrentTime
                                            """, new {CurrentTime = DateTime.UtcNow});
    }

    public async Task DeactivateAccount(UserEntity userEntity)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(DeactivateAccount), DateTime.UtcNow);
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("""
                                      UPDATE users
                                      SET is_active = false
                                      WHERE id = @Id
                                      """, userEntity);
    }
}