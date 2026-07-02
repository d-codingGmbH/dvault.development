using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes the hub endpoints that participate in a Data Vault link.
/// </summary>
public sealed class DataVaultLinkMetadata {
  /// <summary>
  /// Initializes a new link metadata declaration.
  /// </summary>
  public DataVaultLinkMetadata(string name, IEnumerable<DataVaultMetadataReference> endpoints)
      : this(name, CreateHubParticipants(endpoints), []) {
  }

  /// <summary>
  /// Initializes a new link metadata declaration with dependent child key columns.
  /// </summary>
  public DataVaultLinkMetadata(
      string name,
      IEnumerable<DataVaultMetadataReference> endpoints,
      IEnumerable<string> dependentChildKeyNames)
      : this(name, CreateHubParticipants(endpoints), dependentChildKeyNames) {
  }

  internal DataVaultLinkMetadata(
      string name,
      IEnumerable<DataVaultLinkParticipantMetadata> participants)
      : this(name, participants, []) {
  }

  internal DataVaultLinkMetadata(
      string name,
      IEnumerable<DataVaultLinkParticipantMetadata> participants,
      IEnumerable<string> dependentChildKeyNames) {
    Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
    Participants = RequireHubParticipants(participants);
    Endpoints = Participants.Select(participant => participant.HubReference).ToArray();
    DependentChildKeyNames = ReadDependentChildKeyNames(dependentChildKeyNames, Participants);
    DependentChildKeys = DependentChildKeyNames
        .Select(name => new DataVaultLinkDependentChildKeyMetadata(name))
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
  /// Gets the declared dependent child key names in canonical declaration order.
  /// </summary>
  public IReadOnlyList<string> DependentChildKeyNames { get; }

  /// <summary>
  /// Gets the dependent child key column metadata carried by the link.
  /// </summary>
  public IReadOnlyList<DataVaultLinkDependentChildKeyMetadata> DependentChildKeys { get; }

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

  private static IReadOnlyList<DataVaultLinkParticipantMetadata> CreateHubParticipants(
      IEnumerable<DataVaultMetadataReference> endpoints) {
    ArgumentNullException.ThrowIfNull(endpoints);

    return endpoints
        .Select(endpoint => new DataVaultLinkParticipantMetadata(endpoint))
        .ToArray();
  }

  private static IReadOnlyList<DataVaultLinkParticipantMetadata> RequireHubParticipants(
      IEnumerable<DataVaultLinkParticipantMetadata> participants) {
    var values = DataVaultMetadataValidation.RequireItems(
        participants,
        nameof(participants),
        "A link requires at least two hub endpoints.");

    if (values.Count < 2) {
      throw new ArgumentException("A link requires at least two hub endpoints.", nameof(participants));
    }

    return values;
  }

  private static IReadOnlyList<string> ReadDependentChildKeyNames(
      IEnumerable<string> dependentChildKeyNames,
      IReadOnlyList<DataVaultLinkParticipantMetadata> participants) {
    ArgumentNullException.ThrowIfNull(dependentChildKeyNames);

    var values = dependentChildKeyNames.ToArray();
    var dependentChildKeyNameSet = new HashSet<string>(StringComparer.Ordinal);
    foreach (var value in values) {
      ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(dependentChildKeyNames));
      if (!dependentChildKeyNameSet.Add(value)) {
        throw new ArgumentException(
            "Link dependent child key names must be unique by ordinal comparison.",
            nameof(dependentChildKeyNames));
      }
    }

    var participantNameSet = participants
        .Select(participant => participant.SourceEndpointName)
        .ToHashSet(StringComparer.Ordinal);
    foreach (var value in values) {
      if (participantNameSet.Contains(value)) {
        throw new ArgumentException(
            "Link dependent child key names must not overlap produced participant names.",
            nameof(dependentChildKeyNames));
      }
    }

    return values;
  }
}
