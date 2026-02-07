using System.ComponentModel.DataAnnotations;

namespace ArticleService.AppSettings;

public record ConnectionStrings
{
    [Required]
    public required string MongoDb { get; init; }
    [Required]
    public required string Redis { get; init; }
}