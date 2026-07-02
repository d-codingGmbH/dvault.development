using System.Collections.ObjectModel;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides built-in provider capability profiles for the v1 Data Vault translator.
/// </summary>
public static class DataVaultProviderCapabilityProfiles {
  private const string DefaultStableHashAlgorithmId = "sha256-v1";
  private const int DefaultStableHashDigestByteLength = 32;

  /// <summary>
  /// Gets the initial SQLite v1 provider capability profile.
  /// </summary>
  public static DataVaultProviderCapabilityProfile Sqlite { get; } = new(
      "sqlite-v1",
      DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
      DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
      [
          HashKeyBinary(DataVaultLogicalPropertyKind.HashKey),
          Text(DataVaultLogicalPropertyKind.HashDiff),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(DateTimeOffset),
              "TEXT",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Text(DataVaultLogicalPropertyKind.RecordSource),
          HashKeyBinary(DataVaultLogicalPropertyKind.ParticipantReference),
          Text(DataVaultLogicalPropertyKind.DependentChildKey),
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
          HashKeyBinary(DataVaultLogicalPropertyKind.HashKey, "RAW(32)"),
          Text(DataVaultLogicalPropertyKind.HashDiff, "VARCHAR2(64 CHAR)"),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(string),
              "VARCHAR2(33 CHAR)",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Text(DataVaultLogicalPropertyKind.RecordSource, "VARCHAR2(255 CHAR)"),
          HashKeyBinary(DataVaultLogicalPropertyKind.ParticipantReference, "RAW(32)"),
          Text(DataVaultLogicalPropertyKind.DependentChildKey, "VARCHAR2(255 CHAR)"),
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
          HashKeyBinary(DataVaultLogicalPropertyKind.HashKey, "bytea"),
          Text(DataVaultLogicalPropertyKind.HashDiff, "varchar(64)"),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(DateTimeOffset),
              "timestamp with time zone",
              DataVaultProviderValueFormat.NativeDateTimeOffset),
          Text(DataVaultLogicalPropertyKind.RecordSource, "varchar(255)"),
          HashKeyBinary(DataVaultLogicalPropertyKind.ParticipantReference, "bytea"),
          Text(DataVaultLogicalPropertyKind.DependentChildKey, "varchar(255)"),
          Text(DataVaultLogicalPropertyKind.BusinessKey, "varchar(255)"),
          Text(DataVaultLogicalPropertyKind.PayloadText, "text"),
          new(
              DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
              typeof(DateTimeOffset),
              "timestamp with time zone",
              DataVaultProviderValueFormat.NativeDateTimeOffset),
          Integer(DataVaultLogicalPropertyKind.BridgeDepth, "integer"),
          Text(DataVaultLogicalPropertyKind.DrivingKey, "varchar(255)"),
      ],
      maximumIdentifierLength: 63);

  /// <summary>
  /// Gets the DB2 v1 provider capability profile.
  /// </summary>
  public static DataVaultProviderCapabilityProfile Db2 { get; } = new(
      "db2-v1",
      DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
      DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
      [
          HashKeyBinary(DataVaultLogicalPropertyKind.HashKey, "VARBINARY(32)"),
          Text(DataVaultLogicalPropertyKind.HashDiff, "VARCHAR(64)"),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(string),
              "VARCHAR(33)",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Text(DataVaultLogicalPropertyKind.RecordSource, "VARCHAR(255)"),
          HashKeyBinary(DataVaultLogicalPropertyKind.ParticipantReference, "VARBINARY(32)"),
          Text(DataVaultLogicalPropertyKind.DependentChildKey, "VARCHAR(255)"),
          Text(DataVaultLogicalPropertyKind.BusinessKey, "VARCHAR(255)"),
          Text(DataVaultLogicalPropertyKind.PayloadText, "CLOB"),
          new(
              DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
              typeof(string),
              "VARCHAR(33)",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Integer(DataVaultLogicalPropertyKind.BridgeDepth, "INTEGER"),
          Text(DataVaultLogicalPropertyKind.DrivingKey, "VARCHAR(255)"),
      ],
      maximumIdentifierLength: 128,
      allowsIndexesCoveredByPrimaryKey: false,
      unsupportedIncludedIndexColumnMode: DataVaultUnsupportedIncludedIndexColumnMode.AppendToKey);

  /// <summary>
  /// Gets the SQL Server v1 provider capability profile.
  /// </summary>
  public static DataVaultProviderCapabilityProfile SqlServer { get; } = new(
      "sqlserver-v1",
      DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
      DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
      [
          HashKeyBinary(DataVaultLogicalPropertyKind.HashKey, "varbinary(32)"),
          Text(DataVaultLogicalPropertyKind.HashDiff, "nvarchar(64)"),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(DateTimeOffset),
              "datetimeoffset",
              DataVaultProviderValueFormat.NativeDateTimeOffset),
          Text(DataVaultLogicalPropertyKind.RecordSource, "nvarchar(255)"),
          HashKeyBinary(DataVaultLogicalPropertyKind.ParticipantReference, "varbinary(32)"),
          Text(DataVaultLogicalPropertyKind.DependentChildKey, "nvarchar(255)"),
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
          HashKeyBinary(DataVaultLogicalPropertyKind.HashKey, "varbinary(32)"),
          Text(DataVaultLogicalPropertyKind.HashDiff, "varchar(64)"),
          new(
              DataVaultLogicalPropertyKind.LoadTimestamp,
              typeof(string),
              "varchar(33)",
              DataVaultProviderValueFormat.Iso8601UtcText),
          Text(DataVaultLogicalPropertyKind.RecordSource, "varchar(255)"),
          HashKeyBinary(DataVaultLogicalPropertyKind.ParticipantReference, "varbinary(32)"),
          Text(DataVaultLogicalPropertyKind.DependentChildKey, "varchar(255)"),
          Text(DataVaultLogicalPropertyKind.BusinessKey, "varchar(255)"),
          Text(DataVaultLogicalPropertyKind.PayloadText, "longtext"),
          new(
              DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
              typeof(string),
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

  private static DataVaultProviderTypeMapping HashKeyText(
      DataVaultLogicalPropertyKind logicalPropertyKind,
      string nativeStoreType = "TEXT") {
    return new DataVaultProviderTypeMapping(
        logicalPropertyKind,
        typeof(string),
        nativeStoreType,
        DataVaultProviderValueFormat.LowercaseHexText,
        DataVaultHashKeyStorageProfile.HexString,
        DefaultStableHashAlgorithmId,
        DefaultStableHashDigestByteLength,
        "lowercase-hex-no-prefix",
        "none-string-model");
  }

  private static DataVaultProviderTypeMapping HashKeyBinary(
      DataVaultLogicalPropertyKind logicalPropertyKind,
      string nativeStoreType = "BLOB") {
    return new DataVaultProviderTypeMapping(
        logicalPropertyKind,
        typeof(string),
        nativeStoreType,
        DataVaultProviderValueFormat.LowercaseHexBinary,
        DataVaultHashKeyStorageProfile.Binary,
        DefaultStableHashAlgorithmId,
        DefaultStableHashDigestByteLength,
        "lowercase-hex-no-prefix",
        "lowercase-hex-string-to-bytes");
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
