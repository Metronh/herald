using MongoDB.Driver;
using SetUp.Interfaces.Databases;
using SetUp.Interfaces.Repository;
using SetUp.Models;

namespace SetUp.Repository;

public class ArticleRepository : IArticleRepository
{
    private readonly ILogger<ArticleRepository> _logger;
    private readonly IMongoDbConnection _mongoDbConnection;
    
    public ArticleRepository(ILogger<ArticleRepository> logger, IMongoDbConnection mongoDbConnection)
    {
        _logger = logger;
        _mongoDbConnection = mongoDbConnection;
    }
    
    public async Task UploadArticles(List<Article> articles)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(ArticleRepository), nameof(UploadArticles), DateTime.UtcNow);
        var collection = _mongoDbConnection.GetCollection();
        await collection.InsertManyAsync(articles);
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(ArticleRepository), nameof(UploadArticles), DateTime.UtcNow);
    }

    public async Task DeleteArticles()
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(ArticleRepository), nameof(DeleteArticles), DateTime.UtcNow);
        var collection = _mongoDbConnection.GetCollection();
        await collection.DeleteManyAsync(FilterDefinition<Article>.Empty);
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(ArticleRepository), nameof(DeleteArticles), DateTime.UtcNow);
    }
}