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
      : this(
          hubs,
          links,
          satellites,
          Array.Empty<DataVaultPointInTimeMetadata>(),
          Array.Empty<DataVaultBridgeMetadata>(),
          Array.Empty<DataVaultPitMetadata>()) {
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
      IEnumerable<DataVaultPointInTimeMetadata> pointInTimeTables)
      : this(
          hubs,
          links,
          satellites,
          pointInTimeTables,
          Array.Empty<DataVaultBridgeMetadata>(),
          Array.Empty<DataVaultPitMetadata>()) {
  }

  /// <summary>
  /// Initializes a new aggregate metadata model with optional bridge declarations.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  /// <param name="bridges">The bridge metadata declarations to validate and expose.</param>
  public DataVaultMetadataModel(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      IEnumerable<DataVaultBridgeMetadata> bridges)
      : this(
          hubs,
          links,
          satellites,
          Array.Empty<DataVaultPointInTimeMetadata>(),
          bridges,
          Array.Empty<DataVaultPitMetadata>()) {
  }

  /// <summary>
  /// Initializes a new aggregate metadata model with optional PIT declarations.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  /// <param name="pits">The point-in-time metadata declarations to translate.</param>
  public DataVaultMetadataModel(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      IEnumerable<DataVaultPitMetadata> pits)
      : this(
          hubs,
          links,
          satellites,
          Array.Empty<DataVaultPointInTimeMetadata>(),
          Array.Empty<DataVaultBridgeMetadata>(),
          pits) {
  }

  /// <summary>
  /// Initializes a new aggregate metadata model with optional point-in-time and bridge declarations.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  /// <param name="pointInTimeTables">The point-in-time metadata declarations to validate and expose.</param>
  /// <param name="bridges">The bridge metadata declarations to validate and expose.</param>
  public DataVaultMetadataModel(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      IEnumerable<DataVaultPointInTimeMetadata> pointInTimeTables,
      IEnumerable<DataVaultBridgeMetadata> bridges)
      : this(hubs, links, satellites, pointInTimeTables, bridges, Array.Empty<DataVaultPitMetadata>()) {
  }

  private DataVaultMetadataModel(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      IEnumerable<DataVaultPointInTimeMetadata> pointInTimeTables,
      IEnumerable<DataVaultBridgeMetadata> bridges,
      IEnumerable<DataVaultPitMetadata> pits) {
    Hubs = DataVaultMetadataValidation.RequireItems(hubs, nameof(hubs));
    Links = DataVaultMetadataValidation.RequireItems(links, nameof(links));
    Satellites = DataVaultMetadataValidation.RequireItems(satellites, nameof(satellites));
    PointInTimeTables = DataVaultMetadataValidation.RequireItems(pointInTimeTables, nameof(pointInTimeTables));
    Bridges = DataVaultMetadataValidation.RequireItems(bridges, nameof(bridges));
    Pits = DataVaultMetadataValidation.RequireItems(pits, nameof(pits));
    ValidatePointInTimeTables();
    ValidateBridges();
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
  /// Gets the bridge metadata declarations to validate and expose.
  /// </summary>
  public IReadOnlyList<DataVaultBridgeMetadata> Bridges { get; }

  /// <summary>
  /// Gets the point-in-time metadata declarations to translate.
  /// </summary>
  public IReadOnlyList<DataVaultPitMetadata> Pits { get; }

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

  /// <summary>
  /// Creates a new aggregate metadata model from provider-neutral Data Vault declarations with bridge declarations.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  /// <param name="bridges">The bridge metadata declarations to validate and expose.</param>
  /// <returns>The aggregate metadata model.</returns>
  public static DataVaultMetadataModel Create(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      IEnumerable<DataVaultBridgeMetadata> bridges) {
    return new DataVaultMetadataModel(hubs, links, satellites, bridges);
  }

  /// <summary>
  /// Creates a new aggregate metadata model from provider-neutral Data Vault declarations with PIT declarations.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  /// <param name="pits">The point-in-time metadata declarations to translate.</param>
  /// <returns>The aggregate metadata model.</returns>
  public static DataVaultMetadataModel Create(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      IEnumerable<DataVaultPitMetadata> pits) {
    return new DataVaultMetadataModel(hubs, links, satellites, pits);
  }

  /// <summary>
  /// Creates a new aggregate metadata model from provider-neutral Data Vault declarations with point-in-time and bridge declarations.
  /// </summary>
  /// <param name="hubs">The hub metadata declarations to translate.</param>
  /// <param name="links">The link metadata declarations to translate.</param>
  /// <param name="satellites">The satellite metadata declarations to translate.</param>
  /// <param name="pointInTimeTables">The point-in-time metadata declarations to validate and expose.</param>
  /// <param name="bridges">The bridge metadata declarations to validate and expose.</param>
  /// <returns>The aggregate metadata model.</returns>
  public static DataVaultMetadataModel Create(
      IEnumerable<DataVaultHubMetadata> hubs,
      IEnumerable<DataVaultLinkMetadata> links,
      IEnumerable<DataVaultSatelliteMetadata> satellites,
      IEnumerable<DataVaultPointInTimeMetadata> pointInTimeTables,
      IEnumerable<DataVaultBridgeMetadata> bridges) {
    return new DataVaultMetadataModel(hubs, links, satellites, pointInTimeTables, bridges);
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

  private void ValidateBridges() {
    foreach (var bridge in Bridges.Where(bridge => bridge.RequiresReferenceValidation)) {
      ValidateBridge(bridge);
    }
  }

  private void ValidateBridge(DataVaultBridgeMetadata bridge) {
    var sourceHub = RequireDeclaredHub(bridge, bridge.SourceHubReference, "source");
    var targetHub = RequireDeclaredHub(bridge, bridge.TargetHubReference, "target");
    var link = RequireDeclaredLink(bridge, bridge.LinkReference);
    var sourceParticipantOrdinal = ResolveParticipantOrdinal(
        bridge,
        link,
        sourceHub.Name,
        bridge.SourceParticipantOrdinal,
        "source");
    var targetParticipantOrdinal = ResolveParticipantOrdinal(
        bridge,
        link,
        targetHub.Name,
        bridge.TargetParticipantOrdinal,
        "target");

    if (bridge.Kind == DataVaultBridgeKind.Hierarchy) {
      ValidateHierarchyBridge(
          bridge,
          link,
          sourceHub.Name,
          targetHub.Name,
          sourceParticipantOrdinal,
          targetParticipantOrdinal);
    }
  }

  private DataVaultHubMetadata RequireDeclaredHub(
      DataVaultBridgeMetadata bridge,
      DataVaultMetadataReference hubReference,
      string roleName) {
    var matches = Hubs
        .Where(hub => string.Equals(hub.Name, hubReference.Name, StringComparison.Ordinal))
        .ToArray();

    if (matches.Length == 0) {
      throw new ArgumentException(
          "Bridge '" +
          bridge.Name +
          "' references " +
          roleName +
          " hub '" +
          hubReference.Name +
          "' that is not declared in the same metadata model.",
          "bridges");
    }

    if (matches.Length > 1) {
      throw new ArgumentException(
          "Bridge '" +
          bridge.Name +
          "' references " +
          roleName +
          " hub '" +
          hubReference.Name +
          "' but that hub name is declared more than once.",
          "bridges");
    }

    return matches[0];
  }

  private DataVaultLinkMetadata RequireDeclaredLink(
      DataVaultBridgeMetadata bridge,
      DataVaultMetadataReference linkReference) {
    var matches = Links
        .Where(link => string.Equals(link.Name, linkReference.Name, StringComparison.Ordinal))
        .ToArray();

    if (matches.Length == 0) {
      throw new ArgumentException(
          "Bridge '" +
          bridge.Name +
          "' references link '" +
          linkReference.Name +
          "' that is not declared in the same metadata model.",
          "bridges");
    }

    if (matches.Length > 1) {
      throw new ArgumentException(
          "Bridge '" +
          bridge.Name +
          "' references link '" +
          linkReference.Name +
          "' but that link name is declared more than once.",
          "bridges");
    }

    return matches[0];
  }

  private static int ResolveParticipantOrdinal(
      DataVaultBridgeMetadata bridge,
      DataVaultLinkMetadata link,
      string hubName,
      int? participantOrdinal,
      string roleName) {
    if (participantOrdinal.HasValue) {
      var ordinal = participantOrdinal.Value;
      if (ordinal >= link.Participants.Count) {
        throw new ArgumentException(
            "Bridge '" +
            bridge.Name +
            "' selects " +
            roleName +
            " participant ordinal " +
            ordinal +
            " but link '" +
            link.Name +
            "' declares only " +
            link.Participants.Count +
            " participant(s).",
            "bridges");
      }

      var participantHubName = link.Participants[ordinal].HubReference.Name;
      if (!string.Equals(participantHubName, hubName, StringComparison.Ordinal)) {
        throw new ArgumentException(
            "Bridge '" +
            bridge.Name +
            "' selects " +
            roleName +
            " participant ordinal " +
            ordinal +
            " for hub '" +
            hubName +
            "', but that ordinal resolves to hub '" +
            participantHubName +
            "'.",
            "bridges");
      }

      return ordinal;
    }

    var matchingOrdinals = link.Participants
        .Select((participant, index) => new { participant, index })
        .Where(item => string.Equals(item.participant.HubReference.Name, hubName, StringComparison.Ordinal))
        .Select(item => item.index)
        .ToArray();

    if (matchingOrdinals.Length == 0) {
      throw new ArgumentException(
          "Bridge '" +
          bridge.Name +
          "' selects " +
          roleName +
          " hub '" +
          hubName +
          "', but link '" +
          link.Name +
          "' does not declare that participant.",
          "bridges");
    }

    if (matchingOrdinals.Length > 1) {
      throw new ArgumentException(
          "Bridge '" +
          bridge.Name +
          "' has ambiguous " +
          roleName +
          " endpoint selection for hub '" +
          hubName +
          "' on link '" +
          link.Name +
          "'. Declare the participant ordinal explicitly.",
          "bridges");
    }

    return matchingOrdinals[0];
  }

  private static void ValidateHierarchyBridge(
      DataVaultBridgeMetadata bridge,
      DataVaultLinkMetadata link,
      string sourceHubName,
      string targetHubName,
      int sourceParticipantOrdinal,
      int targetParticipantOrdinal) {
    if (!string.Equals(sourceHubName, targetHubName, StringComparison.Ordinal)) {
      throw new ArgumentException(
          "Hierarchy bridge '" +
          bridge.Name +
          "' must use the same recursive hub for its source and target endpoints.",
          "bridges");
    }

    if (link.Participants.Count != 2 ||
        link.Participants.Any(participant => !string.Equals(participant.HubReference.Name, sourceHubName, StringComparison.Ordinal))) {
      throw new ArgumentException(
          "Hierarchy bridge '" +
          bridge.Name +
          "' must traverse a two-participant self-link over hub '" +
          sourceHubName +
          "'.",
          "bridges");
    }

    if (sourceParticipantOrdinal == targetParticipantOrdinal) {
      throw new ArgumentException(
          "Hierarchy bridge '" +
          bridge.Name +
          "' resolves ancestor and descendant selectors to the same link participant.",
          "bridges");
    }
  }
}
