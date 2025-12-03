namespace UserService.Entities;

public class UserEntity
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required bool Administrator { get; init; }
    public string? Password { get; set; }
    public required DateOnly CreatedAt { get; init; }
}