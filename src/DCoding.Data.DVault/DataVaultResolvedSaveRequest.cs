namespace DCoding.Data.DVault;

/// <summary>
/// Carries one explicit save request with the effective metadata resolved at the request boundary.
/// </summary>
public sealed class DataVaultResolvedSaveRequest {
  /// <summary>
  /// Initializes a new resolved save request.
  /// </summary>
  /// <param name="request">The original explicit save request.</param>
  /// <param name="loadTimestamp">The effective UTC load timestamp shared by all operations in the request.</param>
  /// <param name="recordSource">The effective record source shared by all operations in the request.</param>
  public DataVaultResolvedSaveRequest(
      DataVaultSaveRequest request,
      DateTimeOffset loadTimestamp,
      string recordSource) {
    ArgumentNullException.ThrowIfNull(request);
    ArgumentException.ThrowIfNullOrWhiteSpace(recordSource);

    if (loadTimestamp.Offset != TimeSpan.Zero) {
      throw new ArgumentException("Data Vault resolved load timestamps must use a zero UTC offset.", nameof(loadTimestamp));
    }

    Request = request;
    LoadTimestamp = loadTimestamp;
    RecordSource = recordSource;
  }

  /// <summary>
  /// Gets the original explicit save request.
  /// </summary>
  public DataVaultSaveRequest Request { get; }

  /// <summary>
  /// Gets the effective UTC load timestamp shared by all operations in the request.
  /// </summary>
  public DateTimeOffset LoadTimestamp { get; }

  /// <summary>
  /// Gets the effective record source shared by all operations in the request.
  /// </summary>
  public string RecordSource { get; }
}
