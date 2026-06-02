using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class SqliteIdempotencyPreflightTests {
  [Fact]
  public async Task CompareAsyncPassesAgainstCreatedSqliteSchema() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<TranslatedDataVaultSchemaContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;

    await using var context = new TranslatedDataVaultSchemaContext(options);
    await context.Database.EnsureCreatedAsync();

    var report = await DataVaultIdempotencyPreflight.CompareAsync(CreateMetadataModel(), context);

    Assert.Equal(DataVaultIdempotencyPreflightStatus.Passed, report.Status);
    Assert.False(report.IsBlocked);
    Assert.Empty(report.Findings);
    Assert.Contains(
        report.ExpectedStructures,
        structure => structure.TableName == "SatCustomerContact" &&
            structure.Kind == "secondary-index" &&
            structure.ColumnNames.SequenceEqual(["CustomerHashKey", "LoadTimestamp", "HashDiff"]) &&
            structure.DescendingColumnNames.SequenceEqual(["LoadTimestamp"]));
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    return LiveSchemaReaderContractFixture.CreateCanonicalMetadataModel();
  }

  private sealed class TranslatedDataVaultSchemaContext(DbContextOptions<TranslatedDataVaultSchemaContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());
    }
  }
}
