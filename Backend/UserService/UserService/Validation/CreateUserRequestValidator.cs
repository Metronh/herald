using FluentValidation;
using UserService.Models.Request;

namespace UserService.Validation;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(user => user.Username).NotEmpty().Length(min:5,max:50)
            .Matches(@"^[^\s]+$")
            .WithMessage("Email cannot contain whitespace.");
        RuleFor(user => user.FirstName).NotEmpty().Length(min:2,max:50);
        RuleFor(user => user.LastName).NotEmpty().Length(min:2,max:50);
        RuleFor(user => user.Password).NotEmpty().Length(min:5,max:50);
        RuleFor(user => user.Email).NotEmpty().EmailAddress().Length(min:5,max:254);
    }
}