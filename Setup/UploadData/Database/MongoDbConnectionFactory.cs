using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UploadData.AppSettings;
using UploadData.Interfaces.Database;
using UploadData.Models;

namespace UploadData.Database;

public class MongoDbConnectionFactory : IMongoDbConnectionFactory
{
    private readonly ConnectionStrings _connectionStrings;
    private readonly ILogger<MongoDbConnectionFactory> _logger;

    public MongoDbConnectionFactory(IOptions<ConnectionStrings> connectionStrings,
        ILogger<MongoDbConnectionFactory> logger)
    {
        _logger = logger;
        _connectionStrings = connectionStrings.Value;
    }

    public IMongoCollection<Article> GetCollection()
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(MongoDbConnectionFactory), nameof(GetCollection), DateTime.UtcNow);
        var mongoClient = new MongoClient(connectionString: _connectionStrings.MongoDb);
        var collection = mongoClient.GetDatabase("articlesDB").GetCollection<Article>("articles");
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(MongoDbConnectionFactory), nameof(GetCollection), DateTime.UtcNow);
        return collection;
    }
}