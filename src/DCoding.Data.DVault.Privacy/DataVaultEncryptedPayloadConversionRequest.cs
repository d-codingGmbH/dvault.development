namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Describes one provider-neutral encrypted-payload conversion request for a caller-owned key provider.
/// </summary>
public sealed class DataVaultEncryptedPayloadConversionRequest {
  /// <summary>
  /// Creates a provider-neutral encrypted-payload conversion request.
  /// </summary>
  /// <param name="encryptedPayloadAlias">The stable encrypted-payload alias registered by the caller.</param>
  /// <param name="direction">The requested conversion direction.</param>
  /// <param name="value">The payload value to convert.</param>
  public DataVaultEncryptedPayloadConversionRequest(
      string encryptedPayloadAlias,
      DataVaultEncryptedPayloadConversionDirection direction,
      string value) {
    if (string.IsNullOrWhiteSpace(encryptedPayloadAlias)) {
      throw new ArgumentException("Encrypted payload alias must be non-empty.", nameof(encryptedPayloadAlias));
    }

    ArgumentNullException.ThrowIfNull(value);

    EncryptedPayloadAlias = encryptedPayloadAlias;
    Direction = direction;
    Value = value;
  }

  /// <summary>
  /// Gets the stable encrypted-payload alias registered by the caller.
  /// </summary>
  public string EncryptedPayloadAlias { get; }

  /// <summary>
  /// Gets the requested conversion direction.
  /// </summary>
  public DataVaultEncryptedPayloadConversionDirection Direction { get; }

  /// <summary>
  /// Gets the payload value to convert.
  /// </summary>
  public string Value { get; }
}
