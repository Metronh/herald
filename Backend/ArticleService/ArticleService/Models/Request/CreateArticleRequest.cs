namespace ArticleService.Models.Request;

public record CreateArticleRequest
{
    public required string Author { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required DateOnly DatePublished { get; init; }
}