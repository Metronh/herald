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
        try
        {
            using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync();
            await dbConnection.ExecuteAsync("""
                                            INSERT INTO users (Id, Username, Email, FirstName, LastName, Password) 
                                            VALUES 
                                                (@Id, @Username, @Email, @FirstName, @LastName, @Password)
                                            """, user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task UploadArticles(List<Article> articles)
    {
        try
        {
            var collection = _mongoDbConnectionFactory.GetCollection();
                await collection.InsertManyAsync(articles);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}