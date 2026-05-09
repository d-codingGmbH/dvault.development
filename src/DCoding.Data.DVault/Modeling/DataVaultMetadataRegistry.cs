using System.Collections.ObjectModel;
using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Provides immutable, deterministic lookup over Data Vault metadata declarations and provider capability profiles.
/// </summary>
public sealed class DataVaultMetadataRegistry {
  private readonly IReadOnlyDictionary<string, DataVaultHubMetadata> _hubsByName;
  private readonly IReadOnlyDictionary<Type, DataVaultHubMetadata> _hubsByClrType;
  private readonly IReadOnlyDictionary<string, DataVaultLinkMetadata> _linksByName;
  private readonly IReadOnlyDictionary<Type, DataVaultLinkMetadata> _linksByClrType;
  private readonly IReadOnlyDictionary<SatelliteKey, DataVaultSatelliteMetadata> _satellitesByKey;
  private readonly IReadOnlyDictionary<SatelliteClrKey, DataVaultSatelliteMetadata> _satellitesByClrKey;
  private readonly IReadOnlyDictionary<ParentKey, IReadOnlyList<DataVaultSatelliteMetadata>> _satellitesByParent;
  private readonly IReadOnlyDictionary<string, DataVaultPointInTimeMetadata> _pointInTimeTablesByName;
  private readonly IReadOnlyDictionary<Type, DataVaultPointInTimeMetadata> _pointInTimeTablesByClrType;
  private readonly IReadOnlyDictionary<string, DataVaultBridgeMetadata> _bridgesByName;
  private readonly IReadOnlyDictionary<Type, DataVaultBridgeMetadata> _bridgesByClrType;
  private readonly IReadOnlyDictionary<string, DataVaultPitMetadata> _pitsByName;
  private readonly IReadOnlyDictionary<Type, DataVaultPitMetadata> _pitsByClrType;
  private readonly IReadOnlyDictionary<string, DataVaultProviderCapabilityProfile> _providerCapabilityProfilesByName;

  private DataVaultMetadataRegistry(
      DataVaultMetadataModel metadataModel,
      IEnumerable<DataVaultProviderCapabilityProfile> providerCapabilityProfiles,
      IEnumerable<DataVaultMetadataClrMapping> clrMappings) {
    ArgumentNullException.ThrowIfNull(metadataModel);

    Hubs = Copy(metadataModel.Hubs);
    Links = Copy(metadataModel.Links);
    Satellites = Copy(metadataModel.Satellites);
    PointInTimeTables = Copy(metadataModel.PointInTimeTables);
    Bridges = Copy(metadataModel.Bridges);
    Pits = Copy(metadataModel.Pits);
    ProviderCapabilityProfiles = CopyProviderCapabilityProfiles(providerCapabilityProfiles);

    _hubsByName = CreateNameIndex(Hubs, hub => hub.Name, DataVaultMetadataRegistryKind.Hub, "metadataModel");
    _linksByName = CreateNameIndex(Links, link => link.Name, DataVaultMetadataRegistryKind.Link, "metadataModel");
    _satellitesByKey = CreateSatelliteIndex(Satellites);
    _satellitesByParent = CreateSatellitesByParent(Satellites);
    _pointInTimeTablesByName = CreateNameIndex(
        PointInTimeTables,
        pointInTimeTable => pointInTimeTable.Name,
        DataVaultMetadataRegistryKind.PointInTimeTable,
        "metadataModel");
    _bridgesByName = CreateNameIndex(Bridges, bridge => bridge.Name, DataVaultMetadataRegistryKind.Bridge, "metadataModel");
    _pitsByName = CreateNameIndex(Pits, pit => pit.Name, DataVaultMetadataRegistryKind.Pit, "metadataModel");
    _providerCapabilityProfilesByName = CreateProviderCapabilityProfileIndex(ProviderCapabilityProfiles);

    ValidateMetadataDependencies();

    var clrLookup = CreateClrLookup(clrMappings);
    _hubsByClrType = clrLookup.Hubs;
    _linksByClrType = clrLookup.Links;
    _satellitesByClrKey = clrLookup.Satellites;
    _pointInTimeTablesByClrType = clrLookup.PointInTimeTables;
    _bridgesByClrType = clrLookup.Bridges;
    _pitsByClrType = clrLookup.Pits;
  }

  /// <summary>
  /// Gets hub metadata declarations in canonical declaration order.
  /// </summary>
  public IReadOnlyList<DataVaultHubMetadata> Hubs { get; }

