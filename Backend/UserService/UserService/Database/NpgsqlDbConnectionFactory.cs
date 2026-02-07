using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;
using UserService.AppSettings;
using UserService.Interfaces.Database;

namespace UserService.Database;

public class NpgsqlDbConnectionFactory : IDbConnectionFactory
{
    private readonly ConnectionStrings _connectionStrings;
    private readonly ILogger<NpgsqlDbConnectionFactory> _logger;


    public NpgsqlDbConnectionFactory(ConnectionStrings connectionStrings,
        ILogger<NpgsqlDbConnectionFactory> logger)
    {
        _connectionStrings = connectionStrings;
        _logger = logger;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(NpgsqlDbConnectionFactory), nameof(CreateConnectionAsync), DateTime.UtcNow);

        var connection = new NpgsqlConnection(_connectionStrings.PostgresDb);
        await connection.OpenAsync(token);
        
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(NpgsqlDbConnectionFactory), nameof(CreateConnectionAsync), DateTime.UtcNow);

        return connection;
    }
}