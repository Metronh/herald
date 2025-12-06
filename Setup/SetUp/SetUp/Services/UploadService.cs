using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SetUp.AppSettings;
using SetUp.Helpers;
using SetUp.Interfaces.Helpers;
using SetUp.Interfaces.Repository;
using SetUp.Interfaces.Services;
using SetUp.Models;

namespace SetUp.Services;

public class UploadService : IUploadService
{
    private readonly IUserRepository _userRepository;
    private readonly CsvLocations _csvLocations;
    private readonly ILogger<UploadService> _logger;


    public UploadService(IOptions<CsvLocations> csvLocation, IUserRepository userRepository,
        ILogger<UploadService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
        _csvLocations = csvLocation.Value;
    }

    public async Task UploadArticles()
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UploadService), nameof(UploadArticles), DateTime.UtcNow);
        IReadCsvHelper<Article> readCsvHelper = new ReadCsvHelper<Article>();
        List<Article> articles = readCsvHelper.GetItemsFromCsv(_csvLocations.ArticlesCsv);
        await _userRepository.UploadArticles(articles: articles);
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(UploadService), nameof(UploadUsers), DateTime.UtcNow);
    }

    public async Task UploadUsers()
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(UploadService), nameof(UploadUsers), DateTime.UtcNow);
        IReadCsvHelper<User> readCsvHelper = new ReadCsvHelper<User>();
        List<User> users = readCsvHelper.GetItemsFromCsv(_csvLocations.UsersCsv);
        var passwordHasher = new PasswordHasher<User>();

        Parallel.ForEach(users,
            user => user.Password = passwordHasher.HashPassword(user: user, password: user.Password));

        var numberOfUsersUploaded = 0;
        foreach (var user in users)
        {
            await _userRepository.UploadUser(user: user);
            numberOfUsersUploaded++;
        }

        _logger.LogInformation("{Class}.{Method} uploaded {numberOfUsersUploaded} users", nameof(UploadService),
            nameof(UploadUsers), numberOfUsersUploaded);
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(UploadService), nameof(UploadUsers), DateTime.UtcNow);
    }
}