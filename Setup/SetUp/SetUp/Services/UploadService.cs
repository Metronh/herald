using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SetUp.AppSettings;
using SetUp.Database;
using SetUp.Helpers;
using SetUp.Interfaces.Helpers;
using SetUp.Interfaces.Repository;
using SetUp.Interfaces.Services;
using SetUp.Models;

namespace SetUp.Services;

public class UploadService : IUploadService
{
    private readonly IUserRepository _userRepository;
    private readonly IArticleRepository _articleRepository;
    private readonly CsvLocations _csvLocations;
    private readonly IReadCsvHelper _readCsvHelper;
    private readonly ILogger<UploadService> _logger;


    public UploadService(IOptions<CsvLocations> csvLocation, IUserRepository userRepository,
        ILogger<UploadService> logger, IArticleRepository articleRepository, IReadCsvHelper readCsvHelper)
    {
        _userRepository = userRepository;
        _logger = logger;
        _articleRepository = articleRepository;
        _readCsvHelper = readCsvHelper;
        _csvLocations = csvLocation.Value;
    }

    public async Task UploadArticles()
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UploadService), nameof(UploadArticles), DateTime.UtcNow);
        var articles = _readCsvHelper.GetItemsFromCsv<Article>(_csvLocations.ArticlesCsv).ToList();
        await _articleRepository.UploadArticles(articles);
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(UploadService), nameof(UploadArticles), DateTime.UtcNow);
    }

    public async Task UploadUsers()
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UploadService), nameof(UploadUsers), DateTime.UtcNow);
        var users = _readCsvHelper.GetItemsFromCsv<User>(_csvLocations.UsersCsv).ToList();
        var passwordHasher = new PasswordHasher<User>();

        Parallel.ForEach(users,
            user => user.Password = passwordHasher.HashPassword(user: user, password: user.Password));

        await _userRepository.UploadUsers(users);

        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(UploadService), nameof(UploadUsers), DateTime.UtcNow);
    }
}