using CommunicationsFunctions.Interfaces.Database;
using CommunicationsFunctions.Interfaces.Repository;
using CommunicationsFunctions.Models.Entities;
using CommunicationsFunctions.Models.Enums;
using Dapper;

namespace CommunicationsFunctions.Repository;

public class UsersRepository : IUsersRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UsersRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task RegisterCommunication(CommunicationEntity communicationEntity)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
                                      INSERT INTO communications (communication_id, 
                                                                  user_id,
                                                                  created_at, 
                                                                  communication_type, 
                                                                  communication_title, 
                                                                  communication_address, 
                                                                  sent_at, 
                                                                  status) 
                                      VALUES 
                                      (@CommunicationId, @UserId, @CreatedAt, @CommunicationType, @CommunicationTitle, @CommunicationAddress, @SentAt, @Status)
                                      """, communicationEntity);
    }

    public async Task SuccessfulCommunications(Guid communicationId)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        string sql = """
                     UPDATE communications
                     SET status = @Status,
                         sent_at = @SentAt
                     WHERE communication_id = @CommunicationId
                     """;
        await connection.ExecuteAsync(sql, new
        {
            CommunicationId = communicationId,
            Status = nameof(CommunicationStatus.Sent),
            SentAt = DateTime.UtcNow,
        });
    }
}