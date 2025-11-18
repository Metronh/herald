using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Options;
using UploadData.AppSettings;
using UploadData.Interfaces;
using UploadData.Interfaces.Services;
using UploadData.Models;

namespace UploadData.Services;

public class ReadFilesService(IOptions<ArticlesLocation> articleLocations, IOptions<UserDataLocation> userDataLocation) : IReadArticlesService
{
    private readonly ArticlesLocation _articlesLocation = articleLocations.Value;
    private readonly UserDataLocation _userDataLocation = userDataLocation.Value;
    public List<Article> GetArticles()
    {
        var articles = new List<Article>();
        try
        {
            articles.Add(new Article(Author: "Author 1", ArticleText: File.ReadAllText(_articlesLocation.Text1)));
            articles.Add(new Article(Author: "Author 2", ArticleText: File.ReadAllText(_articlesLocation.Text2)));
            articles.Add(new Article(Author: "Author 3", ArticleText: File.ReadAllText(_articlesLocation.Text3)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return articles;

    }

    public List<User> GetUsers()
    {
        List<User> users;
        try
        {
            using (var reader = new StreamReader(_userDataLocation.AccountsCsv))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                users = csv.GetRecords<User>().ToList();
            }
        }
        catch
        {
            Console.WriteLine("Error here");
            throw;
        }

        return users;
    }
}