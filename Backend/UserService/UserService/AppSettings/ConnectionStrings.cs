namespace UserService.AppSettings;

public record ConnectionStrings
{
    public required string PostgresDb { get; init; }
}