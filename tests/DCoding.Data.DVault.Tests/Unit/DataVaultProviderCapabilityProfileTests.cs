using System.Globalization;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultProviderCapabilityProfileTests {
  [Fact]
  public void SqliteProfileDeclaresExplicitUnsupportedFunctionAndConcurrencyBaselines() {
    var profile = DataVaultProviderCapabilityProfiles.Sqlite;

    Assert.Equal("sqlite-v1", profile.ProfileName);
    Assert.Equal(DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported, profile.SqlFunctionSupport);
    Assert.Equal(DataVaultProviderConcurrencySupport.NoneInV1Unsupported, profile.ConcurrencySupport);

    var functionException = Assert.Throws<NotSupportedException>(() => profile.RequireSqlFunction("computed_hash"));
    var concurrencyException = Assert.Throws<NotSupportedException>(() => profile.RequireConcurrencySignal("rowversion"));

    Assert.Contains("sqlite-v1", functionException.Message, StringComparison.Ordinal);
    Assert.Contains("SQL function computed_hash", functionException.Message, StringComparison.Ordinal);
    Assert.Contains("sqlite-v1", concurrencyException.Message, StringComparison.Ordinal);
    Assert.Contains("concurrency signal rowversion", concurrencyException.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void SqliteProfileDeclaresBoundedTextAndTimestampMappings() {
    var profile = DataVaultProviderCapabilityProfiles.Sqlite;

    Assert.Equal(
        [
            DataVaultLogicalPropertyKind.HashKey,
            DataVaultLogicalPropertyKind.HashDiff,
            DataVaultLogicalPropertyKind.LoadTimestamp,
            DataVaultLogicalPropertyKind.RecordSource,
            DataVaultLogicalPropertyKind.ParticipantReference,
            DataVaultLogicalPropertyKind.BusinessKey,
            DataVaultLogicalPropertyKind.PayloadText,
            DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
            DataVaultLogicalPropertyKind.BridgeDepth,
            DataVaultLogicalPropertyKind.DrivingKey,
        ],
        profile.TypeMappings.Select(mapping => mapping.LogicalPropertyKind));

    AssertMapping(profile, DataVaultLogicalPropertyKind.HashKey, typeof(string), DataVaultProviderValueFormat.LowercaseHexText);
    AssertMapping(profile, DataVaultLogicalPropertyKind.HashDiff, typeof(string), DataVaultProviderValueFormat.Text);
    AssertMapping(profile, DataVaultLogicalPropertyKind.LoadTimestamp, typeof(DateTimeOffset), DataVaultProviderValueFormat.Iso8601UtcText);
    AssertMapping(profile, DataVaultLogicalPropertyKind.RecordSource, typeof(string), DataVaultProviderValueFormat.Text);
    AssertMapping(profile, DataVaultLogicalPropertyKind.ParticipantReference, typeof(string), DataVaultProviderValueFormat.LowercaseHexText);
    AssertMapping(profile, DataVaultLogicalPropertyKind.BusinessKey, typeof(string), DataVaultProviderValueFormat.Text);
    AssertMapping(profile, DataVaultLogicalPropertyKind.DrivingKey, typeof(string), DataVaultProviderValueFormat.Text);
    AssertMapping(profile, DataVaultLogicalPropertyKind.PayloadText, typeof(string), DataVaultProviderValueFormat.Text);
    AssertMapping(profile, DataVaultLogicalPropertyKind.SatelliteSnapshotReference, typeof(DateTimeOffset), DataVaultProviderValueFormat.Iso8601UtcText);
    AssertMapping(profile, DataVaultLogicalPropertyKind.BridgeDepth, typeof(int), "INTEGER", DataVaultProviderValueFormat.NativeInteger);
  }

  [Fact]
  public void OracleProfileDeclaresExplicitUnsupportedFunctionAndConcurrencyBaselines() {
    var profile = DataVaultProviderCapabilityProfiles.Oracle;

    Assert.Equal("oracle-v1", profile.ProfileName);
    Assert.Equal(DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported, profile.SqlFunctionSupport);
    Assert.Equal(DataVaultProviderConcurrencySupport.NoneInV1Unsupported, profile.ConcurrencySupport);
    Assert.False(profile.AllowsIndexesCoveredByPrimaryKey);
    Assert.Equal(
        DataVaultUnsupportedIncludedIndexColumnMode.AppendToKey,
        profile.UnsupportedIncludedIndexColumnMode);

    var functionException = Assert.Throws<NotSupportedException>(() => profile.RequireSqlFunction("computed_hash"));
    var concurrencyException = Assert.Throws<NotSupportedException>(() => profile.RequireConcurrencySignal("rowversion"));

    Assert.Contains("oracle-v1", functionException.Message, StringComparison.Ordinal);
    Assert.Contains("SQL function computed_hash", functionException.Message, StringComparison.Ordinal);
    Assert.Contains("oracle-v1", concurrencyException.Message, StringComparison.Ordinal);
    Assert.Contains("concurrency signal rowversion", concurrencyException.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void OracleProfileDeclaresNativeStorageMappingsForAllLogicalPropertyKinds() {
    var profile = DataVaultProviderCapabilityProfiles.Oracle;

    Assert.Equal(
        [
            DataVaultLogicalPropertyKind.HashKey,
            DataVaultLogicalPropertyKind.HashDiff,
            DataVaultLogicalPropertyKind.LoadTimestamp,
            DataVaultLogicalPropertyKind.RecordSource,
            DataVaultLogicalPropertyKind.ParticipantReference,
            DataVaultLogicalPropertyKind.BusinessKey,
            DataVaultLogicalPropertyKind.PayloadText,
            DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
            DataVaultLogicalPropertyKind.BridgeDepth,
            DataVaultLogicalPropertyKind.DrivingKey,
        ],
        profile.TypeMappings.Select(mapping => mapping.LogicalPropertyKind));
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.HashKey,
        typeof(string),
        "VARCHAR2(64 CHAR)",
        DataVaultProviderValueFormat.LowercaseHexText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.HashDiff,
        typeof(string),
        "VARCHAR2(64 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(string),
        "VARCHAR2(33 CHAR)",
        DataVaultProviderValueFormat.Iso8601UtcText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.RecordSource,
        typeof(string),
        "VARCHAR2(255 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.ParticipantReference,
        typeof(string),
        "VARCHAR2(64 CHAR)",
        DataVaultProviderValueFormat.LowercaseHexText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.BusinessKey,
        typeof(string),
        "VARCHAR2(255 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.DrivingKey,
        typeof(string),
        "VARCHAR2(255 CHAR)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.PayloadText,
        typeof(string),
        "CLOB",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
        typeof(string),
        "VARCHAR2(33 CHAR)",
        DataVaultProviderValueFormat.Iso8601UtcText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.BridgeDepth,
        typeof(int),
        "NUMBER(10)",
        DataVaultProviderValueFormat.NativeInteger);
  }

  [Fact]
  public void MySqlPomeloProfileDeclaresBoundedTextAndTimestampMappings() {
    var profile = DataVaultProviderCapabilityProfiles.MySql;

    Assert.Equal("mysql-pomelo-v1", profile.ProfileName);
    Assert.Equal(DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported, profile.SqlFunctionSupport);
    Assert.Equal(DataVaultProviderConcurrencySupport.NoneInV1Unsupported, profile.ConcurrencySupport);
    Assert.Equal(64, profile.MaximumIdentifierLength);
    Assert.Equal(
        DataVaultUnsupportedIncludedIndexColumnMode.Ignore,
        profile.UnsupportedIncludedIndexColumnMode);
    Assert.Equal(
        [
            DataVaultLogicalPropertyKind.HashKey,
            DataVaultLogicalPropertyKind.HashDiff,
            DataVaultLogicalPropertyKind.LoadTimestamp,
            DataVaultLogicalPropertyKind.RecordSource,
            DataVaultLogicalPropertyKind.ParticipantReference,
            DataVaultLogicalPropertyKind.BusinessKey,
            DataVaultLogicalPropertyKind.PayloadText,
            DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
            DataVaultLogicalPropertyKind.BridgeDepth,
            DataVaultLogicalPropertyKind.DrivingKey,
        ],
        profile.TypeMappings.Select(mapping => mapping.LogicalPropertyKind));

    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.HashKey,
        typeof(string),
        "varchar(64)",
        DataVaultProviderValueFormat.LowercaseHexText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.HashDiff,
        typeof(string),
        "varchar(64)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(DateTimeOffset),
        "varchar(33)",
        DataVaultProviderValueFormat.Iso8601UtcText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.RecordSource,
        typeof(string),
        "varchar(255)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.ParticipantReference,
        typeof(string),
        "varchar(64)",
        DataVaultProviderValueFormat.LowercaseHexText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.BusinessKey,
        typeof(string),
        "varchar(255)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.DrivingKey,
        typeof(string),
        "varchar(255)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.PayloadText,
        typeof(string),
        "longtext",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
        typeof(DateTimeOffset),
        "varchar(33)",
        DataVaultProviderValueFormat.Iso8601UtcText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.BridgeDepth,
        typeof(int),
        "int",
        DataVaultProviderValueFormat.NativeInteger);
  }

  [Fact]
  public void PostgresProfileDeclaresNativeTimestampMappings() {
    var profile = DataVaultProviderCapabilityProfiles.Postgres;

    Assert.Equal("postgres-v1", profile.ProfileName);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(DateTimeOffset),
        "timestamp with time zone",
        DataVaultProviderValueFormat.NativeDateTimeOffset);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
        typeof(DateTimeOffset),
        "timestamp with time zone",
        DataVaultProviderValueFormat.NativeDateTimeOffset);
  }

  [Fact]
  public void SqlServerProfileDeclaresNativeTimestampMappings() {
    var profile = DataVaultProviderCapabilityProfiles.SqlServer;

    Assert.Equal("sqlserver-v1", profile.ProfileName);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(DateTimeOffset),
        "datetimeoffset",
        DataVaultProviderValueFormat.NativeDateTimeOffset);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
        typeof(DateTimeOffset),
        "datetimeoffset",
        DataVaultProviderValueFormat.NativeDateTimeOffset);
  }

  [Fact]
  public void Db2ProfileDeclaresIsoTextTimestampMappings() {
    var profile = DataVaultProviderCapabilityProfiles.Db2;

    Assert.Equal("db2-v1", profile.ProfileName);
    Assert.Equal(DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported, profile.SqlFunctionSupport);
    Assert.Equal(DataVaultProviderConcurrencySupport.NoneInV1Unsupported, profile.ConcurrencySupport);
    Assert.Equal(128, profile.MaximumIdentifierLength);
    Assert.False(profile.AllowsIndexesCoveredByPrimaryKey);
    Assert.Equal(
        DataVaultUnsupportedIncludedIndexColumnMode.AppendToKey,
        profile.UnsupportedIncludedIndexColumnMode);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.HashKey,
        typeof(string),
        "VARCHAR(64)",
        DataVaultProviderValueFormat.LowercaseHexText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.HashDiff,
        typeof(string),
        "VARCHAR(64)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(string),
        "VARCHAR(33)",
        DataVaultProviderValueFormat.Iso8601UtcText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.RecordSource,
        typeof(string),
        "VARCHAR(255)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.ParticipantReference,
        typeof(string),
        "VARCHAR(64)",
        DataVaultProviderValueFormat.LowercaseHexText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.BusinessKey,
        typeof(string),
        "VARCHAR(255)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.DrivingKey,
        typeof(string),
        "VARCHAR(255)",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.PayloadText,
        typeof(string),
        "CLOB",
        DataVaultProviderValueFormat.Text);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
        typeof(string),
        "VARCHAR(33)",
        DataVaultProviderValueFormat.Iso8601UtcText);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.BridgeDepth,
        typeof(int),
        "INTEGER",
        DataVaultProviderValueFormat.NativeInteger);
  }

  [Fact]
  public void LoadTimestampStorageCanBeProjectedToUtcTicksWithoutChangingProviderDefault() {
    var profile = DataVaultProviderCapabilityProfiles.Oracle;
    var providerDefault = profile.WithLoadTimestampStorage(DataVaultLoadTimestampStorage.ProviderDefault);
    var ticksProfile = profile.WithLoadTimestampStorage(DataVaultLoadTimestampStorage.UtcTicks);

    Assert.Same(profile, providerDefault);
    Assert.Equal("oracle-v1-loadts-utc-ticks", ticksProfile.ProfileName);
    Assert.Equal(
        DataVaultUnsupportedIncludedIndexColumnMode.AppendToKey,
        ticksProfile.UnsupportedIncludedIndexColumnMode);
    AssertMapping(
        ticksProfile,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(long),
        "NUMBER(19)",
        DataVaultProviderValueFormat.UtcTicks);
    AssertMapping(
        ticksProfile,
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
        typeof(long),
        "NUMBER(19)",
        DataVaultProviderValueFormat.UtcTicks);
    AssertMapping(
        ticksProfile,
        DataVaultLogicalPropertyKind.HashKey,
        typeof(string),
        "VARCHAR2(64 CHAR)",
        DataVaultProviderValueFormat.LowercaseHexText);
  }

  [Fact]
  public void Db2LoadTimestampStorageCanBeProjectedToUtcTicks() {
    var profile = DataVaultProviderCapabilityProfiles.Db2.WithLoadTimestampStorage(DataVaultLoadTimestampStorage.UtcTicks);

    Assert.Equal("db2-v1-loadts-utc-ticks", profile.ProfileName);
    Assert.Equal(128, profile.MaximumIdentifierLength);
    Assert.False(profile.AllowsIndexesCoveredByPrimaryKey);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(long),
        "BIGINT",
        DataVaultProviderValueFormat.UtcTicks);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
        typeof(long),
        "BIGINT",
        DataVaultProviderValueFormat.UtcTicks);
  }

  [Theory]
  [InlineData("sqlite-v1", "sha256-v1", 32)]
  [InlineData("sqlite-v1", "sha1-v1", 20)]
  [InlineData("sqlite-v1", "sha256-128-v1", 16)]
  [InlineData("sqlite-v1", "sha256-160-v1", 20)]
  [InlineData("oracle-v1", "sha256-v1", 32)]
  [InlineData("oracle-v1", "sha1-v1", 20)]
  [InlineData("oracle-v1", "sha256-128-v1", 16)]
  [InlineData("oracle-v1", "sha256-160-v1", 20)]
  [InlineData("postgres-v1", "sha256-v1", 32)]
  [InlineData("postgres-v1", "sha1-v1", 20)]
  [InlineData("postgres-v1", "sha256-128-v1", 16)]
  [InlineData("postgres-v1", "sha256-160-v1", 20)]
  [InlineData("sqlserver-v1", "sha256-v1", 32)]
  [InlineData("sqlserver-v1", "sha1-v1", 20)]
  [InlineData("sqlserver-v1", "sha256-128-v1", 16)]
  [InlineData("sqlserver-v1", "sha256-160-v1", 20)]
  [InlineData("db2-v1", "sha256-v1", 32)]
  [InlineData("db2-v1", "sha1-v1", 20)]
  [InlineData("db2-v1", "sha256-128-v1", 16)]
  [InlineData("db2-v1", "sha256-160-v1", 20)]
  [InlineData("mysql-pomelo-v1", "sha256-v1", 32)]
  [InlineData("mysql-pomelo-v1", "sha1-v1", 20)]
  [InlineData("mysql-pomelo-v1", "sha256-128-v1", 16)]
  [InlineData("mysql-pomelo-v1", "sha256-160-v1", 20)]
  public void HashKeyHexStringStorageProfilesSizeKeysAndReferencesByStableHashDigest(
      string profileName,
      string algorithmId,
      int digestByteLength) {
    var profile = SelectProfile(profileName).WithStableHashAlgorithm(algorithmId, digestByteLength);
    var expectedStoreType = ExpectedHexStringStoreType(profileName, digestByteLength);

    AssertHashKeyMapping(
        profile,
        DataVaultLogicalPropertyKind.HashKey,
        expectedStoreType,
        DataVaultHashKeyStorageProfile.HexString,
        DataVaultProviderValueFormat.LowercaseHexText,
        algorithmId,
        digestByteLength,
        "none-string-model");
    AssertHashKeyMapping(
        profile,
        DataVaultLogicalPropertyKind.ParticipantReference,
        expectedStoreType,
        DataVaultHashKeyStorageProfile.HexString,
        DataVaultProviderValueFormat.LowercaseHexText,
        algorithmId,
        digestByteLength,
        "none-string-model");
  }

  [Theory]
  [InlineData("sqlite-v1", "BLOB")]
  [InlineData("oracle-v1", "RAW(20)")]
  [InlineData("postgres-v1", "bytea")]
  [InlineData("sqlserver-v1", "varbinary(20)")]
  [InlineData("db2-v1", "VARBINARY(20)")]
  [InlineData("mysql-pomelo-v1", "varbinary(20)")]
  public void BinaryHashKeyStorageProfileIsExplicitOptInAndKeepsStringModelBoundary(
      string profileName,
      string expectedStoreType) {
    var profile = SelectProfile(profileName).WithHashKeyStorageProfile(
        DataVaultHashKeyStorageProfile.Binary,
        "sha1-v1",
        20);

    AssertHashKeyMapping(
        profile,
        DataVaultLogicalPropertyKind.HashKey,
        expectedStoreType,
        DataVaultHashKeyStorageProfile.Binary,
        DataVaultProviderValueFormat.LowercaseHexBinary,
        "sha1-v1",
        20,
        "lowercase-hex-string-to-bytes");
  }

  [Fact]
  public void LoadTimestampStorageCanBeProjectedToIsoTextForNativeTimestampProfiles() {
    var profile = DataVaultProviderCapabilityProfiles.SqlServer.WithLoadTimestampStorage(DataVaultLoadTimestampStorage.Iso8601UtcText);

    Assert.Equal("sqlserver-v1-loadts-iso8601", profile.ProfileName);
    AssertMapping(
        profile,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        typeof(string),
        "nvarchar(33)",
        DataVaultProviderValueFormat.Iso8601UtcText);
  }

  [Fact]
  public void RequiredTypeMappingLookupFailsDeterministicallyWhenCapabilityIsMissing() {
    var profile = new DataVaultProviderCapabilityProfile(
        "test-profile",
        DataVaultProviderSqlFunctionSupport.NoneInV1Unsupported,
        DataVaultProviderConcurrencySupport.NoneInV1Unsupported,
        []);

    var exception = Assert.Throws<NotSupportedException>(() =>
        profile.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.PayloadText));

    Assert.Contains("test-profile", exception.Message, StringComparison.Ordinal);
    Assert.Contains("type mapping for PayloadText", exception.Message, StringComparison.Ordinal);
  }

  private static void AssertMapping(
      DataVaultProviderCapabilityProfile profile,
      DataVaultLogicalPropertyKind logicalPropertyKind,
      Type expectedClrType,
      DataVaultProviderValueFormat expectedValueFormat) {
    AssertMapping(profile, logicalPropertyKind, expectedClrType, "TEXT", expectedValueFormat);
  }

  private static void AssertMapping(
      DataVaultProviderCapabilityProfile profile,
      DataVaultLogicalPropertyKind logicalPropertyKind,
      Type expectedClrType,
      string expectedNativeStoreType,
      DataVaultProviderValueFormat expectedValueFormat) {
    var mapping = profile.GetRequiredTypeMapping(logicalPropertyKind);

    Assert.Equal(logicalPropertyKind, mapping.LogicalPropertyKind);
    Assert.Equal(expectedClrType, mapping.ModelClrType);
    Assert.Equal(expectedNativeStoreType, mapping.NativeStoreType);
    Assert.Equal(expectedValueFormat, mapping.ValueFormat);
  }

  private static void AssertHashKeyMapping(
      DataVaultProviderCapabilityProfile profile,
      DataVaultLogicalPropertyKind logicalPropertyKind,
      string expectedStoreType,
      DataVaultHashKeyStorageProfile expectedStorageProfile,
      DataVaultProviderValueFormat expectedValueFormat,
      string expectedAlgorithmId,
      int expectedDigestByteLength,
      string expectedConversionBehavior) {
    var mapping = profile.GetRequiredTypeMapping(logicalPropertyKind);

    Assert.Equal(typeof(string), mapping.ModelClrType);
    Assert.Equal(expectedStoreType, mapping.NativeStoreType);
    Assert.Equal(expectedValueFormat, mapping.ValueFormat);
    Assert.Equal(expectedStorageProfile, mapping.HashKeyStorageProfile);
    Assert.Equal(expectedAlgorithmId, mapping.StableHashAlgorithmId);
    Assert.Equal(expectedDigestByteLength, mapping.DigestByteLength);
    Assert.Equal("lowercase-hex-no-prefix", mapping.DigestEncoding);
    Assert.Equal(expectedConversionBehavior, mapping.ConversionBehavior);
  }

  private static DataVaultProviderCapabilityProfile SelectProfile(string profileName) {
    return profileName switch {
      "sqlite-v1" => DataVaultProviderCapabilityProfiles.Sqlite,
      "oracle-v1" => DataVaultProviderCapabilityProfiles.Oracle,
      "postgres-v1" => DataVaultProviderCapabilityProfiles.Postgres,
      "sqlserver-v1" => DataVaultProviderCapabilityProfiles.SqlServer,
      "db2-v1" => DataVaultProviderCapabilityProfiles.Db2,
      "mysql-pomelo-v1" => DataVaultProviderCapabilityProfiles.MySql,
      _ => throw new ArgumentOutOfRangeException(nameof(profileName), profileName, "Unsupported test profile."),
    };
  }

  private static string ExpectedHexStringStoreType(
      string profileName,
      int digestByteLength) {
    var hexLength = digestByteLength * 2;
    return profileName switch {
      "sqlite-v1" => "TEXT",
      "oracle-v1" => "VARCHAR2(" + hexLength.ToString(CultureInfo.InvariantCulture) + " CHAR)",
      "postgres-v1" or "mysql-pomelo-v1" => "varchar(" + hexLength.ToString(CultureInfo.InvariantCulture) + ")",
      "sqlserver-v1" => "nvarchar(" + hexLength.ToString(CultureInfo.InvariantCulture) + ")",
      "db2-v1" => "VARCHAR(" + hexLength.ToString(CultureInfo.InvariantCulture) + ")",
      _ => throw new ArgumentOutOfRangeException(nameof(profileName), profileName, "Unsupported test profile."),
    };
  }
}
