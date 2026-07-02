using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class LiveSchemaReaderContractFixtureTests {
  [Fact]
  public void CanonicalMetadataScenarioUsesSharedCustomerOrderContactStateContract() {
    var metadataModel = LiveSchemaReaderContractFixture.CreateCanonicalMetadataModel();

    Assert.Equal(["Customer", "Order"], metadataModel.Hubs.Select(hub => hub.Name));
    Assert.Equal(["Customer Id"], metadataModel.Hubs[0].BusinessKeyNames);
    Assert.Equal(["Order Id"], metadataModel.Hubs[1].BusinessKeyNames);

    var link = Assert.Single(metadataModel.Links);
    Assert.Equal("CustomerOrder", link.Name);
    Assert.Equal(["Customer", "Order"], link.Endpoints.Select(endpoint => endpoint.Name));

    Assert.Equal(["Contact", "State"], metadataModel.Satellites.Select(satellite => satellite.Name));
    Assert.Equal("Customer", metadataModel.Satellites[0].Parent.Name);
    Assert.Equal(["Email Address"], metadataModel.Satellites[0].DescriptiveAttributeNames);
    Assert.Equal("CustomerOrder", metadataModel.Satellites[1].Parent.Name);
    Assert.Equal(["State Code"], metadataModel.Satellites[1].DescriptiveAttributeNames);
  }

  [Fact]
  public void ExpectedSqliteSnapshotDefinesDeterministicLiveSchemaContractSurface() {
    var snapshot = LiveSchemaReaderContractFixture.CreateExpectedSnapshot(DataVaultProviderCapabilityProfiles.Sqlite);

    Assert.Equal(
        [
            "table:HubCustomer",
            "columns:HubCustomer:0:CustomerHashKey:BLOB|1:LoadTimestamp:TEXT|2:RecordSource:TEXT|3:CustomerId:TEXT",
            "primary-key:HubCustomer:PkHubCustomerCustomerHashKey:CustomerHashKey",
            "index:HubCustomer:IxHubCustomerBusinessKeyCustomerId:True:CustomerId",
            "table:HubOrder",
            "columns:HubOrder:0:OrderHashKey:BLOB|1:LoadTimestamp:TEXT|2:RecordSource:TEXT|3:OrderId:TEXT",
            "primary-key:HubOrder:PkHubOrderOrderHashKey:OrderHashKey",
            "index:HubOrder:IxHubOrderBusinessKeyOrderId:True:OrderId",
            "table:LinkCustomerOrder",
            "columns:LinkCustomerOrder:0:CustomerOrderHashKey:BLOB|1:LoadTimestamp:TEXT|2:RecordSource:TEXT|3:CustomerHashKey:BLOB|4:OrderHashKey:BLOB",
            "primary-key:LinkCustomerOrder:PkLinkCustomerOrderCustomerOrderHashKey:CustomerOrderHashKey",
            "index:LinkCustomerOrder:IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey:False:CustomerHashKey|OrderHashKey",
            "table:SatCustomerContact",
            "columns:SatCustomerContact:0:CustomerHashKey:BLOB|1:HashDiff:TEXT|2:LoadTimestamp:TEXT|3:RecordSource:TEXT|4:EmailAddress:TEXT",
            "primary-key:SatCustomerContact:PkSatCustomerContactCustomerHashKeyLoadTimestamp:CustomerHashKey|LoadTimestamp",
            "index:SatCustomerContact:IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp:False:CustomerHashKey|LoadTimestamp|HashDiff",
            "table:SatCustomerOrderState",
            "columns:SatCustomerOrderState:0:CustomerOrderHashKey:BLOB|1:HashDiff:TEXT|2:LoadTimestamp:TEXT|3:RecordSource:TEXT|4:StateCode:TEXT",
            "primary-key:SatCustomerOrderState:PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp:CustomerOrderHashKey|LoadTimestamp",
            "index:SatCustomerOrderState:IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp:False:CustomerOrderHashKey|LoadTimestamp|HashDiff",
        ],
        LiveSchemaReaderContractFixture.CreateSnapshotSignatures(snapshot));
  }

  [Fact]
  public void ExpectedSnapshotsUseProviderStorageTypesWithoutChangingLogicalShape() {
    var sqlite = FindTable(
        LiveSchemaReaderContractFixture.CreateExpectedSnapshot(DataVaultProviderCapabilityProfiles.Sqlite),
        "HubCustomer");
    var postgres = FindTable(
        LiveSchemaReaderContractFixture.CreateExpectedSnapshot(DataVaultProviderCapabilityProfiles.Postgres),
        "HubCustomer");
    var sqlServer = FindTable(
        LiveSchemaReaderContractFixture.CreateExpectedSnapshot(DataVaultProviderCapabilityProfiles.SqlServer),
        "HubCustomer");
    var oracle = FindTable(
        LiveSchemaReaderContractFixture.CreateExpectedSnapshot(DataVaultProviderCapabilityProfiles.Oracle),
        "HubCustomer");
    var db2 = FindTable(
        LiveSchemaReaderContractFixture.CreateExpectedSnapshot(DataVaultProviderCapabilityProfiles.Db2),
        "HubCustomer");
    var mySql = FindTable(
        LiveSchemaReaderContractFixture.CreateExpectedSnapshot(DataVaultProviderCapabilityProfiles.MySql),
        "HubCustomer");

    Assert.Equal(["CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId"], sqlite.Columns.Select(column => column.ColumnName));
    Assert.Equal(["BLOB", "TEXT", "TEXT", "TEXT"], sqlite.Columns.Select(column => column.ProviderStorageType));
    Assert.Equal(["bytea", "timestamp with time zone", "varchar(255)", "varchar(255)"], postgres.Columns.Select(column => column.ProviderStorageType));
    Assert.Equal(["varbinary(32)", "datetimeoffset", "nvarchar(255)", "nvarchar(255)"], sqlServer.Columns.Select(column => column.ProviderStorageType));
    Assert.Equal(["RAW(32)", "VARCHAR2(33 CHAR)", "VARCHAR2(255 CHAR)", "VARCHAR2(255 CHAR)"], oracle.Columns.Select(column => column.ProviderStorageType));
    Assert.Equal(["VARBINARY(32)", "VARCHAR(33)", "VARCHAR(255)", "VARCHAR(255)"], db2.Columns.Select(column => column.ProviderStorageType));
    Assert.Equal(["varbinary(32)", "varchar(33)", "varchar(255)", "varchar(255)"], mySql.Columns.Select(column => column.ProviderStorageType));
  }

  [Theory]
  [InlineData("sqlite-v1", "TEXT", "BLOB")]
  [InlineData("oracle-v1", "VARCHAR2(32 CHAR)", "RAW(16)")]
  [InlineData("postgres-v1", "varchar(32)", "bytea")]
  [InlineData("sqlserver-v1", "nvarchar(32)", "varbinary(16)")]
  [InlineData("db2-v1", "VARCHAR(32)", "VARBINARY(16)")]
  [InlineData("mysql-pomelo-v1", "varchar(32)", "varbinary(16)")]
  public void ExpectedSnapshotsProjectHashStorageProfilesWithoutChangingLogicalShape(
      string profileName,
      string expectedHexStringStoreType,
      string expectedBinaryStoreType) {
    var hexStringProfile = SelectProfile(profileName).WithHashKeyStorageProfile(
        DataVaultHashKeyStorageProfile.HexString,
        "sha256-128-v1",
        16);
    var binaryProfile = SelectProfile(profileName).WithHashKeyStorageProfile(
        DataVaultHashKeyStorageProfile.Binary,
        "sha256-128-v1",
        16);
    var hexStringSnapshot = LiveSchemaReaderContractFixture.CreateExpectedSnapshot(hexStringProfile);
    var binarySnapshot = LiveSchemaReaderContractFixture.CreateExpectedSnapshot(binaryProfile);

    Assert.Equal(
        hexStringSnapshot.Tables.Select(table => table.TableName),
        binarySnapshot.Tables.Select(table => table.TableName));
    foreach (var pair in hexStringSnapshot.Tables.Zip(binarySnapshot.Tables)) {
      Assert.Equal(
          pair.First.Columns.Select(column => (column.Ordinal, column.ColumnName)),
          pair.Second.Columns.Select(column => (column.Ordinal, column.ColumnName)));
      Assert.Equal(pair.First.PrimaryKey.ConstraintName, pair.Second.PrimaryKey.ConstraintName);
      Assert.Equal(pair.First.PrimaryKey.ColumnNames, pair.Second.PrimaryKey.ColumnNames);
      Assert.Equal(
          pair.First.Indexes.Select(index => (index.IndexName, index.IsUnique, ColumnNames: string.Join("|", index.ColumnNames))),
          pair.Second.Indexes.Select(index => (index.IndexName, index.IsUnique, ColumnNames: string.Join("|", index.ColumnNames))));
    }

    Assert.All(
        HashKeyColumns(hexStringSnapshot),
        column => Assert.Equal(expectedHexStringStoreType, column.ProviderStorageType));
    Assert.All(
        HashKeyColumns(binarySnapshot),
        column => Assert.Equal(expectedBinaryStoreType, column.ProviderStorageType));
    Assert.Equal(
        [expectedHexStringStoreType, expectedHexStringStoreType, expectedHexStringStoreType],
        FindTable(hexStringSnapshot, "LinkCustomerOrder")
            .Columns
            .Where(column => column.ColumnName.EndsWith("HashKey", StringComparison.Ordinal))
            .Select(column => column.ProviderStorageType));
    Assert.Equal(
        [expectedBinaryStoreType, expectedBinaryStoreType, expectedBinaryStoreType],
        FindTable(binarySnapshot, "LinkCustomerOrder")
            .Columns
            .Where(column => column.ColumnName.EndsWith("HashKey", StringComparison.Ordinal))
            .Select(column => column.ProviderStorageType));
  }

  [Fact]
  public void ExpectedSnapshotsClassifyProviderSpecificSatelliteIndexColumnContracts() {
    Assert.Equal(
        ["CustomerHashKey", "LoadTimestamp", "HashDiff"],
        FindIndex(DataVaultProviderCapabilityProfiles.Sqlite, "SatCustomerContact").ColumnNames);
    Assert.Equal(
        ["CustomerHashKey", "LoadTimestamp"],
        FindIndex(DataVaultProviderCapabilityProfiles.Postgres, "SatCustomerContact").ColumnNames);
    Assert.Equal(
        ["CustomerHashKey", "LoadTimestamp"],
        FindIndex(DataVaultProviderCapabilityProfiles.SqlServer, "SatCustomerContact").ColumnNames);
    Assert.Equal(
        ["CustomerHashKey", "LoadTimestamp"],
        FindIndex(DataVaultProviderCapabilityProfiles.MySql, "SatCustomerContact").ColumnNames);
    Assert.Equal(
        ["CustomerHashKey", "LoadTimestamp", "HashDiff"],
        FindIndex(DataVaultProviderCapabilityProfiles.Oracle, "SatCustomerContact").ColumnNames);
    Assert.Equal(
        ["CustomerHashKey", "LoadTimestamp", "HashDiff"],
        FindIndex(DataVaultProviderCapabilityProfiles.Db2, "SatCustomerContact").ColumnNames);
  }

  [Fact]
  public void PhysicalIdentifierResolverMatchesProviderMaximumIdentifierLengthContract() {
    var producedName = "IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp";

    var physicalName = LiveSchemaReaderContractFixture.ResolvePhysicalIdentifierName(
        producedName,
        DataVaultProviderCapabilityProfiles.MySql);

    Assert.Equal(64, physicalName.Length);
    Assert.StartsWith("IxSatCustomerOrderStateSatelliteParentCustomerOrderHash", physicalName, StringComparison.Ordinal);
    Assert.NotEqual(producedName, physicalName);
    Assert.Equal(
        physicalName,
        LiveSchemaReaderContractFixture.ResolvePhysicalIdentifierName(
            producedName,
            DataVaultProviderCapabilityProfiles.MySql));
    Assert.Equal(
        producedName,
        LiveSchemaReaderContractFixture.ResolvePhysicalIdentifierName(
            producedName,
            DataVaultProviderCapabilityProfiles.Sqlite));

    var db2ProducedName = producedName +
        "WithAdditionalDb2SpecificBusinessQualifierThatExceedsTheOneHundredTwentyEightCharacterIdentifierLimit";
    var db2PhysicalName = LiveSchemaReaderContractFixture.ResolvePhysicalIdentifierName(
        db2ProducedName,
        DataVaultProviderCapabilityProfiles.Db2);

    Assert.Equal(128, db2PhysicalName.Length);
    Assert.NotEqual(db2ProducedName, db2PhysicalName);
    Assert.Equal(
        db2PhysicalName,
        LiveSchemaReaderContractFixture.ResolvePhysicalIdentifierName(
            db2ProducedName,
            DataVaultProviderCapabilityProfiles.Db2));
  }

  private static DataVaultLiveSchemaTable FindTable(DataVaultLiveSchemaSnapshot snapshot, string tableName) {
    return snapshot.Tables.Single(table => string.Equals(table.TableName, tableName, StringComparison.Ordinal));
  }

  private static DataVaultLiveSchemaIndex FindIndex(
      DataVaultProviderCapabilityProfile providerCapabilities,
      string tableName) {
    var table = FindTable(
        LiveSchemaReaderContractFixture.CreateExpectedSnapshot(providerCapabilities),
        tableName);

    return Assert.Single(table.Indexes);
  }

  private static DataVaultProviderCapabilityProfile SelectProfile(string profileName) {
    return profileName switch {
      "sqlite-v1" => DataVaultProviderCapabilityProfiles.Sqlite,
      "oracle-v1" => DataVaultProviderCapabilityProfiles.Oracle,
      "postgres-v1" => DataVaultProviderCapabilityProfiles.Postgres,
      "sqlserver-v1" => DataVaultProviderCapabilityProfiles.SqlServer,
      "db2-v1" => DataVaultProviderCapabilityProfiles.Db2,
      "mysql-pomelo-v1" => DataVaultProviderCapabilityProfiles.MySql,
      _ => throw new ArgumentOutOfRangeException(nameof(profileName), profileName, "Unknown provider profile."),
    };
  }

  private static IEnumerable<DataVaultLiveSchemaColumn> HashKeyColumns(DataVaultLiveSchemaSnapshot snapshot) {
    return snapshot.Tables.SelectMany(table => table.Columns)
        .Where(column => column.ColumnName.EndsWith("HashKey", StringComparison.Ordinal));
  }
}
