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

    public ArticlesService(IMongoDbConnectionFactory mongoDbConnectionFactory)
    {
        _mongoDbConnectionFactory = mongoDbConnectionFactory;
    }


    public async Task<List<ArticleResponse>> GetArticleByTitle(GetArticlesByTitleRequest request)
    {
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

        return response;
    }

    public async Task CreateArticle(CreatArticleRequest request)
    {
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
    }
}