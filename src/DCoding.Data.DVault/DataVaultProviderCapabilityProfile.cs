using System.Collections.ObjectModel;
using System.Globalization;

namespace DCoding.Data.DVault;

/// <summary>
/// Describes one provider capability profile consumed by the Data Vault EF metadata translator.
/// </summary>
public sealed class DataVaultProviderCapabilityProfile {
  private readonly IReadOnlyDictionary<DataVaultLogicalPropertyKind, DataVaultProviderTypeMapping> _typeMappingsByKind;

  /// <summary>
  /// Initializes a new provider capability profile.
  /// </summary>
  /// <param name="profileName">The deterministic profile name used in diagnostics and metadata.</param>
  /// <param name="sqlFunctionSupport">The SQL-function support declaration.</param>
  /// <param name="concurrencySupport">The concurrency support declaration.</param>
  /// <param name="typeMappings">The provider type mappings declared by the profile.</param>
  /// <param name="maximumIdentifierLength">The provider-specific maximum physical identifier length, when enforced.</param>
  /// <param name="allowsIndexesCoveredByPrimaryKey">
  /// A value indicating whether the provider accepts secondary indexes whose column list matches the primary key.
  /// </param>
  /// <param name="unsupportedIncludedIndexColumnMode">
  /// How include columns are projected when the provider has no native included-index-column support.
  /// </param>
  public DataVaultProviderCapabilityProfile(
      string profileName,
      DataVaultProviderSqlFunctionSupport sqlFunctionSupport,
      DataVaultProviderConcurrencySupport concurrencySupport,
      IEnumerable<DataVaultProviderTypeMapping> typeMappings,
      int? maximumIdentifierLength = null,
      bool allowsIndexesCoveredByPrimaryKey = true,
      DataVaultUnsupportedIncludedIndexColumnMode unsupportedIncludedIndexColumnMode =
          DataVaultUnsupportedIncludedIndexColumnMode.AppendToKey) {
    ArgumentException.ThrowIfNullOrWhiteSpace(profileName);
    ArgumentNullException.ThrowIfNull(typeMappings);
    if (maximumIdentifierLength <= 0) {
      throw new ArgumentOutOfRangeException(nameof(maximumIdentifierLength));
    }

    if (!Enum.IsDefined(unsupportedIncludedIndexColumnMode)) {
      throw new ArgumentOutOfRangeException(nameof(unsupportedIncludedIndexColumnMode));
    }

    var mappings = typeMappings.ToArray();
    var mappingsByKind = new Dictionary<DataVaultLogicalPropertyKind, DataVaultProviderTypeMapping>();
    foreach (var mapping in mappings) {
      ArgumentNullException.ThrowIfNull(mapping);

      if (!mappingsByKind.TryAdd(mapping.LogicalPropertyKind, mapping)) {
        throw new ArgumentException("Provider type mappings must not contain duplicate logical property kinds.", nameof(typeMappings));
      }
    }

    ProfileName = profileName;
    SqlFunctionSupport = sqlFunctionSupport;
    ConcurrencySupport = concurrencySupport;
    TypeMappings = new ReadOnlyCollection<DataVaultProviderTypeMapping>(mappings);
    MaximumIdentifierLength = maximumIdentifierLength;
    AllowsIndexesCoveredByPrimaryKey = allowsIndexesCoveredByPrimaryKey;
    UnsupportedIncludedIndexColumnMode = unsupportedIncludedIndexColumnMode;
    _typeMappingsByKind = mappingsByKind;
  }

  /// <summary>
  /// Gets the deterministic profile name used in diagnostics and metadata.
  /// </summary>
  public string ProfileName { get; }

  /// <summary>
  /// Gets the SQL-function support declaration.
  /// </summary>
  public DataVaultProviderSqlFunctionSupport SqlFunctionSupport { get; }

  /// <summary>
  /// Gets the concurrency support declaration.
  /// </summary>
  public DataVaultProviderConcurrencySupport ConcurrencySupport { get; }

