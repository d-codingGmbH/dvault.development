using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class SqliteLiveSchemaDriftTests {
  [Fact]
  public async Task ReadAsyncReturnsMatchingSqliteLiveSchemaWithNoDrift() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();

    var options = new DbContextOptionsBuilder<TranslatedDataVaultSchemaContext>()
        .UseSqlite(CreateConnectionString(database))
        .Options;

    await using var context = new TranslatedDataVaultSchemaContext(options);
    await context.Database.EnsureCreatedAsync();

    var readResult = await DataVaultLiveSchemaReader.ReadAsync(context);
    var report = DataVaultLiveSchemaDriftReporter.Compare(
        CreateMetadataModel(),
        readResult,
        DataVaultProviderCapabilityProfiles.Sqlite);

    Assert.Equal(DataVaultLiveSchemaReadStatus.Succeeded, readResult.Status);
    Assert.NotNull(readResult.Snapshot);
    Assert.Contains(readResult.Snapshot.Tables, table => table.TableName == "HubCustomer");
    Assert.False(report.HasBlockingDifferences);
    Assert.Empty(report.Differences);
    Assert.Equal("DVault model drift: no differences.", report.ToDisplayString());
  }

  [Fact]
  public void CompareReportsDeterministicLiveSchemaDifferences() {
    var liveSchema = new DataVaultLiveSchemaSnapshot(
        [
            new DataVaultLiveSchemaTable(
                "HubCustomer",
                [
                    new DataVaultLiveSchemaColumn("CustomerHashKey", 0, "TEXT"),
                    new DataVaultLiveSchemaColumn("LoadTimestamp", 1, "INTEGER"),
                    new DataVaultLiveSchemaColumn("SourceSystem", 2, "TEXT"),
                    new DataVaultLiveSchemaColumn("CustomerIdentifier", 3, "TEXT"),
                ],
                new DataVaultLiveSchemaPrimaryKey(
                    "PkHubCustomerWrong",
                    ["CustomerHashKey", "LoadTimestamp"]),
                [
                    new DataVaultLiveSchemaIndex(
                        "IxHubCustomerBusinessKeyCustomerId",
                        ["CustomerIdentifier"],
                        isUnique: false),
                ]),
        ]);

    var report = DataVaultLiveSchemaDriftReporter.Compare(CreateCustomerOnlyMetadataModel(), liveSchema);
    var repeatedReport = DataVaultLiveSchemaDriftReporter.Compare(CreateCustomerOnlyMetadataModel(), liveSchema);

    Assert.True(report.HasBlockingDifferences);
    Assert.Equal(
        report.Differences.Select(CreateDifferenceSignature).ToArray(),
        repeatedReport.Differences.Select(CreateDifferenceSignature).ToArray());
    Assert.Equal(
        report.Differences
            .OrderBy(difference => difference.ElementKind)
            .ThenBy(difference => difference.LogicalName, StringComparer.Ordinal)
            .ThenBy(difference => difference.ProducedName ?? string.Empty, StringComparer.Ordinal)
            .ThenBy(difference => difference.Code, StringComparer.Ordinal)
            .ThenBy(difference => difference.PropertyPath, StringComparer.Ordinal)
            .Select(CreateDifferenceSignature)
            .ToArray(),
        report.Differences.Select(CreateDifferenceSignature).ToArray());
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "live-column-storage-type-mismatch" &&
            difference.LogicalName == "Hub:Customer.LoadTimestamp" &&
            difference.ExpectedValue == "TEXT" &&
            difference.ActualValue == "INTEGER");
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "live-column-name-mismatch" &&
            difference.ProducedName == "RecordSource" &&
            difference.ActualValue == "SourceSystem");
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "live-primary-key-name-mismatch" &&
            difference.ExpectedValue == "PkHubCustomerCustomerHashKey" &&
            difference.ActualValue == "PkHubCustomerWrong");
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "live-primary-key-column-mismatch" &&
            difference.ExpectedValue == "CustomerHashKey" &&
            difference.ActualValue == "CustomerHashKey|LoadTimestamp");
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "live-index-column-mismatch" &&
            difference.ExpectedValue == "CustomerId" &&
            difference.ActualValue == "CustomerIdentifier");
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "live-index-uniqueness-mismatch" &&
            difference.ExpectedValue == bool.TrueString &&
            difference.ActualValue == bool.FalseString);
  }

  [Fact]
  public void CompareReportsRenamedLiveTableWithStableCode() {
    var liveSchema = new DataVaultLiveSchemaSnapshot(
        [
            new DataVaultLiveSchemaTable(
                "HubCustomerArchive",
                [
                    new DataVaultLiveSchemaColumn("CustomerHashKey", 0, "TEXT"),
                    new DataVaultLiveSchemaColumn("LoadTimestamp", 1, "TEXT"),
                    new DataVaultLiveSchemaColumn("RecordSource", 2, "TEXT"),
                    new DataVaultLiveSchemaColumn("CustomerId", 3, "TEXT"),
                ],
                new DataVaultLiveSchemaPrimaryKey("PkHubCustomerCustomerHashKey", ["CustomerHashKey"]),
                [new DataVaultLiveSchemaIndex("IxHubCustomerBusinessKeyCustomerId", ["CustomerId"], isUnique: true)]),
        ]);

    var report = DataVaultLiveSchemaDriftReporter.Compare(CreateCustomerOnlyMetadataModel(), liveSchema);

    var difference = Assert.Single(report.Differences);
    Assert.True(report.HasBlockingDifferences);
    Assert.Equal("live-table-name-mismatch", difference.Code);
    Assert.Equal(DataVaultModelDriftSeverity.Blocking, difference.Severity);
    Assert.Equal("HubCustomer", difference.ExpectedValue);
    Assert.Equal("HubCustomerArchive", difference.ActualValue);
  }

  [Fact]
  public async Task ReadAsyncReturnsUnsupportedProviderResultWithoutThrowing() {
    var options = new DbContextOptionsBuilder<UnsupportedProviderContext>().Options;
    await using var context = new UnsupportedProviderContext(options);

    var readResult = await DataVaultLiveSchemaReader.ReadAsync(context);
    var report = DataVaultLiveSchemaDriftReporter.Compare(CreateCustomerOnlyMetadataModel(), readResult);

    Assert.Equal(DataVaultLiveSchemaReadStatus.UnsupportedProvider, readResult.Status);
    Assert.Null(readResult.Snapshot);
    Assert.True(report.HasBlockingDifferences);
    Assert.Contains(
        report.Differences,
        difference => difference.Code == "live-schema-provider-unsupported" &&
            difference.ExpectedValue == DataVaultLiveSchemaReadStatus.Succeeded.ToString() &&
            difference.ActualValue == DataVaultLiveSchemaReadStatus.UnsupportedProvider.ToString());
  }

  [Fact]
  public async Task ReadAsyncReturnsUnavailableForUnopenableSqliteDatabase() {
    var missingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing");
    var databasePath = Path.Combine(missingDirectory, "dvault.db");
    var options = new DbContextOptionsBuilder<TranslatedDataVaultSchemaContext>()
        .UseSqlite("Data Source=" + databasePath + ";Pooling=False")
        .Options;
    await using var context = new TranslatedDataVaultSchemaContext(options);

    var readResult = await DataVaultLiveSchemaReader.ReadAsync(context);
    var report = DataVaultLiveSchemaDriftReporter.Compare(CreateMetadataModel(), readResult);

    Assert.Equal(DataVaultLiveSchemaReadStatus.Unavailable, readResult.Status);
    Assert.Null(readResult.Snapshot);
    Assert.True(report.HasBlockingDifferences);
    Assert.Contains(report.Differences, difference => difference.Code == "live-schema-unavailable");
  }

  private static string CreateConnectionString(SqliteTestDatabase database) {
    return "Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False";
  }

  private static DataVaultMetadataModel CreateCustomerOnlyMetadataModel() {
    return new DataVaultMetadataModel([new DataVaultHubMetadata("Customer", ["Customer Id"])], [], []);
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

  private static string CreateDifferenceSignature(DataVaultModelDriftDifference difference) {
    return string.Join(
        "|",
        difference.ElementKind,
        difference.LogicalName,
        difference.ProducedName ?? string.Empty,
        difference.Code,
        difference.PropertyPath,
        difference.ExpectedValue ?? string.Empty,
        difference.ActualValue ?? string.Empty);
  }

  private sealed class TranslatedDataVaultSchemaContext(DbContextOptions<TranslatedDataVaultSchemaContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());
    }
  }

  private sealed class UnsupportedProviderContext(DbContextOptions<UnsupportedProviderContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateCustomerOnlyMetadataModel());
    }
  }
}
