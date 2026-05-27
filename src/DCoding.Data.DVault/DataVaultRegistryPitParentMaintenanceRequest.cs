using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies one PIT metadata declaration from the authoritative DbContext registry for bounded parent maintenance.
/// </summary>
public sealed class DataVaultRegistryPitParentMaintenanceRequest {
  /// <summary>
  /// Initializes a new registry-backed PIT parent-maintenance request by logical PIT name.
  /// </summary>
  /// <param name="pitName">The exact logical PIT metadata name to resolve from the authoritative registry.</param>
  /// <param name="parentHashKeys">The exact parent hash keys to recompute.</param>
  public DataVaultRegistryPitParentMaintenanceRequest(
      string pitName,
      IEnumerable<string> parentHashKeys) {
    ArgumentNullException.ThrowIfNull(parentHashKeys);

    PitName = DataVaultMetadataValidation.RequireName(pitName, nameof(pitName));
    ParentHashKeys = DataVaultLatestSatelliteReadRequest.RequireParentHashKeys(parentHashKeys);
  }

  /// <summary>
  /// Initializes a new registry-backed PIT parent-maintenance request by exact CLR mapping.
  /// </summary>
  /// <param name="pitClrType">The exact CLR type mapped to a PIT declaration in the authoritative registry.</param>
  /// <param name="parentHashKeys">The exact parent hash keys to recompute.</param>
  public DataVaultRegistryPitParentMaintenanceRequest(
      Type pitClrType,
      IEnumerable<string> parentHashKeys) {
    ArgumentNullException.ThrowIfNull(pitClrType);
    ArgumentNullException.ThrowIfNull(parentHashKeys);

    PitClrType = pitClrType;
    ParentHashKeys = DataVaultLatestSatelliteReadRequest.RequireParentHashKeys(parentHashKeys);
  }

  /// <summary>
  /// Gets the exact logical PIT metadata name to resolve, when name-based lookup was selected.
  /// </summary>
  public string? PitName { get; }

  /// <summary>
  /// Gets the exact CLR type to resolve, when CLR mapping lookup was selected.
  /// </summary>
  public Type? PitClrType { get; }

  /// <summary>
  /// Gets the exact parent hash keys to recompute, deduplicated by ordinal string comparison.
  /// </summary>
  public IReadOnlyList<string> ParentHashKeys { get; }
}
