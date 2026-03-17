using CommunicationsFunctions.Models.Entities;

namespace CommunicationsFunctions.Interfaces.Repository;

public interface IUsersRepository
{
    public Task RegisterCommunication(CommunicationEntity communicationEntity);
    public Task SuccessfulCommunications(Guid communicationId);
}