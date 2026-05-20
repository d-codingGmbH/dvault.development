using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultTelemetrySqliteTests {
  [Fact]
  public async Task TelemetryObserverReceivesProviderNeutralSaveAndReadSummariesThroughSqlite() {
    var observer = new CapturingTelemetryObserver();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<TelemetryReadWriteContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();
    services.AddSingleton<IDataVaultTelemetryObserver>(observer);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var metadata = CreateMetadata();
    string customerHashKey;

    await using (var context = new TelemetryReadWriteContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              new DateTimeOffset(2026, 5, 20, 8, 0, 0, TimeSpan.Zero),
              "crm-import",
              [new DataVaultHubSaveOperation(metadata.Customer, [new("Customer Id", "C-100")])],
              []));
      customerHashKey = Assert.Single(hubResult.SavedRecords).HashKey;

      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              new DateTimeOffset(2026, 5, 20, 8, 5, 0, TimeSpan.Zero),
              "crm-import",
              [],
              [],
              [new DataVaultSatelliteSaveOperation(
                  metadata.Profile,
                  customerHashKey,
                  [new("Name", "Alice Adams")],
                  "profile-hash-1")]));
    }

    await using (var context = new TelemetryReadWriteContext(options)) {
      var rows = await readService.ReadLatestSatelliteAsync(
          context,
          new DataVaultLatestSatelliteReadRequest(metadata.Profile, [customerHashKey]),
          row => row.RequiredString("Name"));

      Assert.Equal("Alice Adams", Assert.Single(rows));
    }

    Assert.Collection(
        observer.SaveSummaries,
        hubSave => {
          Assert.Equal(DataVaultTelemetryOutcome.Succeeded, hubSave.Outcome);
          Assert.Equal(DataVaultSaveTelemetryOperationKind.SingleRequest, hubSave.OperationKind);
          Assert.Equal(1, hubSave.HubOperationCount);
          Assert.Equal(1, hubSave.RowsWritten);
          Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback, hubSave.StrategyStatus);
          Assert.Contains(DataVaultSaveStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered, hubSave.FallbackCauseKinds);
        },
        satelliteSave => {
          Assert.Equal(DataVaultTelemetryOutcome.Succeeded, satelliteSave.Outcome);
          Assert.Equal(1, satelliteSave.SatelliteOperationCount);
          Assert.Equal(1, satelliteSave.RowsWritten);
          Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback, satelliteSave.StrategyStatus);
        });

    var readSummary = Assert.Single(observer.ReadSummaries);
    Assert.Equal(DataVaultTelemetryOutcome.Succeeded, readSummary.Outcome);
    Assert.Equal(DataVaultReadTelemetryFamily.LatestSatellite, readSummary.Family);
    Assert.Equal(1, readSummary.RequestedKeyCount);
    Assert.Equal(1, readSummary.ReturnedRowCount);
    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback, readSummary.StrategyStatus);
    Assert.Contains(DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered, readSummary.FallbackCauseKinds);
  }

  private static TelemetryMetadata CreateMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata("Profile", customer.ToReference(), ["Name"]);

    return new TelemetryMetadata(customer, profile);
  }

  private sealed class TelemetryReadWriteContext(DbContextOptions<TelemetryReadWriteContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      var metadata = CreateMetadata();
      modelBuilder.ApplyDataVaultMetadata(
          new DataVaultMetadataModel([metadata.Customer], [], [metadata.Profile]),
          DataVaultProviderCapabilityProfiles.Sqlite);
    }
  }

  private sealed class CapturingTelemetryObserver : IDataVaultTelemetryObserver {
    public List<DataVaultSaveTelemetrySummary> SaveSummaries { get; } = [];

    public List<DataVaultReadTelemetrySummary> ReadSummaries { get; } = [];

    public void RecordSave(DataVaultSaveTelemetrySummary summary) {
      SaveSummaries.Add(summary);
    }

    public void RecordRead(DataVaultReadTelemetrySummary summary) {
      ReadSummaries.Add(summary);
    }
  }

  private sealed record TelemetryMetadata(
      DataVaultHubMetadata Customer,
      DataVaultSatelliteMetadata Profile);
}