  /// <summary>
  /// Gets link metadata declarations in canonical declaration order.
  /// </summary>
  public IReadOnlyList<DataVaultLinkMetadata> Links { get; }

  /// <summary>
  /// Gets satellite metadata declarations in canonical declaration order.
  /// </summary>
  public IReadOnlyList<DataVaultSatelliteMetadata> Satellites { get; }

  /// <summary>
  /// Gets legacy point-in-time table declarations in canonical declaration order.
  /// </summary>
  public IReadOnlyList<DataVaultPointInTimeMetadata> PointInTimeTables { get; }

  /// <summary>
  /// Gets bridge metadata declarations in canonical declaration order.
  /// </summary>
  public IReadOnlyList<DataVaultBridgeMetadata> Bridges { get; }

  /// <summary>
  /// Gets PIT metadata declarations in canonical declaration order.
  /// </summary>
  public IReadOnlyList<DataVaultPitMetadata> Pits { get; }

  /// <summary>
  /// Gets provider capability profiles in canonical declaration order.
  /// </summary>
  public IReadOnlyList<DataVaultProviderCapabilityProfile> ProviderCapabilityProfiles { get; }

  /// <summary>
  /// Creates an immutable registry from a metadata model.
  /// </summary>
  public static DataVaultMetadataRegistry Create(DataVaultMetadataModel metadataModel) {
    return Create(
        metadataModel,
        Array.Empty<DataVaultProviderCapabilityProfile>(),
        Array.Empty<DataVaultMetadataClrMapping>());
  }

  /// <summary>
  /// Creates an immutable registry from a metadata model and provider capability profiles.
  /// </summary>
  public static DataVaultMetadataRegistry Create(
      DataVaultMetadataModel metadataModel,
      IEnumerable<DataVaultProviderCapabilityProfile> providerCapabilityProfiles) {
    return Create(
        metadataModel,
        providerCapabilityProfiles,
        Array.Empty<DataVaultMetadataClrMapping>());
  }

  /// <summary>
  /// Creates an immutable registry from a metadata model, provider capability profiles, and optional CLR mappings.
  /// </summary>
  public static DataVaultMetadataRegistry Create(
      DataVaultMetadataModel metadataModel,
      IEnumerable<DataVaultProviderCapabilityProfile> providerCapabilityProfiles,
      IEnumerable<DataVaultMetadataClrMapping> clrMappings) {
    ArgumentNullException.ThrowIfNull(providerCapabilityProfiles);
    ArgumentNullException.ThrowIfNull(clrMappings);

    return new DataVaultMetadataRegistry(metadataModel, providerCapabilityProfiles, clrMappings);
  }

  /// <summary>
  /// Attempts to find hub metadata by exact logical name.
  /// </summary>
  public bool TryGetHub(string name, out DataVaultHubMetadata? hub) {
    return _hubsByName.TryGetValue(RequireLookupName(name, nameof(name)), out hub);
  }

  /// <summary>
  /// Attempts to find hub metadata by exact CLR type.
  /// </summary>
  public bool TryGetHub(Type clrType, out DataVaultHubMetadata? hub) {
    ArgumentNullException.ThrowIfNull(clrType);

    return _hubsByClrType.TryGetValue(clrType, out hub);
  }

  /// <summary>
  /// Attempts to find link metadata by exact logical name.
  /// </summary>
  public bool TryGetLink(string name, out DataVaultLinkMetadata? link) {
    return _linksByName.TryGetValue(RequireLookupName(name, nameof(name)), out link);
  }

  /// <summary>
  /// Attempts to find link metadata by exact CLR type.
  /// </summary>
  public bool TryGetLink(Type clrType, out DataVaultLinkMetadata? link) {
    ArgumentNullException.ThrowIfNull(clrType);

    return _linksByClrType.TryGetValue(clrType, out link);
  }

  /// <summary>
  /// Attempts to find satellite metadata by exact parent reference and logical name.
  /// </summary>
  public bool TryGetSatellite(
      DataVaultMetadataReference parent,
      string name,
      out DataVaultSatelliteMetadata? satellite) {
    var key = new SatelliteKey(
        CreateSatelliteParentKey(parent, nameof(parent)),
        RequireLookupName(name, nameof(name)));

    return _satellitesByKey.TryGetValue(key, out satellite);
  }

  /// <summary>
  /// Attempts to find satellite metadata by exact parent reference and CLR type.
  /// </summary>
  public bool TryGetSatellite(
      DataVaultMetadataReference parent,
      Type clrType,
      out DataVaultSatelliteMetadata? satellite) {
    ArgumentNullException.ThrowIfNull(clrType);

    var key = new SatelliteClrKey(CreateSatelliteParentKey(parent, nameof(parent)), clrType);

    return _satellitesByClrKey.TryGetValue(key, out satellite);
  }

