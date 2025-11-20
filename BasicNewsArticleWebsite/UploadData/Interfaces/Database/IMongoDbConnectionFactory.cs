using MongoDB.Driver;
using UploadData.Models;

namespace UploadData.Interfaces.Database;

public interface IMongoDbConnectionFactory
{
    IMongoCollection<Article> GetCollection();
}