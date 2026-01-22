namespace UserService.Entities;

public class LoginSessionEntity
{
    public Guid LoginSessionId { get; init; }
    public Guid UserId { get; init; }
    public DateTime LoginTime { get; init; }
    public DateTime? LogoutTime { get; init; }
    public bool SessionActive { get; set; }
}