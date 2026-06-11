using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one participating hub and hash-key reference in a Data Vault link.
/// </summary>
public sealed class DataVaultLinkParticipantMetadata {
  /// <summary>
  /// Initializes a new link participant metadata declaration.
  /// </summary>
  /// <param name="hubReference">The hub referenced by this link participant.</param>
  public DataVaultLinkParticipantMetadata(DataVaultMetadataReference hubReference)
      : this(hubReference, hubReference?.Name ?? string.Empty) {
  }

  internal DataVaultLinkParticipantMetadata(
      DataVaultMetadataReference hubReference,
      string sourceEndpointName) {
    HubReference = DataVaultMetadataValidation.RequireHubReference(hubReference, nameof(hubReference));
    SourceEndpointName = DataVaultMetadataValidation.RequireName(sourceEndpointName, nameof(sourceEndpointName));
    HashKeyMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.HashKey);
  }

  /// <summary>
  /// Gets the participating hub reference.
  /// </summary>
  public DataVaultMetadataReference HubReference { get; }

  internal string SourceEndpointName { get; }

  /// <summary>
  /// Gets the technical hash-key metadata used to reference the participating hub key.
  /// </summary>
  public TechnicalMetadataColumnContract HashKeyMetadata { get; }
}
