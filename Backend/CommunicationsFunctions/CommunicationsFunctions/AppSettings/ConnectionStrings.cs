using System.ComponentModel.DataAnnotations;

namespace CommunicationsFunctions.AppSettings;

public record ConnectionStrings
{
    [Required]
    public required string PostgresDb { get; init; }
}