using ArticleService.Entities;
using ArticleService.Interfaces.Repository;
using ArticleService.Interfaces.Services;
using ArticleService.Models.Request;
using ArticleService.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArticleServiceTests.ServicesTest;

public class ArticlesServiceTest
{
    private readonly IArticlesService _articlesService;
    private readonly Mock<IArticlesRepository> _mockArticlesRepository;
    private readonly Mock<ILogger<ArticlesService>> _mockLogger;

    public ArticlesServiceTest()
    {
        _mockArticlesRepository = new Mock<IArticlesRepository>();
        _mockLogger = new Mock<ILogger<ArticlesService>>();
        _articlesService =
            new ArticlesService(articlesRepository: _mockArticlesRepository.Object, logger: _mockLogger.Object);
    }

    [Fact]
    public async Task GetArticleByTitle_WithMatchingTitle_ReturnsArticles()
    {
        // Arrange
        var article = new ArticleEntity
        {
            Id = Guid.NewGuid(),
            Author = "Jane Doe",
            Content = "Content 1",
            DatePublished = DateOnly.FromDateTime(DateTime.UtcNow),
            Title = "Title 1",
        };
        var articles = new List<ArticleEntity>
        {
            article
        };

        _mockArticlesRepository.Setup(x => x.GetArticlesByTitle("Title 1"))
            .ReturnsAsync(articles.Where(x => x.Title.Equals("Title 1")).ToList);

        var request = new GetArticlesByTitleRequest
        {
            PossibleTitle = "Title 1",
        };
        
        // Act 
        var result = await _articlesService.GetArticleByTitle(request);
        
        // Assert
        Assert.True(result.Count.Equals(1));
        Assert.True(result.Exists(x => x.Title.Equals(article.Title)));
    }

    [Fact]
    public async Task GetArticleByTitle_WithNotMatchingTitle_ReturnsEmptyList()
    {
        // Arrange
        var article = new ArticleEntity
        {
            Id = Guid.NewGuid(),
            Author = "Jane Doe",
            Content = "Content 1",
            DatePublished = DateOnly.FromDateTime(DateTime.UtcNow),
            Title = "Title 1",
        };
        var articles = new List<ArticleEntity>
        {
            article
        };

        var title = "NoArticle";
        
        _mockArticlesRepository.Setup(x => x.GetArticlesByTitle(title))
            .ReturnsAsync(articles.Where(x => x.Title.Equals(title)).ToList);

        var request = new GetArticlesByTitleRequest
        {
            PossibleTitle = title,
        };
        
        // Act

        var result = await _articlesService.GetArticleByTitle(request);
        
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateArticle_WithCorrectInformation_Completes()
    {
        // Arrange
        var request = new CreateArticleRequest
        {
            Author = "John Doe",
            Content = "Content",
            DatePublished = DateOnly.FromDateTime(DateTime.UtcNow),
            Title = "Current Title",
        };
        
        // Act
        await _articlesService.CreateArticle(request);
        // Assert
        
        _mockArticlesRepository.Verify(x => x.CreateArticle(It.IsAny<ArticleEntity>()),Times.Once);
    }
}