using System.Data;
using CommunicationsFunctions.AppSettings;
using CommunicationsFunctions.Interfaces.Database;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace CommunicationsFunctions.Database;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly ILogger<NpgsqlConnectionFactory> _logger;
    private readonly ConnectionStrings _connectionStrings;
    
    public NpgsqlConnectionFactory(ConnectionStrings connectionStrings, ILogger<NpgsqlConnectionFactory> logger)
    {
        _connectionStrings = connectionStrings;
        _logger = logger;
    }
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var connection = new NpgsqlConnection(_connectionStrings.PostgresDb);
        await connection.OpenAsync(cancellationToken: token);
        return connection;
    }
}