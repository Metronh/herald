using Dapper;
using Microsoft.Extensions.Options;
using UserService.AppSettings;
using UserService.Entities;
using UserService.Interfaces.Database;
using UserService.Interfaces.Repository;

namespace UserService.Repository;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<UserRepository> _logger;
    private readonly JwtInformation _jwtInformation;

    public UserRepository(IDbConnectionFactory connectionFactory, ILogger<UserRepository> logger,
        JwtInformation jwtInformation)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _jwtInformation = jwtInformation;
    }

    public async Task CreateUser(UserEntity user)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(CreateUser), DateTime.UtcNow);
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(
            """
            INSERT INTO users (id, username, email, first_name, last_name, Administrator, Password) 
            VALUES 
                (@Id, @Username, @Email, @FirstName, @LastName, @Administrator, @Password)
            """, user
        );

        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(UserRepository), nameof(CreateUser), DateTime.UtcNow);
    }

    public async Task<UserEntity?> GetUserLoginDetails(string username)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(GetUserLoginDetails), DateTime.UtcNow);

        using var connection = await _connectionFactory.CreateConnectionAsync();


        var user = await connection.QueryFirstOrDefaultAsync<UserEntity>("""
                                                                         SELECT * FROM users WHERE username = @Username
                                                                         """, new { Username = username });


        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(UserRepository), nameof(GetUserLoginDetails), DateTime.UtcNow);
        return user;
    }

    public async Task RegisterLogin(UserEntity user)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(RegisterLogin), DateTime.UtcNow);
        var loginSessionEntity = new LoginSessionEntity
        {
            LoginSessionId = Guid.NewGuid(),
            LoginTime = DateTime.UtcNow,
            UserId = user.Id,
            LogoutTime = DateTime.UtcNow.AddMinutes(_jwtInformation.ExpiryTime)
        };
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("""
                                            INSERT INTO login_sessions (login_session_id, user_id, login_time, logout_time) 
                                            VALUES (@LoginSessionId, @UserId, @LoginTime, @LogoutTime)
                                            """, loginSessionEntity);
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
                                            """, new{CurrentTime = DateTime.UtcNow});
    }
}