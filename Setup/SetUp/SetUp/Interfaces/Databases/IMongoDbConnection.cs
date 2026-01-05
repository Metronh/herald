using MongoDB.Driver;
using SetUp.Models;

namespace SetUp.Interfaces.Databases;

public interface IMongoDbConnection
{
    IMongoCollection<Article> GetCollection();
}