  /// <summary>
  /// Gets the provider type mappings declared by the profile.
  /// </summary>
  public IReadOnlyList<DataVaultProviderTypeMapping> TypeMappings { get; }

  /// <summary>
  /// Gets the provider-specific maximum physical identifier length, if the provider enforces one.
  /// </summary>
  public int? MaximumIdentifierLength { get; }

  /// <summary>
  /// Gets a value indicating whether the provider accepts secondary indexes whose column list matches the primary key.
  /// </summary>
  public bool AllowsIndexesCoveredByPrimaryKey { get; }

  /// <summary>
  /// Gets how include columns are projected when the provider has no native included-index-column support.
  /// </summary>
  public DataVaultUnsupportedIncludedIndexColumnMode UnsupportedIncludedIndexColumnMode { get; }

  /// <summary>
  /// Returns the required type mapping for one logical property kind.
  /// </summary>
  /// <param name="logicalPropertyKind">The logical property kind required by the caller.</param>
  /// <returns>The matching provider type mapping.</returns>
  /// <exception cref="NotSupportedException">The profile does not declare the requested capability.</exception>
  public DataVaultProviderTypeMapping GetRequiredTypeMapping(DataVaultLogicalPropertyKind logicalPropertyKind) {
    if (_typeMappingsByKind.TryGetValue(logicalPropertyKind, out var mapping)) {
      return mapping;
    }

    throw UnsupportedCapability("type mapping for " + logicalPropertyKind);
  }

  /// <summary>
  /// Creates a copy of this profile with load-timestamp mappings adapted to the requested storage format.
  /// </summary>
  /// <param name="storage">The load-timestamp storage format to project.</param>
  /// <returns>The current profile for <see cref="DataVaultLoadTimestampStorage.ProviderDefault" />; otherwise a transformed profile.</returns>
  public DataVaultProviderCapabilityProfile WithLoadTimestampStorage(DataVaultLoadTimestampStorage storage) {
    if (storage == DataVaultLoadTimestampStorage.ProviderDefault) {
      return this;
    }

    return new DataVaultProviderCapabilityProfile(
        ProfileName + GetLoadTimestampStorageProfileSuffix(storage),
        SqlFunctionSupport,
        ConcurrencySupport,
        TypeMappings.Select(mapping => IsLoadTimestampMapping(mapping.LogicalPropertyKind)
            ? CreateLoadTimestampMapping(mapping.LogicalPropertyKind, storage)
            : mapping),
        MaximumIdentifierLength,
        AllowsIndexesCoveredByPrimaryKey,
        UnsupportedIncludedIndexColumnMode);
  }

  /// <summary>
  /// Creates a copy of this profile with hash-key storage mappings sized for the selected stable-hash algorithm.
  /// </summary>
  /// <param name="algorithmId">The stable-hash algorithm id used by the model.</param>
  /// <param name="digestByteLength">The digest byte length produced by the stable-hash algorithm.</param>
  /// <returns>A transformed profile whose hash-key mappings carry the selected algorithm facts.</returns>
  public DataVaultProviderCapabilityProfile WithStableHashAlgorithm(
      string algorithmId,
      int digestByteLength) {
    return WithHashKeyStorageProfile(
        DataVaultHashKeyStorageProfile.Binary,
        algorithmId,
        digestByteLength);
  }

  /// <summary>
  /// Creates a copy of this profile with hash-key storage mappings projected to the requested physical profile.
  /// </summary>
  /// <param name="storageProfile">The physical hash-key storage profile to project.</param>
  /// <param name="algorithmId">The stable-hash algorithm id used by the model.</param>
  /// <param name="digestByteLength">The digest byte length produced by the stable-hash algorithm.</param>
  /// <returns>A transformed profile whose hash-key mappings carry the selected storage and algorithm facts.</returns>
  public DataVaultProviderCapabilityProfile WithHashKeyStorageProfile(
      DataVaultHashKeyStorageProfile storageProfile,
      string algorithmId,
      int digestByteLength) {
    ArgumentException.ThrowIfNullOrWhiteSpace(algorithmId);
    if (digestByteLength <= 0) {
      throw new ArgumentOutOfRangeException(nameof(digestByteLength));
    }

    if (!Enum.IsDefined(storageProfile)) {
      throw new ArgumentOutOfRangeException(nameof(storageProfile));
    }

    return new DataVaultProviderCapabilityProfile(
        ProfileName,
        SqlFunctionSupport,
        ConcurrencySupport,
        TypeMappings.Select(mapping => IsHashKeyMapping(mapping.LogicalPropertyKind)
            ? CreateHashKeyMapping(mapping.LogicalPropertyKind, storageProfile, algorithmId, digestByteLength)
            : mapping),
        MaximumIdentifierLength,
        AllowsIndexesCoveredByPrimaryKey,
        UnsupportedIncludedIndexColumnMode);
  }

