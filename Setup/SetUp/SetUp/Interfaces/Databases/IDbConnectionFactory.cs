using System.Data;

namespace SetUp.Interfaces.Databases;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}