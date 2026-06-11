using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one ordered endpoint binding declared by a Data Vault bridge.
/// </summary>
internal sealed class DataVaultBridgeEndpointMetadata {
  /// <summary>
  /// Initializes a new bridge endpoint binding.
  /// </summary>
  /// <param name="role">The role carried by this bridge endpoint binding.</param>
  /// <param name="hubReference">The hub type referenced by the bridge endpoint.</param>
  /// <param name="sourceEndpointName">The source link participant name bound by this endpoint.</param>
  internal DataVaultBridgeEndpointMetadata(
      DataVaultBridgeEndpointRole role,
      DataVaultMetadataReference hubReference,
      string sourceEndpointName) {
    HubReference = DataVaultMetadataValidation.RequireHubReference(hubReference, nameof(hubReference));
    SourceEndpointName = DataVaultMetadataValidation.RequireName(sourceEndpointName, nameof(sourceEndpointName));
    Role = role;
    HashKeyMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.HashKey);
  }

  /// <summary>
  /// Gets the role carried by this bridge endpoint binding.
  /// </summary>
  internal DataVaultBridgeEndpointRole Role { get; }

  /// <summary>
  /// Gets the hub type referenced by the bridge endpoint.
  /// </summary>
  internal DataVaultMetadataReference HubReference { get; }

  /// <summary>
  /// Gets the source link participant name bound by this endpoint.
  /// </summary>
  internal string SourceEndpointName { get; }

  /// <summary>
  /// Gets the technical hash-key metadata used to reference the bridge endpoint hub key.
  /// </summary>
  internal TechnicalMetadataColumnContract HashKeyMetadata { get; }
}
