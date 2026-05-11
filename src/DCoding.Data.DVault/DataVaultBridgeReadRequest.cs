using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes a provider-neutral read request over one generated bridge table.
/// </summary>
public sealed class DataVaultBridgeReadRequest {
  /// <summary>
  /// Initializes a new many-to-many bridge read request.
  /// </summary>
  /// <param name="bridge">The bridge metadata declaration to read.</param>
  /// <param name="endpoint">The many-to-many endpoint whose hash keys are supplied by the request.</param>
  /// <param name="endpointHashKeys">The endpoint hash keys to match.</param>
  public DataVaultBridgeReadRequest(
      DataVaultBridgeMetadata bridge,
      DataVaultBridgeTraversalEndpoint endpoint,
      IEnumerable<string> endpointHashKeys)
      : this(bridge, endpoint, endpointHashKeys, maximumDepth: null) {
  }

  /// <summary>
  /// Initializes a new bridge read request with an optional bounded hierarchy depth.
  /// </summary>
  /// <param name="bridge">The bridge metadata declaration to read.</param>
  /// <param name="endpoint">The endpoint whose hash keys are supplied by the request.</param>
  /// <param name="endpointHashKeys">The endpoint hash keys to match.</param>
  /// <param name="maximumDepth">The inclusive maximum traversal depth for hierarchy bridges.</param>
  public DataVaultBridgeReadRequest(
      DataVaultBridgeMetadata bridge,
      DataVaultBridgeTraversalEndpoint endpoint,
      IEnumerable<string> endpointHashKeys,
      int? maximumDepth) {
    ArgumentNullException.ThrowIfNull(bridge);
    ArgumentNullException.ThrowIfNull(endpointHashKeys);

    Endpoint = RequireEndpoint(endpoint, nameof(endpoint));
    ValidateDepth(maximumDepth, nameof(maximumDepth));
    ValidateBridgeRequest(bridge, Endpoint, maximumDepth);

    Bridge = bridge;
    EndpointHashKeys = RequireEndpointHashKeys(endpointHashKeys);
    MaximumDepth = maximumDepth;
  }

  /// <summary>
  /// Gets the bridge metadata declaration to read.
  /// </summary>
  public DataVaultBridgeMetadata Bridge { get; }

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

  internal static DataVaultBridgeTraversalEndpoint RequireEndpoint(
      DataVaultBridgeTraversalEndpoint endpoint,
      string parameterName) {
    if (!Enum.IsDefined(typeof(DataVaultBridgeTraversalEndpoint), endpoint)) {
      throw new ArgumentOutOfRangeException(parameterName, endpoint, "Unsupported bridge traversal endpoint.");
    }

    return endpoint;
  }

  internal static IReadOnlyList<string> RequireEndpointHashKeys(IEnumerable<string> endpointHashKeys) {
    var values = endpointHashKeys
        .Distinct(StringComparer.Ordinal)
        .ToArray();

    foreach (var value in values) {
      ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(endpointHashKeys));
    }

    return values;
  }

  private static void ValidateDepth(int? maximumDepth, string parameterName) {
    if (maximumDepth.GetValueOrDefault() < 0 && maximumDepth.HasValue) {
      throw new ArgumentOutOfRangeException(
          parameterName,
          maximumDepth,
          "Bridge hierarchy maximum depth must be zero or greater.");
    }
  }

  private static void ValidateBridgeRequest(
      DataVaultBridgeMetadata bridge,
      DataVaultBridgeTraversalEndpoint endpoint,
      int? maximumDepth) {
    switch (bridge.Kind) {
      case DataVaultBridgeKind.ManyToMany:
        if (endpoint is not DataVaultBridgeTraversalEndpoint.From and not DataVaultBridgeTraversalEndpoint.To) {
          throw new ArgumentException(
              "Bridge traversal request for many-to-many bridge '" +
              bridge.Name +
              "' must use endpoint From or To, not '" +
              endpoint +
              "'.",
              nameof(endpoint));
        }

        if (maximumDepth.HasValue) {
          throw new ArgumentException(
              "Bridge traversal request for many-to-many bridge '" +
              bridge.Name +
              "' does not support hierarchy depth constraints.",
              nameof(maximumDepth));
        }

        return;

      case DataVaultBridgeKind.Hierarchy:
        if (endpoint is not DataVaultBridgeTraversalEndpoint.Ancestor and not DataVaultBridgeTraversalEndpoint.Descendant) {
          throw new ArgumentException(
              "Bridge traversal request for hierarchy bridge '" +
              bridge.Name +
              "' must use endpoint Ancestor or Descendant, not '" +
              endpoint +
              "'.",
              nameof(endpoint));
        }

        if (!maximumDepth.HasValue) {
          throw new ArgumentException(
              "Bridge traversal request for hierarchy bridge '" +
              bridge.Name +
              "' must specify a bounded maximum depth.",
              nameof(maximumDepth));
        }

        return;

      default:
        throw new NotSupportedException(
            "Bridge traversal request for bridge '" +
            bridge.Name +
            "' declares unsupported bridge kind '" +
            bridge.Kind +
            "'.");
    }
  }
}
