using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies the Data Vault metadata structures that can be referenced by another metadata declaration.
/// </summary>
public enum DataVaultMetadataReferenceKind {
  /// <summary>
  /// References a hub metadata declaration.
  /// </summary>
  Hub,

  /// <summary>
  /// References a link metadata declaration.
  /// </summary>
  Link,

  /// <summary>
  /// References a satellite metadata declaration.
  /// </summary>
  Satellite,
}

/// <summary>
/// Identifies the supported provider-neutral bridge traversal shapes.
/// </summary>
public enum DataVaultBridgeKind {
  /// <summary>
  /// Represents a bridge that traverses one link between a source hub and a target hub.
  /// </summary>
  ManyToMany,

  /// <summary>
  /// Represents a bridge that traverses one recursive link as a directional hierarchy edge.
  /// </summary>
  Hierarchy,
}

/// <summary>
/// Represents a named hub, link, or satellite metadata target.
/// </summary>
public sealed class DataVaultMetadataReference {
  private DataVaultMetadataReference(DataVaultMetadataReferenceKind kind, string name) {
    Kind = kind;
    Name = name;
  }

  /// <summary>
  /// Gets the kind of metadata declaration being referenced.
  /// </summary>
  public DataVaultMetadataReferenceKind Kind { get; }

  /// <summary>
  /// Gets the referenced hub, link, or satellite name.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Creates a reference to hub metadata.
  /// </summary>
  public static DataVaultMetadataReference Hub(string name) {
    return new DataVaultMetadataReference(
        DataVaultMetadataReferenceKind.Hub,
        DataVaultMetadataValidation.RequireName(name, nameof(name)));
  }

  /// <summary>
  /// Creates a reference to link metadata.
  /// </summary>
  public static DataVaultMetadataReference Link(string name) {
    return new DataVaultMetadataReference(
        DataVaultMetadataReferenceKind.Link,
        DataVaultMetadataValidation.RequireName(name, nameof(name)));
  }

  /// <summary>
  /// Creates a reference to satellite metadata.
  /// </summary>
  public static DataVaultMetadataReference Satellite(string name) {
    return new DataVaultMetadataReference(
        DataVaultMetadataReferenceKind.Satellite,
        DataVaultMetadataValidation.RequireName(name, nameof(name)));
  }
}

/// <summary>
/// Describes one business-key column declared by a Data Vault hub.
/// </summary>
public sealed class DataVaultBusinessKeyMetadata {
  /// <summary>
  /// Initializes a new business-key metadata declaration.
  /// </summary>
  /// <param name="columnName">The provider-neutral business-key column name.</param>
  public DataVaultBusinessKeyMetadata(string columnName) {
    ColumnName = DataVaultMetadataValidation.RequireName(columnName, nameof(columnName));
  }

  /// <summary>
  /// Gets the provider-neutral business-key column name.
  /// </summary>
  public string ColumnName { get; }
}

/// <summary>
/// Describes one participating hub and hash-key reference in a Data Vault link.
/// </summary>
public sealed class DataVaultLinkParticipantMetadata {
  /// <summary>
  /// Initializes a new link participant metadata declaration.
  /// </summary>
  /// <param name="hubReference">The hub referenced by this link participant.</param>
  public DataVaultLinkParticipantMetadata(DataVaultMetadataReference hubReference) {
    HubReference = DataVaultMetadataValidation.RequireHubReference(hubReference, nameof(hubReference));
    HashKeyMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.HashKey);
  }

  /// <summary>
  /// Gets the participating hub reference.
  /// </summary>
  public DataVaultMetadataReference HubReference { get; }

  /// <summary>
  /// Gets the technical hash-key metadata used to reference the participating hub key.
  /// </summary>
  public TechnicalMetadataColumnContract HashKeyMetadata { get; }
}

/// <summary>
/// Identifies the role of one bridge endpoint binding.
/// </summary>
internal enum DataVaultBridgeEndpointRole {
  /// <summary>
  /// Many-to-many source endpoint.
  /// </summary>
  From,

