using CommunicationsFunctions.Models;
using FluentValidation;

namespace CommunicationsFunctions.Validators;

public class SendEmailRequestValidator : AbstractValidator<SendWelcomeEmailRequest>
{
    public SendEmailRequestValidator()
    {
        RuleFor(sendEmailRequest => sendEmailRequest.Email).EmailAddress();
        RuleFor(sendEmailRequest => sendEmailRequest.Email).NotEmpty();
    }
}