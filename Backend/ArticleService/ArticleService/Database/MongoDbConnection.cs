using ArticleService.AppSettings;
using ArticleService.Entities;
using ArticleService.Interfaces.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ArticleService.Database;

public class MongoDbConnection : IMongoDbConnection
{
    private readonly ILogger<MongoDbConnection> _logger;
    private readonly string _database = "articlesDB";
    private readonly string _collectionName = "articles";
    private readonly IMongoCollection<ArticleEntity> _collection;

    public MongoDbConnection(IOptions<ConnectionStrings> connectionStrings,
        ILogger<MongoDbConnection> logger)
    {
        _logger = logger;
        var mongoClient = new MongoClient(connectionString: connectionStrings.Value.MongoDb);
        _collection = mongoClient.GetDatabase(_database).GetCollection<ArticleEntity>(_collectionName);
        logger.LogInformation("MongoDB connection initialized for collection: {Collection}",
            "articles");
    }

    public IMongoCollection<ArticleEntity> GetCollection()
    {
        return _collection;
    }
}