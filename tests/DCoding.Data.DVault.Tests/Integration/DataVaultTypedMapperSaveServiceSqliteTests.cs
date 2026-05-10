using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultTypedMapperSaveServiceSqliteTests {
  [Fact]
  public async Task TypedSaveHelpersPersistHubThenOrdinarySatelliteThroughSqlite() {
    var loadTimestamp = new DateTimeOffset(2026, 5, 10, 10, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataModel(CreateMetadataModel()));
    services.AddDVaultSqlite();
    services.AddDbContext<TypedMapperSaveServiceContext>(
        options => options
            .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
            .UseDataVaultMetadata());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var hubMapper = new CustomerHubMapper();
    var profileMapper = new CustomerProfileSatelliteMapper();

    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TypedMapperSaveServiceContext>();
    await context.Database.EnsureCreatedAsync();

    var hubResult = await saveService.SaveHubAsync(
        context,
        new CustomerSource("C-100", "DE"),
        hubMapper,
        loadTimestamp,
        "typed-import");
    var customerHashKey = Assert.Single(hubResult.SavedRecords).HashKey;

    var profileResult = await saveService.SaveOrdinaryHubSatelliteAsync(
        context,
        new CustomerProfileSource(
            customerHashKey,
            "Alice Adams",
            "active",
            "profile-hash"),
        profileMapper,
        loadTimestamp.AddMinutes(1),
        "typed-import");
    var latestRows = await readService.ReadLatestSatelliteRowsAsync(
        context,
        new DataVaultRegistryLatestSatelliteReadRequest(
            DataVaultMetadataReference.Hub("Customer"),
            "Profile",
            [customerHashKey]));
    var latestRow = Assert.Single(latestRows);

    Assert.Equal(1, hubResult.RowsWritten);
    Assert.Equal(1, profileResult.RowsWritten);
    Assert.Equal("typed-import", latestRow.RecordSource);
    Assert.Equal("Alice Adams", latestRow.PayloadValues["customer_name"]);
    Assert.Equal("active", latestRow.PayloadValues["customer_status"]);
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id", "Region Code"])],
        [],
        [
            new DataVaultSatelliteMetadata(
                "Profile",
                DataVaultMetadataReference.Hub("Customer"),
                ["customer_name", "customer_status"]),
        ]);
  }

  private sealed class CustomerHubMapper : IDataVaultHubMapper<CustomerSource> {
    public DataVaultRegistryHubSaveOperation Map(CustomerSource source) {
      ArgumentNullException.ThrowIfNull(source);

      return new DataVaultRegistryHubSaveOperation(
          "Customer",
          [
              new("Customer Id", source.CustomerId),
              new("Region Code", source.RegionCode),
          ]);
    }
  }

  private sealed class CustomerProfileSatelliteMapper : IDataVaultSatelliteMapper<CustomerProfileSource> {
    public DataVaultRegistrySatelliteSaveOperation Map(CustomerProfileSource source) {
      ArgumentNullException.ThrowIfNull(source);

      return new DataVaultRegistrySatelliteSaveOperation(
          DataVaultMetadataReference.Hub("Customer"),
          "Profile",
          source.CustomerHashKey,
          [
              new("customer_name", source.CustomerName),
              new("customer_status", source.CustomerStatus),
          ],
          source.HashDiff);
    }
  }

  private sealed class TypedMapperSaveServiceContext(
      DbContextOptions<TypedMapperSaveServiceContext> options) : DbContext(options) {
  }

  private sealed record CustomerSource(string CustomerId, string RegionCode);

  private sealed record CustomerProfileSource(
      string CustomerHashKey,
      string CustomerName,
      string CustomerStatus,
      string HashDiff);
}
