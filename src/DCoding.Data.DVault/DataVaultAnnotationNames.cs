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
  /// Identifies the annotation that stores the authoritative Data Vault metadata source kind for an Entity Framework model.
  /// </summary>
  public const string MetadataSourceKind = "DCoding.Data.DVault:MetadataSourceKind";

  /// <summary>
  /// Identifies the annotation that stores the deterministic fingerprint of the authoritative Data Vault metadata source.
  /// </summary>
  public const string MetadataSourceFingerprint = "DCoding.Data.DVault:MetadataSourceFingerprint";
}

internal static class DataVaultInternalAnnotationNames {
  public const string ProviderIncludedIndexPropertyNames =
      "DCoding.Data.DVault:ProviderIncludedIndexPropertyNames";
}

/// <summary>
/// Identifies the provider-neutral Data Vault role carried by an Entity Framework property.
/// </summary>
public enum DataVaultPropertyRole {
  /// <summary>
  /// Property carries a Data Vault technical metadata value.
  /// </summary>
  Technical,

  /// <summary>
  /// Property carries a hub business-key value.
  /// </summary>
  BusinessKey,

  /// <summary>
  /// Property carries a link participant hash-key reference.
  /// </summary>
  ParticipantReference,

  /// <summary>
  /// Property carries a satellite descriptive payload value.
  /// </summary>
  Payload,

  /// <summary>
  /// Property carries a PIT satellite snapshot load-timestamp reference.
  /// </summary>
  SnapshotReference,

  /// <summary>
  /// Property carries a bridge hierarchy traversal depth value.
  /// </summary>
  BridgeDepth,

  /// <summary>
  /// Property carries a multi-active satellite driving-key value.
  /// </summary>
  DrivingKey,
}
