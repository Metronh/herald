using MongoDB.Driver;
using SetUp.Models;

namespace SetUp.Interfaces.Databases;

public interface IMongoDbConnectionFactory
{
    IMongoCollection<Article> GetCollection();
}