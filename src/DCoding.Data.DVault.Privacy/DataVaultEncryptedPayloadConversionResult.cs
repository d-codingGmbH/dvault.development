namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Describes the caller-owned decision for one encrypted-payload conversion request.
/// </summary>
public sealed class DataVaultEncryptedPayloadConversionResult {
  private DataVaultEncryptedPayloadConversionResult(bool isApproved, string? value, string? declineReason) {
    IsApproved = isApproved;
    Value = value;
    DeclineReason = declineReason;
  }

  /// <summary>
  /// Gets whether the caller approved and completed the conversion.
  /// </summary>
  public bool IsApproved { get; }

  /// <summary>
  /// Gets the converted value when the caller approved the request.
  /// </summary>
  public string? Value { get; }

  /// <summary>
  /// Gets the redaction-safe reason supplied when the caller declined the request.
  /// </summary>
  public string? DeclineReason { get; }

  /// <summary>
  /// Creates an approved conversion result with the caller-produced value.
  /// </summary>
  /// <param name="value">The converted payload value.</param>
  /// <returns>An approved conversion result.</returns>
  public static DataVaultEncryptedPayloadConversionResult Approved(string value) {
    ArgumentNullException.ThrowIfNull(value);

    return new DataVaultEncryptedPayloadConversionResult(isApproved: true, value, declineReason: null);
  }

  /// <summary>
  /// Creates a declined conversion result without exposing key material or payload values.
  /// </summary>
  /// <param name="declineReason">A redaction-safe decline reason.</param>
  /// <returns>A declined conversion result.</returns>
  public static DataVaultEncryptedPayloadConversionResult Declined(string declineReason) {
    if (string.IsNullOrWhiteSpace(declineReason)) {
      throw new ArgumentException("Decline reason must be non-empty.", nameof(declineReason));
    }

    return new DataVaultEncryptedPayloadConversionResult(isApproved: false, value: null, declineReason);
  }
}
