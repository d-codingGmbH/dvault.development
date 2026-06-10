using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Identifies the provider-aware logical property kinds used by the v1 Data Vault EF translator.
/// </summary>
public enum DataVaultLogicalPropertyKind {
  /// <summary>
  /// Data Vault hash key technical value.
  /// </summary>
  HashKey,

  /// <summary>
  /// Data Vault hash diff technical value.
  /// </summary>
  HashDiff,

  /// <summary>
  /// Data Vault load timestamp technical value.
  /// </summary>
  LoadTimestamp,

  /// <summary>
  /// Data Vault record source technical value.
  /// </summary>
  RecordSource,

  /// <summary>
  /// Link participant hash-key reference value.
  /// </summary>
  ParticipantReference,

  /// <summary>
  /// Hub business-key value.
  /// </summary>
  BusinessKey,

  /// <summary>
  /// Satellite text payload value.
  /// </summary>
  PayloadText,

  /// <summary>
  /// PIT satellite snapshot load-timestamp reference value.
  /// </summary>
  SatelliteSnapshotReference,

  /// <summary>
  /// Integer hierarchy depth value produced for bridge traversal metadata.
  /// </summary>
  BridgeDepth,

  /// <summary>
  /// Multi-active satellite driving-key value.
  /// </summary>
  DrivingKey,
}

/// <summary>
/// Identifies the SQL-function capability set exposed by a provider profile.
/// </summary>
public enum DataVaultProviderSqlFunctionSupport {
  /// <summary>
  /// The v1 profile declares no required SQL functions and treats SQL-function requests as unsupported.
  /// </summary>
  NoneInV1Unsupported,
}

/// <summary>
/// Identifies the concurrency capability set exposed by a provider profile.
/// </summary>
public enum DataVaultProviderConcurrencySupport {
  /// <summary>
  /// The v1 profile declares no concurrency tokens or mutable-record conflict signals.
  /// </summary>
  NoneInV1Unsupported,
}

/// <summary>
/// Identifies the value format used by a provider type mapping.
/// </summary>
public enum DataVaultProviderValueFormat {
  /// <summary>
  /// Values are persisted as provider text without a provider-specific transformation.
  /// </summary>
  Text,

  /// <summary>
  /// Timestamp values are persisted as ISO 8601 UTC text.
  /// </summary>
  Iso8601UtcText,

  /// <summary>
  /// Timestamp values are persisted through the provider's native <see cref="DateTimeOffset" /> mapping.
  /// </summary>
  NativeDateTimeOffset,

  /// <summary>
  /// Integer values are persisted through the provider's native integer mapping.
  /// </summary>
  NativeInteger,

  /// <summary>
  /// Timestamp values are persisted as UTC <see cref="DateTime" /> ticks in a native 64-bit integer column.
  /// </summary>
  UtcTicks,
}

/// <summary>
/// Identifies the physical load-timestamp storage shape used when provider profiles are projected to EF metadata.
/// </summary>
public enum DataVaultLoadTimestampStorage {
  /// <summary>
  /// Keeps the provider profile's default load-timestamp storage mapping.
  /// </summary>
  ProviderDefault,

  /// <summary>
  /// Persists load timestamps as ISO 8601 UTC text.
  /// </summary>
  Iso8601UtcText,

  /// <summary>
  /// Persists load timestamps as UTC <see cref="DateTime" /> ticks in a native 64-bit integer column.
  /// </summary>
  UtcTicks,
}

/// <summary>
/// Identifies how a provider profile should handle index include columns when the provider has no native include support.
/// </summary>
public enum DataVaultUnsupportedIncludedIndexColumnMode {
  /// <summary>
  /// Drops include columns for providers that cannot persist them as native include columns.
  /// </summary>
  Ignore,

