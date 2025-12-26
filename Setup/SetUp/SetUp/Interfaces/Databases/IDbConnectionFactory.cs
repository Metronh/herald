using System.Data;
using Npgsql;

namespace SetUp.Interfaces.Databases;

public interface IDbConnectionFactory
{
    Task<NpgsqlConnection> CreateConnectionAsync(CancellationToken token = default);
}