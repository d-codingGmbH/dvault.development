namespace DCoding.Data.DVault;

/// <summary>
/// Describes a classified live database schema read outcome.
/// </summary>
public sealed class DataVaultLiveSchemaReadResult {
  /// <summary>
  /// Initializes a new live schema read result.
  /// </summary>
  /// <param name="status">The classified read status.</param>
  /// <param name="providerName">The Entity Framework provider name, when available.</param>
  /// <param name="snapshot">The successful live schema snapshot, or <see langword="null" /> for non-success statuses.</param>
  /// <param name="message">A deterministic human-readable result message.</param>
  public DataVaultLiveSchemaReadResult(
      DataVaultLiveSchemaReadStatus status,
      string? providerName,
      DataVaultLiveSchemaSnapshot? snapshot,
      string message) {
    if (!Enum.IsDefined(status)) {
      throw new ArgumentOutOfRangeException(nameof(status));
    }

    if (status == DataVaultLiveSchemaReadStatus.Succeeded && snapshot is null) {
      throw new ArgumentException("A successful Data Vault live schema read result must include a snapshot.", nameof(snapshot));
    }

    if (status != DataVaultLiveSchemaReadStatus.Succeeded && snapshot is not null) {
      throw new ArgumentException("A non-success Data Vault live schema read result must not include a snapshot.", nameof(snapshot));
    }

    ArgumentException.ThrowIfNullOrWhiteSpace(message);

    Status = status;
    ProviderName = providerName;
    Snapshot = snapshot;
    Message = message;
  }

  /// <summary>
  /// Gets the classified read status.
  /// </summary>
  public DataVaultLiveSchemaReadStatus Status { get; }

  /// <summary>
  /// Gets the Entity Framework provider name, when available.
  /// </summary>
  public string? ProviderName { get; }

  /// <summary>
  /// Gets the successful live schema snapshot, or <see langword="null" /> when no snapshot was available.
  /// </summary>
  public DataVaultLiveSchemaSnapshot? Snapshot { get; }

  /// <summary>
  /// Gets a deterministic human-readable result message.
  /// </summary>
  public string Message { get; }

  /// <summary>
  /// Gets a value indicating whether a snapshot was read successfully.
  /// </summary>
  public bool IsSucceeded => Status == DataVaultLiveSchemaReadStatus.Succeeded;

  /// <summary>
  /// Creates a successful live schema read result.
  /// </summary>
  /// <param name="providerName">The Entity Framework provider name.</param>
  /// <param name="snapshot">The live schema snapshot.</param>
  /// <returns>A successful classified read result.</returns>
  public static DataVaultLiveSchemaReadResult Success(string? providerName, DataVaultLiveSchemaSnapshot snapshot) {
    ArgumentNullException.ThrowIfNull(snapshot);

    return new DataVaultLiveSchemaReadResult(
        DataVaultLiveSchemaReadStatus.Succeeded,
        providerName,
        snapshot,
        "DVault live schema snapshot read successfully.");
  }

  /// <summary>
  /// Creates an unsupported-provider live schema read result.
  /// </summary>
  /// <param name="providerName">The Entity Framework provider name, when available.</param>
  /// <returns>An unsupported-provider classified read result.</returns>
  public static DataVaultLiveSchemaReadResult UnsupportedProvider(string? providerName) {
    return new DataVaultLiveSchemaReadResult(
        DataVaultLiveSchemaReadStatus.UnsupportedProvider,
        providerName,
        snapshot: null,
        "DVault live schema reading is not supported for provider '" + FormatProviderName(providerName) + "'.");
  }

  /// <summary>
  /// Creates an unavailable live schema read result.
  /// </summary>
  /// <param name="providerName">The Entity Framework provider name, when available.</param>
  /// <param name="message">The provider-specific availability failure message.</param>
  /// <returns>An unavailable classified read result.</returns>
  public static DataVaultLiveSchemaReadResult Unavailable(string? providerName, string message) {
    ArgumentException.ThrowIfNullOrWhiteSpace(message);

    return new DataVaultLiveSchemaReadResult(
        DataVaultLiveSchemaReadStatus.Unavailable,
        providerName,
        snapshot: null,
        "DVault live schema was unavailable for provider '" + FormatProviderName(providerName) + "': " + message);
  }

  private static string FormatProviderName(string? providerName) {
    return string.IsNullOrWhiteSpace(providerName) ? "<unknown>" : providerName;
  }
}
