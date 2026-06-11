using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes the minimum point-in-time projection metadata consumed by the EF metadata translator.
/// </summary>
public sealed class DataVaultPitMetadata {
  /// <summary>
  /// Initializes a new PIT metadata declaration from satellite names in declaration order.
  /// </summary>
  /// <param name="parent">The hub or link metadata reference declared as the PIT parent.</param>
  /// <param name="satelliteNames">The satellite metadata names included in declaration order.</param>
  public DataVaultPitMetadata(DataVaultMetadataReference parent, IEnumerable<string> satelliteNames)
      : this(parent, CreateSatelliteReferences(satelliteNames)) {
  }

  /// <summary>
  /// Initializes a new PIT metadata declaration from satellite references in declaration order.
  /// </summary>
  /// <param name="parent">The hub or link metadata reference declared as the PIT parent.</param>
  /// <param name="satellites">The satellite metadata references included in declaration order.</param>
  public DataVaultPitMetadata(
      DataVaultMetadataReference parent,
      IEnumerable<DataVaultPitSatelliteReferenceMetadata> satellites)
      : this(
          parent,
          DataVaultMetadataValidation.RequireItems(satellites, nameof(satellites))) {
  }

  private DataVaultPitMetadata(
      DataVaultMetadataReference parent,
      IReadOnlyList<DataVaultPitSatelliteReferenceMetadata> satellites)
      : this(CreateDefaultName(parent, satellites), parent, satellites) {
  }

  internal DataVaultPitMetadata(
      string name,
      DataVaultMetadataReference parent,
      IEnumerable<string> satelliteNames)
      : this(name, parent, CreateSatelliteReferences(satelliteNames)) {
  }

  internal DataVaultPitMetadata(
      string name,
      DataVaultMetadataReference parent,
      IEnumerable<DataVaultPitSatelliteReferenceMetadata> satellites) {
    Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
    ArgumentNullException.ThrowIfNull(parent);

    Parent = parent;
    Satellites = DataVaultMetadataValidation.RequireItems(satellites, nameof(satellites));
    HashKeyMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.HashKey);
    LoadTimestampMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.LoadTimestamp);
    TechnicalMetadataColumns =
    [
        HashKeyMetadata,
        LoadTimestampMetadata,
    ];
  }

  /// <summary>
  /// Gets the deterministic provider-neutral PIT metadata name.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the hub or link metadata reference declared as the PIT parent.
  /// </summary>
  public DataVaultMetadataReference Parent { get; }

  /// <summary>
  /// Gets the satellite metadata references included in declaration order.
  /// </summary>
  public IReadOnlyList<DataVaultPitSatelliteReferenceMetadata> Satellites { get; }

  /// <summary>
  /// Gets the required parent hash-key technical metadata for the PIT projection.
  /// </summary>
  public TechnicalMetadataColumnContract HashKeyMetadata { get; }

  /// <summary>
  /// Gets the required PIT load-timestamp technical metadata.
  /// </summary>
  public TechnicalMetadataColumnContract LoadTimestampMetadata { get; }

  /// <summary>
  /// Gets the required technical metadata columns for PIT records.
  /// </summary>
  public IReadOnlyList<TechnicalMetadataColumnContract> TechnicalMetadataColumns { get; }

  private static IReadOnlyList<DataVaultPitSatelliteReferenceMetadata> CreateSatelliteReferences(
      IEnumerable<string> satelliteNames) {
    ArgumentNullException.ThrowIfNull(satelliteNames);

    return satelliteNames
        .Select(satelliteName => new DataVaultPitSatelliteReferenceMetadata(satelliteName))
        .ToArray();
  }

  private static string CreateDefaultName(
      DataVaultMetadataReference parent,
      IEnumerable<DataVaultPitSatelliteReferenceMetadata> satellites) {
    ArgumentNullException.ThrowIfNull(parent);

    return CreateDefaultName(parent.Name, satellites);
  }

  private static string CreateDefaultName(
      string parentName,
      IEnumerable<DataVaultPitSatelliteReferenceMetadata> satellites) {
    var namingPolicy = DefaultNamingPolicy.Instance;

    return namingPolicy.NormalizeProducedIdentifier(parentName) +
        string.Concat(satellites.Select(satellite => namingPolicy.NormalizeProducedIdentifier(satellite.SatelliteName)));
  }
}
