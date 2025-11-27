using UploadData.Interfaces.Repository;
using UploadData.Models;
using Dapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UploadData.AppSettings;
using UploadData.Database;
using UploadData.Interfaces.Database;

namespace UploadData.Repository;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IMongoDbConnectionFactory _mongoDbConnectionFactory;

    public UserRepository(IDbConnectionFactory dbConnectionFactory, IMongoDbConnectionFactory mongoDbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _mongoDbConnectionFactory = mongoDbConnectionFactory;
    }

    public async Task UploadUser(User user)
    {
        using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync("""
                                        INSERT INTO users (Id, Username, Email, FirstName, LastName, Password) 
                                        VALUES 
                                            (@Id, @Username, @Email, @FirstName, @LastName, @Password)
                                        """, user);
    }

    public async Task UploadArticles(List<Article> articles)
    {
        var collection = _mongoDbConnectionFactory.GetCollection();
        await collection.InsertManyAsync(articles);
    }
}