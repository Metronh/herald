using System.Data;

namespace UploadData.Database.Interfaces;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}