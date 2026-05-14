namespace DCoding.Data.DVault;

/// <summary>
/// Configures the optional Data Vault SaveChanges metadata interceptor.
/// </summary>
public sealed class DataVaultSaveChangesMetadataInterceptorOptions {
  private Func<DateTimeOffset>? _loadTimestampProvider;
  private Func<string>? _recordSourceProvider;

  /// <summary>
  /// Configures the load timestamp value used for missing Added-row technical metadata.
  /// </summary>
  /// <param name="loadTimestamp">The load timestamp value to apply.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultSaveChangesMetadataInterceptorOptions UseLoadTimestamp(DateTimeOffset loadTimestamp) {
    _loadTimestampProvider = () => loadTimestamp;
    return this;
  }

  /// <summary>
  /// Configures the load timestamp provider used for missing Added-row technical metadata.
  /// </summary>
  /// <param name="loadTimestampProvider">The load timestamp provider to invoke once per SaveChanges operation when needed.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultSaveChangesMetadataInterceptorOptions UseLoadTimestamp(Func<DateTimeOffset> loadTimestampProvider) {
    ArgumentNullException.ThrowIfNull(loadTimestampProvider);

    _loadTimestampProvider = loadTimestampProvider;
    return this;
  }

  /// <summary>
  /// Configures the record source value used for missing Added-row technical metadata.
  /// </summary>
  /// <param name="recordSource">The record source value to apply.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultSaveChangesMetadataInterceptorOptions UseRecordSource(string recordSource) {
    ArgumentException.ThrowIfNullOrWhiteSpace(recordSource);

    _recordSourceProvider = () => recordSource;
    return this;
  }

  /// <summary>
  /// Configures the record source provider used for missing Added-row technical metadata.
  /// </summary>
  /// <param name="recordSourceProvider">The record source provider to invoke once per SaveChanges operation when needed.</param>
  /// <returns>The current options instance.</returns>
  public DataVaultSaveChangesMetadataInterceptorOptions UseRecordSource(Func<string> recordSourceProvider) {
    ArgumentNullException.ThrowIfNull(recordSourceProvider);

    _recordSourceProvider = recordSourceProvider;
    return this;
  }

  internal DateTimeOffset ResolveLoadTimestamp() {
    if (_loadTimestampProvider is null) {
      throw new InvalidOperationException(
          "The Data Vault SaveChanges metadata interceptor requires a load timestamp provider before it can populate missing LoadTimestamp values.");
    }

    return _loadTimestampProvider().ToUniversalTime();
  }

  internal string ResolveRecordSource() {
    if (_recordSourceProvider is null) {
      throw new InvalidOperationException(
          "The Data Vault SaveChanges metadata interceptor requires a record-source provider before it can populate missing RecordSource values.");
    }

    var recordSource = _recordSourceProvider();
    if (string.IsNullOrWhiteSpace(recordSource)) {
      throw new InvalidOperationException(
          "The Data Vault SaveChanges metadata interceptor record-source provider must return a non-empty value.");
    }

    return recordSource;
  }
}
