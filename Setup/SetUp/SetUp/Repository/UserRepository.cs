using SetUp.Database;
using SetUp.Interfaces.Databases;
using SetUp.Interfaces.Repository;
using SetUp.Models;

namespace SetUp.Repository;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly UsersDbContext _usersDbContext;

    public UserRepository(IMongoDbConnection mongoDbConnection,
        ILogger<UserRepository> logger, UsersDbContext usersDbContext)
    {
        _logger = logger;
        _usersDbContext = usersDbContext;
    }

    public async Task UploadUsers(List<User> users)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(UploadUsers), DateTime.UtcNow);
        
        _usersDbContext.AddRange(users);
        await _usersDbContext.SaveChangesAsync();
        
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(UserRepository), nameof(UploadUsers), DateTime.UtcNow);
    }
}