  /// <summary>
  /// Gets satellite metadata for one exact parent reference in canonical declaration order.
  /// </summary>
  public IReadOnlyList<DataVaultSatelliteMetadata> GetSatellites(DataVaultMetadataReference parent) {
    var key = CreateSatelliteParentKey(parent, nameof(parent));

    return _satellitesByParent.TryGetValue(key, out var satellites) ? satellites : Array.Empty<DataVaultSatelliteMetadata>();
  }

  /// <summary>
  /// Attempts to find legacy point-in-time table metadata by exact logical name.
  /// </summary>
  public bool TryGetPointInTimeTable(
      string name,
      out DataVaultPointInTimeMetadata? pointInTimeTable) {
    return _pointInTimeTablesByName.TryGetValue(RequireLookupName(name, nameof(name)), out pointInTimeTable);
  }

  /// <summary>
  /// Attempts to find legacy point-in-time table metadata by exact CLR type.
  /// </summary>
  public bool TryGetPointInTimeTable(
      Type clrType,
      out DataVaultPointInTimeMetadata? pointInTimeTable) {
    ArgumentNullException.ThrowIfNull(clrType);

    return _pointInTimeTablesByClrType.TryGetValue(clrType, out pointInTimeTable);
  }

  /// <summary>
  /// Attempts to find bridge metadata by exact logical name.
  /// </summary>
  public bool TryGetBridge(string name, out DataVaultBridgeMetadata? bridge) {
    return _bridgesByName.TryGetValue(RequireLookupName(name, nameof(name)), out bridge);
  }

  /// <summary>
  /// Attempts to find bridge metadata by exact CLR type.
  /// </summary>
  public bool TryGetBridge(Type clrType, out DataVaultBridgeMetadata? bridge) {
    ArgumentNullException.ThrowIfNull(clrType);

    return _bridgesByClrType.TryGetValue(clrType, out bridge);
  }

  /// <summary>
  /// Attempts to find PIT metadata by exact logical name.
  /// </summary>
  public bool TryGetPit(string name, out DataVaultPitMetadata? pit) {
    return _pitsByName.TryGetValue(RequireLookupName(name, nameof(name)), out pit);
  }

  /// <summary>
  /// Attempts to find PIT metadata by exact CLR type.
  /// </summary>
  public bool TryGetPit(Type clrType, out DataVaultPitMetadata? pit) {
    ArgumentNullException.ThrowIfNull(clrType);

    return _pitsByClrType.TryGetValue(clrType, out pit);
  }

  /// <summary>
  /// Attempts to find provider capability profile metadata by exact profile name.
  /// </summary>
  public bool TryGetProviderCapabilityProfile(
      string profileName,
      out DataVaultProviderCapabilityProfile? providerCapabilityProfile) {
    return _providerCapabilityProfilesByName.TryGetValue(
        RequireLookupName(profileName, nameof(profileName)),
        out providerCapabilityProfile);
  }

  private static IReadOnlyList<T> Copy<T>(IEnumerable<T> items)
      where T : class {
    return new ReadOnlyCollection<T>(items.ToArray());
  }

  private static IReadOnlyList<DataVaultProviderCapabilityProfile> CopyProviderCapabilityProfiles(
      IEnumerable<DataVaultProviderCapabilityProfile> providerCapabilityProfiles) {
    ArgumentNullException.ThrowIfNull(providerCapabilityProfiles);

    var values = providerCapabilityProfiles.ToArray();
    foreach (var value in values) {
      ArgumentNullException.ThrowIfNull(value, nameof(providerCapabilityProfiles));
    }

    return new ReadOnlyCollection<DataVaultProviderCapabilityProfile>(values);
  }

  private static IReadOnlyDictionary<string, T> CreateNameIndex<T>(
      IReadOnlyList<T> items,
      Func<T, string> getName,
      DataVaultMetadataRegistryKind kind,
      string parameterName) {
    var valuesByName = new Dictionary<string, T>(StringComparer.Ordinal);
    foreach (var item in items) {
      var name = getName(item);
      if (!valuesByName.TryAdd(name, item)) {
        throw DuplicateLogicalName(kind, name, parameterName);
      }
    }

    return valuesByName;
  }

