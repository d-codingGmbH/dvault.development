namespace DCoding.Data.DVault;

/// <summary>
/// Carries the request data used while resolving one effective record source.
/// </summary>
public sealed class DataVaultRecordSourceResolutionContext {
  /// <summary>
  /// Initializes a new record-source resolution context.
  /// </summary>
  /// <param name="request">The explicit save request being resolved.</param>
  /// <param name="loadTimestamp">The already resolved UTC load timestamp for the request.</param>
  public DataVaultRecordSourceResolutionContext(
      DataVaultSaveRequest request,
      DateTimeOffset loadTimestamp) {
    ArgumentNullException.ThrowIfNull(request);

    if (loadTimestamp.Offset != TimeSpan.Zero) {
      throw new ArgumentException("Data Vault resolved load timestamps must use a zero UTC offset.", nameof(loadTimestamp));
    }

    Request = request;
    LoadTimestamp = loadTimestamp;
  }

  /// <summary>
  /// Gets the explicit save request being resolved.
  /// </summary>
  public DataVaultSaveRequest Request { get; }

  /// <summary>
  /// Gets the already resolved UTC load timestamp for the request.
  /// </summary>
  public DateTimeOffset LoadTimestamp { get; }
}
