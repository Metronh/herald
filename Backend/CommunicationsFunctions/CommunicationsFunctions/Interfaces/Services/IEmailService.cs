using CommunicationsFunctions.Models;

namespace CommunicationsFunctions.Interfaces.Services;

public interface IEmailService
{
    public Task SendEmail(SendEmailRequest request);
}