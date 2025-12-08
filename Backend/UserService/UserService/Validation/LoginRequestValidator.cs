using FluentValidation;
using UserService.Models.Request;

namespace UserService.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(loginRequest => loginRequest.Username).NotEmpty().Length(min:5,max:50)
            .Matches(@"^[^\s]+$")
            .WithMessage("Username cannot contain whitespace.");
        RuleFor(loginRequest => loginRequest.Password).NotEmpty().Length(min:5,max:50);
    }
}