using ArticleService.Models.Request;
using ArticleService.Models.Response;

namespace ArticleService.Interfaces.Services;

public interface IArticlesService
{
    public Task<List<ArticleResponse>> GetArticleByTitle(GetArticlesByTitleRequest request);
    public Task CreateArticle(CreateArticleRequest request);
}