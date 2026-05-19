using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies one bridge metadata declaration for explicit provider-neutral maintenance.
/// </summary>
public sealed class DataVaultBridgeMaintenanceRequest {
  /// <summary>
  /// Initializes a new bridge maintenance request.
  /// </summary>
  /// <param name="bridge">The bridge metadata declaration that owns the generated bridge table.</param>
  public DataVaultBridgeMaintenanceRequest(DataVaultBridgeMetadata bridge) {
    ArgumentNullException.ThrowIfNull(bridge);

    Bridge = bridge;
  }

  /// <summary>
  /// Gets the bridge metadata declaration that owns the generated bridge table.
  /// </summary>
  public DataVaultBridgeMetadata Bridge { get; }
}
