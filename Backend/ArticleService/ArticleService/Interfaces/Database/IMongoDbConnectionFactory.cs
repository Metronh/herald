using ArticleService.Entities;
using MongoDB.Driver;

namespace ArticleService.Interfaces.Database;

public interface IMongoDbConnectionFactory
{
    IMongoCollection<ArticleEntity> GetCollection();
}