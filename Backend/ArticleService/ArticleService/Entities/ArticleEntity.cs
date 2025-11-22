using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ArticleService.Entities;

public record ArticleEntity
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; init; } = Guid.NewGuid();
    [BsonElement("title")] 
    public required string Title { get; init; }
    [BsonElement("author")] 
    public required string Author { get; set; }
    [BsonElement("datePublished")] 
    public required DateOnly DatePublished { get; init; }
    [BsonElement("content")] 
    public required string Content { get; init; }
};