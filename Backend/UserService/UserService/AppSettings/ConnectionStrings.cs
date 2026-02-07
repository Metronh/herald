using System.ComponentModel.DataAnnotations;

namespace UserService.AppSettings;

public record ConnectionStrings
{
    [Required]
    public required string PostgresDb { get; init; }
}