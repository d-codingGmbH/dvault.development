using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes bounded PIT maintenance for explicit parent hash keys.
/// </summary>
public sealed class DataVaultPitParentMaintenanceRequest {
  /// <summary>
  /// Initializes a new bounded PIT maintenance request.
  /// </summary>
  /// <param name="pit">The PIT metadata declaration whose generated table should be maintained.</param>
  /// <param name="parentHashKeys">The exact parent hash keys to recompute.</param>
  public DataVaultPitParentMaintenanceRequest(
      DataVaultPitMetadata pit,
      IEnumerable<string> parentHashKeys) {
    ArgumentNullException.ThrowIfNull(pit);
    ArgumentNullException.ThrowIfNull(parentHashKeys);

    Pit = pit;
    ParentHashKeys = DataVaultLatestSatelliteReadRequest.RequireParentHashKeys(parentHashKeys);
  }

  /// <summary>
  /// Gets the PIT metadata declaration whose generated table should be maintained.
  /// </summary>
  public DataVaultPitMetadata Pit { get; }

  /// <summary>
  /// Gets the exact parent hash keys to recompute, deduplicated by ordinal string comparison.
  /// </summary>
  public IReadOnlyList<string> ParentHashKeys { get; }
}
