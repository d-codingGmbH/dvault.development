namespace DCoding.Data.DVault;

/// <summary>
/// Describes a provider-neutral bridge read request whose bridge metadata is resolved from the DbContext registry.
/// </summary>
public sealed class DataVaultRegistryBridgeReadRequest {
  /// <summary>
  /// Initializes a new registry-backed bridge read request.
  /// </summary>
  /// <param name="bridgeName">The bridge metadata name to resolve from the authoritative registry.</param>
  /// <param name="endpoint">The endpoint whose hash keys are supplied by the request.</param>
  /// <param name="endpointHashKeys">The endpoint hash keys to match.</param>
  public DataVaultRegistryBridgeReadRequest(
      string bridgeName,
      DataVaultBridgeTraversalEndpoint endpoint,
      IEnumerable<string> endpointHashKeys)
      : this(bridgeName, endpoint, endpointHashKeys, maximumDepth: null) {
  }

  /// <summary>
  /// Initializes a new registry-backed bridge read request with an optional bounded hierarchy depth.
  /// </summary>
  /// <param name="bridgeName">The bridge metadata name to resolve from the authoritative registry.</param>
  /// <param name="endpoint">The endpoint whose hash keys are supplied by the request.</param>
  /// <param name="endpointHashKeys">The endpoint hash keys to match.</param>
  /// <param name="maximumDepth">The inclusive maximum traversal depth for hierarchy bridges.</param>
  public DataVaultRegistryBridgeReadRequest(
      string bridgeName,
      DataVaultBridgeTraversalEndpoint endpoint,
      IEnumerable<string> endpointHashKeys,
      int? maximumDepth) {
    ArgumentException.ThrowIfNullOrWhiteSpace(bridgeName);
    ArgumentNullException.ThrowIfNull(endpointHashKeys);

    BridgeName = bridgeName;
    Endpoint = DataVaultBridgeReadRequest.RequireEndpoint(endpoint, nameof(endpoint));
    EndpointHashKeys = DataVaultBridgeReadRequest.RequireEndpointHashKeys(endpointHashKeys);
    if (maximumDepth.GetValueOrDefault() < 0 && maximumDepth.HasValue) {
      throw new ArgumentOutOfRangeException(
          nameof(maximumDepth),
          maximumDepth,
          "Bridge hierarchy maximum depth must be zero or greater.");
    }

    MaximumDepth = maximumDepth;
  }

  /// <summary>
  /// Gets the bridge metadata name to resolve from the authoritative registry.
  /// </summary>
  public string BridgeName { get; }

  /// <summary>
  /// Gets the endpoint whose hash keys are supplied by the request.
  /// </summary>
  public DataVaultBridgeTraversalEndpoint Endpoint { get; }

  /// <summary>
  /// Gets the endpoint hash keys to match.
  /// </summary>
  public IReadOnlyList<string> EndpointHashKeys { get; }

  /// <summary>
  /// Gets the inclusive maximum traversal depth for hierarchy bridges.
  /// </summary>
  public int? MaximumDepth { get; }
}
