using CsvHelper.Configuration.Attributes;

namespace SetUp.Models;

public class User
{
    [Name("username")] public required string Username { get; init; }
    [Name("email")] public required string Email { get; init; }
    [Name("first_name")] public required string FirstName { get; init; }
    [Name("last_name")] public required string LastName { get; init; }
    [Name("password")] public required string Password { get; set; }
    [Ignore] public required Guid Id { get; init; } = Guid.NewGuid();
    [Ignore] public required bool Administrator { get; init; } = false;
}