using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class SqliteProviderCapabilityProfileTests {
  [Fact]
  public void SqliteProfileHexStringStorageDeclarationsWorkWithRawSqliteTextValues() {
    var profile = DataVaultProviderCapabilityProfiles.Sqlite.WithHashKeyStorageProfile(
        DataVaultHashKeyStorageProfile.HexString,
        "sha256-v1",
        32);
    var timestampMapping = profile.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.LoadTimestamp);
    var hashKeyMapping = profile.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.HashKey);

    Assert.Equal(DataVaultProviderValueFormat.Iso8601UtcText, timestampMapping.ValueFormat);
    Assert.Equal(DataVaultProviderValueFormat.LowercaseHexText, hashKeyMapping.ValueFormat);

    const string hashKeyValue = "0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdef";

    using var database = SqliteTestDatabase.CreateInMemory();
    using var connection = database.CreateOpenConnection();
    connection.ExecuteNonQuery(
        $"""
              CREATE TABLE vault_projection (
                load_timestamp {timestampMapping.NativeStoreType} NOT NULL,
                hash_key {hashKeyMapping.NativeStoreType} NOT NULL
              );

              INSERT INTO vault_projection (load_timestamp, hash_key)
              VALUES ('2026-04-29T10:15:00Z', '{hashKeyValue}');
              """);

    Assert.Equal("text", connection.ExecuteScalarString("SELECT typeof(load_timestamp) FROM vault_projection"));
    Assert.Equal("text", connection.ExecuteScalarString("SELECT typeof(hash_key) FROM vault_projection"));
    Assert.Equal("2026-04-29T10:15:00Z", connection.ExecuteScalarString("SELECT load_timestamp FROM vault_projection"));
    Assert.Equal(hashKeyValue, connection.ExecuteScalarString("SELECT hash_key FROM vault_projection"));
  }
}
