using ArticleService.AppSettings;
using ArticleService.Entities;
using ArticleService.Interfaces.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ArticleService.Database;

public class MongoDbConnectionFactory : IMongoDbConnectionFactory
{
    private readonly ConnectionStrings _connectionStrings;
    private readonly ILogger<MongoDbConnectionFactory> _logger;
    private readonly string _database = "articlesDB";
    private readonly string _collection = "articles";

    public MongoDbConnectionFactory(IOptions<ConnectionStrings> connectionStrings,
        ILogger<MongoDbConnectionFactory> logger)
    {
        _logger = logger;
        _connectionStrings = connectionStrings.Value;
    }

    public IMongoCollection<ArticleEntity> GetCollection()
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(MongoDbConnectionFactory), nameof(GetCollection), DateTime.UtcNow);
        
        var mongoClient = new MongoClient(connectionString: _connectionStrings.MongoDb);
        var collection = mongoClient.GetDatabase(_database).GetCollection<ArticleEntity>(_collection);
        
        _logger.LogInformation("{Class}.{Method} complete at {Time}",
            nameof(MongoDbConnectionFactory), nameof(GetCollection), DateTime.UtcNow);
        return collection;
    }
}