  /// <summary>
  /// Many-to-many target endpoint.
  /// </summary>
  To,

  /// <summary>
  /// Hierarchy ancestor endpoint.
  /// </summary>
  Ancestor,

  /// <summary>
  /// Hierarchy descendant endpoint.
  /// </summary>
  Descendant,
}

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

/// <summary>
/// Identifies optional bridge projection features outside the baseline provider-neutral v1 mapping.
/// </summary>
[Flags]
internal enum DataVaultBridgeProjectionFeatures {
  /// <summary>
  /// The bridge asks only for the baseline provider-neutral projection.
  /// </summary>
  None = 0,

  /// <summary>
  /// The bridge asks for effectivity-window columns.
  /// </summary>
  EffectivityWindow = 1,

  /// <summary>
  /// The bridge asks for additional path payload columns.
  /// </summary>
  PathPayload = 2,

  /// <summary>
  /// The bridge asks for closure maintenance state.
  /// </summary>
  ClosureMaintenance = 4,

  /// <summary>
  /// The bridge asks for generated Entity Framework relationship graph metadata.
  /// </summary>
  RelationshipGraph = 8,
}

/// <summary>
/// Describes one provider-neutral bridge traversal over a declared Data Vault link.
/// </summary>
public sealed class DataVaultBridgeMetadata {
  /// <summary>
  /// Initializes a bridge metadata declaration whose endpoint participants can be resolved by hub reference.
  /// </summary>
  public DataVaultBridgeMetadata(
      string name,
      DataVaultBridgeKind kind,
      DataVaultMetadataReference sourceHubReference,
      DataVaultMetadataReference linkReference,
      DataVaultMetadataReference targetHubReference)
      : this(
          name,
          kind,
          sourceHubReference,
          linkReference,
          targetHubReference,
          sourceParticipantOrdinal: null,
          targetParticipantOrdinal: null) {
  }

  /// <summary>
  /// Initializes a bridge metadata declaration with explicit endpoint participant ordinals.
  /// </summary>
  public DataVaultBridgeMetadata(
      string name,
      DataVaultBridgeKind kind,
      DataVaultMetadataReference sourceHubReference,
      DataVaultMetadataReference linkReference,
      DataVaultMetadataReference targetHubReference,
      int sourceParticipantOrdinal,
      int targetParticipantOrdinal)
      : this(
          name,
          kind,
          sourceHubReference,
          linkReference,
          targetHubReference,
          (int?)sourceParticipantOrdinal,
          targetParticipantOrdinal) {
  }

  private DataVaultBridgeMetadata(
      string name,
      DataVaultBridgeKind kind,
      DataVaultMetadataReference sourceHubReference,
      DataVaultMetadataReference linkReference,
      DataVaultMetadataReference targetHubReference,
      int? sourceParticipantOrdinal,
      int? targetParticipantOrdinal) {
    Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
    Kind = DataVaultMetadataValidation.RequireBridgeKind(kind, nameof(kind));
    SourceHubReference = DataVaultMetadataValidation.RequireHubReference(
        sourceHubReference,
        nameof(sourceHubReference));
    LinkReference = DataVaultMetadataValidation.RequireLinkReference(linkReference, nameof(linkReference));
    TargetHubReference = DataVaultMetadataValidation.RequireHubReference(
        targetHubReference,
        nameof(targetHubReference));
    SourceParticipantOrdinal = RequireParticipantOrdinal(sourceParticipantOrdinal, nameof(sourceParticipantOrdinal));
    TargetParticipantOrdinal = RequireParticipantOrdinal(targetParticipantOrdinal, nameof(targetParticipantOrdinal));
    Source = LinkReference;
    Endpoints = CreateDefaultEndpoints(Kind, SourceHubReference, TargetHubReference);
    ProjectionFeatures = DataVaultBridgeProjectionFeatures.None;
    RequiresReferenceValidation = true;
  }

