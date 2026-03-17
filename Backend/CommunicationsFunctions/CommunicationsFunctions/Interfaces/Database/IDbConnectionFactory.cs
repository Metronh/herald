using System.Data;

namespace CommunicationsFunctions.Interfaces.Database;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}