using Dapper;
using SetUp.Interfaces.Databases;
using SetUp.Interfaces.Repository;
using SetUp.Models;

namespace SetUp.Repository;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IMongoDbConnection _mongoDbConnection;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(IDbConnectionFactory dbConnectionFactory, IMongoDbConnection mongoDbConnection,
        ILogger<UserRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _mongoDbConnection = mongoDbConnection;
        _logger = logger;
    }

    public async Task UploadUser(User user)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(UploadUser), DateTime.UtcNow);
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync("""
                                        INSERT INTO users (Id, Username, Email, FirstName, LastName, Administrator, Password) 
                                        VALUES 
                                            (@Id, @Username, @Email, @FirstName, @LastName, @Administrator, @Password)
                                        """, user);
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(UserRepository), nameof(UploadUser), DateTime.UtcNow);
    }

    public async Task UploadArticles(IEnumerable<Article> articles)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UserRepository), nameof(UploadArticles), DateTime.UtcNow);
        var collection = _mongoDbConnection.GetCollection();
        await collection.InsertManyAsync(articles);
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(UserRepository), nameof(UploadArticles), DateTime.UtcNow);
    }
}