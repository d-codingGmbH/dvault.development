namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Groups provider-neutral Data Vault metadata declarations for Entity Framework translation.
/// </summary>
public sealed class DataVaultMetadataModel {
  /// <summary>
  /// Initializes a new aggregate metadata model.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  public DataVaultMetadataModel(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites)
      : this(hubs, links, satellites, Array.Empty<DataVaultPointInTimeMetadata>()) {
  }

  /// <summary>
  /// Initializes a new aggregate metadata model with optional point-in-time declarations.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  /// <param name="pointInTimeTables">The point-in-time metadata declarations to validate and expose.</param>
  public DataVaultMetadataModel(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      IEnumerable<DataVaultPointInTimeMetadata> pointInTimeTables) {
    Hubs = RequireItems(hubs, nameof(hubs));
    Links = RequireItems(links, nameof(links));
    Satellites = RequireItems(satellites, nameof(satellites));
    PointInTimeTables = RequireItems(pointInTimeTables, nameof(pointInTimeTables));

    ValidatePointInTimeTables();
  }

  /// <summary>
  /// Gets the hub metadata declarations to translate.
  /// </summary>
  public IReadOnlyList<DataVaultHubMetadata> Hubs { get; }

  /// <summary>
  /// Gets the link metadata declarations to translate.
  /// </summary>
  public IReadOnlyList<DataVaultLinkMetadata> Links { get; }

  /// <summary>
  /// Gets the satellite metadata declarations to translate.
  /// </summary>
  public IReadOnlyList<DataVaultSatelliteMetadata> Satellites { get; }

  /// <summary>
  /// Gets the point-in-time metadata declarations to validate and expose.
  /// </summary>
  public IReadOnlyList<DataVaultPointInTimeMetadata> PointInTimeTables { get; }

  /// <summary>
  /// Creates a new aggregate metadata model from provider-neutral Data Vault declarations.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  /// <returns>The aggregate metadata model.</returns>
  public static DataVaultMetadataModel Create(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites) {
    return new DataVaultMetadataModel(hubs, links, satellites);
  }

  /// <summary>
  /// Creates a new aggregate metadata model from provider-neutral Data Vault declarations with point-in-time tables.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  /// <param name="pointInTimeTables">The point-in-time metadata declarations to validate and expose.</param>
  /// <returns>The aggregate metadata model.</returns>
  public static DataVaultMetadataModel Create(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      IEnumerable<DataVaultPointInTimeMetadata> pointInTimeTables) {
    return new DataVaultMetadataModel(hubs, links, satellites, pointInTimeTables);
  }

  private static IReadOnlyList<T> RequireItems<T>(IEnumerable<T> items, string parameterName)
      where T : class {
    ArgumentNullException.ThrowIfNull(items, parameterName);

    var values = items.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Metadata declaration collections must not contain null values.", parameterName);
      }
    }

    return values;
  }

  private void ValidatePointInTimeTables() {
    var hubNames = new HashSet<string>(Hubs.Select(hub => hub.Name), StringComparer.Ordinal);
    var satellitesByName = Satellites
        .GroupBy(satellite => satellite.Name, StringComparer.Ordinal)
        .ToDictionary(group => group.Key, group => group.ToArray(), StringComparer.Ordinal);

    foreach (var pointInTimeTable in PointInTimeTables) {
      if (!hubNames.Contains(pointInTimeTable.HubReference.Name)) {
        throw PointInTimeValidationException(
            pointInTimeTable,
            "references missing hub '" + pointInTimeTable.HubReference.Name + "'.");
      }

      if (pointInTimeTable.SatelliteReferences.Count == 0) {
        throw PointInTimeValidationException(pointInTimeTable, "requires at least one satellite reference.");
      }

      var satelliteNames = new HashSet<string>(StringComparer.Ordinal);
      foreach (var satelliteReference in pointInTimeTable.SatelliteReferences) {
        if (!satelliteNames.Add(satelliteReference.Name)) {
          throw PointInTimeValidationException(
              pointInTimeTable,
              "references satellite '" + satelliteReference.Name + "' more than once.");
        }

        if (!satellitesByName.TryGetValue(satelliteReference.Name, out var satellites)) {
          throw PointInTimeValidationException(
              pointInTimeTable,
              "references missing satellite '" + satelliteReference.Name + "'.");
        }

        if (!satellites.Any(satellite => IsSatelliteForHub(satellite, pointInTimeTable.HubReference.Name))) {
          throw PointInTimeValidationException(
              pointInTimeTable,
              "references satellite '" +
              satelliteReference.Name +
              "' that does not belong to hub '" +
              pointInTimeTable.HubReference.Name +
              "'.");
        }
      }
    }
  }

  private static bool IsSatelliteForHub(DataVaultSatelliteMetadata satellite, string hubName) {
    return satellite.Parent.Kind == DataVaultMetadataReferenceKind.Hub &&
        string.Equals(satellite.Parent.Name, hubName, StringComparison.Ordinal);
  }

  private static ArgumentException PointInTimeValidationException(
      DataVaultPointInTimeMetadata pointInTimeTable,
      string message) {
    return new ArgumentException(
        "Point-in-time table '" + pointInTimeTable.Name + "' " + message,
        "pointInTimeTables");
  }
}
