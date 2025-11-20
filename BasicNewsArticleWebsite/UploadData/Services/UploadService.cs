using Dapper;
using Microsoft.Extensions.Options;
using UploadData.AppSettings;
using UploadData.Database;
using UploadData.Helpers;
using UploadData.Interfaces;
using UploadData.Interfaces.Helpers;
using UploadData.Interfaces.Repository;
using UploadData.Interfaces.Services;
using UploadData.Models;

namespace UploadData.Services;

public class UploadService : IUploadService
{
    private readonly IUserRepository _userRepository;
    private readonly CsvLocations _csvLocations;
    

    public UploadService(IOptions<CsvLocations> csvLocation ,IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _csvLocations = csvLocation.Value;
    }

    public async Task UploadArticles()
    {
        IReadCsvHelper<Article> readCsvHelper = new ReadCsvHelper<Article>();
        List<Article> articles = readCsvHelper.GetItemsFromCsv(_csvLocations.ArticlesCsv);
        await _userRepository.UploadArticles(articles: articles);
    }

    public async Task UploadUsers()
    {
        IReadCsvHelper<User> readCsvHelper = new ReadCsvHelper<User>();
        List<User> users = readCsvHelper.GetItemsFromCsv(_csvLocations.UsersCsv);
        foreach (var user in users)
        {
            await _userRepository.UploadUser(user: user);
        }
    }
}