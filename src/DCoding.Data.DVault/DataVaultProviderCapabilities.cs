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
  public DataVaultProviderCapabilityProfile(
      string profileName,
      DataVaultProviderSqlFunctionSupport sqlFunctionSupport,
      DataVaultProviderConcurrencySupport concurrencySupport,
      IEnumerable<DataVaultProviderTypeMapping> typeMappings) {
    ArgumentException.ThrowIfNullOrWhiteSpace(profileName);
    ArgumentNullException.ThrowIfNull(typeMappings);

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
              typeof(DateTimeOffset),
              "TIMESTAMP WITH TIME ZONE",
              DataVaultProviderValueFormat.NativeDateTimeOffset),
          Text(DataVaultLogicalPropertyKind.RecordSource, "VARCHAR2(255 CHAR)"),
          Text(DataVaultLogicalPropertyKind.ParticipantReference, "VARCHAR2(64 CHAR)"),
          Text(DataVaultLogicalPropertyKind.BusinessKey, "VARCHAR2(255 CHAR)"),
          Text(DataVaultLogicalPropertyKind.PayloadText, "CLOB"),
      ]);

  private static DataVaultProviderTypeMapping Text(
      DataVaultLogicalPropertyKind logicalPropertyKind,
      string nativeStoreType = "TEXT") {
    return new DataVaultProviderTypeMapping(
        logicalPropertyKind,
        typeof(string),
        nativeStoreType,
        DataVaultProviderValueFormat.Text);
  }
}
