using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

public sealed class SqliteDataVaultSchemaTests {
  [Fact]
  public void ApplyDataVaultMetadataCreatesExpectedSqliteSchema() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();

    var options = new DbContextOptionsBuilder<TranslatedDataVaultSchemaContext>()
        .UseSqlite(CreateConnectionString(database))
        .Options;

    using (var context = new TranslatedDataVaultSchemaContext(options)) {
      context.Database.EnsureCreated();
    }

    using var connection = database.CreateOpenConnection();

    Assert.Equal(
        "HubCustomer|HubOrder|LinkCustomerOrder|SatCustomerContact|SatCustomerOrderState",
        TableNames(connection));
    AssertTable(
        connection,
        "HubCustomer",
        ["CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId"],
        "PkHubCustomerCustomerHashKey",
        ["CustomerHashKey"],
        "IxHubCustomerBusinessKeyCustomerId",
        ["CustomerId"],
        expectedIndexUnique: true);
    AssertTable(
        connection,
        "HubOrder",
        ["OrderHashKey", "LoadTimestamp", "RecordSource", "OrderId"],
        "PkHubOrderOrderHashKey",
        ["OrderHashKey"],
        "IxHubOrderBusinessKeyOrderId",
        ["OrderId"],
        expectedIndexUnique: true);
    AssertTable(
        connection,
        "LinkCustomerOrder",
        ["CustomerOrderHashKey", "LoadTimestamp", "RecordSource", "CustomerHashKey", "OrderHashKey"],
        "PkLinkCustomerOrderCustomerOrderHashKey",
        ["CustomerOrderHashKey"],
        "IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey",
        ["CustomerHashKey", "OrderHashKey"],
        expectedIndexUnique: false);
    AssertTable(
        connection,
        "SatCustomerContact",
        ["CustomerHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "EmailAddress"],
        "PkSatCustomerContactCustomerHashKeyLoadTimestamp",
        ["CustomerHashKey", "LoadTimestamp"],
        "IxSatCustomerContactSatelliteParentCustomerHashKey",
        ["CustomerHashKey"],
        expectedIndexUnique: false);
    AssertTable(
        connection,
        "SatCustomerOrderState",
        ["CustomerOrderHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "StateCode"],
        "PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp",
        ["CustomerOrderHashKey", "LoadTimestamp"],
        "IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKey",
        ["CustomerOrderHashKey"],
        expectedIndexUnique: false);
  }

  [Fact]
  public void UseDataVaultAloneDoesNotCreateDataVaultTablesInSqlite() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();

    var options = new DbContextOptionsBuilder<BareDataVaultSchemaContext>()
        .UseSqlite(CreateConnectionString(database))
        .Options;

    using (var context = new BareDataVaultSchemaContext(options)) {
      context.Database.EnsureCreated();
    }

    using var connection = database.CreateOpenConnection();

    Assert.Equal(
        "0",
        connection.ExecuteScalarString(
            "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND " +
            "(name LIKE 'Hub%' OR name LIKE 'Link%' OR name LIKE 'Sat%');"));
  }

  private static void AssertTable(
      SqliteTestConnection connection,
      string tableName,
      string[] expectedColumnNames,
      string expectedPrimaryKeyName,
      string[] expectedPrimaryKeyColumnNames,
      string expectedIndexName,
      string[] expectedIndexColumnNames,
      bool expectedIndexUnique) {
    var createSql = connection.ExecuteScalarString(
        "SELECT sql FROM sqlite_master WHERE type = 'table' AND name = " + SqlLiteral(tableName) + ";");

    Assert.NotNull(createSql);
    Assert.Contains("CONSTRAINT \"" + expectedPrimaryKeyName + "\"", createSql!, StringComparison.Ordinal);
    Assert.Contains("PRIMARY KEY", createSql!, StringComparison.Ordinal);
    Assert.Equal(string.Join("|", expectedColumnNames), ColumnNames(connection, tableName));
    Assert.Equal(string.Join("|", expectedPrimaryKeyColumnNames), PrimaryKeyColumnNames(connection, tableName));
    Assert.Equal(expectedIndexUnique ? "1" : "0", IndexUniqueValue(connection, tableName, expectedIndexName));
    Assert.Equal(string.Join("|", expectedIndexColumnNames), IndexColumnNames(connection, expectedIndexName));
  }

  private static string CreateConnectionString(SqliteTestDatabase database) {
    return "Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False";
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    return new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata("Customer", ["Customer Id"]),
            new DataVaultHubMetadata("Order", ["Order Id"]),
        ],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]),
        ],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"]),
            new DataVaultSatelliteMetadata(
                "State",
                DataVaultMetadataReference.Link("CustomerOrder"),
                ["State Code"]),
        ]);
  }

  private static string? TableNames(SqliteTestConnection connection) {
    return connection.ExecuteScalarString(
        "SELECT group_concat(name, '|') FROM (" +
        "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%' ORDER BY name);");
  }

  private static string? ColumnNames(SqliteTestConnection connection, string tableName) {
    return connection.ExecuteScalarString(
        "SELECT group_concat(name, '|') FROM (" +
        "SELECT name FROM pragma_table_info(" + SqlLiteral(tableName) + ") ORDER BY cid);");
  }

  private static string? PrimaryKeyColumnNames(SqliteTestConnection connection, string tableName) {
    return connection.ExecuteScalarString(
        "SELECT group_concat(name, '|') FROM (" +
        "SELECT name FROM pragma_table_info(" + SqlLiteral(tableName) + ") WHERE pk > 0 ORDER BY pk);");
  }

  private static string? IndexUniqueValue(SqliteTestConnection connection, string tableName, string indexName) {
    return connection.ExecuteScalarString(
        "SELECT \"unique\" FROM pragma_index_list(" + SqlLiteral(tableName) + ") " +
        "WHERE name = " + SqlLiteral(indexName) + ";");
  }

  private static string? IndexColumnNames(SqliteTestConnection connection, string indexName) {
    return connection.ExecuteScalarString(
        "SELECT group_concat(name, '|') FROM (" +
        "SELECT name FROM pragma_index_info(" + SqlLiteral(indexName) + ") ORDER BY seqno);");
  }

  private static string SqlLiteral(string value) {
    return "'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }

  private sealed class TranslatedDataVaultSchemaContext(DbContextOptions<TranslatedDataVaultSchemaContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());
    }
  }

  private sealed class BareDataVaultSchemaContext(DbContextOptions<BareDataVaultSchemaContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.UseDataVault();
    }
  }
}
