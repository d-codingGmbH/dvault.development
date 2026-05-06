namespace DCoding.Data.DVault;

/// <summary>
/// Carries the request data used while resolving one effective load timestamp.
/// </summary>
public sealed class DataVaultLoadTimestampResolutionContext {
  /// <summary>
  /// Initializes a new load timestamp resolution context.
  /// </summary>
  /// <param name="request">The explicit save request being resolved.</param>
  public DataVaultLoadTimestampResolutionContext(DataVaultSaveRequest request) {
    ArgumentNullException.ThrowIfNull(request);

    Request = request;
  }

  /// <summary>
  /// Gets the explicit save request being resolved.
  /// </summary>
  public DataVaultSaveRequest Request { get; }
}
