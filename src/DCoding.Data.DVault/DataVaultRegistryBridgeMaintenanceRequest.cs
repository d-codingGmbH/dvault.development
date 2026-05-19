using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies one bridge metadata declaration from the authoritative DbContext registry by logical bridge name.
/// </summary>
public sealed class DataVaultRegistryBridgeMaintenanceRequest {
  /// <summary>
  /// Initializes a new registry-backed bridge maintenance request.
  /// </summary>
  /// <param name="bridgeName">The exact logical bridge metadata name to resolve from the authoritative registry.</param>
  public DataVaultRegistryBridgeMaintenanceRequest(string bridgeName) {
    BridgeName = DataVaultMetadataValidation.RequireName(bridgeName, nameof(bridgeName));
  }

  /// <summary>
  /// Gets the exact logical bridge metadata name to resolve from the authoritative registry.
  /// </summary>
  public string BridgeName { get; }
}
