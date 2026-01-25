using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SetUp.AppSettings;
using SetUp.Interfaces.Databases;
using SetUp.Models;

namespace SetUp.Database;

public class MongoDbConnection : IMongoDbConnection
{
    private readonly ILogger<MongoDbConnection> _logger;
    private readonly IMongoCollection<Article> _collection;

    public MongoDbConnection(IOptions<ConnectionStrings> connectionStrings,
        ILogger<MongoDbConnection> logger)
    {
        _logger = logger;
        var mongoClient = new MongoClient(connectionStrings.Value.MongoDb);
        _collection = mongoClient.GetDatabase("articlesDB").GetCollection<Article>("articles");
        logger.LogInformation("MongoDB connection initialized for collection: {Collection}",
            "articles");
    }

    public IMongoCollection<Article> GetCollection()
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(MongoDbConnection), nameof(GetCollection), DateTime.UtcNow);
        _logger.LogInformation("{Class}.{Method} complete at {Time}",
            nameof(MongoDbConnection), nameof(GetCollection), DateTime.UtcNow);
        return _collection;
    }
}