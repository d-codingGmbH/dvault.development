using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one materialized row returned by a provider-neutral bridge read.
/// </summary>
public sealed class DataVaultBridgeReadRecord {
  internal DataVaultBridgeReadRecord(
      string metadataName,
      string tableName,
      IReadOnlyList<DataVaultBridgeEndpointReadValue> endpointHashKeys,
      int? traversalDepth) {
    MetadataName = metadataName;
    TableName = tableName;
    EndpointHashKeys = new ReadOnlyCollection<DataVaultBridgeEndpointReadValue>(endpointHashKeys.ToArray());
    TraversalDepth = traversalDepth;
  }

  /// <summary>
  /// Gets the bridge metadata declaration name.
  /// </summary>
  public string MetadataName { get; }

  /// <summary>
  /// Gets the produced bridge table name.
  /// </summary>
  public string TableName { get; }

  /// <summary>
  /// Gets endpoint hash keys in the generated bridge metadata column order.
  /// </summary>
  public IReadOnlyList<DataVaultBridgeEndpointReadValue> EndpointHashKeys { get; }

  /// <summary>
  /// Gets the hierarchy traversal depth, or null for many-to-many bridge rows.
  /// </summary>
  public int? TraversalDepth { get; }
}
