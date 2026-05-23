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
    var endpointHashKeyArray = endpointHashKeys as DataVaultBridgeEndpointReadValue[] ?? endpointHashKeys.ToArray();
    EndpointHashKeys = new ReadOnlyCollection<DataVaultBridgeEndpointReadValue>(endpointHashKeyArray);
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
