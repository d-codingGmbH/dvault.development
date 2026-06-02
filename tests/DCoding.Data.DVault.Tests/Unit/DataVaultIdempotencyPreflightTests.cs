using DCoding.Data.DVault.Modeling;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultIdempotencyPreflightTests {
  [Fact]
  public void ComparePassesForTranslatedHubLinkSatellitePitAndBridgeBaseline() {
    var metadataModel = CreateFullMetadataModel();
    var expectedStructures = DataVaultIdempotencyPreflight.CreateExpectedStructures(
        metadataModel,
        DataVaultProviderCapabilityProfiles.Sqlite);
    var liveSchemaReadResult = DataVaultLiveSchemaReadResult.Success(
        "Microsoft.EntityFrameworkCore.Sqlite",
        CreateLiveSchema(expectedStructures));

    var report = DataVaultIdempotencyPreflight.Compare(
        metadataModel,
        liveSchemaReadResult,
        DataVaultProviderCapabilityProfiles.Sqlite);

    Assert.Equal(DataVaultIdempotencyPreflightStatus.Passed, report.Status);
    Assert.False(report.IsBlocked);
    Assert.Empty(report.Findings);
    Assert.Contains(report.ExpectedStructures, structure => structure.TableName == "HubCustomer" && structure.OperationFamily == "hub-save-idempotency");
    Assert.Contains(report.ExpectedStructures, structure => structure.TableName == "LinkCustomerOrder" && structure.OperationFamily == "link-save-idempotency");
    Assert.Contains(report.ExpectedStructures, structure => structure.TableName == "SatCustomerProfile" && structure.OperationFamily == "satellite-latest-state");
    Assert.Contains(report.ExpectedStructures, structure => structure.TableName == "PitCustomerProfileStatus" && structure.OperationFamily == "pit-as-of-read");
    Assert.Contains(report.ExpectedStructures, structure => structure.TableName == "BridgeCustomerOrder" && structure.OperationFamily == "bridge-traversal-maintenance");
    Assert.Contains("DVault idempotency preflight: passed", report.ToDisplayString(), StringComparison.Ordinal);
  }

  [Fact]
  public void CompareBlocksForMissingAndMismatchedIdempotencyStructures() {
    var metadataModel = CreateCustomerOnlyMetadataModel();
    var liveSchema = new DataVaultLiveSchemaSnapshot(
        [
            new DataVaultLiveSchemaTable(
                "HubCustomer",
                [
                    new DataVaultLiveSchemaColumn("CustomerHashKey", 0, "TEXT"),
                    new DataVaultLiveSchemaColumn("CustomerId", 1, "TEXT"),
                ],
                new DataVaultLiveSchemaPrimaryKey(
                    "PkHubCustomerWrong",
                    ["CustomerHashKey", "CustomerId"]),
                [
                    new DataVaultLiveSchemaIndex(
                        "IxHubCustomerBusinessKeyCustomerId",
                        ["CustomerHashKey"],
                        isUnique: false),
                ]),
        ]);

    var report = DataVaultIdempotencyPreflight.Compare(metadataModel, liveSchema);

    Assert.Equal(DataVaultIdempotencyPreflightStatus.Blocked, report.Status);
    Assert.True(report.IsBlocked);
    Assert.Contains(
        report.Findings,
        finding => finding.Code == "idempotency-primary-key-name-mismatch" &&
            finding.TableName == "HubCustomer" &&
            finding.OperationFamily == "hub-save-idempotency");
    Assert.Contains(
        report.Findings,
        finding => finding.Code == "idempotency-primary-key-column-mismatch" &&
            finding.ExpectedValue == "CustomerHashKey" &&
            finding.ActualValue == "CustomerHashKey|CustomerId");
    Assert.Contains(
        report.Findings,
        finding => finding.Code == "idempotency-index-column-mismatch" &&
            finding.StructureKind == "secondary-index");
    Assert.Contains(
        report.Findings,
        finding => finding.Code == "idempotency-index-uniqueness-mismatch" &&
            finding.ExpectedValue == bool.TrueString &&
            finding.ActualValue == bool.FalseString);
  }

  [Fact]
  public void CompareSurfacesUnsupportedAndUnavailableLiveSchemaWithoutProviderExceptionDetails() {
    var metadataModel = CreateCustomerOnlyMetadataModel();
    var unsupported = DataVaultIdempotencyPreflight.Compare(
        metadataModel,
        DataVaultLiveSchemaReadResult.UnsupportedProvider("Unit.Unsupported"));
    var unavailable = DataVaultIdempotencyPreflight.Compare(
        metadataModel,
        DataVaultLiveSchemaReadResult.Unavailable("Microsoft.EntityFrameworkCore.Sqlite", "Data Source=/tmp/raw-secret.db failed"));

    Assert.Equal(DataVaultIdempotencyPreflightStatus.UnsupportedProvider, unsupported.Status);
    Assert.True(unsupported.IsBlocked);
    Assert.Contains(unsupported.Findings, finding => finding.Code == "idempotency-live-schema-provider-unsupported");
    Assert.Equal(DataVaultIdempotencyPreflightStatus.UnavailableLiveSchema, unavailable.Status);
    Assert.True(unavailable.IsBlocked);
    Assert.Contains(unavailable.Findings, finding => finding.Code == "idempotency-live-schema-unavailable");
    Assert.DoesNotContain("raw-secret", unavailable.ToDisplayString(), StringComparison.Ordinal);
  }

  [Fact]
  public void ExpectedStructuresHonorIncludedColumnAndRedundantIndexProviderCaveats() {
    var metadataModel = CreateFullMetadataModel();

    var sqliteStructures = DataVaultIdempotencyPreflight.CreateExpectedStructures(
        metadataModel,
        DataVaultProviderCapabilityProfiles.Sqlite);
    var mySqlStructures = DataVaultIdempotencyPreflight.CreateExpectedStructures(
        metadataModel,
        DataVaultProviderCapabilityProfiles.MySql);
    var oracleStructures = DataVaultIdempotencyPreflight.CreateExpectedStructures(
        metadataModel,
        DataVaultProviderCapabilityProfiles.Oracle);

    var sqliteSatelliteIndex = FindStructure(sqliteStructures, "SatCustomerProfile", "secondary-index");
    Assert.Equal(["CustomerHashKey", "LoadTimestamp", "HashDiff"], sqliteSatelliteIndex.ColumnNames);
    Assert.Empty(sqliteSatelliteIndex.IncludedColumnNames);
    Assert.Equal(["LoadTimestamp"], sqliteSatelliteIndex.DescendingColumnNames);

    var mySqlSatelliteIndex = FindStructure(mySqlStructures, "SatCustomerProfile", "secondary-index");
    Assert.Equal(["CustomerHashKey", "LoadTimestamp"], mySqlSatelliteIndex.ColumnNames);
    Assert.Empty(mySqlSatelliteIndex.IncludedColumnNames);
    Assert.Equal(["LoadTimestamp"], mySqlSatelliteIndex.DescendingColumnNames);

    Assert.Contains(sqliteStructures, structure => structure.TableName == "PitCustomerProfileStatus" && structure.Kind == "secondary-index");
    Assert.DoesNotContain(oracleStructures, structure => structure.TableName == "PitCustomerProfileStatus" && structure.Kind == "secondary-index");
  }

  private static DataVaultIdempotencyPreflightStructure FindStructure(
      IReadOnlyList<DataVaultIdempotencyPreflightStructure> structures,
      string tableName,
      string kind) {
    return structures.Single(structure =>
        string.Equals(structure.TableName, tableName, StringComparison.Ordinal) &&
        string.Equals(structure.Kind, kind, StringComparison.Ordinal));
  }

  private static DataVaultLiveSchemaSnapshot CreateLiveSchema(
      IReadOnlyList<DataVaultIdempotencyPreflightStructure> expectedStructures) {
    var tables = expectedStructures
        .GroupBy(structure => structure.TableName, StringComparer.Ordinal)
        .Select(group => {
          var primaryKey = group.Single(structure => structure.Kind == "primary-key");
          var indexes = group
              .Where(structure => structure.Kind == "secondary-index")
              .Select(structure => new DataVaultLiveSchemaIndex(
                  structure.Name,
                  structure.ColumnNames,
                  structure.IsUnique,
                  structure.DescendingColumnNames,
                  structure.IncludedColumnNames));
          var columns = group
              .SelectMany(structure => structure.ColumnNames.Concat(structure.IncludedColumnNames))
              .Distinct(StringComparer.Ordinal)
              .Select((columnName, ordinal) => new DataVaultLiveSchemaColumn(columnName, ordinal, "TEXT"));

          return new DataVaultLiveSchemaTable(
              group.Key,
              columns,
              new DataVaultLiveSchemaPrimaryKey(primaryKey.Name, primaryKey.ColumnNames),
              indexes);
        });

    return new DataVaultLiveSchemaSnapshot(tables);
  }

  private static DataVaultMetadataModel CreateCustomerOnlyMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        []);
  }

  private static DataVaultMetadataModel CreateFullMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var profile = new DataVaultSatelliteMetadata("Profile", customer.ToReference(), ["Customer Name"]);
    var status = new DataVaultSatelliteMetadata("Status", customer.ToReference(), ["Status Code"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference());

    return new DataVaultMetadataModel(
        [customer, order],
        [customerOrder],
        [profile, status],
        Array.Empty<DataVaultPointInTimeMetadata>(),
        [bridge],
        [pit]);
  }
}
