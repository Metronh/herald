using ArticleService.Entities;
using MongoDB.Driver;

namespace ArticleService.Interfaces.Database;

public interface IMongoDbConnection
{
    IMongoCollection<ArticleEntity> GetCollection();
}