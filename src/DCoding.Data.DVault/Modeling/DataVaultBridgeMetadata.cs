using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

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