  /// <summary>
  /// Initializes a new bridge metadata declaration.
  /// </summary>
  internal DataVaultBridgeMetadata(
      string name,
      DataVaultBridgeKind kind,
      DataVaultMetadataReference source,
      IEnumerable<DataVaultBridgeEndpointMetadata> endpoints,
      DataVaultBridgeProjectionFeatures projectionFeatures = DataVaultBridgeProjectionFeatures.None) {
    Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
    Source = DataVaultMetadataValidation.RequireLinkReference(source, nameof(source));
    LinkReference = Source;
    Kind = DataVaultMetadataValidation.RequireBridgeKind(kind, nameof(kind));
    Endpoints = RequireEndpointRoles(
        DataVaultMetadataValidation.RequireItems(
            endpoints,
            nameof(endpoints),
            "A bridge requires at least one endpoint binding."),
        Kind,
        nameof(endpoints));
    (SourceHubReference, TargetHubReference) = ResolveEndpointReferences(Kind, Endpoints);
    SourceParticipantOrdinal = null;
    TargetParticipantOrdinal = null;
    ProjectionFeatures = projectionFeatures;
    RequiresReferenceValidation = false;
  }

  /// <summary>
  /// Gets the bridge name.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the bridge traversal kind.
  /// </summary>
  public DataVaultBridgeKind Kind { get; }

  /// <summary>
  /// Gets the hub that starts the bridge traversal.
  /// </summary>
  public DataVaultMetadataReference SourceHubReference { get; }

  /// <summary>
  /// Gets the link traversed by the bridge.
  /// </summary>
  public DataVaultMetadataReference LinkReference { get; }

  /// <summary>
  /// Gets the hub reached by the bridge traversal.
  /// </summary>
  public DataVaultMetadataReference TargetHubReference { get; }

  /// <summary>
  /// Gets the source-side link participant ordinal, when declared explicitly.
  /// </summary>
  public int? SourceParticipantOrdinal { get; }

  /// <summary>
  /// Gets the target-side link participant ordinal, when declared explicitly.
  /// </summary>
  public int? TargetParticipantOrdinal { get; }

  /// <summary>
  /// Gets the source link traversed by the bridge.
  /// </summary>
  internal DataVaultMetadataReference Source { get; }

  /// <summary>
  /// Gets the ordered endpoint bindings projected by the bridge.
  /// </summary>
  internal IReadOnlyList<DataVaultBridgeEndpointMetadata> Endpoints { get; }

  /// <summary>
  /// Gets the optional bridge projection features requested beyond the baseline.
  /// </summary>
  internal DataVaultBridgeProjectionFeatures ProjectionFeatures { get; }

  internal bool RequiresReferenceValidation { get; }

  /// <summary>
  /// Creates a many-to-many bridge declaration whose endpoint participants can be resolved by hub reference.
  /// </summary>
  public static DataVaultBridgeMetadata ManyToMany(
      string name,
      DataVaultMetadataReference sourceHubReference,
      DataVaultMetadataReference linkReference,
      DataVaultMetadataReference targetHubReference) {
    return new DataVaultBridgeMetadata(
        name,
        DataVaultBridgeKind.ManyToMany,
        sourceHubReference,
        linkReference,
        targetHubReference);
  }

  /// <summary>
  /// Creates a many-to-many bridge declaration with explicit endpoint participant ordinals.
  /// </summary>
  public static DataVaultBridgeMetadata ManyToMany(
      string name,
      DataVaultMetadataReference sourceHubReference,
      DataVaultMetadataReference linkReference,
      DataVaultMetadataReference targetHubReference,
      int sourceParticipantOrdinal,
      int targetParticipantOrdinal) {
    return new DataVaultBridgeMetadata(
        name,
        DataVaultBridgeKind.ManyToMany,
        sourceHubReference,
        linkReference,
        targetHubReference,
        sourceParticipantOrdinal,
        targetParticipantOrdinal);
  }

