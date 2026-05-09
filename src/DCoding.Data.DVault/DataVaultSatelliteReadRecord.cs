namespace DCoding.Data.DVault;

/// <summary>
/// Describes one materialized satellite row returned by a Data Vault read service.
/// </summary>
public sealed class DataVaultSatelliteReadRecord {
  internal DataVaultSatelliteReadRecord(
      string metadataName,
      string tableName,
      string parentHashKey,
      IReadOnlyDictionary<string, string> drivingKeyValues,
      string hashDiff,
      DateTimeOffset loadTimestamp,
      string recordSource,
      IReadOnlyDictionary<string, string> payloadValues) {
    MetadataName = metadataName;
    TableName = tableName;
    ParentHashKey = parentHashKey;
    DrivingKeyValues = drivingKeyValues;
    HashDiff = hashDiff;
    LoadTimestamp = loadTimestamp;
    RecordSource = recordSource;
    PayloadValues = payloadValues;
  }

  /// <summary>
  /// Gets the satellite metadata declaration name.
  /// </summary>
  public string MetadataName { get; }

  /// <summary>
  /// Gets the produced satellite table name.
  /// </summary>
  public string TableName { get; }

  /// <summary>
  /// Gets the parent hub or link hash key.
  /// </summary>
  public string ParentHashKey { get; }

  /// <summary>
  /// Gets multi-active driving-key values keyed by metadata driving-key name.
  /// </summary>
  public IReadOnlyDictionary<string, string> DrivingKeyValues { get; }

  /// <summary>
  /// Gets the satellite hash diff.
  /// </summary>
  public string HashDiff { get; }

  /// <summary>
  /// Gets the satellite load timestamp normalized to UTC.
  /// </summary>
  public DateTimeOffset LoadTimestamp { get; }

  /// <summary>
  /// Gets the satellite record source.
  /// </summary>
  public string RecordSource { get; }

  /// <summary>
  /// Gets payload values keyed by metadata payload column name.
  /// </summary>
  public IReadOnlyDictionary<string, string> PayloadValues { get; }
}
