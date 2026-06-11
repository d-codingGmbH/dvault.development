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
      : this(name, CreateHubParticipants(endpoints)) {
  }

  internal DataVaultLinkMetadata(
      string name,
      IEnumerable<DataVaultLinkParticipantMetadata> participants) {
    Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
    Participants = RequireHubParticipants(participants);
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
}
