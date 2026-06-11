using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one provider-neutral point-in-time table declaration for one hub and ordered satellite snapshots.
/// </summary>
public sealed class DataVaultPointInTimeMetadata {
  /// <summary>
  /// Initializes a new point-in-time metadata declaration.
  /// </summary>
  /// <param name="name">The provider-neutral point-in-time table name base.</param>
  /// <param name="hubReference">The hub referenced by the point-in-time table.</param>
  /// <param name="satelliteReferences">The ordered satellite references captured by the point-in-time table.</param>
  public DataVaultPointInTimeMetadata(
      string name,
      DataVaultMetadataReference hubReference,
      IEnumerable<DataVaultMetadataReference> satelliteReferences) {
    Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
    HubReference = DataVaultMetadataValidation.RequireHubReference(hubReference, nameof(hubReference));
    SatelliteReferences = RequireSatelliteReferences(satelliteReferences);
  }

  /// <summary>
  /// Gets the point-in-time table name base.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the hub referenced by the point-in-time table.
  /// </summary>
  public DataVaultMetadataReference HubReference { get; }

  /// <summary>
  /// Gets the ordered satellite references captured by the point-in-time table.
  /// </summary>
  public IReadOnlyList<DataVaultMetadataReference> SatelliteReferences { get; }

  private static IReadOnlyList<DataVaultMetadataReference> RequireSatelliteReferences(
      IEnumerable<DataVaultMetadataReference> satelliteReferences) {
    ArgumentNullException.ThrowIfNull(satelliteReferences);

    var values = satelliteReferences.ToArray();
    foreach (var reference in values) {
      DataVaultMetadataValidation.RequireSatelliteReference(reference, nameof(satelliteReferences));
    }

    return values;
  }
}
