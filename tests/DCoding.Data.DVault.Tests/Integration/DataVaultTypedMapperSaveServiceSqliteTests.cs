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

  private static DataVaultMetadataModel CreateMetadataModel() {
    return new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata("Customer", ["Customer Id", "Region Code"]),
            new DataVaultHubMetadata("Order", ["Order Id"]),
        ],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]),
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

[DataVaultHubMapping("Customer")]
[DataVaultBusinessKeyBinding(0, "Customer Id", nameof(CustomerId))]
[DataVaultBusinessKeyBinding(1, "Region Code", nameof(RegionCode))]
internal sealed record GeneratedCustomerSource(string CustomerId, string RegionCode);

[DataVaultHubMapping("Order")]
[DataVaultBusinessKeyBinding(0, "Order Id", nameof(OrderId))]
internal sealed record GeneratedOrderSource(string OrderId);

[DataVaultLinkMapping("CustomerOrder")]
[DataVaultLinkParticipantBinding(0, "Customer", nameof(CustomerHashKey))]
[DataVaultLinkParticipantBinding(1, "Order", nameof(OrderHashKey))]
internal sealed record GeneratedCustomerOrderSource(string CustomerHashKey, string OrderHashKey);

[DataVaultHubSatelliteMapping("Customer", "Profile")]
[DataVaultSatelliteParentHashKeyBinding(nameof(CustomerHashKey))]
[DataVaultSatellitePayloadBinding(0, "customer_name", nameof(CustomerName))]
[DataVaultSatellitePayloadBinding(1, "customer_status", nameof(CustomerStatus))]
[DataVaultSatelliteHashDiffBinding(nameof(HashDiff))]
internal sealed record GeneratedCustomerProfileSource(
    string CustomerHashKey,
    string CustomerName,
    string CustomerStatus,
    string HashDiff);

[DataVaultHubSatelliteMapping("Customer", "ContactChannel")]
[DataVaultSatelliteParentHashKeyBinding(nameof(CustomerHashKey))]
[DataVaultSatelliteDrivingKeyBinding(0, "Contact Type", nameof(ContactType))]
[DataVaultSatelliteDrivingKeyBinding(1, "Region Code", nameof(RegionCode))]
[DataVaultSatellitePayloadBinding(0, "Email Address", nameof(EmailAddress))]
[DataVaultSatelliteHashDiffBinding(nameof(HashDiff))]
internal sealed record GeneratedCustomerContactSource(
    string CustomerHashKey,
    string ContactType,
    string RegionCode,
    string EmailAddress,
    string HashDiff);