  private static IReadOnlyDictionary<string, DataVaultProviderCapabilityProfile> CreateProviderCapabilityProfileIndex(
      IReadOnlyList<DataVaultProviderCapabilityProfile> providerCapabilityProfiles) {
    var valuesByName = new Dictionary<string, DataVaultProviderCapabilityProfile>(StringComparer.Ordinal);
    foreach (var providerCapabilityProfile in providerCapabilityProfiles) {
      if (!valuesByName.TryAdd(providerCapabilityProfile.ProfileName, providerCapabilityProfile)) {
        throw new ArgumentException(
            "Duplicate provider capability profile logical name '" + providerCapabilityProfile.ProfileName + "'.",
            "providerCapabilityProfiles");
      }
    }

    return valuesByName;
  }

  private static IReadOnlyDictionary<SatelliteKey, DataVaultSatelliteMetadata> CreateSatelliteIndex(
      IReadOnlyList<DataVaultSatelliteMetadata> satellites) {
    var valuesByKey = new Dictionary<SatelliteKey, DataVaultSatelliteMetadata>();
    foreach (var satellite in satellites) {
      var key = new SatelliteKey(
          CreateSatelliteParentKey(satellite.Parent, "metadataModel"),
          satellite.Name);
      if (!valuesByKey.TryAdd(key, satellite)) {
        throw new ArgumentException(
            "Duplicate satellite metadata logical name '" +
            satellite.Name +
            "' under " +
            FormatParent(satellite.Parent) +
            ".",
            "metadataModel");
      }
    }

    return valuesByKey;
  }

  private static IReadOnlyDictionary<ParentKey, IReadOnlyList<DataVaultSatelliteMetadata>> CreateSatellitesByParent(
      IReadOnlyList<DataVaultSatelliteMetadata> satellites) {
    var satellitesByParent = new Dictionary<ParentKey, List<DataVaultSatelliteMetadata>>();
    foreach (var satellite in satellites) {
      var key = CreateSatelliteParentKey(satellite.Parent, "metadataModel");
      if (!satellitesByParent.TryGetValue(key, out var parentSatellites)) {
        parentSatellites = [];
        satellitesByParent.Add(key, parentSatellites);
      }

      parentSatellites.Add(satellite);
    }

    return satellitesByParent.ToDictionary(
        item => item.Key,
        item => (IReadOnlyList<DataVaultSatelliteMetadata>)new ReadOnlyCollection<DataVaultSatelliteMetadata>(item.Value),
        EqualityComparer<ParentKey>.Default);
  }

  private void ValidateMetadataDependencies() {
    ValidateLinks();
    ValidateSatellites();
    ValidatePointInTimeTables();
    ValidateBridges();
    ValidatePits();
  }

  private void ValidateLinks() {
    foreach (var link in Links) {
      foreach (var participant in link.Participants) {
        if (!_hubsByName.ContainsKey(participant.HubReference.Name)) {
          throw MissingDependency(
              DataVaultMetadataRegistryKind.Link,
              link.Name,
              DataVaultMetadataRegistryKind.Hub,
              participant.HubReference.Name);
        }
      }
    }
  }

  private void ValidateSatellites() {
    foreach (var satellite in Satellites) {
      if (satellite.Parent.Kind == DataVaultMetadataReferenceKind.Hub) {
        if (!_hubsByName.ContainsKey(satellite.Parent.Name)) {
          throw MissingDependency(
              DataVaultMetadataRegistryKind.Satellite,
              satellite.Name,
              DataVaultMetadataRegistryKind.Hub,
              satellite.Parent.Name);
        }

        continue;
      }

      if (satellite.Parent.Kind == DataVaultMetadataReferenceKind.Link) {
        if (!_linksByName.ContainsKey(satellite.Parent.Name)) {
          throw MissingDependency(
              DataVaultMetadataRegistryKind.Satellite,
              satellite.Name,
              DataVaultMetadataRegistryKind.Link,
              satellite.Parent.Name);
        }

        continue;
      }

      throw new ArgumentException(
          "Satellite metadata '" +
          satellite.Name +
          "' declares unsupported parent kind '" +
          satellite.Parent.Kind +
          "' for parent '" +
          satellite.Parent.Name +
          "'.",
          "metadataModel");
    }
  }