  /// <summary>
  /// Creates a hierarchy bridge declaration with explicit ancestor and descendant participant ordinals.
  /// </summary>
  public static DataVaultBridgeMetadata Hierarchy(
      string name,
      DataVaultMetadataReference ancestorHubReference,
      DataVaultMetadataReference linkReference,
      DataVaultMetadataReference descendantHubReference,
      int ancestorParticipantOrdinal,
      int descendantParticipantOrdinal) {
    return new DataVaultBridgeMetadata(
        name,
        DataVaultBridgeKind.Hierarchy,
        ancestorHubReference,
        linkReference,
        descendantHubReference,
        ancestorParticipantOrdinal,
        descendantParticipantOrdinal);
  }

  private static int? RequireParticipantOrdinal(int? participantOrdinal, string parameterName) {
    if (participantOrdinal.GetValueOrDefault() < 0 && participantOrdinal.HasValue) {
      throw new ArgumentOutOfRangeException(
          parameterName,
          participantOrdinal,
          "Bridge participant ordinals must be zero or greater.");
    }

    return participantOrdinal;
  }

  private static IReadOnlyList<DataVaultBridgeEndpointMetadata> CreateDefaultEndpoints(
      DataVaultBridgeKind kind,
      DataVaultMetadataReference sourceHubReference,
      DataVaultMetadataReference targetHubReference) {
    return kind switch {
      DataVaultBridgeKind.ManyToMany =>
      [
          new DataVaultBridgeEndpointMetadata(
              DataVaultBridgeEndpointRole.From,
              sourceHubReference,
              sourceHubReference.Name),
          new DataVaultBridgeEndpointMetadata(
              DataVaultBridgeEndpointRole.To,
              targetHubReference,
              targetHubReference.Name),
      ],
      DataVaultBridgeKind.Hierarchy =>
      [
          new DataVaultBridgeEndpointMetadata(
              DataVaultBridgeEndpointRole.Ancestor,
              sourceHubReference,
              sourceHubReference.Name),
          new DataVaultBridgeEndpointMetadata(
              DataVaultBridgeEndpointRole.Descendant,
              targetHubReference,
              targetHubReference.Name),
      ],
      _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unsupported bridge kind."),
    };
  }

  private static (DataVaultMetadataReference SourceHubReference, DataVaultMetadataReference TargetHubReference)
      ResolveEndpointReferences(
          DataVaultBridgeKind kind,
          IReadOnlyList<DataVaultBridgeEndpointMetadata> endpoints) {
    return kind switch {
      DataVaultBridgeKind.ManyToMany => (
          endpoints.Single(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.From).HubReference,
          endpoints.Single(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.To).HubReference),
      DataVaultBridgeKind.Hierarchy => (
          endpoints.Single(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.Ancestor).HubReference,
          endpoints.Single(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.Descendant).HubReference),
      _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unsupported bridge kind."),
    };
  }

  private static IReadOnlyList<DataVaultBridgeEndpointMetadata> RequireEndpointRoles(
      IReadOnlyList<DataVaultBridgeEndpointMetadata> endpoints,
      DataVaultBridgeKind kind,
      string parameterName) {
    return kind switch {
      DataVaultBridgeKind.ManyToMany => RequireEndpointRoles(
          endpoints,
          parameterName,
          DataVaultBridgeEndpointRole.From,
          DataVaultBridgeEndpointRole.To,
          "A many-to-many bridge requires exactly one From endpoint and exactly one To endpoint."),
      DataVaultBridgeKind.Hierarchy => RequireEndpointRoles(
          endpoints,
          parameterName,
          DataVaultBridgeEndpointRole.Ancestor,
          DataVaultBridgeEndpointRole.Descendant,
          "A hierarchy bridge requires exactly one Ancestor endpoint and exactly one Descendant endpoint."),
      _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unsupported bridge kind."),
    };
  }

