using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;

namespace SetUp.Models;

public class User
{
    [Column("username")][MaxLength(50)][Name("username")] public required string Username { get; init; }
    [Column("email")][MaxLength(100)][Name("email")] public required string Email { get; init; }
    [Column("firstname")][MaxLength(50)][Name("first_name")] public required string FirstName { get; init; }
    [Column("lastname")][MaxLength(50)][Name("last_name")] public required string LastName { get; init; }
    [Column("password")][MaxLength(200)][Name("password")] public required string Password { get; set; }
    [Column("id")][Key][Ignore] public required Guid Id { get; init; } = Guid.NewGuid();
    [Column("administrator")][Ignore] public required bool Administrator { get; init; } = false;
}