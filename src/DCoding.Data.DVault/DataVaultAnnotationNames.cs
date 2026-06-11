namespace DCoding.Data.DVault;

/// <summary>
/// Defines DVault-owned provider-neutral annotation names used on Entity Framework metadata.
/// </summary>
public static class DataVaultAnnotationNames {
  /// <summary>
  /// Identifies the annotation that stores the active provider-neutral DVault conventions on an Entity Framework model.
  /// </summary>
  public const string Conventions = "DCoding.Data.DVault:Conventions";

  /// <summary>
  /// Identifies the annotation that stores the deterministic DVault-produced name for an entity, property, key, or index.
  /// </summary>
  public const string ProducedName = "DCoding.Data.DVault:ProducedName";

  /// <summary>
  /// Identifies the annotation that stores the Data Vault entity kind for an Entity Framework entity type.
  /// </summary>
  public const string EntityKind = "DCoding.Data.DVault:EntityKind";

  /// <summary>
  /// Identifies the annotation that stores the source provider-neutral metadata name.
  /// </summary>
  public const string MetadataName = "DCoding.Data.DVault:MetadataName";

  /// <summary>
  /// Identifies the annotation that stores a satellite parent reference kind.
  /// </summary>
  public const string ParentReferenceKind = "DCoding.Data.DVault:ParentReferenceKind";

  /// <summary>
  /// Identifies the annotation that stores a satellite parent reference name.
  /// </summary>
  public const string ParentReferenceName = "DCoding.Data.DVault:ParentReferenceName";

  /// <summary>
  /// Identifies the annotation that stores the deterministic zero-based declaration ordinal for an EF metadata item.
  /// </summary>
  public const string Ordinal = "DCoding.Data.DVault:Ordinal";

  /// <summary>
  /// Identifies the annotation that stores the provider-neutral role of an Entity Framework property.
  /// </summary>
  public const string PropertyRole = "DCoding.Data.DVault:PropertyRole";

  /// <summary>
  /// Identifies the annotation that stores the reusable Data Vault technical metadata role for a property.
  /// </summary>
  public const string TechnicalColumnRole = "DCoding.Data.DVault:TechnicalColumnRole";

  /// <summary>
  /// Identifies the annotation that stores the provider capability profile selected for a model or projected property.
  /// </summary>
  public const string ProviderProfile = "DCoding.Data.DVault:ProviderProfile";

  /// <summary>
  /// Identifies the annotation that stores the provider-aware logical property kind.
  /// </summary>
  public const string ProviderLogicalPropertyKind = "DCoding.Data.DVault:ProviderLogicalPropertyKind";

  /// <summary>
  /// Identifies the annotation that stores the native provider storage type declared by the capability profile.
  /// </summary>
  public const string ProviderStorageType = "DCoding.Data.DVault:ProviderStorageType";

  /// <summary>
  /// Identifies the annotation that stores the provider value format declared by the capability profile.
  /// </summary>
  public const string ProviderValueFormat = "DCoding.Data.DVault:ProviderValueFormat";

  /// <summary>
  /// Identifies the annotation that stores the hash-key physical storage profile.
  /// </summary>
  public const string HashKeyStorageProfile = "DCoding.Data.DVault:HashKeyStorageProfile";

  /// <summary>
  /// Identifies the annotation that stores the stable-hash algorithm id for hash-key columns.
  /// </summary>
  public const string StableHashAlgorithmId = "DCoding.Data.DVault:StableHashAlgorithmId";

  /// <summary>
  /// Identifies the annotation that stores the stable-hash digest byte length for hash-key columns.
  /// </summary>
  public const string StableHashDigestByteLength = "DCoding.Data.DVault:StableHashDigestByteLength";

  /// <summary>
  /// Identifies the annotation that stores the stable-hash digest encoding for hash-key columns.
  /// </summary>
  public const string StableHashDigestEncoding = "DCoding.Data.DVault:StableHashDigestEncoding";

  /// <summary>
  /// Identifies the annotation that stores the EF conversion behavior for hash-key columns.
  /// </summary>
  public const string HashKeyConversionBehavior = "DCoding.Data.DVault:HashKeyConversionBehavior";

  /// <summary>
  /// Identifies the annotation that stores the authoritative Data Vault metadata source kind for an Entity Framework model.
  /// </summary>
  public const string MetadataSourceKind = "DCoding.Data.DVault:MetadataSourceKind";

  /// <summary>
  /// Identifies the annotation that stores the deterministic fingerprint of the authoritative Data Vault metadata source.
  /// </summary>
  public const string MetadataSourceFingerprint = "DCoding.Data.DVault:MetadataSourceFingerprint";
}
