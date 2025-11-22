using ArticleService.AppSettings;
using ArticleService.Entities;
using ArticleService.Interfaces.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ArticleService.Database;

public class MongoDbConnectionFactory : IMongoDbConnectionFactory
{
    private readonly ConnectionStrings _connectionStrings;
    private readonly string _database = "articlesDB";
    private readonly string _collection = "articles";

    public MongoDbConnectionFactory(IOptions<ConnectionStrings> connectionStrings)
    {
        _connectionStrings = connectionStrings.Value;
    }

    public IMongoCollection<ArticleEntity> GetCollection()
    {
        var mongoClient = new MongoClient(connectionString: _connectionStrings.MongoDb);
        var collection = mongoClient.GetDatabase(_database).GetCollection<ArticleEntity>(_collection);
        return collection;
    }
}