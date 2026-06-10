using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class LiveSchemaReaderContractOutcomeTests {
  [Fact]
  public async Task CompareAsyncUsesExplicitReaderSuccessContractWithoutDrift() {
    await using var context = new ContractContext(new DbContextOptionsBuilder<ContractContext>().Options);
    var reader = new StubLiveSchemaReader(
        DataVaultLiveSchemaReadResult.Success(
            "Microsoft.EntityFrameworkCore.Sqlite",
            LiveSchemaReaderContractFixture.CreateExpectedSnapshot(DataVaultProviderCapabilityProfiles.Sqlite)));

    var report = await DataVaultLiveSchemaDriftReporter.CompareAsync(
        LiveSchemaReaderContractFixture.CreateCanonicalMetadataModel(),
        context,
        reader);

    Assert.False(report.HasBlockingDifferences);
    Assert.Empty(report.Differences);
    Assert.Equal("DVault model drift: no differences.", report.ToDisplayString());
  }

  [Fact]
  public void CompareUsesProviderProjectedPhysicalIdentifiersForExpectedLiveSchema() {
    var metadataModel = new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata(
                "CustomerAccountWithExtremelyVerboseProviderIdentifierPreflightProjectionName",
                ["Customer Business Identifier With Extremely Verbose Provider Identifier Preflight Column Name"]),
        ],
        [],
        []);
    var modelBuilder = new ModelBuilder();
    modelBuilder.ApplyDataVaultMetadata(metadataModel, DataVaultProviderCapabilityProfiles.MySql);
    var liveSchema = CreateLiveSchemaSnapshot(modelBuilder.Model);

    var report = DataVaultLiveSchemaDriftReporter.Compare(
        metadataModel,
        liveSchema,
        DataVaultProviderCapabilityProfiles.MySql);

    var liveTable = Assert.Single(liveSchema.Tables);
    Assert.Contains("_", liveTable.TableName, StringComparison.Ordinal);
    Assert.True(liveTable.TableName.Length <= 64);
    Assert.Contains(liveTable.Columns, column => column.ColumnName.Contains('_', StringComparison.Ordinal));
    Assert.Empty(report.Differences);
  }

  [Fact]
  public async Task CompareAsyncClassifiesUnsupportedReaderResultWithoutThrowing() {
    await using var context = new ContractContext(new DbContextOptionsBuilder<ContractContext>().Options);
    var reader = new StubLiveSchemaReader(DataVaultLiveSchemaReadResult.UnsupportedProvider("Contract.Unsupported"));

    var report = await DataVaultLiveSchemaDriftReporter.CompareAsync(
        LiveSchemaReaderContractFixture.CreateCustomerOnlyMetadataModel(),
        context,
        reader);

    Assert.True(report.HasBlockingDifferences);
    var difference = Assert.Single(report.Differences);
    Assert.Equal("live-schema-provider-unsupported", difference.Code);
    Assert.Equal(DataVaultLiveSchemaReadStatus.Succeeded.ToString(), difference.ExpectedValue);
    Assert.Equal(DataVaultLiveSchemaReadStatus.UnsupportedProvider.ToString(), difference.ActualValue);
  }

  [Fact]
  public async Task Db2LiveSchemaBoundaryIsExplicitlyUnsupportedUntilAReaderExists() {
    await using var context = new ContractContext(new DbContextOptionsBuilder<ContractContext>().Options);
    var reader = new UnsupportedDataVaultLiveSchemaReader(DataVaultLiveSchemaReader.Db2ProviderName);

    var readResult = await reader.ReadAsync(context);

    Assert.True(DataVaultLiveSchemaReader.IsExplicitlyUnsupportedProviderName(DataVaultLiveSchemaReader.Db2ProviderName));
    Assert.Equal(DataVaultLiveSchemaReadStatus.UnsupportedProvider, readResult.Status);
    Assert.Equal("IBM.EntityFrameworkCore", readResult.ProviderName);
    Assert.Null(readResult.Snapshot);
    Assert.Contains("IBM.EntityFrameworkCore", readResult.Message, StringComparison.Ordinal);
  }

  [Fact]
  public async Task CompareAsyncClassifiesUnavailableReaderResultWithoutThrowing() {
    await using var context = new ContractContext(new DbContextOptionsBuilder<ContractContext>().Options);
    var reader = new StubLiveSchemaReader(
        DataVaultLiveSchemaReadResult.Unavailable("Contract.Unavailable", "catalog connection failed"));

    var report = await DataVaultLiveSchemaDriftReporter.CompareAsync(
        LiveSchemaReaderContractFixture.CreateCustomerOnlyMetadataModel(),
        context,
        reader);

    Assert.True(report.HasBlockingDifferences);
    var difference = Assert.Single(report.Differences);
    Assert.Equal("live-schema-unavailable", difference.Code);
    Assert.Equal(DataVaultLiveSchemaReadStatus.Succeeded.ToString(), difference.ExpectedValue);
    Assert.Equal(DataVaultLiveSchemaReadStatus.Unavailable.ToString(), difference.ActualValue);
    Assert.Contains("catalog connection failed", difference.Message, StringComparison.Ordinal);
  }

  private sealed class ContractContext(DbContextOptions<ContractContext> options) : DbContext(options);

  private static DataVaultLiveSchemaSnapshot CreateLiveSchemaSnapshot(IReadOnlyModel model) {
    var table = Assert.Single(model.GetEntityTypes());
    var tableName = table.GetTableName()!;
    var tableIdentifier = StoreObjectIdentifier.Table(tableName, table.GetSchema());
    var primaryKey = table.FindPrimaryKey()!;

    return new DataVaultLiveSchemaSnapshot(
        [
            new DataVaultLiveSchemaTable(
                tableName,
                table.GetProperties()
                    .OrderBy(property => property.GetColumnOrder() ?? 0)
                    .Select(property => new DataVaultLiveSchemaColumn(
                        property.GetColumnName(tableIdentifier)!,
                        property.GetColumnOrder() ?? 0,
                        property.GetColumnType()!))
                    .ToArray(),
                new DataVaultLiveSchemaPrimaryKey(
                    primaryKey.GetName()!,
                    primaryKey.Properties
                        .Select(property => property.GetColumnName(tableIdentifier)!)
                        .ToArray()),
                table.GetIndexes()
                    .Select(index => new DataVaultLiveSchemaIndex(
                        index.GetDatabaseName()!,
                        index.Properties
                            .Select(property => property.GetColumnName(tableIdentifier)!)
                            .ToArray(),
                        index.IsUnique))
                    .ToArray()),
        ]);
  }

  private sealed class StubLiveSchemaReader(DataVaultLiveSchemaReadResult result) : IDataVaultLiveSchemaReader {
    public Task<DataVaultLiveSchemaReadResult> ReadAsync(
        DbContext dbContext,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(dbContext);

      return Task.FromResult(result);
    }
  }
}
