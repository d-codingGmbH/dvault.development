using System.Runtime.CompilerServices;
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

  [Fact]
  public async Task GeneratedMapperHelpersPersistSupportedRegistryBackedShapesThroughSqlite() {
    var loadTimestamp = new DateTimeOffset(2026, 5, 10, 11, 0, 0, TimeSpan.Zero);
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
    var hubMapper = GeneratedCustomerSourceDataVaultHubMapping.CreateMapper();
    var orderMapper = GeneratedOrderSourceDataVaultHubMapping.CreateMapper();
    var linkMapper = GeneratedCustomerOrderSourceDataVaultLinkMapping.CreateMapper();
    var profileMapper = GeneratedCustomerProfileSourceDataVaultHubSatelliteMapping.CreateMapper();
    var contactMapper = GeneratedCustomerContactSourceDataVaultHubSatelliteMapping.CreateMapper();

    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TypedMapperSaveServiceContext>();
    await context.Database.EnsureCreatedAsync();

    var hubResult = await saveService.SaveHubAsync(
        context,
        new GeneratedCustomerSource("C-200", "US"),
        hubMapper,
        loadTimestamp,
        "generated-import");
    var customerHashKey = Assert.Single(hubResult.SavedRecords).HashKey;
    var orderResult = await saveService.SaveHubAsync(
        context,
        new GeneratedOrderSource("O-900"),
        orderMapper,
        loadTimestamp,
        "generated-import");
    var orderHashKey = Assert.Single(orderResult.SavedRecords).HashKey;

    var linkResult = await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            loadTimestamp.AddMinutes(1),
            "generated-import",
            [],
            [linkMapper.Map(new GeneratedCustomerOrderSource(customerHashKey, orderHashKey))]));
    var profileResult = await saveService.SaveOrdinaryHubSatelliteAsync(
        context,
        new GeneratedCustomerProfileSource(
            customerHashKey,
            "Bob Brown",
            "prospect",
            "generated-profile-hash"),
        profileMapper,
        loadTimestamp.AddMinutes(2),
        "generated-import");
    var contactResult = await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            loadTimestamp.AddMinutes(3),
            "generated-import",
            [],
            [],
            [contactMapper.Map(new GeneratedCustomerContactSource(
                customerHashKey,
                "billing",
                "US",
                "billing-us@example.test",
                "generated-contact-hash"))]));

    var profileRows = await readService.ReadLatestSatelliteRowsAsync(
        context,
        new DataVaultRegistryLatestSatelliteReadRequest(
            DataVaultMetadataReference.Hub("Customer"),
            "Profile",
            [customerHashKey]));
    var profileRow = Assert.Single(profileRows);
    var contactRows = await readService.ReadLatestSatelliteRowsAsync(
        context,
        new DataVaultRegistryLatestSatelliteReadRequest(
            DataVaultMetadataReference.Hub("Customer"),
            "ContactChannel",
            [customerHashKey]));
    var contactRow = Assert.Single(contactRows);

    Assert.Equal(1, hubResult.RowsWritten);
    Assert.Equal(1, orderResult.RowsWritten);
    Assert.Equal(1, linkResult.RowsWritten);
    Assert.Equal(1, profileResult.RowsWritten);
    Assert.Equal(1, contactResult.RowsWritten);
    Assert.Equal("generated-import", profileRow.RecordSource);
    Assert.Equal("Bob Brown", profileRow.PayloadValues["customer_name"]);
    Assert.Equal("prospect", profileRow.PayloadValues["customer_status"]);
    Assert.Equal("generated-import", contactRow.RecordSource);
    Assert.Equal("billing", contactRow.DrivingKeyValues["Contact Type"]);
    Assert.Equal("US", contactRow.DrivingKeyValues["Region Code"]);
    Assert.Equal("billing-us@example.test", contactRow.PayloadValues["Email Address"]);
  }

  [Fact]
  public async Task GeneratedSameHubLinkMapperPersistsRoleBearingParticipantsThroughExplicitSaveHelper() {
    var loadTimestamp = new DateTimeOffset(2026, 6, 26, 9, 0, 0, TimeSpan.Zero);
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
    var customerMapper = GeneratedCustomerSourceDataVaultHubMapping.CreateMapper();
    var sameHubLinkMapper = GeneratedCustomerIdentityMatchSourceDataVaultLinkMapping.CreateMapper();

    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TypedMapperSaveServiceContext>();
    await context.Database.EnsureCreatedAsync();

    var customerResult = await saveService.SaveHubsAsync(
        context,
        [
            new GeneratedCustomerSource("C-500", "US"),
            new GeneratedCustomerSource("C-501", "US"),
        ],
        customerMapper,
        loadTimestamp,
        "generated-same-hub-import");
    var customerHashKeys = customerResult.SavedRecords.Select(record => record.HashKey).ToArray();
    var linkSource = new GeneratedCustomerIdentityMatchSource(customerHashKeys[0], customerHashKeys[1]);
    var linkOperation = sameHubLinkMapper.Map(linkSource);

    var linkResult = await saveService.SaveLinkAsync(
        context,
        linkSource,
        sameHubLinkMapper,
        loadTimestamp.AddMinutes(1),
        "generated-same-hub-import");
    var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerIdentityMatch").AsNoTracking().SingleAsync();

    Assert.Equal(2, customerResult.RowsWritten);
    Assert.Equal("CustomerIdentityMatch", linkOperation.LinkName);
    Assert.Equal(customerHashKeys[0], linkOperation.ParticipantHashKeyValues["SourceCustomer"]);
    Assert.Equal(customerHashKeys[1], linkOperation.ParticipantHashKeyValues["MatchedCustomer"]);
    Assert.Equal(1, linkResult.RowsWritten);
    Assert.Equal("generated-same-hub-import", linkRow["RecordSource"]);
    Assert.Equal(customerHashKeys[0], linkRow["SourceCustomerHashKey"]);
    Assert.Equal(customerHashKeys[1], linkRow["MatchedCustomerHashKey"]);
    Assert.Matches("^[0-9a-f]{64}$", Assert.IsType<string>(linkRow["CustomerIdentityMatchHashKey"]));
  }

  [Fact]
  public async Task TypedAsyncSaveHelpersPersistSupportedMapperShapesThroughSqlite() {
    var loadTimestamp = new DateTimeOffset(2026, 5, 30, 12, 0, 0, TimeSpan.Zero);
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
    var customerMapper = GeneratedCustomerSourceDataVaultHubMapping.CreateMapper();
    var orderMapper = GeneratedOrderSourceDataVaultHubMapping.CreateMapper();
    var linkMapper = GeneratedCustomerOrderSourceDataVaultLinkMapping.CreateMapper();
    var profileMapper = GeneratedCustomerProfileSourceDataVaultHubSatelliteMapping.CreateMapper();

    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TypedMapperSaveServiceContext>();
    await context.Database.EnsureCreatedAsync();

    var customerResult = await saveService.SaveHubsAsync(
        context,
        CreateAsyncSources([
            new GeneratedCustomerSource("C-300", "DE"),
            new GeneratedCustomerSource("C-400", "FR"),
        ]),
        customerMapper,
        loadTimestamp,
        "typed-async-import",
        1);
    var orderResult = await saveService.SaveHubsAsync(
        context,
        CreateAsyncSources([
            new GeneratedOrderSource("O-300"),
            new GeneratedOrderSource("O-400"),
        ]),
        orderMapper,
        loadTimestamp,
        "typed-async-import",
        2);
    var customerHashKeys = customerResult.SavedRecords.Select(record => record.HashKey).ToArray();
    var orderHashKeys = orderResult.SavedRecords.Select(record => record.HashKey).ToArray();

    var linkResult = await saveService.SaveLinksAsync(
        context,
        CreateAsyncSources([
            new GeneratedCustomerOrderSource(customerHashKeys[0], orderHashKeys[0]),
            new GeneratedCustomerOrderSource(customerHashKeys[1], orderHashKeys[1]),
        ]),
        linkMapper,
        loadTimestamp.AddMinutes(1),
        "typed-async-import",
        2);
    var profileResult = await saveService.SaveOrdinaryHubSatellitesAsync(
        context,
        CreateAsyncSources([
            new GeneratedCustomerProfileSource(
                customerHashKeys[0],
                "Carol Clark",
                "active",
                "async-profile-hash-1"),
            new GeneratedCustomerProfileSource(
                customerHashKeys[1],
                "Dana Diaz",
                "prospect",
                "async-profile-hash-2"),
        ]),
        profileMapper,
        loadTimestamp.AddMinutes(2),
        "typed-async-import",
        1);
    var linkRows = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().ToListAsync();
    var profileRows = await readService.ReadLatestSatelliteRowsAsync(
        context,
        new DataVaultRegistryLatestSatelliteReadRequest(
            DataVaultMetadataReference.Hub("Customer"),
            "Profile",
            customerHashKeys));

    Assert.Equal(2, customerResult.RowsWritten);
    Assert.Equal(2, orderResult.RowsWritten);
    Assert.Equal(2, linkResult.RowsWritten);
    Assert.Equal(2, profileResult.RowsWritten);
    Assert.Equal(2, linkRows.Count);
    Assert.Equal(["Carol Clark", "Dana Diaz"], profileRows.Select(row => row.PayloadValues["customer_name"]).ToArray());
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id", "Region Code"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);

    return new DataVaultMetadataModel(
        [customer, order],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [customer.ToReference(), order.ToReference()]),
            CreateCustomerIdentityMatchLinkMetadata(),
        ],
        [
            new DataVaultSatelliteMetadata(
                "Profile",
                DataVaultMetadataReference.Hub("Customer"),
                ["customer_name", "customer_status"]),
            new DataVaultSatelliteMetadata(
                "ContactChannel",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"],
                ["Contact Type", "Region Code"]),
        ]);
  }

  private static DataVaultLinkMetadata CreateCustomerIdentityMatchLinkMetadata() {
    var customer = DataVaultMetadataReference.Hub("Customer");

    return new DataVaultLinkMetadata(
        "CustomerIdentityMatch",
        [
            new DataVaultLinkParticipantMetadata(customer, "SourceCustomer"),
            new DataVaultLinkParticipantMetadata(customer, "MatchedCustomer"),
        ]);
  }

  private static async IAsyncEnumerable<T> CreateAsyncSources<T>(
      IReadOnlyList<T> sources,
      [EnumeratorCancellation] CancellationToken cancellationToken = default) {
    foreach (var source in sources) {
      cancellationToken.ThrowIfCancellationRequested();
      await Task.Yield();
      yield return source;
    }
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
