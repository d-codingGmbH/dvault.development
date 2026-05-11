using System.Globalization;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultPitReadServiceSqliteTests {
  [Theory]
  [InlineData(DataVaultLoadTimestampStorage.ProviderDefault)]
  [InlineData(DataVaultLoadTimestampStorage.Iso8601UtcText)]
  [InlineData(DataVaultLoadTimestampStorage.UtcTicks)]
  public async Task PitReadMaterializesLatestVisibleRowsAndMissingSnapshotsAcrossStorageModesThroughSqlite(
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    var metadata = CreateMetadata();
    var importTimestamp = new DateTimeOffset(2026, 5, 11, 8, 0, 0, TimeSpan.Zero);
    var statusTimestamp = new DateTimeOffset(2026, 5, 11, 9, 59, 0, TimeSpan.Zero);
    var profileTimestamp = new DateTimeOffset(2026, 5, 11, 10, 58, 0, TimeSpan.Zero);
    var olderPitTimestamp = new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero);
    var selectedPitTimestamp = new DateTimeOffset(2026, 5, 11, 11, 0, 0, TimeSpan.Zero);
    var asOf = new DateTimeOffset(2026, 5, 11, 12, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<PitReadContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, PitReadModelCacheKeyFactory>()
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    string customerHashKey;

    await using (var context = new PitReadContext(options, loadTimestampStorage)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              importTimestamp,
              "crm-import",
              [new(metadata.Customer, [new("Customer Id", "C-100")])],
              []));
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              statusTimestamp,
              "crm-import",
              [],
              [],
              [
                  new(
                      metadata.Profile,
                      customerHashKey,
                      [new("Customer Name", "Alice Adams"), new("Customer Tier", "Gold")],
                      "profile-hash-1"),
                  new(
                      metadata.Status,
                      customerHashKey,
                      [new("Status Code", "Active")],
                      "status-hash-1"),
              ]));
      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              profileTimestamp,
              "crm-change",
              [],
              [],
              [
                  new(
                      metadata.Profile,
                      customerHashKey,
                      [new("Customer Name", "Alice Baker"), new("Customer Tier", "Platinum")],
                      "profile-hash-2"),
              ]));

      var pitRows = context.Set<Dictionary<string, object>>("PitCustomerProfileStatus");
      pitRows.Add(CreatePitRow(
          loadTimestampStorage,
          customerHashKey,
          olderPitTimestamp,
          statusTimestamp,
          statusTimestamp));
      pitRows.Add(CreatePitRow(
          loadTimestampStorage,
          customerHashKey,
          selectedPitTimestamp,
          profileTimestamp,
          statusSnapshotTimestamp: null));
      await context.SaveChangesAsync();
    }

    await using (var context = new PitReadContext(options, loadTimestampStorage)) {
      var request = new DataVaultPitAsOfReadRequest(
          metadata.Pit,
          [customerHashKey, "customer-hash-missing"],
          asOf);
      var records = await readService.ReadPitRowsAsync(context, request);
      var projectedRows = await readService.ReadPitAsync(
          context,
          request,
          row => {
            var profile = row.RequiredSatellite("Profile");
            var status = row.OptionalSatellite("Status");

            return new CustomerSnapshotRead(
                row.RequiredString("ParentHashKey"),
                row.RequiredDateTimeOffset("LoadTimestamp"),
                profile.RequiredString("Customer Name"),
                profile.RequiredString("Customer Tier"),
                status?.NullableString("Status Code"));
          });
      var emptyRows = await readService.ReadPitRowsAsync(
          context,
          new DataVaultPitAsOfReadRequest(
              metadata.Pit,
              [customerHashKey],
              olderPitTimestamp.AddMinutes(-1)));

      var record = Assert.Single(records);
      Assert.Equal(customerHashKey, record.ParentHashKey);
      Assert.Equal(selectedPitTimestamp, record.LoadTimestamp);
      Assert.True(record.SatelliteSnapshotsByName.ContainsKey("Profile"));
      Assert.True(record.SatelliteSnapshotsByName.ContainsKey("Status"));
      Assert.Collection(
          record.SatelliteSnapshots,
          profile => {
            Assert.Equal("Profile", profile.SatelliteName);
            Assert.Equal(0, profile.Ordinal);
            Assert.True(profile.IsPresent);
            Assert.Equal(profileTimestamp, profile.SnapshotLoadTimestamp);
            Assert.Equal("profile-hash-2", profile.HashDiff);
            Assert.Equal("crm-change", profile.RecordSource);
            Assert.Equal("Alice Baker", profile.PayloadValues["Customer Name"]);
            Assert.Equal("Platinum", profile.PayloadValues["Customer Tier"]);
          },
          status => {
            Assert.Equal("Status", status.SatelliteName);
            Assert.Equal(1, status.Ordinal);
            Assert.False(status.IsPresent);
            Assert.Null(status.SnapshotLoadTimestamp);
            Assert.Null(status.HashDiff);
            Assert.Null(status.RecordSource);
            Assert.Empty(status.PayloadValues);
          });

      var projectedRow = Assert.Single(projectedRows);
      Assert.Equal(customerHashKey, projectedRow.ParentHashKey);
      Assert.Equal(selectedPitTimestamp, projectedRow.LoadTimestamp);
      Assert.Equal("Alice Baker", projectedRow.CustomerName);
      Assert.Equal("Platinum", projectedRow.CustomerTier);
      Assert.Null(projectedRow.StatusCode);
      Assert.Empty(emptyRows);
    }
  }

  private static Dictionary<string, object> CreatePitRow(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      string parentHashKey,
      DateTimeOffset pitLoadTimestamp,
      DateTimeOffset profileSnapshotTimestamp,
      DateTimeOffset? statusSnapshotTimestamp) {
    return new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = parentHashKey,
      ["LoadTimestamp"] = ToStoredTimestamp(loadTimestampStorage, pitLoadTimestamp),
      ["ProfileLoadTimestamp"] = ToStoredTimestamp(loadTimestampStorage, profileSnapshotTimestamp),
      ["StatusLoadTimestamp"] = statusSnapshotTimestamp.HasValue
            ? ToStoredTimestamp(loadTimestampStorage, statusSnapshotTimestamp.Value)
            : null!,
    };
  }

  private static object ToStoredTimestamp(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      DateTimeOffset timestamp) {
    var utcTimestamp = timestamp.ToUniversalTime();
    return loadTimestampStorage switch {
      DataVaultLoadTimestampStorage.Iso8601UtcText => utcTimestamp.ToString("O", CultureInfo.InvariantCulture),
      DataVaultLoadTimestampStorage.UtcTicks => utcTimestamp.UtcDateTime.Ticks,
      _ => utcTimestamp,
    };
  }

  private static PitReadMetadata CreateMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
      ["Customer Name", "Customer Tier"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
      ["Status Code"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
    var model = new DataVaultMetadataModel([customer], [], [profile, status], [pit]);

    return new PitReadMetadata(customer, profile, status, pit, model);
  }

  private static string GetHashKey(
      DataVaultSaveResult result,
      DataVaultTableKind kind,
      string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && record.MetadataName == metadataName)
        .HashKey;
  }

  private sealed class PitReadContext(
      DbContextOptions<PitReadContext> options,
      DataVaultLoadTimestampStorage loadTimestampStorage) : DbContext(options) {
    public DataVaultLoadTimestampStorage LoadTimestampStorage { get; } = loadTimestampStorage;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateMetadata().Model,
          DataVaultProviderCapabilityProfiles.Sqlite,
          LoadTimestampStorage);
    }
  }

  private sealed class PitReadModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is PitReadContext pitReadContext
          ? (context.GetType(), pitReadContext.LoadTimestampStorage, designTime)
          : (object)(context.GetType(), designTime);
    }
  }

  private sealed record PitReadMetadata(
      DataVaultHubMetadata Customer,
      DataVaultSatelliteMetadata Profile,
      DataVaultSatelliteMetadata Status,
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record CustomerSnapshotRead(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      string CustomerName,
      string CustomerTier,
      string? StatusCode);
}
