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

    AssertMapping(profile, DataVaultLogicalPropertyKind.HashKey, typeof(string), DataVaultProviderValueFormat.Text);
    AssertMapping(profile, DataVaultLogicalPropertyKind.HashDiff, typeof(string), DataVaultProviderValueFormat.Text);
    AssertMapping(profile, DataVaultLogicalPropertyKind.LoadTimestamp, typeof(DateTimeOffset), DataVaultProviderValueFormat.Iso8601UtcText);
    AssertMapping(profile, DataVaultLogicalPropertyKind.RecordSource, typeof(string), DataVaultProviderValueFormat.Text);
    AssertMapping(profile, DataVaultLogicalPropertyKind.ParticipantReference, typeof(string), DataVaultProviderValueFormat.Text);
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
        DataVaultProviderValueFormat.Text);
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
        DataVaultProviderValueFormat.Text);
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
        DataVaultProviderValueFormat.Text);
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
        DataVaultProviderValueFormat.Text);
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
}
