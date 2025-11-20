using MongoDB.Driver;
using UploadData.Models;

namespace UploadData.Database.Interfaces;

public interface IMongoDbConnectionFactory
{
    IMongoCollection<Article> GetCollection();
}