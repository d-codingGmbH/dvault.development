using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Carries the Entity Framework context and latest/as-of satellite read request used by provider-specific DVault read strategies.
/// </summary>
public sealed class DataVaultProviderReadStrategyContext {
  /// <summary>
  /// Initializes a new provider read strategy context.
  /// </summary>
  /// <param name="dbContext">The context whose model has been configured with Data Vault metadata.</param>
  /// <param name="request">The latest/as-of satellite read request to execute.</param>
  public DataVaultProviderReadStrategyContext(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    DbContext = dbContext;
    Request = request;
  }

  /// <summary>
  /// Gets the context whose model has been configured with Data Vault metadata.
  /// </summary>
  public DbContext DbContext { get; }

  /// <summary>
  /// Gets the latest/as-of satellite read request to execute.
  /// </summary>
  public DataVaultLatestSatelliteReadRequest Request { get; }
}