  private void ValidatePointInTimeTables() {
    foreach (var pointInTimeTable in PointInTimeTables) {
      if (!_hubsByName.ContainsKey(pointInTimeTable.HubReference.Name)) {
        throw MissingDependency(
            DataVaultMetadataRegistryKind.PointInTimeTable,
            pointInTimeTable.Name,
            DataVaultMetadataRegistryKind.Hub,
            pointInTimeTable.HubReference.Name);
      }

      if (pointInTimeTable.SatelliteReferences.Count == 0) {
        throw new ArgumentException(
            "Point-in-time table metadata '" + pointInTimeTable.Name + "' requires at least one satellite reference.",
            "metadataModel");
      }

      var satelliteNames = new HashSet<string>(StringComparer.Ordinal);
      foreach (var satelliteReference in pointInTimeTable.SatelliteReferences) {
        if (!satelliteNames.Add(satelliteReference.Name)) {
          throw new ArgumentException(
              "Point-in-time table metadata '" +
              pointInTimeTable.Name +
              "' references satellite '" +
              satelliteReference.Name +
              "' more than once.",
              "metadataModel");
        }

        if (!SatelliteNameExists(satelliteReference.Name)) {
          throw MissingDependency(
              DataVaultMetadataRegistryKind.PointInTimeTable,
              pointInTimeTable.Name,
              DataVaultMetadataRegistryKind.Satellite,
              satelliteReference.Name);
        }

        if (!SatelliteExists(DataVaultMetadataReference.Hub(pointInTimeTable.HubReference.Name), satelliteReference.Name)) {
          throw new ArgumentException(
              "Point-in-time table metadata '" +
              pointInTimeTable.Name +
              "' references satellite '" +
              satelliteReference.Name +
              "' that does not belong to hub '" +
              pointInTimeTable.HubReference.Name +
              "'.",
              "metadataModel");
        }
      }
    }
  }

  private void ValidateBridges() {
    foreach (var bridge in Bridges) {
      if (!_hubsByName.ContainsKey(bridge.SourceHubReference.Name)) {
        throw MissingDependency(
            DataVaultMetadataRegistryKind.Bridge,
            bridge.Name,
            DataVaultMetadataRegistryKind.Hub,
            bridge.SourceHubReference.Name);
      }

      if (!_hubsByName.ContainsKey(bridge.TargetHubReference.Name)) {
        throw MissingDependency(
            DataVaultMetadataRegistryKind.Bridge,
            bridge.Name,
            DataVaultMetadataRegistryKind.Hub,
            bridge.TargetHubReference.Name);
      }

      if (!_linksByName.TryGetValue(bridge.LinkReference.Name, out var link)) {
        throw MissingDependency(
            DataVaultMetadataRegistryKind.Bridge,
            bridge.Name,
            DataVaultMetadataRegistryKind.Link,
            bridge.LinkReference.Name);
      }

      if (bridge.RequiresReferenceValidation) {
        ValidateBridgeEndpointSelection(bridge, link);
      }
      else {
        ValidateBridgeEndpointHubReferences(bridge);
      }
    }
  }

  private void ValidateBridgeEndpointSelection(
      DataVaultBridgeMetadata bridge,
      DataVaultLinkMetadata link) {
    var sourceParticipantOrdinal = ResolveParticipantOrdinal(
        bridge,
        link,
        bridge.SourceHubReference.Name,
        bridge.SourceParticipantOrdinal,
        "source");
    var targetParticipantOrdinal = ResolveParticipantOrdinal(
        bridge,
        link,
        bridge.TargetHubReference.Name,
        bridge.TargetParticipantOrdinal,
        "target");

    if (bridge.Kind == DataVaultBridgeKind.Hierarchy) {
      ValidateHierarchyBridge(
          bridge,
          link,
          sourceParticipantOrdinal,
          targetParticipantOrdinal);
    }
  }

  private void ValidateBridgeEndpointHubReferences(DataVaultBridgeMetadata bridge) {
    foreach (var endpoint in bridge.Endpoints) {
      if (!_hubsByName.ContainsKey(endpoint.HubReference.Name)) {
        throw MissingDependency(
            DataVaultMetadataRegistryKind.Bridge,
            bridge.Name,
            DataVaultMetadataRegistryKind.Hub,
            endpoint.HubReference.Name);
      }
    }
  }

