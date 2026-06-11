using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

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
