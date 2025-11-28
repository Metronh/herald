using ArticleService.Entities;
using ArticleService.Interfaces.Database;
using ArticleService.Interfaces.Services;
using ArticleService.Models.Request;
using ArticleService.Models.Response;
using MongoDB.Driver;

namespace ArticleService.Services;

public class ArticlesService : IArticlesService
{
    private readonly IMongoDbConnectionFactory _mongoDbConnectionFactory;
    private readonly ILogger<ArticlesService> _logger;

    public ArticlesService(IMongoDbConnectionFactory mongoDbConnectionFactory, ILogger<ArticlesService> logger)
    {
        _mongoDbConnectionFactory = mongoDbConnectionFactory;
        _logger = logger;
    }


    public async Task<List<ArticleResponse>> GetArticleByTitle(GetArticlesByTitleRequest request)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(ArticlesService), nameof(GetArticleByTitle), DateTime.UtcNow);
        var collection = _mongoDbConnectionFactory.GetCollection();
        
        List<ArticleEntity> articleSearchResult =
            await collection.Find(a => a.Title.Equals(request.PossibleTitle)).ToListAsync();

        var response = new List<ArticleResponse>();

        foreach (var articleEntity in articleSearchResult)
        {
            response.Add(new ArticleResponse
            {
                Author = articleEntity.Author,
                Title = articleEntity.Title,
                DatePublished = articleEntity.DatePublished,
                Content = articleEntity.Content,
            });
        }
        _logger.LogInformation("{Class}.{Method} complete at {Time}",
            nameof(ArticlesService), nameof(GetArticleByTitle), DateTime.UtcNow);
        return response;
    }

    public async Task CreateArticle(CreateArticleRequest request)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(ArticlesService), nameof(CreateArticle), DateTime.UtcNow);
        var collection = _mongoDbConnectionFactory.GetCollection();
        var article = new ArticleEntity
        {
            Author = request.Author,
            Content = request.Content,
            DatePublished = request.DatePublished,
            Title = request.Title,
            Id = Guid.NewGuid()
        };

        await collection.InsertOneAsync(article);
        
        _logger.LogInformation("{Class}.{Method} complete at {Time}",
            nameof(ArticlesService), nameof(CreateArticle), DateTime.UtcNow);
    }
}