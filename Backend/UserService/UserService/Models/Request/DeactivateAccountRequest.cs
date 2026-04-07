namespace UserService.Models.Request;

public record DeactivateAccountRequest
{
    public required string Username { get; set; }
    public required string Password { get; init; }
};