  private static IReadOnlyList<DataVaultBridgeEndpointMetadata> RequireEndpointRoles(
      IReadOnlyList<DataVaultBridgeEndpointMetadata> endpoints,
      string parameterName,
      DataVaultBridgeEndpointRole firstRole,
      DataVaultBridgeEndpointRole secondRole,
      string message) {
    if (endpoints.Count != 2 ||
        endpoints.Count(endpoint => endpoint.Role == firstRole) != 1 ||
        endpoints.Count(endpoint => endpoint.Role == secondRole) != 1) {
      throw new ArgumentException(message, parameterName);
    }

    return endpoints;
  }
}

/// <summary>
/// Describes one payload column declared by a Data Vault satellite.
/// </summary>
public sealed class DataVaultSatellitePayloadMetadata {
  /// <summary>
  /// Initializes a new satellite payload metadata declaration.
  /// </summary>
  /// <param name="columnName">The provider-neutral satellite payload column name.</param>
  public DataVaultSatellitePayloadMetadata(string columnName) {
    ColumnName = DataVaultMetadataValidation.RequireName(columnName, nameof(columnName));
  }

  /// <summary>
  /// Gets the provider-neutral satellite payload column name.
  /// </summary>
  public string ColumnName { get; }
}

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

/// <summary>
/// Describes the identifying metadata for a Data Vault hub.
/// </summary>
public sealed class DataVaultHubMetadata {
  /// <summary>
  /// Initializes a new hub metadata declaration.
  /// </summary>
  public DataVaultHubMetadata(string name, IEnumerable<string> businessKeyNames) {
    Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
    BusinessKeyNames = DataVaultMetadataValidation.RequireNames(
        businessKeyNames,
        nameof(businessKeyNames),
        "A hub requires at least one business-key name.");
    BusinessKeyColumns = BusinessKeyNames
        .Select(columnName => new DataVaultBusinessKeyMetadata(columnName))
        .ToArray();
    HashKeyMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.HashKey);
    LoadTimestampMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.LoadTimestamp);
    RecordSourceMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.RecordSource);
    TechnicalMetadataColumns =
    [
        HashKeyMetadata,
        LoadTimestampMetadata,
        RecordSourceMetadata,
    ];
  }

  /// <summary>
  /// Gets the hub name.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the business-key names that identify the hub.
  /// </summary>
  public IReadOnlyList<string> BusinessKeyNames { get; }

  /// <summary>
  /// Gets the business-key column metadata that identifies the hub.
  /// </summary>
  public IReadOnlyList<DataVaultBusinessKeyMetadata> BusinessKeyColumns { get; }

  /// <summary>
  /// Gets the required hash-key technical metadata for the hub.
  /// </summary>
  public TechnicalMetadataColumnContract HashKeyMetadata { get; }

  /// <summary>
  /// Gets the required load-timestamp technical metadata for the hub.
  /// </summary>
  public TechnicalMetadataColumnContract LoadTimestampMetadata { get; }

  /// <summary>
  /// Gets the required record-source technical metadata for the hub.
  /// </summary>
  public TechnicalMetadataColumnContract RecordSourceMetadata { get; }

  /// <summary>
  /// Gets the required technical metadata columns for hub records.
  /// </summary>
  public IReadOnlyList<TechnicalMetadataColumnContract> TechnicalMetadataColumns { get; }

  /// <summary>
  /// Creates a reference to this hub metadata declaration.
  /// </summary>
  public DataVaultMetadataReference ToReference() {
    return DataVaultMetadataReference.Hub(Name);
  }
}