  private void ValidatePits() {
    foreach (var pit in Pits) {
      ValidatePitParent(pit);

      if (pit.Satellites.Count == 0) {
        throw new ArgumentException(
            "PIT metadata '" + pit.Name + "' requires at least one satellite reference.",
            "metadataModel");
      }

      var satelliteNames = new HashSet<string>(StringComparer.Ordinal);
      foreach (var satelliteReference in pit.Satellites) {
        if (!satelliteNames.Add(satelliteReference.SatelliteName)) {
          throw new ArgumentException(
              "PIT metadata '" +
              pit.Name +
              "' references satellite '" +
              satelliteReference.SatelliteName +
              "' more than once.",
              "metadataModel");
        }

        if (!SatelliteNameExists(satelliteReference.SatelliteName)) {
          throw MissingDependency(
              DataVaultMetadataRegistryKind.Pit,
              pit.Name,
              DataVaultMetadataRegistryKind.Satellite,
              satelliteReference.SatelliteName);
        }

        if (!SatelliteExists(pit.Parent, satelliteReference.SatelliteName)) {
          throw new ArgumentException(
              "PIT metadata '" +
              pit.Name +
              "' references satellite '" +
              satelliteReference.SatelliteName +
              "' that does not belong to " +
              FormatParent(pit.Parent) +
              ".",
              "metadataModel");
        }
      }
    }
  }

  private void ValidatePitParent(DataVaultPitMetadata pit) {
    if (pit.Parent.Kind == DataVaultMetadataReferenceKind.Hub) {
      if (!_hubsByName.ContainsKey(pit.Parent.Name)) {
        throw MissingDependency(
            DataVaultMetadataRegistryKind.Pit,
            pit.Name,
            DataVaultMetadataRegistryKind.Hub,
            pit.Parent.Name);
      }

      return;
    }

    if (pit.Parent.Kind == DataVaultMetadataReferenceKind.Link) {
      if (!_linksByName.ContainsKey(pit.Parent.Name)) {
        throw MissingDependency(
            DataVaultMetadataRegistryKind.Pit,
            pit.Name,
            DataVaultMetadataRegistryKind.Link,
            pit.Parent.Name);
      }

      return;
    }

    throw new ArgumentException(
        "PIT metadata '" +
        pit.Name +
        "' declares unsupported parent kind '" +
        pit.Parent.Kind +
        "' for parent '" +
        pit.Parent.Name +
        "'.",
        "metadataModel");
  }

  private bool SatelliteNameExists(string name) {
    return Satellites.Any(satellite => string.Equals(satellite.Name, name, StringComparison.Ordinal));
  }

