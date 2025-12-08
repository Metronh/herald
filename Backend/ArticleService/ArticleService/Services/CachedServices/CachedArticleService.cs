using ArticleService.Interfaces.Services;
using ArticleService.Models.Request;
using ArticleService.Models.Response;
using Microsoft.Extensions.Caching.Hybrid;

namespace ArticleService.Services.CachedServices;

public class CachedArticleService : IArticlesService
{
    private readonly IArticlesService _articlesService;
    private readonly HybridCache _hybridCache;
    private readonly ILogger<CachedArticleService> _logger;

    public CachedArticleService(IArticlesService articlesService, HybridCache hybridCache,
        ILogger<CachedArticleService> logger)
    {
        _articlesService = articlesService;
        _hybridCache = hybridCache;
        _logger = logger;
    }

    public async Task<List<ArticleResponse>> GetArticleByTitle(GetArticlesByTitleRequest request)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(CachedArticleService), nameof(GetArticleByTitle), DateTime.UtcNow);
        
        List<ArticleResponse> result = await _hybridCache.GetOrCreateAsync(request.PossibleTitle,
            async entry => await _articlesService.GetArticleByTitle(request),
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(2)
            }, tags:["Articles"]);

        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(CachedArticleService), nameof(GetArticleByTitle), DateTime.UtcNow);
        return result;
    }

    public async Task CreateArticle(CreateArticleRequest request)
    {
        await _articlesService.CreateArticle(request);
    }
}