/// <summary>
/// Describes the hub endpoints that participate in a Data Vault link.
/// </summary>
public sealed class DataVaultLinkMetadata {
  /// <summary>
  /// Initializes a new link metadata declaration.
  /// </summary>
  public DataVaultLinkMetadata(string name, IEnumerable<DataVaultMetadataReference> endpoints) {
    Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
    Participants = RequireHubParticipants(endpoints);
    Endpoints = Participants.Select(participant => participant.HubReference).ToArray();
    HashKeyMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.HashKey);
    LoadTimestampMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.LoadTimestamp);
    RecordSourceMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.RecordSource);
    TechnicalMetadataColumns =
    [
        HashKeyMetadata,
        LoadTimestampMetadata,
        RecordSourceMetadata,
    ];
  }

  /// <summary>
  /// Gets the link name.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the hub endpoints connected by the link.
  /// </summary>
  public IReadOnlyList<DataVaultMetadataReference> Endpoints { get; }

  /// <summary>
  /// Gets the participating hub and hash-key metadata connected by the link.
  /// </summary>
  public IReadOnlyList<DataVaultLinkParticipantMetadata> Participants { get; }

  /// <summary>
  /// Gets the required relationship hash-key technical metadata for the link.
  /// </summary>
  public TechnicalMetadataColumnContract HashKeyMetadata { get; }

  /// <summary>
  /// Gets the required load-timestamp technical metadata for the link.
  /// </summary>
  public TechnicalMetadataColumnContract LoadTimestampMetadata { get; }

  /// <summary>
  /// Gets the required record-source technical metadata for the link.
  /// </summary>
  public TechnicalMetadataColumnContract RecordSourceMetadata { get; }

  /// <summary>
  /// Gets the required technical metadata columns for link records.
  /// </summary>
  public IReadOnlyList<TechnicalMetadataColumnContract> TechnicalMetadataColumns { get; }

  /// <summary>
  /// Creates a reference to this link metadata declaration.
  /// </summary>
  public DataVaultMetadataReference ToReference() {
    return DataVaultMetadataReference.Link(Name);
  }

  private static IReadOnlyList<DataVaultLinkParticipantMetadata> RequireHubParticipants(IEnumerable<DataVaultMetadataReference> endpoints) {
    ArgumentNullException.ThrowIfNull(endpoints);

    var values = endpoints.ToArray();
    if (values.Length < 2) {
      throw new ArgumentException("A link requires at least two hub endpoints.", nameof(endpoints));
    }

    foreach (var endpoint in values) {
      DataVaultMetadataValidation.RequireHubReference(endpoint, nameof(endpoints));
    }

    return values
        .Select(endpoint => new DataVaultLinkParticipantMetadata(endpoint))
        .ToArray();
  }
}

/// <summary>
/// Describes the descriptive metadata associated with a hub or link parent.
/// </summary>
public sealed class DataVaultSatelliteMetadata {
  /// <summary>
  /// Initializes a new satellite metadata declaration.
  /// </summary>
  public DataVaultSatelliteMetadata(
      string name,
      DataVaultMetadataReference parent,
      IEnumerable<string> descriptiveAttributeNames) {
    Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
    ArgumentNullException.ThrowIfNull(parent);

    Parent = parent;
    DescriptiveAttributeNames = DataVaultMetadataValidation.RequireNames(
        descriptiveAttributeNames,
        nameof(descriptiveAttributeNames),
        "A satellite requires at least one descriptive attribute name.");
    PayloadColumns = DescriptiveAttributeNames
        .Select(columnName => new DataVaultSatellitePayloadMetadata(columnName))
        .ToArray();
    HashDiffMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.HashDiff);
    LoadTimestampMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.LoadTimestamp);
    RecordSourceMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.RecordSource);
    TechnicalMetadataColumns =
    [
        HashDiffMetadata,
        LoadTimestampMetadata,
        RecordSourceMetadata,
    ];
  }

  /// <summary>
  /// Gets the satellite name.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the hub or link parent associated with the satellite.
  /// </summary>
  public DataVaultMetadataReference Parent { get; }

  /// <summary>
  /// Gets the descriptive attribute names carried by the satellite.
  /// </summary>
  public IReadOnlyList<string> DescriptiveAttributeNames { get; }

  /// <summary>
  /// Gets the payload column metadata carried by the satellite.
  /// </summary>
  public IReadOnlyList<DataVaultSatellitePayloadMetadata> PayloadColumns { get; }

  /// <summary>
  /// Gets the required hash-diff technical metadata for the satellite.
  /// </summary>
  public TechnicalMetadataColumnContract HashDiffMetadata { get; }

  /// <summary>
  /// Gets the required load-timestamp technical metadata for the satellite.
  /// </summary>
  public TechnicalMetadataColumnContract LoadTimestampMetadata { get; }

  /// <summary>
  /// Gets the required record-source technical metadata for the satellite.
  /// </summary>
  public TechnicalMetadataColumnContract RecordSourceMetadata { get; }

  /// <summary>
  /// Gets the required technical metadata columns for satellite records.
  /// </summary>
  public IReadOnlyList<TechnicalMetadataColumnContract> TechnicalMetadataColumns { get; }
}

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
      IEnumerable<DataVaultPitSatelliteReferenceMetadata> satellites) {
    ArgumentNullException.ThrowIfNull(parent);

    Parent = parent;
    Satellites = DataVaultMetadataValidation.RequireItems(satellites, nameof(satellites));
    Name = CreateDefaultName(parent.Name, Satellites);
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
      string parentName,
      IEnumerable<DataVaultPitSatelliteReferenceMetadata> satellites) {
    var namingPolicy = DefaultNamingPolicy.Instance;

    return namingPolicy.NormalizeProducedIdentifier(parentName) +
        string.Concat(satellites.Select(satellite => namingPolicy.NormalizeProducedIdentifier(satellite.SatelliteName)));
  }
}

