using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;
using UploadData.AppSettings;
using UploadData.Interfaces.Database;

namespace UploadData.Database.Implementations;

public class NpgsqlDbConnectionFactory : IDbConnectionFactory
{
    private readonly ConnectionStrings _connectionStrings;

    public NpgsqlDbConnectionFactory(IOptions<ConnectionStrings> connectionStrings)
    {
        _connectionStrings = connectionStrings.Value;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var connection = new NpgsqlConnection(_connectionStrings.PostgresDb);
        await connection.OpenAsync(token);
        return connection;
    }
}