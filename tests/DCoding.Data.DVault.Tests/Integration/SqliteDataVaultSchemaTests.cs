using System.Text;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class SqliteDataVaultSchemaTests {
  private const string StableHashAlgorithmId = "sha256-128-v1";
  private const int StableHashDigestByteLength = 16;

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
        "BridgeCustomerOrder|BridgeSalesRegionHierarchy|HubCustomer|HubOrder|LinkCustomerOrder|SatCustomerContact|SatCustomerContactChannel|SatCustomerOrderState",
        TableNames(connection));
    AssertTable(
        connection,
        "BridgeCustomerOrder",
        ["CustomerHashKey", "OrderHashKey"],
        "PkBridgeCustomerOrderCustomerHashKeyOrderHashKey",
        ["CustomerHashKey", "OrderHashKey"],
        "IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey",
        ["OrderHashKey", "CustomerHashKey"],
        expectedIndexUnique: false);
    Assert.Equal(
        "IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey",
        IndexNames(connection, "BridgeCustomerOrder"));
    Assert.Equal("0", ForeignKeyCount(connection, "BridgeCustomerOrder"));
    AssertTable(
        connection,
        "BridgeSalesRegionHierarchy",
        ["AncestorSalesRegionHashKey", "DescendantSalesRegionHashKey", "TraversalDepth"],
        "PkBridgeSalesRegionHierarchyAncestorSalesRegionHashKeyDescendantSalesRegionHashKey",
        ["AncestorSalesRegionHashKey", "DescendantSalesRegionHashKey"],
        "IxBridgeSalesRegionHierarchyTraversalAncestorSalesRegionHashKeyTraversalDepth",
        ["AncestorSalesRegionHashKey", "TraversalDepth"],
        expectedIndexUnique: false);
    Assert.Equal(
        "IxBridgeSalesRegionHierarchyTraversalAncestorSalesRegionHashKeyTraversalDepth|" +
        "IxBridgeSalesRegionHierarchyTraversalDescendantSalesRegionHashKeyAncestorSalesRegionHashKey",
        IndexNames(connection, "BridgeSalesRegionHierarchy"));
    Assert.Equal(
        "0",
        IndexUniqueValue(
            connection,
            "BridgeSalesRegionHierarchy",
            "IxBridgeSalesRegionHierarchyTraversalDescendantSalesRegionHashKeyAncestorSalesRegionHashKey"));
    Assert.Equal(
        "DescendantSalesRegionHashKey|AncestorSalesRegionHashKey",
        IndexColumnNames(connection, "IxBridgeSalesRegionHierarchyTraversalDescendantSalesRegionHashKeyAncestorSalesRegionHashKey"));
    Assert.Equal("0", ForeignKeyCount(connection, "BridgeSalesRegionHierarchy"));
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
        "IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp",
        ["CustomerHashKey", "LoadTimestamp", "HashDiff"],
        expectedIndexUnique: false);
    AssertTable(
        connection,
        "SatCustomerContactChannel",
        ["CustomerHashKey", "ContactType", "RegionCode", "HashDiff", "LoadTimestamp", "RecordSource", "EmailAddress"],
        "PkSatCustomerContactChannelCustomerHashKeyContactTypeRegionCodeLoadTimestamp",
        ["CustomerHashKey", "ContactType", "RegionCode", "LoadTimestamp"],
        "IxSatCustomerContactChannelSatelliteParentCustomerHashKeyContactTypeRegionCodeLoadTimestamp",
        ["CustomerHashKey", "ContactType", "RegionCode", "LoadTimestamp", "HashDiff"],
        expectedIndexUnique: false);
    AssertTable(
        connection,
        "SatCustomerOrderState",
        ["CustomerOrderHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "StateCode"],
        "PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp",
        ["CustomerOrderHashKey", "LoadTimestamp"],
        "IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp",
        ["CustomerOrderHashKey", "LoadTimestamp", "HashDiff"],
        expectedIndexUnique: false);
  }

  [Theory]
  [InlineData(DataVaultHashKeyStorageProfile.HexString, "TEXT", DataVaultProviderValueFormat.LowercaseHexText, "none-string-model")]
  [InlineData(DataVaultHashKeyStorageProfile.Binary, "BLOB", DataVaultProviderValueFormat.LowercaseHexBinary, "lowercase-hex-string-to-bytes")]
  public void ApplyDataVaultMetadataSizesHashColumnsFromActiveHashStorageProfile(
      DataVaultHashKeyStorageProfile storageProfile,
      string expectedStoreType,
      DataVaultProviderValueFormat expectedValueFormat,
      string expectedConversionBehavior) {
    using var schemaDatabase = SqliteTestDatabase.CreateTemporaryFile();
    using var pitDatabase = SqliteTestDatabase.CreateTemporaryFile();
    var schemaOptions = new DbContextOptionsBuilder<TranslatedDataVaultSchemaContext>()
        .UseSqlite(CreateConnectionString(schemaDatabase))
        .ReplaceService<IModelCacheKeyFactory, StorageProfileSchemaModelCacheKeyFactory>()
        .Options;
    var pitOptions = new DbContextOptionsBuilder<PitDataVaultSchemaContext>()
        .UseSqlite(CreateConnectionString(pitDatabase))
        .ReplaceService<IModelCacheKeyFactory, StorageProfileSchemaModelCacheKeyFactory>()
        .Options;

    using var schemaContext = new TranslatedDataVaultSchemaContext(schemaOptions, storageProfile);
    using var pitContext = new PitDataVaultSchemaContext(pitOptions, storageProfile);

    AssertHashProperty(
        schemaContext.Model,
        "HubCustomer",
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.HashKey,
        storageProfile,
        expectedStoreType,
        expectedValueFormat,
        expectedConversionBehavior);
    AssertHashProperty(
        schemaContext.Model,
        "LinkCustomerOrder",
        "CustomerOrderHashKey",
        DataVaultLogicalPropertyKind.HashKey,
        storageProfile,
        expectedStoreType,
        expectedValueFormat,
        expectedConversionBehavior);
    AssertHashProperty(
        schemaContext.Model,
        "LinkCustomerOrder",
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference,
        storageProfile,
        expectedStoreType,
        expectedValueFormat,
        expectedConversionBehavior);
    AssertHashProperty(
        schemaContext.Model,
        "LinkCustomerOrder",
        "OrderHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference,
        storageProfile,
        expectedStoreType,
        expectedValueFormat,
        expectedConversionBehavior);
    AssertHashProperty(
        schemaContext.Model,
        "SatCustomerContact",
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.HashKey,
        storageProfile,
        expectedStoreType,
        expectedValueFormat,
        expectedConversionBehavior);
    AssertHashProperty(
        schemaContext.Model,
        "BridgeCustomerOrder",
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference,
        storageProfile,
        expectedStoreType,
        expectedValueFormat,
        expectedConversionBehavior);
    AssertHashProperty(
        schemaContext.Model,
        "BridgeCustomerOrder",
        "OrderHashKey",
        DataVaultLogicalPropertyKind.ParticipantReference,
        storageProfile,
        expectedStoreType,
        expectedValueFormat,
        expectedConversionBehavior);
    AssertHashProperty(
        pitContext.Model,
        "PitCustomerProfileStatus",
        "CustomerHashKey",
        DataVaultLogicalPropertyKind.HashKey,
        storageProfile,
        expectedStoreType,
        expectedValueFormat,
        expectedConversionBehavior);
  }

  [Fact]
  public void ApplyDataVaultMetadataCodeFirstCreatesSameSqliteSchemaAsMetadataBaseline() {
    using var metadataFirstDatabase = SqliteTestDatabase.CreateTemporaryFile();
    using var codeFirstDatabase = SqliteTestDatabase.CreateTemporaryFile();

    var metadataFirstOptions = new DbContextOptionsBuilder<MetadataFirstParityDataVaultSchemaContext>()
        .UseSqlite(CreateConnectionString(metadataFirstDatabase))
        .Options;
    var codeFirstOptions = new DbContextOptionsBuilder<CodeFirstParityDataVaultSchemaContext>()
        .UseSqlite(CreateConnectionString(codeFirstDatabase))
        .Options;

    using (var context = new MetadataFirstParityDataVaultSchemaContext(metadataFirstOptions)) {
      context.Database.EnsureCreated();
    }

    using (var context = new CodeFirstParityDataVaultSchemaContext(codeFirstOptions)) {
      context.Database.EnsureCreated();
    }

    using var metadataFirstConnection = metadataFirstDatabase.CreateOpenConnection();
    using var codeFirstConnection = codeFirstDatabase.CreateOpenConnection();

    Assert.Equal(
        CreateCanonicalSchemaSnapshot(metadataFirstConnection),
        CreateCanonicalSchemaSnapshot(codeFirstConnection));
    Assert.Equal(
        "HubCustomer|HubOrder|LinkCustomerOrder|SatCustomerContact|SatCustomerContactChannel",
        TableNames(codeFirstConnection));
    AssertTable(
        codeFirstConnection,
        "HubCustomer",
        ["CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerHashKeyValue", "CustomerId"],
        "PkHubCustomerCustomerHashKey",
        ["CustomerHashKey"],
        "IxHubCustomerBusinessKeyCustomerHashKeyValueCustomerId",
        ["CustomerHashKeyValue", "CustomerId"],
        expectedIndexUnique: true);
    AssertTable(
        codeFirstConnection,
        "SatCustomerContactChannel",
        ["CustomerHashKey", "ContactType", "RegionCode", "HashDiff", "LoadTimestamp", "RecordSource", "HashDiffValue", "RecordSourceValue"],
        "PkSatCustomerContactChannelCustomerHashKeyContactTypeRegionCodeLoadTimestamp",
        ["CustomerHashKey", "ContactType", "RegionCode", "LoadTimestamp"],
        "IxSatCustomerContactChannelSatelliteParentCustomerHashKeyContactTypeRegionCodeLoadTimestamp",
        ["CustomerHashKey", "ContactType", "RegionCode", "LoadTimestamp", "HashDiff"],
        expectedIndexUnique: false);
  }

  [Fact]
  public async Task ApplyDataVaultMetadataCreatesAndReadsBaselinePitSqliteTable() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();

    var options = new DbContextOptionsBuilder<PitDataVaultSchemaContext>()
        .UseSqlite(CreateConnectionString(database))
        .Options;
    var pitLoadTimestamp = new DateTimeOffset(2026, 5, 6, 10, 0, 0, TimeSpan.Zero);
    var profileLoadTimestamp = pitLoadTimestamp.AddMinutes(-5);
    var statusLoadTimestamp = pitLoadTimestamp.AddMinutes(-2);

    using (var context = new PitDataVaultSchemaContext(options)) {
      context.Database.EnsureCreated();

      context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(
          new Dictionary<string, object> {
            ["CustomerHashKey"] = "customer-hash",
            ["LoadTimestamp"] = pitLoadTimestamp,
            ["ProfileLoadTimestamp"] = profileLoadTimestamp,
            ["StatusLoadTimestamp"] = statusLoadTimestamp,
          });
      await context.SaveChangesAsync();
      context.ChangeTracker.Clear();

      var row = Assert.Single(await context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").AsNoTracking().ToListAsync());

      Assert.Equal("customer-hash", Assert.IsType<string>(row["CustomerHashKey"]));
      Assert.Equal(pitLoadTimestamp, Assert.IsType<DateTimeOffset>(row["LoadTimestamp"]));
      Assert.Equal(profileLoadTimestamp, Assert.IsType<DateTimeOffset>(row["ProfileLoadTimestamp"]));
      Assert.Equal(statusLoadTimestamp, Assert.IsType<DateTimeOffset>(row["StatusLoadTimestamp"]));
    }

    using var connection = database.CreateOpenConnection();

    AssertTable(
        connection,
        "PitCustomerProfileStatus",
        ["CustomerHashKey", "LoadTimestamp", "ProfileLoadTimestamp", "StatusLoadTimestamp"],
        "PkPitCustomerProfileStatusCustomerHashKeyLoadTimestamp",
        ["CustomerHashKey", "LoadTimestamp"],
        "IxPitCustomerProfileStatusTraversalCustomerHashKeyLoadTimestamp",
        ["CustomerHashKey", "LoadTimestamp"],
        expectedIndexUnique: false);
  }

  [Fact]
  public void ApplyDataVaultMetadataMatchesCommittedSqliteSchemaSnapshot() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();

    var options = new DbContextOptionsBuilder<SnapshotDataVaultSchemaContext>()
        .UseSqlite(CreateConnectionString(database))
        .Options;

    using (var context = new SnapshotDataVaultSchemaContext(options)) {
      context.Database.EnsureCreated();
    }

    using var connection = database.CreateOpenConnection();

    Assert.Equal(
        ReadSnapshot("SqliteDataVaultSchemaSnapshot.txt"),
        CreateCanonicalSchemaSnapshot(connection));
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
            "(name LIKE 'Hub%' OR name LIKE 'Link%' OR name LIKE 'Sat%' OR name LIKE 'Bridge%' OR name LIKE 'Pit%');"));
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

  private static void AssertTableWithoutSecondaryIndexes(
      SqliteTestConnection connection,
      string tableName,
      string[] expectedColumnNames,
      string expectedPrimaryKeyName,
      string[] expectedPrimaryKeyColumnNames) {
    var createSql = connection.ExecuteScalarString(
        "SELECT sql FROM sqlite_master WHERE type = 'table' AND name = " + SqlLiteral(tableName) + ";");

    Assert.NotNull(createSql);
    Assert.Contains("CONSTRAINT \"" + expectedPrimaryKeyName + "\"", createSql!, StringComparison.Ordinal);
    Assert.Contains("PRIMARY KEY", createSql!, StringComparison.Ordinal);
    Assert.Equal(string.Join("|", expectedColumnNames), ColumnNames(connection, tableName));
    Assert.Equal(string.Join("|", expectedPrimaryKeyColumnNames), PrimaryKeyColumnNames(connection, tableName));
    Assert.Null(IndexNames(connection, tableName));
  }

  private static string CreateCanonicalSchemaSnapshot(SqliteTestConnection connection) {
    var builder = new StringBuilder();
    builder.AppendLine("# DVault SQLite schema snapshot");
    builder.AppendLine("# Canonical table, column, primary key, and index metadata generated by ApplyDataVaultMetadata.");

    foreach (var tableName in SplitValues(TableNames(connection))) {
      builder.AppendLine();
      builder.AppendLine("table " + tableName);
      builder.AppendLine("  columns: " + FormatList(ColumnNames(connection, tableName)));
      builder.AppendLine(
          "  primary-key: " +
          PrimaryKeyName(connection, tableName) +
          " (" +
          FormatList(PrimaryKeyColumnNames(connection, tableName)) +
          ")");

      foreach (var indexName in SplitValues(IndexNames(connection, tableName))) {
        builder.AppendLine(
            "  index: " +
            indexName +
            " unique=" +
            FormatBoolean(IndexUniqueValue(connection, tableName, indexName)) +
            " (" +
            FormatList(IndexColumnNames(connection, indexName)) +
            ")");
      }
    }

    return NormalizeLineEndings(builder.ToString());
  }

  private static string CreateConnectionString(SqliteTestDatabase database) {
    return "Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False";
  }

  private static DataVaultProviderCapabilityProfile CreateSqliteProfile(DataVaultHashKeyStorageProfile storageProfile) {
    return DataVaultProviderCapabilityProfiles.Sqlite.WithHashKeyStorageProfile(
        storageProfile,
        StableHashAlgorithmId,
        StableHashDigestByteLength);
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
                "ContactChannel",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"],
                ["Contact Type", "Region Code"]),
            new DataVaultSatelliteMetadata(
                "State",
                DataVaultMetadataReference.Link("CustomerOrder"),
                ["State Code"]),
        ],
        [
            new DataVaultBridgeMetadata(
                "CustomerOrder",
                DataVaultBridgeKind.ManyToMany,
                DataVaultMetadataReference.Link("CustomerOrder"),
                [
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.From,
                        DataVaultMetadataReference.Hub("Customer"),
                        "Customer"),
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.To,
                        DataVaultMetadataReference.Hub("Order"),
                        "Order"),
                ]),
            new DataVaultBridgeMetadata(
                "SalesRegionHierarchy",
                DataVaultBridgeKind.Hierarchy,
                DataVaultMetadataReference.Link("SalesRegionParentChild"),
                [
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.Ancestor,
                        DataVaultMetadataReference.Hub("SalesRegion"),
                        "ParentRegion"),
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.Descendant,
                        DataVaultMetadataReference.Hub("SalesRegion"),
                        "ChildRegion"),
                ]),
        ]);
  }

  private static DataVaultMetadataModel CreateCodeFirstParityMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerHashKey", "CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);

    return new DataVaultMetadataModel(
        [customer, order],
        [new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()])],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                customer.ToReference(),
                ["LoadTimestamp", "EmailAddress"]),
            new DataVaultSatelliteMetadata(
                "ContactChannel",
                customer.ToReference(),
                ["HashDiff", "RecordSource"],
                ["ContactType", "RegionCode"]),
        ]);
  }

  private static void ConfigureCodeFirstParityModel(DataVaultCodeFirstModelBuilder vault) {
    vault.Hub<Customer>(hub => {
      hub.BusinessKey(customer => customer.CustomerHashKey);
      hub.BusinessKey(customer => customer.CustomerId);
      hub.Satellite("Contact", satellite => {
        satellite.Payload(customer => customer.LoadTimestamp);
        satellite.Payload(customer => customer.EmailAddress);
      });
      hub.Satellite("ContactChannel", satellite => {
        satellite.DrivingKey(customer => customer.ContactType);
        satellite.DrivingKey(customer => customer.RegionCode);
        satellite.Payload(customer => customer.HashDiff);
        satellite.Payload(customer => customer.RecordSource);
      });
    });
    vault.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));
    vault.Link("CustomerOrder", link => {
      link.Participant<Customer>();
      link.Participant<Order>();
    });
  }

  private static DataVaultMetadataModel CreateSnapshotMetadataModel() {
    return new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata("Customer", ["Customer Id"]),
            new DataVaultHubMetadata("Order", ["Order Id"]),
            new DataVaultHubMetadata("Sales Region", ["Country Code", "Region Code"]),
        ],
        [
            new DataVaultLinkMetadata(
                "CustomerOrderRegion",
                [
                    DataVaultMetadataReference.Hub("Customer"),
                    DataVaultMetadataReference.Hub("Order"),
                    DataVaultMetadataReference.Hub("Sales Region"),
                ]),
        ],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"]),
            new DataVaultSatelliteMetadata(
                "Fulfillment Status",
                DataVaultMetadataReference.Link("CustomerOrderRegion"),
                ["State Code"]),
        ],
        [
            new DataVaultBridgeMetadata(
                "CustomerOrder",
                DataVaultBridgeKind.ManyToMany,
                DataVaultMetadataReference.Link("CustomerOrder"),
                [
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.From,
                        DataVaultMetadataReference.Hub("Customer"),
                        "Customer"),
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.To,
                        DataVaultMetadataReference.Hub("Order"),
                        "Order"),
                ]),
            new DataVaultBridgeMetadata(
                "SalesRegionHierarchy",
                DataVaultBridgeKind.Hierarchy,
                DataVaultMetadataReference.Link("SalesRegionParentChild"),
                [
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.Ancestor,
                        DataVaultMetadataReference.Hub("SalesRegion"),
                        "ParentRegion"),
                    new DataVaultBridgeEndpointMetadata(
                        DataVaultBridgeEndpointRole.Descendant,
                        DataVaultMetadataReference.Hub("SalesRegion"),
                        "ChildRegion"),
                ]),
        ]);
  }

  private static DataVaultMetadataModel CreatePitMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        [
            new DataVaultSatelliteMetadata(
                "Profile",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"]),
            new DataVaultSatelliteMetadata(
                "Status",
                DataVaultMetadataReference.Hub("Customer"),
                ["Status Code"]),
        ],
        [new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile", "Status"])]);
  }

  private static string ReadSnapshot(string fileName) {
    var snapshotPath = Path.Combine(AppContext.BaseDirectory, "Snapshots", fileName);

    return NormalizeLineEndings(File.ReadAllText(snapshotPath));
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

  private static string PrimaryKeyName(SqliteTestConnection connection, string tableName) {
    var createSql = connection.ExecuteScalarString(
        "SELECT sql FROM sqlite_master WHERE type = 'table' AND name = " + SqlLiteral(tableName) + ";");

    Assert.NotNull(createSql);

    const string constraintPrefix = "CONSTRAINT \"";
    const string primaryKeySuffix = "\" PRIMARY KEY";

    var constraintNameStart = createSql!.IndexOf(constraintPrefix, StringComparison.Ordinal);
    if (constraintNameStart < 0) {
      throw new InvalidOperationException("The table " + tableName + " does not contain a named primary key constraint.");
    }

    var constraintNameEnd = createSql.IndexOf(
        primaryKeySuffix,
        constraintNameStart + constraintPrefix.Length,
        StringComparison.Ordinal);

    if (constraintNameEnd < 0) {
      throw new InvalidOperationException("The table " + tableName + " does not contain a named primary key constraint.");
    }

    return createSql.Substring(
        constraintNameStart + constraintPrefix.Length,
        constraintNameEnd - constraintNameStart - constraintPrefix.Length);
  }

  private static string? IndexNames(SqliteTestConnection connection, string tableName) {
    return connection.ExecuteScalarString(
        "SELECT group_concat(name, '|') FROM (" +
        "SELECT name FROM pragma_index_list(" + SqlLiteral(tableName) + ") " +
        "WHERE origin <> 'pk' ORDER BY name);");
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

  private static string? ForeignKeyCount(SqliteTestConnection connection, string tableName) {
    return connection.ExecuteScalarString(
        "SELECT count(*) FROM pragma_foreign_key_list(" + SqlLiteral(tableName) + ");");
  }

  private static string FormatList(string? values) {
    return string.Join(" | ", SplitValues(values));
  }

  private static string FormatBoolean(string? value) {
    return value == "1" ? "true" : "false";
  }

  private static string NormalizeLineEndings(string value) {
    return value.Replace("\r\n", "\n", StringComparison.Ordinal);
  }

  private static string[] SplitValues(string? values) {
    return values is null || values.Length == 0
        ? []
        : values.Split('|');
  }

  private static string SqlLiteral(string value) {
    return "'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }

  private static void AssertHashProperty(
      IModel model,
      string entityName,
      string propertyName,
      DataVaultLogicalPropertyKind expectedLogicalPropertyKind,
      DataVaultHashKeyStorageProfile expectedStorageProfile,
      string expectedStoreType,
      DataVaultProviderValueFormat expectedValueFormat,
      string expectedConversionBehavior) {
    var entity = model.GetEntityTypes().Single(entityType => string.Equals(entityType.Name, entityName, StringComparison.Ordinal));
    var property = entity.FindProperty(propertyName);

    Assert.NotNull(property);
    Assert.Equal(typeof(string), property!.ClrType);
    Assert.Equal(expectedLogicalPropertyKind, AnnotationValue<DataVaultLogicalPropertyKind>(
        property,
        DataVaultAnnotationNames.ProviderLogicalPropertyKind));
    Assert.Equal(expectedStoreType, property.GetColumnType());
    Assert.Equal(expectedStoreType, AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderStorageType));
    Assert.Equal(expectedValueFormat, AnnotationValue<DataVaultProviderValueFormat>(property, DataVaultAnnotationNames.ProviderValueFormat));
    Assert.Equal(expectedStorageProfile, AnnotationValue<DataVaultHashKeyStorageProfile>(
        property,
        DataVaultAnnotationNames.HashKeyStorageProfile));
    Assert.Equal(StableHashAlgorithmId, AnnotationValue<string>(property, DataVaultAnnotationNames.StableHashAlgorithmId));
    Assert.Equal(StableHashDigestByteLength, AnnotationValue<int>(property, DataVaultAnnotationNames.StableHashDigestByteLength));
    Assert.Equal("lowercase-hex-no-prefix", AnnotationValue<string>(property, DataVaultAnnotationNames.StableHashDigestEncoding));
    Assert.Equal(expectedConversionBehavior, AnnotationValue<string>(property, DataVaultAnnotationNames.HashKeyConversionBehavior));
    if (expectedStorageProfile == DataVaultHashKeyStorageProfile.Binary) {
      Assert.NotNull(property.GetValueConverter());
    }
    else {
      Assert.Null(property.GetValueConverter());
    }
  }

  private static T AnnotationValue<T>(IProperty property, string annotationName) {
    return Assert.IsType<T>(property.FindAnnotation(annotationName)?.Value);
  }

  private sealed class TranslatedDataVaultSchemaContext(
      DbContextOptions<TranslatedDataVaultSchemaContext> options,
      DataVaultHashKeyStorageProfile storageProfile = DataVaultHashKeyStorageProfile.HexString) : DbContext(options) {
    public DataVaultHashKeyStorageProfile StorageProfile { get; } = storageProfile;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel(), CreateSqliteProfile(StorageProfile));
    }
  }

  private sealed class MetadataFirstParityDataVaultSchemaContext(
      DbContextOptions<MetadataFirstParityDataVaultSchemaContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateCodeFirstParityMetadataModel());
    }
  }

  private sealed class CodeFirstParityDataVaultSchemaContext(
      DbContextOptions<CodeFirstParityDataVaultSchemaContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ConfigureCodeFirstParityModel);
    }
  }

  private sealed class SnapshotDataVaultSchemaContext(
      DbContextOptions<SnapshotDataVaultSchemaContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateSnapshotMetadataModel());
    }
  }

  private sealed class PitDataVaultSchemaContext(
      DbContextOptions<PitDataVaultSchemaContext> options,
      DataVaultHashKeyStorageProfile storageProfile = DataVaultHashKeyStorageProfile.HexString) : DbContext(options) {
    public DataVaultHashKeyStorageProfile StorageProfile { get; } = storageProfile;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreatePitMetadataModel(), CreateSqliteProfile(StorageProfile));
    }
  }

  private sealed class BareDataVaultSchemaContext(DbContextOptions<BareDataVaultSchemaContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.UseDataVault();
    }
  }

  private sealed class Customer {
    public string ContactType { get; init; } = string.Empty;

    public string CustomerHashKey { get; init; } = string.Empty;

    public string CustomerId { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;

    public string HashDiff { get; init; } = string.Empty;

    public string LoadTimestamp { get; init; } = string.Empty;

    public string RecordSource { get; init; } = string.Empty;

    public string RegionCode { get; init; } = string.Empty;
  }

  private sealed class Order {
    public string OrderId { get; init; } = string.Empty;
  }

  private sealed class StorageProfileSchemaModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context switch {
        TranslatedDataVaultSchemaContext schemaContext => (
            context.GetType(),
            schemaContext.StorageProfile,
            StableHashAlgorithmId,
            StableHashDigestByteLength,
            designTime),
        PitDataVaultSchemaContext pitContext => (
            context.GetType(),
            pitContext.StorageProfile,
            StableHashAlgorithmId,
            StableHashDigestByteLength,
            designTime),
        _ => (object)(context.GetType(), designTime),
      };
    }
  }
}
