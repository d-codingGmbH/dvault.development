using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Summarizes one hub, link, or satellite row persisted by an explicit DVault save request.
/// </summary>
public sealed class DataVaultSavedRecord {
  /// <summary>
  /// Initializes a new saved row summary.
  /// </summary>
  /// <param name="kind">Whether the saved row is a hub, link, or satellite.</param>
  /// <param name="metadataName">The metadata declaration name that produced the row.</param>
  /// <param name="tableName">The produced table name that received the row.</param>
  /// <param name="hashKey">The generated Data Vault hash key persisted for the row, or parent hash key for satellites.</param>
  public DataVaultSavedRecord(DataVaultTableKind kind, string metadataName, string tableName, string hashKey)
      : this(kind, metadataName, tableName, hashKey, []) {
  }

  /// <summary>
  /// Initializes a new saved row summary with multi-active driving-key identity values.
  /// </summary>
  /// <param name="kind">Whether the saved row is a hub, link, or satellite.</param>
  /// <param name="metadataName">The metadata declaration name that produced the row.</param>
  /// <param name="tableName">The produced table name that received the row.</param>
  /// <param name="hashKey">The generated Data Vault hash key persisted for the row, or parent hash key for satellites.</param>
  /// <param name="drivingKeyValues">Driving-key identity values keyed by canonical driving-key name.</param>
  public DataVaultSavedRecord(
      DataVaultTableKind kind,
      string metadataName,
      string tableName,
      string hashKey,
      IEnumerable<KeyValuePair<string, string>> drivingKeyValues)
      : this(kind, metadataName, tableName, hashKey, drivingKeyValues, []) {
  }

  /// <summary>
  /// Initializes a new saved row summary with additional row identity values.
  /// </summary>
  /// <param name="kind">Whether the saved row is a hub, link, or satellite.</param>
  /// <param name="metadataName">The metadata declaration name that produced the row.</param>
  /// <param name="tableName">The produced table name that received the row.</param>
  /// <param name="hashKey">The generated Data Vault hash key persisted for the row, or parent hash key for satellites.</param>
  /// <param name="drivingKeyValues">Driving-key identity values keyed by canonical driving-key name.</param>
  /// <param name="dependentChildKeyValues">Dependent child key identity values keyed by canonical dependent child key name.</param>
  public DataVaultSavedRecord(
      DataVaultTableKind kind,
      string metadataName,
      string tableName,
      string hashKey,
      IEnumerable<KeyValuePair<string, string>> drivingKeyValues,
      IEnumerable<KeyValuePair<string, string>> dependentChildKeyValues) {
    ArgumentException.ThrowIfNullOrWhiteSpace(metadataName);
    ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
    ArgumentException.ThrowIfNullOrWhiteSpace(hashKey);

    Kind = kind;
    MetadataName = metadataName;
    TableName = tableName;
    HashKey = hashKey;
    DrivingKeyValues = DataVaultHubSaveOperation.RequireValues(drivingKeyValues, nameof(drivingKeyValues));
    DependentChildKeyValues = DataVaultHubSaveOperation.RequireValues(
        dependentChildKeyValues,
        nameof(dependentChildKeyValues));
  }

  /// <summary>
  /// Gets whether the saved row is a hub, link, or satellite.
  /// </summary>
  public DataVaultTableKind Kind { get; }

  /// <summary>
  /// Gets the metadata declaration name that produced the row.
  /// </summary>
  public string MetadataName { get; }

  /// <summary>
  /// Gets the produced table name that received the row.
  /// </summary>
  public string TableName { get; }

  /// <summary>
  /// Gets the generated Data Vault hash key persisted for the row, or parent hash key for satellites.
  /// </summary>
  public string HashKey { get; }

  /// <summary>
  /// Gets multi-active driving-key identity values keyed by canonical driving-key name.
  /// </summary>
  public IReadOnlyDictionary<string, string> DrivingKeyValues { get; }

  /// <summary>
  /// Gets dependent child key identity values keyed by canonical dependent child key name.
  /// </summary>
  public IReadOnlyDictionary<string, string> DependentChildKeyValues { get; }
}
