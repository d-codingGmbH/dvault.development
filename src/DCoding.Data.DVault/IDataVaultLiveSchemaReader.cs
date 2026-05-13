using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Reads a bounded live database schema snapshot for Data Vault-owned tables from an Entity Framework context.
/// </summary>
public interface IDataVaultLiveSchemaReader {
  /// <summary>
  /// Reads the live schema for Data Vault-owned tables configured in the supplied context.
  /// </summary>
  /// <param name="dbContext">The context whose provider, model, and connection identify the live schema to read.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading schema metadata.</param>
  /// <returns>
  /// A classified live-schema read result. Unsupported providers and unavailable live databases are reported as result
  /// statuses instead of being silently treated as successful drift checks.
  /// </returns>
  Task<DataVaultLiveSchemaReadResult> ReadAsync(
      DbContext dbContext,
      CancellationToken cancellationToken = default);
}
