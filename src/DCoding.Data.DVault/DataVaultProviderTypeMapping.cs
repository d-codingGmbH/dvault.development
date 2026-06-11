using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one provider-specific native storage mapping for a Data Vault logical property kind.
/// </summary>
public sealed record DataVaultProviderTypeMapping {
  /// <summary>
  /// Initializes a new provider type mapping.
  /// </summary>
  /// <param name="logicalPropertyKind">The Data Vault logical property kind covered by the mapping.</param>
  /// <param name="modelClrType">The CLR type projected into the Entity Framework model.</param>
  /// <param name="nativeStoreType">The native provider storage type declared by the profile.</param>
  /// <param name="valueFormat">The value format declared for persisted values.</param>
  /// <param name="hashKeyStorageProfile">The hash-key storage profile for hash-key values.</param>
  /// <param name="stableHashAlgorithmId">The stable-hash algorithm id that sizes hash-key values.</param>
  /// <param name="digestByteLength">The digest byte length that sizes hash-key values.</param>
  /// <param name="digestEncoding">The logical digest encoding exposed at the model boundary.</param>
  /// <param name="conversionBehavior">The EF conversion behavior used by the mapping.</param>
  public DataVaultProviderTypeMapping(
      DataVaultLogicalPropertyKind logicalPropertyKind,
      Type modelClrType,
      string nativeStoreType,
      DataVaultProviderValueFormat valueFormat,
      DataVaultHashKeyStorageProfile? hashKeyStorageProfile = null,
      string? stableHashAlgorithmId = null,
      int? digestByteLength = null,
      string? digestEncoding = null,
      string? conversionBehavior = null) {
    ArgumentNullException.ThrowIfNull(modelClrType);
    ArgumentException.ThrowIfNullOrWhiteSpace(nativeStoreType);
    if (digestByteLength is <= 0) {
      throw new ArgumentOutOfRangeException(nameof(digestByteLength));
    }

    if (hashKeyStorageProfile is not null && !Enum.IsDefined(hashKeyStorageProfile.Value)) {
      throw new ArgumentOutOfRangeException(nameof(hashKeyStorageProfile));
    }

    LogicalPropertyKind = logicalPropertyKind;
    ModelClrType = modelClrType;
    NativeStoreType = nativeStoreType;
    ValueFormat = valueFormat;
    HashKeyStorageProfile = hashKeyStorageProfile;
    StableHashAlgorithmId = stableHashAlgorithmId;
    DigestByteLength = digestByteLength;
    DigestEncoding = digestEncoding;
    ConversionBehavior = conversionBehavior;
  }

  /// <summary>
  /// Gets the Data Vault logical property kind covered by the mapping.
  /// </summary>
  public DataVaultLogicalPropertyKind LogicalPropertyKind { get; private init; }

  /// <summary>
  /// Gets the CLR type projected into the Entity Framework model.
  /// </summary>
  public Type ModelClrType { get; private init; }

  /// <summary>
  /// Gets the native provider storage type declared by the profile.
  /// </summary>
  public string NativeStoreType { get; private init; }

  /// <summary>
  /// Gets the value format declared for persisted values.
  /// </summary>
  public DataVaultProviderValueFormat ValueFormat { get; private init; }

  /// <summary>
  /// Gets the hash-key storage profile for hash-key values, when this mapping covers a hash key or hash-key reference.
  /// </summary>
  public DataVaultHashKeyStorageProfile? HashKeyStorageProfile { get; private init; }

  /// <summary>
  /// Gets the stable-hash algorithm id that sizes hash-key values, when this mapping covers a hash key or hash-key reference.
  /// </summary>
  public string? StableHashAlgorithmId { get; private init; }

  /// <summary>
  /// Gets the digest byte length that sizes hash-key values, when this mapping covers a hash key or hash-key reference.
  /// </summary>
  public int? DigestByteLength { get; private init; }

  /// <summary>
  /// Gets the logical digest encoding exposed at the model boundary, when this mapping covers a hash key or hash-key reference.
  /// </summary>
  public string? DigestEncoding { get; private init; }

  /// <summary>
  /// Gets the EF conversion behavior used by the mapping.
  /// </summary>
  public string? ConversionBehavior { get; private init; }
}