  private bool SatelliteExists(DataVaultMetadataReference parent, string name) {
    return _satellitesByKey.ContainsKey(new SatelliteKey(CreateSatelliteParentKey(parent, "metadataModel"), name));
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
            "Bridge metadata '" +
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
            "metadataModel");
      }

      var participantHubName = link.Participants[ordinal].HubReference.Name;
      if (!string.Equals(participantHubName, hubName, StringComparison.Ordinal)) {
        throw new ArgumentException(
            "Bridge metadata '" +
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
            "metadataModel");
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
          "Bridge metadata '" +
          bridge.Name +
          "' selects " +
          roleName +
          " hub '" +
          hubName +
          "', but link '" +
          link.Name +
          "' does not declare that participant.",
          "metadataModel");
    }

    if (matchingOrdinals.Length > 1) {
      throw new ArgumentException(
          "Bridge metadata '" +
          bridge.Name +
          "' has ambiguous " +
          roleName +
          " endpoint selection for hub '" +
          hubName +
          "' on link '" +
          link.Name +
          "'. Declare the participant ordinal explicitly.",
          "metadataModel");
    }

    return matchingOrdinals[0];
  }

  private static void ValidateHierarchyBridge(
      DataVaultBridgeMetadata bridge,
      DataVaultLinkMetadata link,
      int sourceParticipantOrdinal,
      int targetParticipantOrdinal) {
    if (!string.Equals(bridge.SourceHubReference.Name, bridge.TargetHubReference.Name, StringComparison.Ordinal)) {
      throw new ArgumentException(
          "Hierarchy bridge metadata '" +
          bridge.Name +
          "' must use the same recursive hub for its source and target endpoints.",
          "metadataModel");
    }

    if (link.Participants.Count != 2 ||
        link.Participants.Any(participant => !string.Equals(
            participant.HubReference.Name,
            bridge.SourceHubReference.Name,
            StringComparison.Ordinal))) {
      throw new ArgumentException(
          "Hierarchy bridge metadata '" +
          bridge.Name +
          "' must traverse a two-participant self-link over hub '" +
          bridge.SourceHubReference.Name +
          "'.",
          "metadataModel");
    }

    if (sourceParticipantOrdinal == targetParticipantOrdinal) {
      throw new ArgumentException(
          "Hierarchy bridge metadata '" +
          bridge.Name +
          "' resolves ancestor and descendant selectors to the same link participant.",
          "metadataModel");
    }
  }

  private ClrLookup CreateClrLookup(IEnumerable<DataVaultMetadataClrMapping> clrMappings) {
    ArgumentNullException.ThrowIfNull(clrMappings);

    var hubs = new Dictionary<Type, DataVaultHubMetadata>();
    var links = new Dictionary<Type, DataVaultLinkMetadata>();
    var satellites = new Dictionary<SatelliteClrKey, DataVaultSatelliteMetadata>();
    var pointInTimeTables = new Dictionary<Type, DataVaultPointInTimeMetadata>();
    var bridges = new Dictionary<Type, DataVaultBridgeMetadata>();
    var pits = new Dictionary<Type, DataVaultPitMetadata>();
    var targets = new HashSet<MappingTargetKey>();

    foreach (var clrMapping in clrMappings) {
      ArgumentNullException.ThrowIfNull(clrMapping, nameof(clrMappings));

      var targetKey = MappingTargetKey.From(clrMapping);
      if (!targets.Add(targetKey)) {
        throw new ArgumentException(
            "Metadata CLR mappings contain more than one CLR type for " +
            FormatKind(clrMapping.Kind) +
            " metadata '" +
            clrMapping.Name +
            "'.",
            "clrMappings");
      }

      switch (clrMapping.Kind) {
        case DataVaultMetadataRegistryKind.Hub:
          AddClrMapping(
              hubs,
              clrMapping,
              ResolveMappingTarget(_hubsByName, clrMapping, "clrMappings"));
          break;
        case DataVaultMetadataRegistryKind.Link:
          AddClrMapping(
              links,
              clrMapping,
              ResolveMappingTarget(_linksByName, clrMapping, "clrMappings"));
          break;
        case DataVaultMetadataRegistryKind.Satellite:
          AddSatelliteClrMapping(satellites, clrMapping);
          break;
        case DataVaultMetadataRegistryKind.PointInTimeTable:
          AddClrMapping(
              pointInTimeTables,
              clrMapping,
              ResolveMappingTarget(_pointInTimeTablesByName, clrMapping, "clrMappings"));
          break;
        case DataVaultMetadataRegistryKind.Bridge:
          AddClrMapping(
              bridges,
              clrMapping,
              ResolveMappingTarget(_bridgesByName, clrMapping, "clrMappings"));
          break;
        case DataVaultMetadataRegistryKind.Pit:
          AddClrMapping(
              pits,
              clrMapping,
              ResolveMappingTarget(_pitsByName, clrMapping, "clrMappings"));
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(clrMappings), clrMapping.Kind, "Unsupported registry kind.");
      }
    }

    return new ClrLookup(hubs, links, satellites, pointInTimeTables, bridges, pits);
  }

  private static void AddClrMapping<T>(
      Dictionary<Type, T> mappingsByClrType,
      DataVaultMetadataClrMapping clrMapping,
      T metadata)
      where T : class {
    if (!mappingsByClrType.TryAdd(clrMapping.ClrType, metadata)) {
      throw AmbiguousClrMapping(clrMapping);
    }
  }

  private void AddSatelliteClrMapping(
      Dictionary<SatelliteClrKey, DataVaultSatelliteMetadata> satellites,
      DataVaultMetadataClrMapping clrMapping) {
    if (clrMapping.Parent is null) {
      throw new ArgumentException(
          "Satellite metadata CLR mapping for '" + clrMapping.Name + "' must declare a hub or link parent.",
          "clrMappings");
    }

    var parentKey = CreateSatelliteParentKey(clrMapping.Parent, "clrMappings");
    var satelliteKey = new SatelliteKey(parentKey, clrMapping.Name);
    if (!_satellitesByKey.TryGetValue(satelliteKey, out var satellite)) {
      throw MissingMappingTarget(clrMapping);
    }

    var clrKey = new SatelliteClrKey(parentKey, clrMapping.ClrType);
    if (!satellites.TryAdd(clrKey, satellite)) {
      throw AmbiguousClrMapping(clrMapping);
    }
  }

  private static T ResolveMappingTarget<T>(
      IReadOnlyDictionary<string, T> metadataByName,
      DataVaultMetadataClrMapping clrMapping,
      string parameterName)
      where T : class {
    if (clrMapping.Parent is not null) {
      throw new ArgumentException(
          FormatKind(clrMapping.Kind) +
          " metadata CLR mapping for '" +
          clrMapping.Name +
          "' must not declare a parent.",
          parameterName);
    }

    if (metadataByName.TryGetValue(clrMapping.Name, out var metadata)) {
      return metadata;
    }

    throw MissingMappingTarget(clrMapping);
  }

  private static ParentKey CreateSatelliteParentKey(DataVaultMetadataReference parent, string parameterName) {
    ArgumentNullException.ThrowIfNull(parent, parameterName);

    if (parent.Kind is not DataVaultMetadataReferenceKind.Hub and not DataVaultMetadataReferenceKind.Link) {
      throw new ArgumentException("A satellite parent must reference a hub or link.", parameterName);
    }

    return new ParentKey(parent.Kind, parent.Name);
  }

  private static string RequireLookupName(string name, string parameterName) {
    return DataVaultMetadataValidation.RequireName(name, parameterName);
  }

  private static ArgumentException DuplicateLogicalName(
      DataVaultMetadataRegistryKind kind,
      string name,
      string parameterName) {
    return new ArgumentException(
        "Duplicate " + FormatKind(kind) + " metadata logical name '" + name + "'.",
        parameterName);
  }

  private static ArgumentException MissingDependency(
      DataVaultMetadataRegistryKind metadataKind,
      string metadataName,
      DataVaultMetadataRegistryKind dependencyKind,
      string dependencyName) {
    return new ArgumentException(
        FormatKind(metadataKind) +
        " metadata '" +
        metadataName +
        "' references missing " +
        FormatKind(dependencyKind) +
        " metadata '" +
        dependencyName +
        "'.",
        "metadataModel");
  }

  private static ArgumentException MissingMappingTarget(DataVaultMetadataClrMapping clrMapping) {
    return new ArgumentException(
        "CLR type mapping for " +
        FormatKind(clrMapping.Kind) +
        " metadata '" +
        clrMapping.Name +
        "' references a metadata declaration that is not present in the registry.",
        "clrMappings");
  }

  private static ArgumentException AmbiguousClrMapping(DataVaultMetadataClrMapping clrMapping) {
    return new ArgumentException(
        "CLR type '" +
        clrMapping.ClrType.FullName +
        "' maps to more than one " +
        FormatKind(clrMapping.Kind) +
        " metadata declaration.",
        "clrMappings");
  }

  private static string FormatKind(DataVaultMetadataRegistryKind kind) {
    return kind switch {
      DataVaultMetadataRegistryKind.Hub => "hub",
      DataVaultMetadataRegistryKind.Link => "link",
      DataVaultMetadataRegistryKind.Satellite => "satellite",
      DataVaultMetadataRegistryKind.PointInTimeTable => "point-in-time table",
      DataVaultMetadataRegistryKind.Bridge => "bridge",
      DataVaultMetadataRegistryKind.Pit => "PIT",
      _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unsupported registry kind."),
    };
  }

  private static string FormatParent(DataVaultMetadataReference parent) {
    return FormatParent(new ParentKey(parent.Kind, parent.Name));
  }

  private static string FormatParent(ParentKey parent) {
    return parent.Kind switch {
      DataVaultMetadataReferenceKind.Hub => "hub '" + parent.Name + "'",
      DataVaultMetadataReferenceKind.Link => "link '" + parent.Name + "'",
      _ => parent.Kind + " '" + parent.Name + "'",
    };
  }

  private readonly record struct ParentKey(DataVaultMetadataReferenceKind Kind, string Name);

  private readonly record struct SatelliteKey(ParentKey Parent, string Name);

  private readonly record struct SatelliteClrKey(ParentKey Parent, Type ClrType);

  private readonly record struct MappingTargetKey(
      DataVaultMetadataRegistryKind Kind,
      ParentKey? Parent,
      string Name) {
    public static MappingTargetKey From(DataVaultMetadataClrMapping clrMapping) {
      var parent = clrMapping.Parent is null
          ? (ParentKey?)null
          : new ParentKey(clrMapping.Parent.Kind, clrMapping.Parent.Name);

      return new MappingTargetKey(clrMapping.Kind, parent, clrMapping.Name);
    }
  }

  private sealed record ClrLookup(
      IReadOnlyDictionary<Type, DataVaultHubMetadata> Hubs,
      IReadOnlyDictionary<Type, DataVaultLinkMetadata> Links,
      IReadOnlyDictionary<SatelliteClrKey, DataVaultSatelliteMetadata> Satellites,
      IReadOnlyDictionary<Type, DataVaultPointInTimeMetadata> PointInTimeTables,
      IReadOnlyDictionary<Type, DataVaultBridgeMetadata> Bridges,
      IReadOnlyDictionary<Type, DataVaultPitMetadata> Pits);
}
