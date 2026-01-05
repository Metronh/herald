using ArticleService.Entities;
using ArticleService.Interfaces.Database;
using ArticleService.Interfaces.Repository;
using MongoDB.Driver;

namespace ArticleService.Repository;

public class ArticlesRepository : IArticlesRepository
{
    private readonly IMongoDbConnection _mongoDbConnection;
    private readonly ILogger<ArticlesRepository> _logger;

    public ArticlesRepository(IMongoDbConnection mongoDbConnection, ILogger<ArticlesRepository> logger)
    {
        _mongoDbConnection = mongoDbConnection;
        _logger = logger;
    }

    public async Task<List<ArticleEntity>> GetArticlesByTitle(string possibleTitle)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(ArticlesRepository), nameof(GetArticlesByTitle), DateTime.UtcNow);
        var collection = _mongoDbConnection.GetCollection();
        
        List<ArticleEntity> articleSearchResult =
            await collection.Find(a => a.Title.Equals(possibleTitle)).ToListAsync();
        
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(ArticlesRepository), nameof(GetArticlesByTitle), DateTime.UtcNow);
        return articleSearchResult;
    }

    public async Task CreateArticle(ArticleEntity article)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(ArticlesRepository), nameof(CreateArticle), DateTime.UtcNow);

        var collection = _mongoDbConnection.GetCollection();
        
        await collection.InsertOneAsync(article);
        
        
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(ArticlesRepository), nameof(GetArticlesByTitle), DateTime.UtcNow);
    }
}