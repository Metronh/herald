using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UploadData.AppSettings;
using UploadData.Database.Interfaces;
using UploadData.Models;

namespace UploadData.Database.Implementations;

public class MongoDbConnectionFactory : IMongoDbConnectionFactory
{
    private readonly ConnectionStrings _connectionStrings;

    public MongoDbConnectionFactory(IOptions<ConnectionStrings> connectionStrings)
    {
        _connectionStrings = connectionStrings.Value;
    }

    public IMongoCollection<Article> GetCollection()
    {
        var mongoClient = new MongoClient(connectionString: _connectionStrings.MongoDb);
        var collection = mongoClient.GetDatabase("articlesDB").GetCollection<Article>("articles");
        return collection;
    }
}