namespace ArticleService.Models.Request;

public record GetArticlesByTitleRequest
{
    public required string PossibleTitle { get; set; }
}