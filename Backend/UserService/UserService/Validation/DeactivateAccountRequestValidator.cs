using FluentValidation;
using UserService.Models.Request;

namespace UserService.Validation;

public class DeactivateAccountRequestValidator : AbstractValidator<DeactivateAccountRequest>
{
    public DeactivateAccountRequestValidator()
    {
        RuleFor(loginRequest => loginRequest.Username).NotEmpty().Length(min:5,max:50)
            .Matches(@"^[^\s]+$")
            .WithMessage("Username cannot contain whitespace.");
        RuleFor(user => user.Password).NotEmpty().Length(min:5,max:50);
    }
}