  /// <summary>
  /// Fails deterministically when a caller requires a SQL function that the profile does not support.
  /// </summary>
  /// <param name="functionName">The SQL function capability required by the caller.</param>
  /// <exception cref="NotSupportedException">The profile does not declare the requested SQL function.</exception>
  public void RequireSqlFunction(string functionName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(functionName);

    throw UnsupportedCapability("SQL function " + functionName);
  }

  /// <summary>
  /// Fails deterministically when a caller requires a concurrency signal that the profile does not support.
  /// </summary>
  /// <param name="signalName">The concurrency signal capability required by the caller.</param>
  /// <exception cref="NotSupportedException">The profile does not declare the requested concurrency signal.</exception>
  public void RequireConcurrencySignal(string signalName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(signalName);

    throw UnsupportedCapability("concurrency signal " + signalName);
  }

  private NotSupportedException UnsupportedCapability(string capabilityName) {
    return new NotSupportedException(
        "Provider capability profile '" + ProfileName + "' does not declare required capability '" + capabilityName + "'.");
  }

  private static bool IsLoadTimestampMapping(DataVaultLogicalPropertyKind logicalPropertyKind) {
    return logicalPropertyKind is
        DataVaultLogicalPropertyKind.LoadTimestamp or
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference;
  }

  private static bool IsHashKeyMapping(DataVaultLogicalPropertyKind logicalPropertyKind) {
    return logicalPropertyKind is
        DataVaultLogicalPropertyKind.HashKey or
        DataVaultLogicalPropertyKind.ParticipantReference;
  }

  private DataVaultProviderTypeMapping CreateHashKeyMapping(
      DataVaultLogicalPropertyKind logicalPropertyKind,
      DataVaultHashKeyStorageProfile storageProfile,
      string algorithmId,
      int digestByteLength) {
    return storageProfile switch {
      DataVaultHashKeyStorageProfile.HexString => new DataVaultProviderTypeMapping(
          logicalPropertyKind,
          typeof(string),
          GetHashKeyHexStringStoreType(digestByteLength),
          DataVaultProviderValueFormat.LowercaseHexText,
          storageProfile,
          algorithmId,
          digestByteLength,
          "lowercase-hex-no-prefix",
          "none-string-model"),
      DataVaultHashKeyStorageProfile.Binary => new DataVaultProviderTypeMapping(
          logicalPropertyKind,
          typeof(string),
          GetHashKeyBinaryStoreType(digestByteLength),
          DataVaultProviderValueFormat.LowercaseHexBinary,
          storageProfile,
          algorithmId,
          digestByteLength,
          "lowercase-hex-no-prefix",
          "lowercase-hex-string-to-bytes"),
      _ => throw new ArgumentOutOfRangeException(nameof(storageProfile), storageProfile, "Unsupported hash-key storage profile."),
    };
  }

  private DataVaultProviderTypeMapping CreateLoadTimestampMapping(
      DataVaultLogicalPropertyKind logicalPropertyKind,
      DataVaultLoadTimestampStorage storage) {
    return storage switch {
      DataVaultLoadTimestampStorage.Iso8601UtcText => new DataVaultProviderTypeMapping(
          logicalPropertyKind,
          typeof(string),
          GetIso8601UtcTextStoreType(),
          DataVaultProviderValueFormat.Iso8601UtcText),
      DataVaultLoadTimestampStorage.UtcTicks => new DataVaultProviderTypeMapping(
          logicalPropertyKind,
          typeof(long),
          GetUtcTicksStoreType(),
          DataVaultProviderValueFormat.UtcTicks),
      _ => throw new ArgumentOutOfRangeException(nameof(storage), storage, "Unsupported Data Vault load timestamp storage."),
    };
  }