internal static class DataVaultMetadataValidation {
  public static string RequireName(string name, string parameterName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(name, parameterName);

    return name;
  }

  public static IReadOnlyList<string> RequireNames(
      IEnumerable<string> names,
      string parameterName,
      string emptyMessage) {
    ArgumentNullException.ThrowIfNull(names, parameterName);

    var values = names.ToArray();
    if (values.Length == 0) {
      throw new ArgumentException(emptyMessage, parameterName);
    }

    foreach (var value in values) {
      ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
    }

    return values;
  }

  public static DataVaultMetadataReference RequireHubReference(
      DataVaultMetadataReference reference,
      string parameterName) {
    ArgumentNullException.ThrowIfNull(reference, parameterName);

    if (reference.Kind != DataVaultMetadataReferenceKind.Hub) {
      throw new ArgumentException("A link participant must reference a hub.", parameterName);
    }

    return reference;
  }

  public static DataVaultMetadataReference RequireSatelliteReference(
      DataVaultMetadataReference reference,
      string parameterName) {
    ArgumentNullException.ThrowIfNull(reference, parameterName);

    if (reference.Kind != DataVaultMetadataReferenceKind.Satellite) {
      throw new ArgumentException("A point-in-time satellite reference must reference a satellite.", parameterName);
    }

    return reference;
  }
  public static DataVaultMetadataReference RequireLinkReference(
      DataVaultMetadataReference reference,
      string parameterName) {
    ArgumentNullException.ThrowIfNull(reference, parameterName);

    if (reference.Kind != DataVaultMetadataReferenceKind.Link) {
      throw new ArgumentException("A bridge source must reference a link.", parameterName);
    }

    return reference;
  }

  public static DataVaultBridgeKind RequireBridgeKind(DataVaultBridgeKind kind, string parameterName) {
    if (!Enum.IsDefined(typeof(DataVaultBridgeKind), kind)) {
      throw new ArgumentOutOfRangeException(parameterName, kind, "Unsupported Data Vault bridge kind.");
    }

    return kind;
  }

  public static IReadOnlyList<T> RequireItems<T>(
      IEnumerable<T> items,
      string parameterName,
      string emptyMessage)
      where T : class {
    ArgumentNullException.ThrowIfNull(items, parameterName);

    var values = items.ToArray();
    if (values.Length == 0) {
      throw new ArgumentException(emptyMessage, parameterName);
    }
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Metadata declaration collections must not contain null values.", parameterName);
      }
    }

    return values;
  }

  public static IReadOnlyList<T> RequireItems<T>(IEnumerable<T> items, string parameterName)
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
}
