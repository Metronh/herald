namespace UserService.Models.Response;

public class LoginResponse : BaseResponse
{
    public bool Success { get; set; }
    public bool AccountLocked { get; set; }
    public string? Token { get; set; }
};