  private string GetIso8601UtcTextStoreType() {
    if (ProfileName.StartsWith("db2-", StringComparison.Ordinal)) {
      return "VARCHAR(33)";
    }

    if (ProfileName.StartsWith("oracle-", StringComparison.Ordinal)) {
      return "VARCHAR2(33 CHAR)";
    }

    if (ProfileName.StartsWith("mysql-", StringComparison.Ordinal)) {
      return "varchar(33)";
    }

    if (ProfileName.StartsWith("sqlserver-", StringComparison.Ordinal)) {
      return "nvarchar(33)";
    }

    if (ProfileName.StartsWith("postgres-", StringComparison.Ordinal)) {
      return "varchar(33)";
    }

    return "TEXT";
  }

  private string GetUtcTicksStoreType() {
    if (ProfileName.StartsWith("oracle-", StringComparison.Ordinal)) {
      return "NUMBER(19)";
    }

    if (ProfileName.StartsWith("sqlite-", StringComparison.Ordinal)) {
      return "INTEGER";
    }

    if (ProfileName.StartsWith("db2-", StringComparison.Ordinal)) {
      return "BIGINT";
    }

    return "bigint";
  }

  private string GetHashKeyHexStringStoreType(int digestByteLength) {
    var hexLength = digestByteLength * 2;
    if (ProfileName.StartsWith("oracle-", StringComparison.Ordinal)) {
      return "VARCHAR2(" + hexLength.ToString(CultureInfo.InvariantCulture) + " CHAR)";
    }

    if (ProfileName.StartsWith("db2-", StringComparison.Ordinal)) {
      return "VARCHAR(" + hexLength.ToString(CultureInfo.InvariantCulture) + ")";
    }

    if (ProfileName.StartsWith("sqlserver-", StringComparison.Ordinal)) {
      return "nvarchar(" + hexLength.ToString(CultureInfo.InvariantCulture) + ")";
    }

    if (ProfileName.StartsWith("postgres-", StringComparison.Ordinal) ||
        ProfileName.StartsWith("mysql-", StringComparison.Ordinal)) {
      return "varchar(" + hexLength.ToString(CultureInfo.InvariantCulture) + ")";
    }

    return "TEXT";
  }

  private string GetHashKeyBinaryStoreType(int digestByteLength) {
    if (ProfileName.StartsWith("oracle-", StringComparison.Ordinal)) {
      return "RAW(" + digestByteLength.ToString(CultureInfo.InvariantCulture) + ")";
    }

    if (ProfileName.StartsWith("db2-", StringComparison.Ordinal)) {
      return "VARBINARY(" + digestByteLength.ToString(CultureInfo.InvariantCulture) + ")";
    }

    if (ProfileName.StartsWith("sqlserver-", StringComparison.Ordinal)) {
      return "varbinary(" + digestByteLength.ToString(CultureInfo.InvariantCulture) + ")";
    }

    if (ProfileName.StartsWith("postgres-", StringComparison.Ordinal)) {
      return "bytea";
    }

    if (ProfileName.StartsWith("mysql-", StringComparison.Ordinal)) {
      return "varbinary(" + digestByteLength.ToString(CultureInfo.InvariantCulture) + ")";
    }

    return "BLOB";
  }

  private static string GetLoadTimestampStorageProfileSuffix(DataVaultLoadTimestampStorage storage) {
    return storage switch {
      DataVaultLoadTimestampStorage.Iso8601UtcText => "-loadts-iso8601",
      DataVaultLoadTimestampStorage.UtcTicks => "-loadts-utc-ticks",
      _ => throw new ArgumentOutOfRangeException(nameof(storage), storage, "Unsupported Data Vault load timestamp storage."),
    };
  }
}
