using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one satellite snapshot reference declared by a point-in-time metadata projection.
/// </summary>
public sealed class DataVaultPitSatelliteReferenceMetadata {
  /// <summary>
  /// Initializes a new PIT satellite reference metadata declaration.
  /// </summary>
  /// <param name="satelliteName">The provider-neutral satellite metadata name.</param>
  /// <param name="isMultiActive">Whether the referenced satellite uses multi-active snapshot semantics.</param>
  public DataVaultPitSatelliteReferenceMetadata(string satelliteName, bool isMultiActive = false) {
    SatelliteName = DataVaultMetadataValidation.RequireName(satelliteName, nameof(satelliteName));
    IsMultiActive = isMultiActive;
  }

  /// <summary>
  /// Gets the provider-neutral satellite metadata name.
  /// </summary>
  public string SatelliteName { get; }

  /// <summary>
  /// Gets a value indicating whether the referenced satellite uses multi-active snapshot semantics.
  /// </summary>
  public bool IsMultiActive { get; }
}
