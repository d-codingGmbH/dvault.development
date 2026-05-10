using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultTypedSatelliteReadServiceSqliteTests {
  [Fact]
  public async Task TypedReadProjectsExplicitRegistryAndLinkParentRowsThroughSqlite() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["customer_name", "customer_status"]);
    var state = new DataVaultSatelliteMetadata(
        "State",
        customerOrder.ToReference(),
        ["State Code"]);
    var metadataModel = new DataVaultMetadataModel(
        [customer, order],
        [customerOrder],
        [profile, state]);
    var firstLoadTimestamp = new DateTimeOffset(2026, 5, 10, 8, 0, 0, TimeSpan.Zero);
    var secondLoadTimestamp = firstLoadTimestamp.AddHours(1);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataModel(metadataModel));
    services.AddDVaultSqlite();
    services.AddDbContext<TypedRegistryReadContext>(
        options => options
            .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
            .UseDataVaultMetadata());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();

    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TypedRegistryReadContext>();
    await context.Database.EnsureCreatedAsync();

    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            firstLoadTimestamp,
            "crm-import",
            [
                new("Customer", [new("Customer Id", "C-100")]),
                new("Order", [new("Order Id", "O-200")]),
            ],
            []));
    var customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
    var orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");

    var linkResult = await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            firstLoadTimestamp,
            "crm-import",
            [],
            [
                new("CustomerOrder", [new("Customer", customerHashKey), new("Order", orderHashKey)]),
            ]));
    var customerOrderHashKey = GetHashKey(linkResult, DataVaultTableKind.Link, "CustomerOrder");

    await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            firstLoadTimestamp,
            "crm-import",
            [],
            [],
            [
                new(
                    DataVaultMetadataReference.Hub("Customer"),
                    "Profile",
                    customerHashKey,
                    [new("customer_name", "Alice Adams"), new("customer_status", "prospect")],
                    "profile-hash-1"),
                new(
                    DataVaultMetadataReference.Link("CustomerOrder"),
                    "State",
                    customerOrderHashKey,
                    [new("State Code", "submitted")],
                    "state-hash-1"),
            ]));
    await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            secondLoadTimestamp,
            "crm-change",
            [],
            [],
            [
                new(
                    DataVaultMetadataReference.Hub("Customer"),
                    "Profile",
                    customerHashKey,
                    [new("customer_name", "Alice Baker"), new("customer_status", "active")],
                    "profile-hash-2"),
                new(
                    DataVaultMetadataReference.Link("CustomerOrder"),
                    "State",
                    customerOrderHashKey,
                    [new("State Code", "fulfilled")],
                    "state-hash-2"),
            ]));

    var explicitLatestRows = await readService.ReadLatestSatelliteAsync(
        context,
        new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey]),
        ProjectProfile);
    var registryAsOfRows = await readService.ReadLatestSatelliteAsync(
        context,
        new DataVaultRegistryLatestSatelliteReadRequest(
            DataVaultMetadataReference.Hub("Customer"),
            "Profile",
            [customerHashKey],
            firstLoadTimestamp),
        ProjectProfile);
    var linkParentRows = await readService.ReadLatestSatelliteAsync(
        context,
        new DataVaultRegistryLatestSatelliteReadRequest(
            DataVaultMetadataReference.Link("CustomerOrder"),
            "State",
            [customerOrderHashKey]),
        row => new OrderStateRead(
            row.RequiredString("ParentHashKey"),
            row.RequiredString("HashDiff"),
            row.RequiredDateTimeOffset("LoadTimestamp"),
            row.RequiredString("RecordSource"),
            row.RequiredString("State Code")));

    var explicitLatestRow = Assert.Single(explicitLatestRows);
    var registryAsOfRow = Assert.Single(registryAsOfRows);
    var linkParentRow = Assert.Single(linkParentRows);

    Assert.Equal(customerHashKey, explicitLatestRow.ParentHashKey);
    Assert.Equal("profile-hash-2", explicitLatestRow.HashDiff);
    Assert.Equal(secondLoadTimestamp, explicitLatestRow.LoadTimestamp);
    Assert.Equal("crm-change", explicitLatestRow.RecordSource);
    Assert.Equal("Alice Baker", explicitLatestRow.CustomerName);
    Assert.Equal("active", explicitLatestRow.CustomerStatus);

    Assert.Equal(customerHashKey, registryAsOfRow.ParentHashKey);
    Assert.Equal("profile-hash-1", registryAsOfRow.HashDiff);
    Assert.Equal(firstLoadTimestamp, registryAsOfRow.LoadTimestamp);
    Assert.Equal("prospect", registryAsOfRow.CustomerStatus);

    Assert.Equal(customerOrderHashKey, linkParentRow.ParentHashKey);
    Assert.Equal("state-hash-2", linkParentRow.HashDiff);
    Assert.Equal("fulfilled", linkParentRow.StateCode);
  }

  [Fact]
  public async Task TypedReadProjectsMultiActiveSatelliteRowsByDrivingKeySeriesThroughSqlite() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "ContactChannel",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type", "Region Code"]);
    var metadataModel = new DataVaultMetadataModel(
        [customer],
        [],
        [contact]);
    var firstLoadTimestamp = new DateTimeOffset(2026, 5, 10, 8, 0, 0, TimeSpan.Zero);
    var changedLoadTimestamp = firstLoadTimestamp.AddHours(1);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataModel(metadataModel));
    services.AddDVaultSqlite();
    services.AddDbContext<TypedRegistryReadContext>(
        options => options
            .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
            .UseDataVaultMetadata());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();

    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TypedRegistryReadContext>();
    await context.Database.EnsureCreatedAsync();

    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            firstLoadTimestamp,
            "crm-import",
            [new("Customer", [new("Customer Id", "C-100")])],
            []));
    var customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

    await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            firstLoadTimestamp,
            "crm-import",
            [],
            [],
            [
                new(
                    DataVaultMetadataReference.Hub("Customer"),
                    "ContactChannel",
                    customerHashKey,
                    [new("Contact Type", "billing"), new("Region Code", "DE")],
                    [new("Email Address", "billing-de@example.test")],
                    "contact-hash-1"),
                new(
                    DataVaultMetadataReference.Hub("Customer"),
                    "ContactChannel",
                    customerHashKey,
                    [new("Contact Type", "shipping"), new("Region Code", "DE")],
                    [new("Email Address", "shipping-de@example.test")],
                    "contact-hash-2"),
            ]));
    await saveService.SaveAsync(
        context,
        new DataVaultRegistrySaveRequest(
            changedLoadTimestamp,
            "crm-change",
            [],
            [],
            [
                new(
                    DataVaultMetadataReference.Hub("Customer"),
                    "ContactChannel",
                    customerHashKey,
                    [new("Contact Type", "billing"), new("Region Code", "DE")],
                    [new("Email Address", "billing-de-new@example.test")],
                    "contact-hash-3"),
            ]));

    var rows = await readService.ReadLatestSatelliteAsync(
        context,
        new DataVaultLatestSatelliteReadRequest(contact, [customerHashKey]),
        row => new CustomerContactRead(
            row.RequiredString("ParentHashKey"),
            row.RequiredString("HashDiff"),
            row.RequiredDateTimeOffset("LoadTimestamp"),
            row.RequiredString("RecordSource"),
            row.RequiredString("Contact Type"),
            row.RequiredString("Region Code"),
            row.NullableString("Email Address")));

    Assert.Collection(
        rows,
        row => {
          Assert.Equal("billing", row.ContactType);
          Assert.Equal("DE", row.RegionCode);
          Assert.Equal("billing-de-new@example.test", row.EmailAddress);
          Assert.Equal("contact-hash-3", row.HashDiff);
          Assert.Equal(changedLoadTimestamp, row.LoadTimestamp);
        },
        row => {
          Assert.Equal("shipping", row.ContactType);
          Assert.Equal("DE", row.RegionCode);
          Assert.Equal("shipping-de@example.test", row.EmailAddress);
          Assert.Equal("contact-hash-2", row.HashDiff);
          Assert.Equal(firstLoadTimestamp, row.LoadTimestamp);
        });
  }

  [Fact]
  public async Task TypedReadAccessorsDistinguishMissingAndNullStringValuesThroughSqlite() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 10, 8, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<TypedExplicitReadContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    string customerHashKey;

    await using (var context = new TypedExplicitReadContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "crm-import",
              [new(customer, [new("Customer Id", "C-100")])],
              []));
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "crm-import",
              [],
              [],
              [new(contact, customerHashKey, [new("Email Address", "initial@example.test")], "contact-hash")]));

      await context.Database.ExecuteSqlRawAsync(
          "UPDATE SatCustomerContact SET EmailAddress = NULL WHERE CustomerHashKey = {0}",
          customerHashKey);
    }

    await using (var context = new TypedExplicitReadContext(options)) {
      var nullableRows = await readService.ReadLatestSatelliteAsync(
          context,
          new DataVaultLatestSatelliteReadRequest(contact, [customerHashKey]),
          row => row.NullableString("Email Address"));
      var nullValueException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
          readService.ReadLatestSatelliteAsync(
              context,
              new DataVaultLatestSatelliteReadRequest(contact, [customerHashKey]),
              row => row.RequiredString("Email Address")));
      var missingNameException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
          readService.ReadLatestSatelliteAsync(
              context,
              new DataVaultLatestSatelliteReadRequest(contact, [customerHashKey]),
              row => row.NullableString("Missing Payload")));

      Assert.Null(Assert.Single(nullableRows));
      AssertTypedProjectionFailure(nullValueException, "null-value", "Contact", "Email Address");
      AssertTypedProjectionFailure(missingNameException, "missing-name", "Contact", "Missing Payload");
    }
  }

  [Fact]
  public async Task TypedReadReportsInvalidLoadTimestampProviderValuesThroughSqlite() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 10, 8, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<TypedExplicitReadContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, TypedExplicitReadModelCacheKeyFactory>()
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    string customerHashKey;

    await using (var context = new TypedExplicitReadContext(options, DataVaultLoadTimestampStorage.Iso8601UtcText)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "crm-import",
              [new(customer, [new("Customer Id", "C-100")])],
              []));
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "crm-import",
              [],
              [],
              [new(contact, customerHashKey, [new("Email Address", "initial@example.test")], "contact-hash")]));

      await context.Database.ExecuteSqlRawAsync(
          "UPDATE SatCustomerContact SET LoadTimestamp = 'not-a-timestamp' WHERE CustomerHashKey = {0}",
          customerHashKey);
    }

    await using (var context = new TypedExplicitReadContext(options, DataVaultLoadTimestampStorage.Iso8601UtcText)) {
      var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
          readService.ReadLatestSatelliteAsync(
              context,
              new DataVaultLatestSatelliteReadRequest(contact, [customerHashKey]),
              row => row.RequiredDateTimeOffset("LoadTimestamp")));

      AssertTypedProjectionFailure(exception, "invalid-value", "Contact", "LoadTimestamp");
    }
  }

  [Theory]
  [InlineData(DataVaultLoadTimestampStorage.ProviderDefault)]
  [InlineData(DataVaultLoadTimestampStorage.Iso8601UtcText)]
  [InlineData(DataVaultLoadTimestampStorage.UtcTicks)]
  public async Task TypedReadNormalizesLoadTimestampAcrossConfiguredStorageModesThroughSqlite(
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 10, 10, 0, 0, TimeSpan.FromHours(2));
    var expectedLoadTimestamp = loadTimestamp.ToUniversalTime();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<TypedExplicitReadContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, TypedExplicitReadModelCacheKeyFactory>()
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    string customerHashKey;

    await using (var context = new TypedExplicitReadContext(options, loadTimestampStorage)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "crm-import",
              [new(customer, [new("Customer Id", "C-100")])],
              []));
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "crm-import",
              [],
              [],
              [new(contact, customerHashKey, [new("Email Address", "initial@example.test")], "contact-hash")]));
    }

    await using (var context = new TypedExplicitReadContext(options, loadTimestampStorage)) {
      var timestamps = await readService.ReadLatestSatelliteAsync(
          context,
          new DataVaultLatestSatelliteReadRequest(contact, [customerHashKey]),
          row => row.RequiredDateTimeOffset("LoadTimestamp"));

      Assert.Equal(expectedLoadTimestamp, Assert.Single(timestamps));
    }
  }

  [Theory]
  [InlineData("ParentHashKey", false)]
  [InlineData("HashDiff", false)]
  [InlineData("LoadTimestamp", true)]
  [InlineData("RecordSource", true)]
  public async Task TypedReadRejectsReservedPayloadAndDrivingKeyNamesBeforeQueryExecution(
      string reservedName,
      bool useDrivingKey) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var satellite = useDrivingKey
        ? new DataVaultSatelliteMetadata("Reserved", customer.ToReference(), ["Email Address"], [reservedName])
        : new DataVaultSatelliteMetadata("Reserved", customer.ToReference(), [reservedName]);
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    await using var context = new DbContext(new DbContextOptionsBuilder().Options);

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadLatestSatelliteAsync(
            context,
            new DataVaultLatestSatelliteReadRequest(satellite, []),
            row => row.RequiredString(reservedName)));

    AssertTypedProjectionFailure(exception, "invalid-value", "Reserved", reservedName);
  }

  private static CustomerProfileRead ProjectProfile(DataVaultSatelliteProjectionRow row) {
    return new CustomerProfileRead(
        row.RequiredString("ParentHashKey"),
        row.RequiredString("HashDiff"),
        row.RequiredDateTimeOffset("LoadTimestamp"),
        row.RequiredString("RecordSource"),
        row.RequiredString("customer_name"),
        row.RequiredString("customer_status"));
  }

  private static string GetHashKey(
      DataVaultSaveResult result,
      DataVaultTableKind kind,
      string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && record.MetadataName == metadataName)
        .HashKey;
  }

  private static void AssertTypedProjectionFailure(
      InvalidOperationException exception,
      string failureKind,
      string satelliteName,
      string mappedName) {
    Assert.Contains(
        "DVault typed satellite projection failed (" + failureKind + ")",
        exception.Message,
        StringComparison.Ordinal);
    Assert.Contains("satellite metadata '" + satelliteName + "'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("mapped name '" + mappedName + "'", exception.Message, StringComparison.Ordinal);
  }

  private static DataVaultMetadataModel CreateExplicitMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);

    return new DataVaultMetadataModel([customer], [], [contact]);
  }

  private sealed class TypedRegistryReadContext(
      DbContextOptions<TypedRegistryReadContext> options) : DbContext(options) {
  }

  private sealed class TypedExplicitReadContext(
      DbContextOptions<TypedExplicitReadContext> options,
      DataVaultLoadTimestampStorage loadTimestampStorage = DataVaultLoadTimestampStorage.ProviderDefault) : DbContext(options) {
    public DataVaultLoadTimestampStorage LoadTimestampStorage { get; } = loadTimestampStorage;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateExplicitMetadataModel(),
          DataVaultProviderCapabilityProfiles.Sqlite,
          LoadTimestampStorage);
    }
  }

  private sealed class TypedExplicitReadModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is TypedExplicitReadContext typedContext
          ? (context.GetType(), typedContext.LoadTimestampStorage, designTime)
          : (object)(context.GetType(), designTime);
    }
  }

  private sealed record CustomerProfileRead(
      string ParentHashKey,
      string HashDiff,
      DateTimeOffset LoadTimestamp,
      string RecordSource,
      string CustomerName,
      string CustomerStatus);

  private sealed record OrderStateRead(
      string ParentHashKey,
      string HashDiff,
      DateTimeOffset LoadTimestamp,
      string RecordSource,
      string StateCode);

  private sealed record CustomerContactRead(
      string ParentHashKey,
      string HashDiff,
      DateTimeOffset LoadTimestamp,
      string RecordSource,
      string ContactType,
      string RegionCode,
      string? EmailAddress);
}
