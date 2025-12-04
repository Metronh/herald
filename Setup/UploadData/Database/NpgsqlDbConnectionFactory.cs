using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;
using UploadData.AppSettings;
using UploadData.Interfaces.Database;

namespace UploadData.Database;

public class NpgsqlDbConnectionFactory : IDbConnectionFactory
{
    private readonly ConnectionStrings _connectionStrings;
    private readonly ILogger<NpgsqlDbConnectionFactory> _logger;

    public NpgsqlDbConnectionFactory(IOptions<ConnectionStrings> connectionStrings,
        ILogger<NpgsqlDbConnectionFactory> logger)
    {
        _logger = logger;
        _connectionStrings = connectionStrings.Value;
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