  /// <summary>
  /// Appends include columns to the index key for providers that cannot persist native include columns.
  /// </summary>
  AppendToKey,
}

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
  public DataVaultProviderTypeMapping(
      DataVaultLogicalPropertyKind logicalPropertyKind,
      Type modelClrType,
      string nativeStoreType,
      DataVaultProviderValueFormat valueFormat) {
    ArgumentNullException.ThrowIfNull(modelClrType);
    ArgumentException.ThrowIfNullOrWhiteSpace(nativeStoreType);

    LogicalPropertyKind = logicalPropertyKind;
    ModelClrType = modelClrType;
    NativeStoreType = nativeStoreType;
    ValueFormat = valueFormat;
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
}

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

    return "bigint";
  }

  private static string GetLoadTimestampStorageProfileSuffix(DataVaultLoadTimestampStorage storage) {
    return storage switch {
      DataVaultLoadTimestampStorage.Iso8601UtcText => "-loadts-iso8601",
      DataVaultLoadTimestampStorage.UtcTicks => "-loadts-utc-ticks",
      _ => throw new ArgumentOutOfRangeException(nameof(storage), storage, "Unsupported Data Vault load timestamp storage."),
    };
  }
}

/// <summary>
/// Provides built-in provider capability profiles for the v1 Data Vault translator.
/// </summary>
public static class DataVaultProviderCapabilityProfiles {
  /// <summary>
  /// Gets the initial SQLite v1 provider capability profile.
  /// </summary>
  public static DataVaultProviderCapabilityProfile Sqlite { get; } = new(
      "sqlite-v1",
      DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
      DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
      [
          Text(DataVaultLogicalPropertyKind.HashKey),
          Text(DataVaultLogicalPropertyKind.HashDiff),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(DateTimeOffset),
              "TEXT",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Text(DataVaultLogicalPropertyKind.RecordSource),
          Text(DataVaultLogicalPropertyKind.ParticipantReference),
          Text(DataVaultLogicalPropertyKind.BusinessKey),
          Text(DataVaultLogicalPropertyKind.PayloadText),
          new(
              DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
              typeof(DateTimeOffset),
              "TEXT",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Integer(DataVaultLogicalPropertyKind.BridgeDepth, "INTEGER"),
          Text(DataVaultLogicalPropertyKind.DrivingKey),
      ]);

  /// <summary>
  /// Gets the initial Oracle v1 provider capability profile.
  /// </summary>
  public static DataVaultProviderCapabilityProfile Oracle { get; } = new(
      "oracle-v1",
      DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
      DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
      [
          Text(DataVaultLogicalPropertyKind.HashKey, "VARCHAR2(64 CHAR)"),
          Text(DataVaultLogicalPropertyKind.HashDiff, "VARCHAR2(64 CHAR)"),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(string),
              "VARCHAR2(33 CHAR)",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Text(DataVaultLogicalPropertyKind.RecordSource, "VARCHAR2(255 CHAR)"),
          Text(DataVaultLogicalPropertyKind.ParticipantReference, "VARCHAR2(64 CHAR)"),
          Text(DataVaultLogicalPropertyKind.BusinessKey, "VARCHAR2(255 CHAR)"),
          Text(DataVaultLogicalPropertyKind.PayloadText, "CLOB"),
          new(
              DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
              typeof(string),
              "VARCHAR2(33 CHAR)",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Integer(DataVaultLogicalPropertyKind.BridgeDepth, "NUMBER(10)"),
          Text(DataVaultLogicalPropertyKind.DrivingKey, "VARCHAR2(255 CHAR)"),
      ],
      allowsIndexesCoveredByPrimaryKey: false);

  /// <summary>
  /// Gets the PostgreSQL v1 provider capability profile.
  /// </summary>
  public static DataVaultProviderCapabilityProfile Postgres { get; } = new(
      "postgres-v1",
      DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
      DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
      [
          Text(DataVaultLogicalPropertyKind.HashKey, "varchar(64)"),
          Text(DataVaultLogicalPropertyKind.HashDiff, "varchar(64)"),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(DateTimeOffset),
              "timestamp with time zone",
              DataVaultProviderValueFormat.NativeDateTimeOffset),
          Text(DataVaultLogicalPropertyKind.RecordSource, "varchar(255)"),
          Text(DataVaultLogicalPropertyKind.ParticipantReference, "varchar(64)"),
          Text(DataVaultLogicalPropertyKind.BusinessKey, "varchar(255)"),
          Text(DataVaultLogicalPropertyKind.PayloadText, "text"),
          new(
              DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
              typeof(DateTimeOffset),
              "timestamp with time zone",
              DataVaultProviderValueFormat.NativeDateTimeOffset),
          Integer(DataVaultLogicalPropertyKind.BridgeDepth, "integer"),
          Text(DataVaultLogicalPropertyKind.DrivingKey, "varchar(255)"),
      ]);

  /// <summary>
  /// Gets the DB2 v1 provider capability profile.
  /// </summary>
  public static DataVaultProviderCapabilityProfile Db2 { get; } = new(
      "db2-v1",
      DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
      DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
      [
          Text(DataVaultLogicalPropertyKind.HashKey, "VARCHAR(64)"),
          Text(DataVaultLogicalPropertyKind.HashDiff, "VARCHAR(64)"),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(string),
              "VARCHAR(33)",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Text(DataVaultLogicalPropertyKind.RecordSource, "VARCHAR(255)"),
          Text(DataVaultLogicalPropertyKind.ParticipantReference, "VARCHAR(64)"),
          Text(DataVaultLogicalPropertyKind.BusinessKey, "VARCHAR(255)"),
          Text(DataVaultLogicalPropertyKind.PayloadText, "CLOB"),
          new(
              DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
              typeof(string),
              "VARCHAR(33)",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Integer(DataVaultLogicalPropertyKind.BridgeDepth, "INTEGER"),
          Text(DataVaultLogicalPropertyKind.DrivingKey, "VARCHAR(255)"),
      ]);

  /// <summary>
  /// Gets the SQL Server v1 provider capability profile.
  /// </summary>
  public static DataVaultProviderCapabilityProfile SqlServer { get; } = new(
      "sqlserver-v1",
      DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
      DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
      [
          Text(DataVaultLogicalPropertyKind.HashKey, "nvarchar(64)"),
          Text(DataVaultLogicalPropertyKind.HashDiff, "nvarchar(64)"),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(DateTimeOffset),
              "datetimeoffset",
              DataVaultProviderValueFormat.NativeDateTimeOffset),
          Text(DataVaultLogicalPropertyKind.RecordSource, "nvarchar(255)"),
          Text(DataVaultLogicalPropertyKind.ParticipantReference, "nvarchar(64)"),
          Text(DataVaultLogicalPropertyKind.BusinessKey, "nvarchar(255)"),
          Text(DataVaultLogicalPropertyKind.PayloadText, "nvarchar(max)"),
          new(
              DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
              typeof(DateTimeOffset),
              "datetimeoffset",
              DataVaultProviderValueFormat.NativeDateTimeOffset),
          Integer(DataVaultLogicalPropertyKind.BridgeDepth, "int"),
          Text(DataVaultLogicalPropertyKind.DrivingKey, "nvarchar(255)"),
      ]);

  /// <summary>
  /// Gets the Pomelo.EntityFrameworkCore.MySql v1 provider capability profile.
  /// </summary>
  public static DataVaultProviderCapabilityProfile MySql { get; } = new(
      "mysql-pomelo-v1",
      DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
      DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
      [
          Text(DataVaultLogicalPropertyKind.HashKey, "varchar(64)"),
          Text(DataVaultLogicalPropertyKind.HashDiff, "varchar(64)"),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(DateTimeOffset),
              "varchar(33)",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Text(DataVaultLogicalPropertyKind.RecordSource, "varchar(255)"),
          Text(DataVaultLogicalPropertyKind.ParticipantReference, "varchar(64)"),
          Text(DataVaultLogicalPropertyKind.BusinessKey, "varchar(255)"),
          Text(DataVaultLogicalPropertyKind.PayloadText, "longtext"),
          new(
              DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
              typeof(DateTimeOffset),
              "varchar(33)",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Integer(DataVaultLogicalPropertyKind.BridgeDepth, "int"),
          Text(DataVaultLogicalPropertyKind.DrivingKey, "varchar(255)"),
      ],
      maximumIdentifierLength: 64,
      unsupportedIncludedIndexColumnMode: DataVaultUnsupportedIncludedIndexColumnMode.Ignore);

  private static DataVaultProviderTypeMapping Text(
      DataVaultLogicalPropertyKind logicalPropertyKind,
      string nativeStoreType = "TEXT") {
    return new DataVaultProviderTypeMapping(
        logicalPropertyKind,
        typeof(string),
        nativeStoreType,
        DataVaultProviderValueFormat.Text);
  }

  private static DataVaultProviderTypeMapping Integer(
      DataVaultLogicalPropertyKind logicalPropertyKind,
      string nativeStoreType) {
    return new DataVaultProviderTypeMapping(
        logicalPropertyKind,
        typeof(int),
        nativeStoreType,
        DataVaultProviderValueFormat.NativeInteger);
  }
}
