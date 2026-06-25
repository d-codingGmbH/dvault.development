using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Provides an explicit EF Core value converter for one caller-registered encrypted-payload alias.
/// </summary>
/// <remarks>
/// The converter routes payload values to the caller-owned key provider and fails closed when the alias,
/// provider, or conversion approval is unavailable. It does not create, store, rotate, or select key material.
/// </remarks>
public sealed class DataVaultEncryptedPayloadValueConverter : ValueConverter<string, string> {
  /// <summary>
  /// Creates an encrypted-payload value converter for one registered alias.
  /// </summary>
  /// <param name="configuration">The opt-in privacy configuration registered by the application.</param>
  /// <param name="encryptedPayloadAlias">The stable encrypted-payload alias to use for conversion.</param>
  public DataVaultEncryptedPayloadValueConverter(
      IDataVaultPrivacyConfiguration configuration,
      string encryptedPayloadAlias)
      : base(
          value => ConvertPayload(
              configuration,
              encryptedPayloadAlias,
              DataVaultEncryptedPayloadConversionDirection.Encrypt,
              value),
          value => ConvertPayload(
              configuration,
              encryptedPayloadAlias,
              DataVaultEncryptedPayloadConversionDirection.Decrypt,
              value)) {
    EnsureConversionCanRun(configuration, encryptedPayloadAlias);
    EncryptedPayloadAlias = encryptedPayloadAlias;
  }

  /// <summary>
  /// Gets the stable encrypted-payload alias used by this converter.
  /// </summary>
  public string EncryptedPayloadAlias { get; }

  private static string ConvertPayload(
      IDataVaultPrivacyConfiguration configuration,
      string encryptedPayloadAlias,
      DataVaultEncryptedPayloadConversionDirection direction,
      string value) {
    var keyProvider = GetEncryptedPayloadKeyProvider(configuration, encryptedPayloadAlias);
    ArgumentNullException.ThrowIfNull(value);

    var result = keyProvider.ConvertEncryptedPayload(
        new DataVaultEncryptedPayloadConversionRequest(encryptedPayloadAlias, direction, value));

    if (result is null) {
      throw new InvalidOperationException(
          "Encrypted payload conversion for alias '" +
          encryptedPayloadAlias +
          "' returned no result.");
    }

    if (!result.IsApproved || result.Value is null) {
      throw CreateDeclinedConversionException(encryptedPayloadAlias, direction, result.DeclineReason);
    }

    return result.Value;
  }

  private static IDataVaultEncryptedPayloadKeyProvider GetEncryptedPayloadKeyProvider(
      IDataVaultPrivacyConfiguration configuration,
      string encryptedPayloadAlias) {
    ArgumentNullException.ThrowIfNull(configuration);

    if (string.IsNullOrWhiteSpace(encryptedPayloadAlias)) {
      throw new ArgumentException("Encrypted payload alias must be non-empty.", nameof(encryptedPayloadAlias));
    }

    if (!configuration.EncryptedPayloadAliases.Contains(encryptedPayloadAlias, StringComparer.Ordinal)) {
      throw new InvalidOperationException(
          "Encrypted payload alias '" +
          encryptedPayloadAlias +
          "' is not registered for explicit DVault privacy conversion.");
    }

    if (configuration.KeyProvider is null) {
      throw new InvalidOperationException(
          "Encrypted payload alias '" +
          encryptedPayloadAlias +
          "' requires a caller-owned DVault privacy key provider.");
    }

    if (configuration.KeyProvider is not IDataVaultEncryptedPayloadKeyProvider encryptedPayloadKeyProvider) {
      throw new InvalidOperationException(
          "Encrypted payload alias '" +
          encryptedPayloadAlias +
          "' requires a caller-owned DVault encrypted payload key provider.");
    }

    return encryptedPayloadKeyProvider;
  }

  private static void EnsureConversionCanRun(
      IDataVaultPrivacyConfiguration configuration,
      string encryptedPayloadAlias) {
    _ = GetEncryptedPayloadKeyProvider(configuration, encryptedPayloadAlias);
  }

  private static InvalidOperationException CreateDeclinedConversionException(
      string encryptedPayloadAlias,
      DataVaultEncryptedPayloadConversionDirection direction,
      string? declineReason) {
    var message =
        "Encrypted payload conversion for alias '" +
        encryptedPayloadAlias +
        "' was declined during " +
        direction +
        ".";

    if (!string.IsNullOrWhiteSpace(declineReason)) {
      message += " Reason: " + declineReason;
    }

    return new InvalidOperationException(message);
  }
}
