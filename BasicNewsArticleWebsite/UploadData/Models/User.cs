using CsvHelper.Configuration.Attributes;

namespace UploadData.Models;

public record User
{
    [Name("username")] public required string Username { get; init; }
    [Name("email")] public required string Email { get; init; }
    [Name("first_name")] public required string FirstName { get; init; }
    [Name("last_name")] public required string LastName { get; init; }
    [Name("password")] public required string Password { get; init; }
    [Ignore] public required Guid Guid { get; init; } = Guid.NewGuid();
}