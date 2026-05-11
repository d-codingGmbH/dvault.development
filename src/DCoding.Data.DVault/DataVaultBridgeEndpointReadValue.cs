namespace DCoding.Data.DVault;

/// <summary>
/// Describes one endpoint hash-key value returned by a provider-neutral bridge read.
/// </summary>
public sealed class DataVaultBridgeEndpointReadValue {
  internal DataVaultBridgeEndpointReadValue(
      DataVaultBridgeTraversalEndpoint endpoint,
      string endpointName,
      string columnName,
      string hashKey) {
    Endpoint = endpoint;
    EndpointName = endpointName;
    ColumnName = columnName;
    HashKey = hashKey;
  }

  /// <summary>
  /// Gets the endpoint role represented by this value.
  /// </summary>
  public DataVaultBridgeTraversalEndpoint Endpoint { get; }

  /// <summary>
  /// Gets the source endpoint metadata name bound to this value.
  /// </summary>
  public string EndpointName { get; }

  /// <summary>
  /// Gets the generated bridge column name that supplied this value.
  /// </summary>
  public string ColumnName { get; }

  /// <summary>
  /// Gets the endpoint hash key.
  /// </summary>
  public string HashKey { get; }
}
