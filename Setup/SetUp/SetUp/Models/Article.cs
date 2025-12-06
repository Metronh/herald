using CsvHelper.Configuration.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SetUp.Models;

public record Article
{
    [Ignore][BsonId][BsonGuidRepresentation(GuidRepresentation.Standard)] public Guid Id { get; init; } = Guid.NewGuid();
    [Name("title")][BsonElement("title")] public required string Title { get; init; }
    [Name("author")][BsonElement("author")] public required string Author { get; set; }
    [Name("date_published")][BsonElement("datePublished")] public required DateOnly DatePublished { get; init; }
    [Name("content")][BsonElement("content")] public required string Content { get; init; }
}