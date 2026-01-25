using SetUp.Interfaces.Repository;
using SetUp.Interfaces.Services;

namespace SetUp.Services;

public class DeleteService : IDeleteService
{
    private readonly ILogger<DeleteService> _logger;
    private readonly IArticleRepository _articleRepository;
    private readonly IUserRepository _userRepository;

    public DeleteService(IArticleRepository articleRepository, ILogger<DeleteService> logger, IUserRepository userRepository)
    {
        _articleRepository = articleRepository;
        _logger = logger;
        _userRepository = userRepository;
    }
    
    public async Task DeleteAllArticles()
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(DeleteService), nameof(DeleteAllArticles), DateTime.UtcNow);
        await _articleRepository.DeleteArticles();
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(DeleteService), nameof(DeleteAllArticles), DateTime.UtcNow);
    }

    public async Task DeleteAllUsers()
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(DeleteService), nameof(DeleteAllUsers), DateTime.UtcNow);
        await _userRepository.DeleteAllUsers();
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(DeleteService), nameof(DeleteAllUsers), DateTime.UtcNow);
    }
}