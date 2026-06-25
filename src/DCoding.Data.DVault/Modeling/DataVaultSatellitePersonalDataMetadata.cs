namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one satellite payload field marked as personal data and its stable encrypted-payload alias.
/// </summary>
public sealed class DataVaultSatellitePersonalDataMetadata {
  /// <summary>
  /// Initializes a new personal-data payload field metadata declaration.
  /// </summary>
  /// <param name="fieldName">The exact logical satellite payload field name.</param>
  /// <param name="encryptedPayloadAlias">The stable provider-neutral encrypted-payload alias.</param>
  public DataVaultSatellitePersonalDataMetadata(
      string fieldName,
      string encryptedPayloadAlias) {
    FieldName = DataVaultMetadataValidation.RequireName(fieldName, nameof(fieldName));
    EncryptedPayloadAlias = DataVaultMetadataValidation.RequireName(encryptedPayloadAlias, nameof(encryptedPayloadAlias));
  }

  /// <summary>
  /// Gets the exact logical satellite payload field name.
  /// </summary>
  public string FieldName { get; }

  /// <summary>
  /// Gets the stable provider-neutral encrypted-payload alias.
  /// </summary>
  public string EncryptedPayloadAlias { get; }
}
