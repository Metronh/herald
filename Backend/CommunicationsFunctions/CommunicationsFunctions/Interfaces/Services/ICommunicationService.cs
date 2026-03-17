using CommunicationsFunctions.Models;

namespace CommunicationsFunctions.Interfaces.Services;

public interface ICommunicationService
{
    public Task SendWelcomeEmailCommunication(SendWelcomeEmailRequest request);
}