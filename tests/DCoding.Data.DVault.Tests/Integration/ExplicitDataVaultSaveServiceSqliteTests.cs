using System.Data.Common;
using System.Globalization;
using System.Runtime.CompilerServices;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class ExplicitDataVaultSaveServiceSqliteTests {
  private const string StableHashAlgorithmId = "sha256-128-v1";
  private const int StableHashDigestByteLength = 16;

  [Fact]
  public async Task DefaultSaveServicePersistsHubAndLinkRowsThroughSqlite() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var loadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, ExplicitSaveServiceModelCacheKeyFactory>()
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "crm-import",
              [
                  new(customer, [new("Customer Id", "C-100")]),
                  new(order, [new("Order Id", "O-200")]),
              ],
              []));

      var customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
      var orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");

      var linkResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "crm-import",
              [],
              [
                  new(customerOrder, [new("Customer", customerHashKey), new("Order", orderHashKey)]),
              ]));

      Assert.Equal(2, hubResult.RowsWritten);
      Assert.Equal(1, linkResult.RowsWritten);
    }

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var customerRow = await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().SingleAsync();
      var orderRow = await context.Set<Dictionary<string, object>>("HubOrder").AsNoTracking().SingleAsync();
      var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().SingleAsync();

      Assert.Equal("C-100", customerRow["CustomerId"]);
      Assert.Equal("O-200", orderRow["OrderId"]);
      Assert.Equal("crm-import", customerRow["RecordSource"]);
      Assert.Equal("crm-import", linkRow["RecordSource"]);
      Assert.Equal(loadTimestamp, customerRow["LoadTimestamp"]);
      Assert.Equal(customerRow["CustomerHashKey"], linkRow["CustomerHashKey"]);
      Assert.Equal(orderRow["OrderHashKey"], linkRow["OrderHashKey"]);
      Assert.Matches("^[0-9a-f]{64}$", Assert.IsType<string>(linkRow["CustomerOrderHashKey"]));
    }
  }

  [Fact]
  public async Task DefaultSaveServicePersistsDependentChildKeyLinkRowsThroughSqlite() {
    var metadataModel = CreateDependentChildKeyMetadataModel();
    var customer = metadataModel.Hubs.Single(hub => hub.Name == "Customer");
    var order = metadataModel.Hubs.Single(hub => hub.Name == "Order");
    var customerOrderLine = Assert.Single(metadataModel.Links);
    var loadTimestamp = new DateTimeOffset(2026, 7, 2, 9, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<DependentChildKeySaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using (var context = new DependentChildKeySaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "order-import",
              [
                  new(customer, [new("Customer Id", "C-100")]),
                  new(order, [new("Order Id", "O-200")]),
              ],
              []));
      var customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
      var orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");

      var linkResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "order-import",
              [],
              [
                  new(
                      customerOrderLine,
                      [new("Customer", customerHashKey), new("Order", orderHashKey)],
                      [new("Line Number", "1")]),
                  new(
                      customerOrderLine,
                      [new("Customer", customerHashKey), new("Order", orderHashKey)],
                      [new("Line Number", "2")]),
              ]));

      Assert.Equal(2, linkResult.RowsWritten);
      Assert.All(linkResult.SavedRecords, record => {
        Assert.Equal(DataVaultTableKind.Link, record.Kind);
        Assert.Equal("CustomerOrderLine", record.MetadataName);
        Assert.True(record.DependentChildKeyValues.ContainsKey("Line Number"));
      });
      Assert.Equal(["1", "2"], linkResult.SavedRecords
          .Select(record => record.DependentChildKeyValues["Line Number"])
          .Order(StringComparer.Ordinal));
    }

    await using (var context = new DependentChildKeySaveServiceContext(options)) {
      var linkRows = await context.Set<Dictionary<string, object>>("LinkCustomerOrderLine")
          .AsNoTracking()
          .OrderBy(row => row["LineNumber"])
          .ToListAsync();

      Assert.Collection(
          linkRows,
          row => Assert.Equal("1", row["LineNumber"]),
          row => Assert.Equal("2", row["LineNumber"]));
      Assert.NotEqual(linkRows[0]["CustomerOrderLineHashKey"], linkRows[1]["CustomerOrderLineHashKey"]);
      Assert.Equal(linkRows[0]["CustomerHashKey"], linkRows[1]["CustomerHashKey"]);
      Assert.Equal(linkRows[0]["OrderHashKey"], linkRows[1]["OrderHashKey"]);
    }
  }

  [Fact]
  public async Task DefaultSaveServiceBatchesUniqueRowExistenceChecksPerTable() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 22, 10, 0, 0, TimeSpan.Zero);
    var replayTimestamp = loadTimestamp.AddMinutes(5);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var commandCounter = new TableSelectCommandCounter("HubCustomer");
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .AddInterceptors(commandCounter)
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var hubOperations = Enumerable.Range(0, 5)
        .Select(customerIndex => new DataVaultHubSaveOperation(
            customer,
            [new("Customer Id", "C-BATCH-" + customerIndex.ToString("0000", CultureInfo.InvariantCulture))]))
        .ToArray();

    await using var context = new ExplicitSaveServiceContext(options);
    await context.Database.EnsureCreatedAsync();

    commandCounter.Reset();
    var firstResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(loadTimestamp, "crm-import", hubOperations, []));

    Assert.Equal(5, firstResult.RowsWritten);
    Assert.Equal(5, firstResult.SavedRecords.Count);
    Assert.Equal(1, commandCounter.SelectCount);

    commandCounter.Reset();
    var replayResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(replayTimestamp, "crm-replay", hubOperations, []));

    Assert.Equal(0, replayResult.RowsWritten);
    Assert.Equal(5, replayResult.SavedRecords.Count);
    Assert.Equal(0, commandCounter.SelectCount);
    AssertSavedRecordsEqual(firstResult.SavedRecords, replayResult.SavedRecords);
  }

  [Fact]
  public async Task DefaultSaveServicePersistsRoleBearingSameHubLinkRowsThroughSqlite() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var customerSameAs = CreateCustomerIdentityMatchLinkMetadata();
    var loadTimestamp = new DateTimeOffset(2026, 5, 17, 10, 15, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<SameAsSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string sourceCustomerHashKey;
    string matchedCustomerHashKey;

    await using (var context = new SameAsSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "crm-import",
              [
                  new(customer, [new("CustomerId", "C-100")]),
                  new(customer, [new("CustomerId", "C-200")]),
              ],
              []));

      var customerHashKeys = hubResult.SavedRecords
          .Where(record => record.Kind == DataVaultTableKind.Hub && record.MetadataName == "Customer")
          .Select(record => record.HashKey)
          .ToArray();

      Assert.Equal(2, hubResult.RowsWritten);
      Assert.Equal(2, customerHashKeys.Length);

      sourceCustomerHashKey = customerHashKeys[0];
      matchedCustomerHashKey = customerHashKeys[1];

      var linkResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "crm-import",
              [],
              [
                  new(
                      customerSameAs,
                      [new("SourceCustomer", sourceCustomerHashKey), new("MatchedCustomer", matchedCustomerHashKey)]),
              ]));

      Assert.Equal(1, linkResult.RowsWritten);
    }

    await using (var context = new SameAsSaveServiceContext(options)) {
      var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerIdentityMatch").AsNoTracking().SingleAsync();

      Assert.Equal("crm-import", linkRow["RecordSource"]);
      Assert.Equal(loadTimestamp, linkRow["LoadTimestamp"]);
      Assert.Equal(sourceCustomerHashKey, linkRow["SourceCustomerHashKey"]);
      Assert.Equal(matchedCustomerHashKey, linkRow["MatchedCustomerHashKey"]);
      Assert.Matches("^[0-9a-f]{64}$", Assert.IsType<string>(linkRow["CustomerIdentityMatchHashKey"]));
    }
  }

  [Theory]
  [InlineData(false)]
  [InlineData(true)]
  public async Task SaveServiceAppliesConfiguredRequestHooksThroughFallbackAndSqliteStrategy(bool useSqliteStrategy) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var request = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero),
        "request-source",
        [new(customer, [new("Customer Id", "C-100")])],
        [new(customerOrder, [new("Customer", "customer-hash"), new("Order", "order-hash")])],
        [new(contact, "customer-hash", [new("Email Address", "first@example.test")], "contact-hash-1")]);
    var resolvedTimestamp = new DateTimeOffset(2026, 5, 4, 12, 30, 0, TimeSpan.Zero);
    var timestampResolver = new CountingLoadTimestampResolver(resolvedTimestamp);
    var recordSourceResolver = new CountingRecordSourceResolver("hooked-source");
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, ExplicitSaveServiceModelCacheKeyFactory>()
        .Options;
    var services = new ServiceCollection();
    services.AddDVault(configure => configure
        .UseLoadTimestampResolver(timestampResolver)
        .UseRecordSourceResolver(recordSourceResolver));
    if (useSqliteStrategy) {
      services.AddDVaultSqlite();
    }

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var result = await saveService.SaveAsync(context, request);

      Assert.Equal(3, result.RowsWritten);
    }

    Assert.Equal(1, timestampResolver.CallCount);
    Assert.Equal(1, recordSourceResolver.CallCount);

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var hubRow = await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().SingleAsync();
      var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().SingleAsync();
      var satelliteRow = await context.Set<Dictionary<string, object>>("SatCustomerContact").AsNoTracking().SingleAsync();

      Assert.Equal(resolvedTimestamp, hubRow["LoadTimestamp"]);
      Assert.Equal(resolvedTimestamp, linkRow["LoadTimestamp"]);
      Assert.Equal(resolvedTimestamp, satelliteRow["LoadTimestamp"]);
      Assert.Equal("hooked-source", hubRow["RecordSource"]);
      Assert.Equal("hooked-source", linkRow["RecordSource"]);
      Assert.Equal("hooked-source", satelliteRow["RecordSource"]);
    }
  }

  [Fact]
  public async Task DefaultSaveServiceReusesExistingHubAndLinkRowsAcrossSqliteContexts() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var firstLoadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    var secondLoadTimestamp = new DateTimeOffset(2026, 4, 30, 12, 45, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, ExplicitSaveServiceModelCacheKeyFactory>()
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string customerHashKey;
    string orderHashKey;
    IReadOnlyList<DataVaultSavedRecord> firstSavedRecords;

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "crm-import",
              [
                  new(customer, [new("Customer Id", "C-100")]),
                  new(order, [new("Order Id", "O-200")]),
              ],
              []));

      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
      orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");

      var linkResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "crm-import",
              [],
              [
                  new(customerOrder, [new("Customer", customerHashKey), new("Order", orderHashKey)]),
              ]));

      Assert.Equal(2, hubResult.RowsWritten);
      Assert.Equal(1, linkResult.RowsWritten);
      firstSavedRecords = hubResult.SavedRecords.Concat(linkResult.SavedRecords).ToArray();
    }

    DataVaultSaveResult replayResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      replayResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              secondLoadTimestamp,
              "crm-replay",
              [
                  new(customer, [new("Customer Id", "C-100")]),
                  new(order, [new("Order Id", "O-200")]),
              ],
              [
                  new(customerOrder, [new("Customer", customerHashKey), new("Order", orderHashKey)]),
              ]));
    }

    Assert.Equal(0, replayResult.RowsWritten);
    AssertSavedRecordsEqual(firstSavedRecords, replayResult.SavedRecords);

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var customerRows = await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().ToListAsync();
      var orderRows = await context.Set<Dictionary<string, object>>("HubOrder").AsNoTracking().ToListAsync();
      var linkRows = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().ToListAsync();
      var customerRow = Assert.Single(customerRows);
      var orderRow = Assert.Single(orderRows);
      var linkRow = Assert.Single(linkRows);

      Assert.Equal(customerHashKey, customerRow["CustomerHashKey"]);
      Assert.Equal(orderHashKey, orderRow["OrderHashKey"]);
      Assert.Equal(customerHashKey, linkRow["CustomerHashKey"]);
      Assert.Equal(orderHashKey, linkRow["OrderHashKey"]);
      Assert.Equal("crm-import", customerRow["RecordSource"]);
      Assert.Equal("crm-import", orderRow["RecordSource"]);
      Assert.Equal("crm-import", linkRow["RecordSource"]);
      Assert.Equal(firstLoadTimestamp, customerRow["LoadTimestamp"]);
      Assert.Equal(firstLoadTimestamp, orderRow["LoadTimestamp"]);
      Assert.Equal(firstLoadTimestamp, linkRow["LoadTimestamp"]);
    }
  }

  [Fact]
  public async Task DefaultSaveServicePersistsSatelliteRowsOnlyWhenHashDiffChanges() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var hubLoadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    var firstSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 10, 30, 0, TimeSpan.Zero);
    var unchangedSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 11, 0, 0, TimeSpan.Zero);
    var changedSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 11, 30, 0, TimeSpan.Zero);
    var returnedSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 12, 0, 0, TimeSpan.Zero);
    var otherParentTimestamp = new DateTimeOffset(2026, 4, 29, 12, 30, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string customerHashKey;
    string otherCustomerHashKey;

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var firstHubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              hubLoadTimestamp,
              "crm-import",
              [new(customer, [new("Customer Id", "C-100")])],
              []));
      var otherHubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              hubLoadTimestamp,
              "crm-import",
              [new(customer, [new("Customer Id", "C-200")])],
              []));

      customerHashKey = GetHashKey(firstHubResult, DataVaultTableKind.Hub, "Customer");
      otherCustomerHashKey = GetHashKey(otherHubResult, DataVaultTableKind.Hub, "Customer");
    }

    DataVaultSaveResult firstSatelliteResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      firstSatelliteResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstSatelliteTimestamp,
              "crm-import",
              [],
              [],
              [
                  new(contact, customerHashKey, [new("Email Address", "first@example.test")], "contact-hash-1"),
              ]));
    }

    DataVaultSaveResult unchangedSatelliteResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      unchangedSatelliteResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              unchangedSatelliteTimestamp,
              "crm-replay",
              [],
              [],
              [
                  new(contact, customerHashKey, [new("Email Address", "ignored@example.test")], "contact-hash-1"),
              ]));
    }

    DataVaultSaveResult changedSatelliteResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      changedSatelliteResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              changedSatelliteTimestamp,
              "crm-change",
              [],
              [],
              [
                  new(contact, customerHashKey, [new("Email Address", "changed@example.test")], "contact-hash-2"),
              ]));
    }

    DataVaultSaveResult returnedSatelliteResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      returnedSatelliteResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              returnedSatelliteTimestamp,
              "crm-return",
              [],
              [],
              [
                  new(contact, customerHashKey, [new("Email Address", "first@example.test")], "contact-hash-1"),
              ]));
    }

    DataVaultSaveResult otherParentSatelliteResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      otherParentSatelliteResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              otherParentTimestamp,
              "crm-import",
              [],
              [],
              [
                  new(contact, otherCustomerHashKey, [new("Email Address", "other@example.test")], "contact-hash-1"),
              ]));
    }

    Assert.Equal(1, firstSatelliteResult.RowsWritten);
    Assert.Equal(0, unchangedSatelliteResult.RowsWritten);
    Assert.Equal(1, changedSatelliteResult.RowsWritten);
    Assert.Equal(1, returnedSatelliteResult.RowsWritten);
    Assert.Equal(1, otherParentSatelliteResult.RowsWritten);
    AssertSingleSavedRecord(
        firstSatelliteResult,
        DataVaultTableKind.Satellite,
        "Contact",
        "SatCustomerContact",
        customerHashKey);
    AssertSingleSavedRecord(
        unchangedSatelliteResult,
        DataVaultTableKind.Satellite,
        "Contact",
        "SatCustomerContact",
        customerHashKey);
    AssertSingleSavedRecord(
        otherParentSatelliteResult,
        DataVaultTableKind.Satellite,
        "Contact",
        "SatCustomerContact",
        otherCustomerHashKey);

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var rows = await context.Set<Dictionary<string, object>>("SatCustomerContact").AsNoTracking().ToListAsync();
      var customerRows = rows
          .Where(row => Assert.IsType<string>(row["CustomerHashKey"]) == customerHashKey)
          .OrderBy(row => (DateTimeOffset)row["LoadTimestamp"])
          .ToArray();
      var otherCustomerRow = Assert.Single(
          rows.Where(row => Assert.IsType<string>(row["CustomerHashKey"]) == otherCustomerHashKey));

      Assert.Equal(4, rows.Count);
      Assert.Equal(3, customerRows.Length);
      AssertSatelliteRow(
          customerRows[0],
          customerHashKey,
          "first@example.test",
          "contact-hash-1",
          firstSatelliteTimestamp,
          "crm-import");
      AssertSatelliteRow(
          customerRows[1],
          customerHashKey,
          "changed@example.test",
          "contact-hash-2",
          changedSatelliteTimestamp,
          "crm-change");
      AssertSatelliteRow(
          customerRows[2],
          customerHashKey,
          "first@example.test",
          "contact-hash-1",
          returnedSatelliteTimestamp,
          "crm-return");
      AssertSatelliteRow(
          otherCustomerRow,
          otherCustomerHashKey,
          "other@example.test",
          "contact-hash-1",
          otherParentTimestamp,
          "crm-import");
    }
  }

  [Fact]
  public async Task DefaultSaveServiceChecksSatelliteHashDiffsAcrossBatchParents() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var hubLoadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    var firstSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 10, 30, 0, TimeSpan.Zero);
    var secondSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 11, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string firstCustomerHashKey;
    string secondCustomerHashKey;

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              hubLoadTimestamp,
              "crm-import",
              [
                  new(customer, [new("Customer Id", "C-100")]),
                  new(customer, [new("Customer Id", "C-200")]),
              ],
              []));

      firstCustomerHashKey = hubResult.SavedRecords[0].HashKey;
      secondCustomerHashKey = hubResult.SavedRecords[1].HashKey;
    }

    DataVaultSaveResult firstBatchResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      firstBatchResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstSatelliteTimestamp,
              "crm-import",
              [],
              [],
              [
                  new(contact, firstCustomerHashKey, [new("Email Address", "first@example.test")], "contact-hash-1"),
                  new(contact, secondCustomerHashKey, [new("Email Address", "second@example.test")], "contact-hash-2"),
              ]));
    }

    DataVaultSaveResult secondBatchResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      secondBatchResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              secondSatelliteTimestamp,
              "crm-import",
              [],
              [],
              [
                  new(contact, firstCustomerHashKey, [new("Email Address", "first-replay@example.test")], "contact-hash-1"),
                  new(contact, secondCustomerHashKey, [new("Email Address", "second-changed@example.test")], "contact-hash-3"),
              ]));
    }

    Assert.Equal(2, firstBatchResult.RowsWritten);
    Assert.Equal(1, secondBatchResult.RowsWritten);
    Assert.Equal(2, secondBatchResult.SavedRecords.Count);

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var rows = await context.Set<Dictionary<string, object>>("SatCustomerContact").AsNoTracking().ToListAsync();
      var firstCustomerRows = rows
          .Where(row => Assert.IsType<string>(row["CustomerHashKey"]) == firstCustomerHashKey)
          .OrderBy(row => (DateTimeOffset)row["LoadTimestamp"])
          .ToArray();
      var secondCustomerRows = rows
          .Where(row => Assert.IsType<string>(row["CustomerHashKey"]) == secondCustomerHashKey)
          .OrderBy(row => (DateTimeOffset)row["LoadTimestamp"])
          .ToArray();

      Assert.Equal(3, rows.Count);
      var firstCustomerRow = Assert.Single(firstCustomerRows);
      Assert.Equal(2, secondCustomerRows.Length);
      AssertSatelliteRow(
          firstCustomerRow,
          firstCustomerHashKey,
          "first@example.test",
          "contact-hash-1",
          firstSatelliteTimestamp,
          "crm-import");
      AssertSatelliteRow(
          secondCustomerRows[0],
          secondCustomerHashKey,
          "second@example.test",
          "contact-hash-2",
          firstSatelliteTimestamp,
          "crm-import");
      AssertSatelliteRow(
          secondCustomerRows[1],
          secondCustomerHashKey,
          "second-changed@example.test",
          "contact-hash-3",
          secondSatelliteTimestamp,
          "crm-import");
    }
  }

  [Fact]
  public async Task DefaultSaveServiceCarriesSatelliteHashDiffsAcrossBulkRequests() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var hubLoadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    var firstSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 10, 30, 0, TimeSpan.Zero);
    var secondSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 11, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string firstCustomerHashKey;
    string secondCustomerHashKey;

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              hubLoadTimestamp,
              "crm-import",
              [
                  new(customer, [new("Customer Id", "C-100")]),
                  new(customer, [new("Customer Id", "C-200")]),
              ],
              []));

      firstCustomerHashKey = hubResult.SavedRecords[0].HashKey;
      secondCustomerHashKey = hubResult.SavedRecords[1].HashKey;
    }

    DataVaultSaveResult bulkResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      bulkResult = await saveService.SaveAsync(
          context,
          new DataVaultBulkSaveRequest(
              [
                  new(
                      firstSatelliteTimestamp,
                      "crm-import",
                      [],
                      [],
                      [
                          new(contact, firstCustomerHashKey, [new("Email Address", "first@example.test")], "contact-hash-1"),
                          new(contact, secondCustomerHashKey, [new("Email Address", "second@example.test")], "contact-hash-2"),
                      ]),
                  new(
                      secondSatelliteTimestamp,
                      "crm-import",
                      [],
                      [],
                      [
                          new(contact, firstCustomerHashKey, [new("Email Address", "first-replay@example.test")], "contact-hash-1"),
                          new(contact, secondCustomerHashKey, [new("Email Address", "second-changed@example.test")], "contact-hash-3"),
                      ]),
              ]));
    }

    Assert.Equal(3, bulkResult.RowsWritten);
    Assert.Equal(4, bulkResult.SavedRecords.Count);

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var rows = await context.Set<Dictionary<string, object>>("SatCustomerContact").AsNoTracking().ToListAsync();
      var firstCustomerRow = Assert.Single(rows
          .Where(row => Assert.IsType<string>(row["CustomerHashKey"]) == firstCustomerHashKey));
      var secondCustomerRows = rows
          .Where(row => Assert.IsType<string>(row["CustomerHashKey"]) == secondCustomerHashKey)
          .OrderBy(row => (DateTimeOffset)row["LoadTimestamp"])
          .ToArray();

      Assert.Equal(3, rows.Count);
      Assert.Equal(2, secondCustomerRows.Length);
      AssertSatelliteRow(
          firstCustomerRow,
          firstCustomerHashKey,
          "first@example.test",
          "contact-hash-1",
          firstSatelliteTimestamp,
          "crm-import");
      AssertSatelliteRow(
          secondCustomerRows[0],
          secondCustomerHashKey,
          "second@example.test",
          "contact-hash-2",
          firstSatelliteTimestamp,
          "crm-import");
      AssertSatelliteRow(
          secondCustomerRows[1],
          secondCustomerHashKey,
          "second-changed@example.test",
          "contact-hash-3",
          secondSatelliteTimestamp,
          "crm-import");
    }
  }

  [Fact]
  public async Task DefaultSaveServiceKeepsBulkSatelliteLatestHashDiffChronological() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var hubLoadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 0, 0, TimeSpan.Zero);
    var firstSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 10, 30, 0, TimeSpan.Zero);
    var secondSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 11, 0, 0, TimeSpan.Zero);
    var thirdSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 11, 30, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string customerHashKey;

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              hubLoadTimestamp,
              "crm-import",
              [new(customer, [new("Customer Id", "C-100")])],
              []));
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
    }

    DataVaultSaveResult bulkResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      bulkResult = await saveService.SaveAsync(
          context,
          new DataVaultBulkSaveRequest(
              [
                  new(
                      secondSatelliteTimestamp,
                      "crm-change",
                      [],
                      [],
                      [new(contact, customerHashKey, [new("Email Address", "changed@example.test")], "contact-hash-2")]),
                  new(
                      firstSatelliteTimestamp,
                      "crm-import",
                      [],
                      [],
                      [new(contact, customerHashKey, [new("Email Address", "first@example.test")], "contact-hash-1")]),
                  new(
                      thirdSatelliteTimestamp,
                      "crm-return",
                      [],
                      [],
                      [new(contact, customerHashKey, [new("Email Address", "first@example.test")], "contact-hash-1")]),
              ]));
    }

    Assert.Equal(3, bulkResult.RowsWritten);

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var rows = (await context.Set<Dictionary<string, object>>("SatCustomerContact")
          .AsNoTracking()
          .ToListAsync())
          .OrderBy(row => (DateTimeOffset)row["LoadTimestamp"])
          .ToArray();

      Assert.Equal(3, rows.Length);
      AssertSatelliteRow(
          rows[0],
          customerHashKey,
          "first@example.test",
          "contact-hash-1",
          firstSatelliteTimestamp,
          "crm-import");
      AssertSatelliteRow(
          rows[1],
          customerHashKey,
          "changed@example.test",
          "contact-hash-2",
          secondSatelliteTimestamp,
          "crm-change");
      AssertSatelliteRow(
          rows[2],
          customerHashKey,
          "first@example.test",
          "contact-hash-1",
          thirdSatelliteTimestamp,
          "crm-return");
    }
  }

  [Fact]
  public async Task ChunkedSaveTreatsEmptySequenceAndEmptyChunksAsNoOps() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new ExplicitSaveServiceContext(options);
    await context.Database.EnsureCreatedAsync();

    var emptySequenceResult = await saveService.SaveAsync(
        context,
        new DataVaultChunkedSaveRequest([]));
    var emptyChunkResult = await saveService.SaveAsync(
        context,
        new DataVaultChunkedSaveRequest([new DataVaultSaveChunk([])]));

    Assert.Equal(0, emptySequenceResult.RowsWritten);
    Assert.Empty(emptySequenceResult.SavedRecords);
    Assert.Equal(0, emptyChunkResult.RowsWritten);
    Assert.Empty(emptyChunkResult.SavedRecords);
  }

  [Fact]
  public async Task AsyncChunkedSaveTreatsEmptySourceAndEmptyChunksAsNoOps() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new ExplicitSaveServiceContext(options);
    await context.Database.EnsureCreatedAsync();

    var emptySourceResult = await saveService.SaveAsync(
        context,
        CreateAsyncChunks([]));
    var emptyChunkResult = await saveService.SaveAsync(
        context,
        CreateAsyncChunks([new DataVaultSaveChunk([])]));

    Assert.Equal(0, emptySourceResult.RowsWritten);
    Assert.Empty(emptySourceResult.SavedRecords);
    Assert.Equal(0, emptyChunkResult.RowsWritten);
    Assert.Empty(emptyChunkResult.SavedRecords);
  }

  [Fact]
  public async Task ChunkedSaveMatchesEquivalentBulkSavedRecordOrderingAcrossOperationKinds() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var parentHashKey = "customer-hash";
    var requests = new[]
    {
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 24, 9, 0, 0, TimeSpan.Zero),
            "satellite-import",
            [],
            [],
            [new(contact, parentHashKey, [new("Email Address", "first@example.test")], "contact-hash-1")]),
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 24, 9, 5, 0, TimeSpan.Zero),
            "hub-import",
            [new(customer, [new("Customer Id", "C-100")])],
            []),
    };
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    using var bulkDatabase = SqliteTestDatabase.CreateTemporaryFile();
    using var chunkedDatabase = SqliteTestDatabase.CreateTemporaryFile();
    var bulkOptions = CreateExplicitSaveServiceOptions(bulkDatabase);
    var chunkedOptions = CreateExplicitSaveServiceOptions(chunkedDatabase);

    DataVaultSaveResult bulkResult;
    await using (var context = new ExplicitSaveServiceContext(bulkOptions)) {
      await context.Database.EnsureCreatedAsync();

      bulkResult = await saveService.SaveAsync(
          context,
          new DataVaultBulkSaveRequest(requests));
    }

    DataVaultSaveResult chunkedResult;
    await using (var context = new ExplicitSaveServiceContext(chunkedOptions)) {
      await context.Database.EnsureCreatedAsync();

      chunkedResult = await saveService.SaveAsync(
          context,
          new DataVaultChunkedSaveRequest(
              [
                  new DataVaultSaveChunk([requests[0]]),
                  new DataVaultSaveChunk([requests[1]]),
              ]));
    }

    Assert.Equal(bulkResult.RowsWritten, chunkedResult.RowsWritten);
    AssertSavedRecordsEqual(bulkResult.SavedRecords, chunkedResult.SavedRecords);
  }

  [Fact]
  public async Task ChunkedSaveMatchesEquivalentBulkOrderingForHubAndLinkRequests() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 24, 10, 0, 0, TimeSpan.Zero);
    var requests = new[]
    {
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-import",
            [new(customer, [new("Customer Id", "C-100")])],
            [new(customerOrder, [new("Customer", "customer-hash-1"), new("Order", "order-hash-1")])]),
        new DataVaultSaveRequest(
            loadTimestamp.AddMinutes(1),
            "order-import",
            [new(order, [new("Order Id", "O-200")])],
            [new(customerOrder, [new("Customer", "customer-hash-2"), new("Order", "order-hash-2")])]),
    };
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    using var bulkDatabase = SqliteTestDatabase.CreateTemporaryFile();
    using var chunkedDatabase = SqliteTestDatabase.CreateTemporaryFile();
    var bulkOptions = CreateExplicitSaveServiceOptions(bulkDatabase);
    var chunkedOptions = CreateExplicitSaveServiceOptions(chunkedDatabase);

    DataVaultSaveResult bulkResult;
    await using (var context = new ExplicitSaveServiceContext(bulkOptions)) {
      await context.Database.EnsureCreatedAsync();

      bulkResult = await saveService.SaveAsync(
          context,
          new DataVaultBulkSaveRequest(requests));
    }

    DataVaultSaveResult chunkedResult;
    await using (var context = new ExplicitSaveServiceContext(chunkedOptions)) {
      await context.Database.EnsureCreatedAsync();

      chunkedResult = await saveService.SaveAsync(
          context,
          new DataVaultChunkedSaveRequest(
              [
                  new DataVaultSaveChunk([requests[0]]),
                  new DataVaultSaveChunk([requests[1]]),
              ]));
    }

    Assert.Equal(bulkResult.RowsWritten, chunkedResult.RowsWritten);
    AssertSavedRecordsEqual(bulkResult.SavedRecords, chunkedResult.SavedRecords);
  }

  [Fact]
  public async Task AsyncChunkedSaveMatchesEquivalentBulkOrderingForHubAndLinkRequests() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 24, 10, 0, 0, TimeSpan.Zero);
    var requests = new[]
    {
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-import",
            [new(customer, [new("Customer Id", "C-100")])],
            [new(customerOrder, [new("Customer", "customer-hash-1"), new("Order", "order-hash-1")])]),
        new DataVaultSaveRequest(
            loadTimestamp.AddMinutes(1),
            "order-import",
            [new(order, [new("Order Id", "O-200")])],
            [new(customerOrder, [new("Customer", "customer-hash-2"), new("Order", "order-hash-2")])]),
    };
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    using var bulkDatabase = SqliteTestDatabase.CreateTemporaryFile();
    using var chunkedDatabase = SqliteTestDatabase.CreateTemporaryFile();
    var bulkOptions = CreateExplicitSaveServiceOptions(bulkDatabase);
    var chunkedOptions = CreateExplicitSaveServiceOptions(chunkedDatabase);

    DataVaultSaveResult bulkResult;
    await using (var context = new ExplicitSaveServiceContext(bulkOptions)) {
      await context.Database.EnsureCreatedAsync();

      bulkResult = await saveService.SaveAsync(
          context,
          new DataVaultBulkSaveRequest(requests));
    }

    DataVaultSaveResult chunkedResult;
    await using (var context = new ExplicitSaveServiceContext(chunkedOptions)) {
      await context.Database.EnsureCreatedAsync();

      chunkedResult = await saveService.SaveAsync(
          context,
          CreateAsyncChunks(
              [
                  new DataVaultSaveChunk([requests[0]]),
                  new DataVaultSaveChunk([requests[1]]),
              ]));
    }

    Assert.Equal(bulkResult.RowsWritten, chunkedResult.RowsWritten);
    AssertSavedRecordsEqual(bulkResult.SavedRecords, chunkedResult.SavedRecords);
  }

  [Fact]
  public async Task ChunkedSaveObservesCancellationBeforeLaterChunks() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var firstRequest = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 24, 10, 0, 0, TimeSpan.Zero),
        "crm-import",
        [new(customer, [new("Customer Id", "C-100")])],
        []);
    var secondRequest = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 24, 10, 5, 0, TimeSpan.Zero),
        "crm-import",
        [new(customer, [new("Customer Id", "C-200")])],
        []);
    using var cancellationSource = new CancellationTokenSource();
    var strategy = new CancelAfterFirstChunkSaveStrategy(cancellationSource);
    var services = new ServiceCollection();
    services.AddDVault();
    services.AddSingleton<IDataVaultProviderSaveStrategy>(strategy);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    await using var context = new ExplicitSaveServiceContext(options);

    await Assert.ThrowsAsync<OperationCanceledException>(() =>
        saveService.SaveAsync(
            context,
            new DataVaultChunkedSaveRequest(
                [
                    new DataVaultSaveChunk([firstRequest]),
                    new DataVaultSaveChunk([secondRequest]),
                ]),
            cancellationSource.Token));

    Assert.Equal(1, strategy.SaveCallCount);
    Assert.Equal([1], strategy.RequestCounts);
  }

  [Fact]
  public async Task AsyncChunkedSaveObservesCancellationDuringEnumerationBeforeLaterChunks() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var firstRequest = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 24, 10, 0, 0, TimeSpan.Zero),
        "crm-import",
        [new(customer, [new("Customer Id", "C-100")])],
        []);
    var secondRequest = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 24, 10, 5, 0, TimeSpan.Zero),
        "crm-import",
        [new(customer, [new("Customer Id", "C-200")])],
        []);
    using var cancellationSource = new CancellationTokenSource();
    var strategy = new CancelAfterFirstChunkSaveStrategy(cancellationSource);
    var services = new ServiceCollection();
    services.AddDVault();
    services.AddSingleton<IDataVaultProviderSaveStrategy>(strategy);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    await using var context = new ExplicitSaveServiceContext(options);

    await Assert.ThrowsAsync<OperationCanceledException>(() =>
        saveService.SaveAsync(
            context,
            CreateAsyncChunks(
                [
                    new DataVaultSaveChunk([firstRequest]),
                    new DataVaultSaveChunk([secondRequest]),
                ]),
            cancellationSource.Token));

    Assert.Equal(1, strategy.SaveCallCount);
    Assert.Equal([1], strategy.RequestCounts);
  }

  [Fact]
  public async Task ChunkedSaveParticipatesInCallerTransactionAcrossChunks() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 24, 11, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();
      await using var transaction = await context.Database.BeginTransactionAsync();

      var result = await saveService.SaveAsync(
          context,
          new DataVaultChunkedSaveRequest(
              [
                  new DataVaultSaveChunk([
                      new DataVaultSaveRequest(
                          loadTimestamp,
                          "crm-import",
                          [
                              new(customer, [new("Customer Id", "C-100")]),
                              new(order, [new("Order Id", "O-200")]),
                          ],
                          []),
                  ]),
                  new DataVaultSaveChunk([
                      new DataVaultSaveRequest(
                          loadTimestamp.AddMinutes(1),
                          "crm-import",
                          [],
                          [
                              new(customerOrder, [new("Customer", "customer-hash"), new("Order", "order-hash")]),
                          ]),
                  ]),
              ]));

      Assert.Equal(3, result.RowsWritten);

      await transaction.RollbackAsync();
    }

    await using (var context = new ExplicitSaveServiceContext(options)) {
      Assert.Empty(await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().ToListAsync());
      Assert.Empty(await context.Set<Dictionary<string, object>>("HubOrder").AsNoTracking().ToListAsync());
      Assert.Empty(await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().ToListAsync());
    }
  }

  [Fact]
  public async Task AsyncChunkedSaveParticipatesInCallerTransactionAcrossChunks() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 24, 11, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();
      await using var transaction = await context.Database.BeginTransactionAsync();

      var result = await saveService.SaveAsync(
          context,
          CreateAsyncChunks(
              [
                  new DataVaultSaveChunk([
                      new DataVaultSaveRequest(
                          loadTimestamp,
                          "crm-import",
                          [
                              new(customer, [new("Customer Id", "C-100")]),
                              new(order, [new("Order Id", "O-200")]),
                          ],
                          []),
                  ]),
                  new DataVaultSaveChunk([
                      new DataVaultSaveRequest(
                          loadTimestamp.AddMinutes(1),
                          "crm-import",
                          [],
                          [
                              new(customerOrder, [new("Customer", "customer-hash"), new("Order", "order-hash")]),
                          ]),
                  ]),
              ]));

      Assert.Equal(3, result.RowsWritten);

      await transaction.RollbackAsync();
    }

    await using (var context = new ExplicitSaveServiceContext(options)) {
      Assert.Empty(await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().ToListAsync());
      Assert.Empty(await context.Set<Dictionary<string, object>>("HubOrder").AsNoTracking().ToListAsync());
      Assert.Empty(await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().ToListAsync());
    }
  }

  [Fact]
  public async Task ChunkedSaveReusesRepeatedHubAndLinkRowsAcrossChunks() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 24, 12, 0, 0, TimeSpan.Zero);
    var request = new DataVaultSaveRequest(
        loadTimestamp,
        "crm-import",
        [
            new(customer, [new("Customer Id", "C-100")]),
            new(order, [new("Order Id", "O-200")]),
        ],
        [
            new(customerOrder, [new("Customer", "customer-hash"), new("Order", "order-hash")]),
        ]);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    DataVaultSaveResult result;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      result = await saveService.SaveAsync(
          context,
          new DataVaultChunkedSaveRequest(
              [
                  new DataVaultSaveChunk([request]),
                  new DataVaultSaveChunk([request]),
              ]));
    }

    Assert.Equal(3, result.RowsWritten);
    Assert.Equal(6, result.SavedRecords.Count);
    AssertSavedRecordsEqual(
        result.SavedRecords.Take(3).ToArray(),
        result.SavedRecords.Skip(3).ToArray());

    await using (var context = new ExplicitSaveServiceContext(options)) {
      Assert.Single(await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().ToListAsync());
      Assert.Single(await context.Set<Dictionary<string, object>>("HubOrder").AsNoTracking().ToListAsync());
      Assert.Single(await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().ToListAsync());
    }
  }

  [Fact]
  public async Task ChunkedSaveCarriesSatelliteHashDiffContinuityAcrossChunks() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var parentHashKey = "customer-hash";
    var firstLoadTimestamp = new DateTimeOffset(2026, 5, 24, 13, 0, 0, TimeSpan.Zero);
    var replayLoadTimestamp = new DateTimeOffset(2026, 5, 24, 13, 5, 0, TimeSpan.Zero);
    var changedLoadTimestamp = new DateTimeOffset(2026, 5, 24, 13, 10, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    DataVaultSaveResult result;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      result = await saveService.SaveAsync(
          context,
          new DataVaultChunkedSaveRequest(
              [
                  new DataVaultSaveChunk([
                      new DataVaultSaveRequest(
                          firstLoadTimestamp,
                          "crm-import",
                          [],
                          [],
                          [new(contact, parentHashKey, [new("Email Address", "first@example.test")], "contact-hash-1")]),
                  ]),
                  new DataVaultSaveChunk([
                      new DataVaultSaveRequest(
                          replayLoadTimestamp,
                          "crm-replay",
                          [],
                          [],
                          [new(contact, parentHashKey, [new("Email Address", "ignored@example.test")], "contact-hash-1")]),
                  ]),
                  new DataVaultSaveChunk([
                      new DataVaultSaveRequest(
                          changedLoadTimestamp,
                          "crm-change",
                          [],
                          [],
                          [new(contact, parentHashKey, [new("Email Address", "changed@example.test")], "contact-hash-2")]),
                  ]),
              ]));
    }

    Assert.Equal(2, result.RowsWritten);
    Assert.Equal(3, result.SavedRecords.Count);

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var rows = (await context.Set<Dictionary<string, object>>("SatCustomerContact")
          .AsNoTracking()
          .ToListAsync())
          .OrderBy(row => (DateTimeOffset)row["LoadTimestamp"])
          .ToArray();

      Assert.Equal(2, rows.Length);
      AssertSatelliteRow(
          rows[0],
          parentHashKey,
          "first@example.test",
          "contact-hash-1",
          firstLoadTimestamp,
          "crm-import");
      AssertSatelliteRow(
          rows[1],
          parentHashKey,
          "changed@example.test",
          "contact-hash-2",
          changedLoadTimestamp,
          "crm-change");
    }
  }

  [Fact]
  public async Task PublicChunkedSaveReportsRetainedStateTelemetryAndReleasesOnSuccess() {
    var observer = new CapturingTelemetryObserver();
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var parentHashKey = "customer-hash";
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();
    services.AddSingleton<IDataVaultTelemetryObserver>(observer);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    DataVaultSaveResult result;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      result = await saveService.SaveAsync(
          context,
          new DataVaultChunkedSaveRequest(
              [
                  new DataVaultSaveChunk([
                      new DataVaultSaveRequest(
                          new DateTimeOffset(2026, 5, 24, 14, 0, 0, TimeSpan.Zero),
                          "crm-import",
                          [],
                          [],
                          [new(contact, parentHashKey, [new("Email Address", "first@example.test")], "contact-hash-1")]),
                  ]),
                  new DataVaultSaveChunk([]),
                  new DataVaultSaveChunk([
                      new DataVaultSaveRequest(
                          new DateTimeOffset(2026, 5, 24, 14, 5, 0, TimeSpan.Zero),
                          "crm-replay",
                          [],
                          [],
                          [new(contact, parentHashKey, [new("Email Address", "ignored@example.test")], "contact-hash-1")]),
                  ]),
              ]));
    }

    Assert.Equal(1, result.RowsWritten);
    Assert.Equal(2, result.SavedRecords.Count);
    var summary = Assert.Single(observer.SaveSummaries);
    Assert.Equal(DataVaultSaveTelemetryOperationKind.ChunkedRequest, summary.OperationKind);
    Assert.Equal(DataVaultTelemetryOutcome.Succeeded, summary.Outcome);
    Assert.Equal(3, summary.ChunkCount);
    Assert.Equal(2, summary.ProcessedChunkCount);
    Assert.Equal(2, summary.RequestCount);
    Assert.Equal(2, summary.SatelliteOperationCount);
    Assert.Equal(0, summary.RetainedStateCurrentCount);
    Assert.Equal(1, summary.RetainedStateHighWaterCount);
    Assert.Empty(summary.ChunkedStateFallbackCauseKinds);
    Assert.Empty(summary.UnsupportedShapeKinds);
    Assert.Contains(DataVaultSaveStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered, summary.FallbackCauseKinds);
    Assert.Contains(
        summary.FallbackExplanations,
        explanation =>
            explanation.Kind == DataVaultSaveStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered &&
            explanation.Remediation.Contains("Register", StringComparison.Ordinal));
    Assert.NotNull(summary.ChunkedTransactionExplanation);
    Assert.Contains(
        "all-or-nothing",
        summary.ChunkedTransactionExplanation!.Remediation,
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task PublicAsyncChunkedSaveReportsRetainedStateTelemetryAndReleasesOnSuccess() {
    var observer = new CapturingTelemetryObserver();
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var parentHashKey = "customer-hash";
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();
    services.AddSingleton<IDataVaultTelemetryObserver>(observer);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    DataVaultSaveResult result;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      result = await saveService.SaveAsync(
          context,
          CreateAsyncChunks(
              [
                  new DataVaultSaveChunk([
                      new DataVaultSaveRequest(
                          new DateTimeOffset(2026, 5, 24, 14, 0, 0, TimeSpan.Zero),
                          "crm-import",
                          [],
                          [],
                          [new(contact, parentHashKey, [new("Email Address", "first@example.test")], "contact-hash-1")]),
                  ]),
                  new DataVaultSaveChunk([]),
                  new DataVaultSaveChunk([
                      new DataVaultSaveRequest(
                          new DateTimeOffset(2026, 5, 24, 14, 5, 0, TimeSpan.Zero),
                          "crm-replay",
                          [],
                          [],
                          [new(contact, parentHashKey, [new("Email Address", "ignored@example.test")], "contact-hash-1")]),
                  ]),
              ]));
    }

    Assert.Equal(1, result.RowsWritten);
    Assert.Equal(2, result.SavedRecords.Count);
    var summary = Assert.Single(observer.SaveSummaries);
    Assert.Equal(DataVaultSaveTelemetryOperationKind.ChunkedRequest, summary.OperationKind);
    Assert.Equal(DataVaultTelemetryOutcome.Succeeded, summary.Outcome);
    Assert.Equal(3, summary.ChunkCount);
    Assert.Equal(2, summary.ProcessedChunkCount);
    Assert.Equal(2, summary.RequestCount);
    Assert.Equal(2, summary.SatelliteOperationCount);
    Assert.Equal(0, summary.RetainedStateCurrentCount);
    Assert.Equal(1, summary.RetainedStateHighWaterCount);
    Assert.Empty(summary.ChunkedStateFallbackCauseKinds);
    Assert.Empty(summary.UnsupportedShapeKinds);
    Assert.NotNull(summary.ChunkedTransactionExplanation);
  }

  [Fact]
  public async Task PublicChunkedSaveReleasesRetainedStateTelemetryOnFailure() {
    var observer = new CapturingTelemetryObserver();
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var parentHashKey = "customer-hash";
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();
    services.AddSingleton<IDataVaultTelemetryObserver>(observer);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new ExplicitSaveServiceContext(options);
    await context.Database.EnsureCreatedAsync();

    await Assert.ThrowsAsync<ArgumentException>(() =>
        saveService.SaveAsync(
            context,
            new DataVaultChunkedSaveRequest(
                [
                    new DataVaultSaveChunk([
                        new DataVaultSaveRequest(
                            new DateTimeOffset(2026, 5, 24, 14, 30, 0, TimeSpan.Zero),
                            "crm-import",
                            [],
                            [],
                            [new(contact, parentHashKey, [new("Email Address", "first@example.test")], "contact-hash-1")]),
                    ]),
                    new DataVaultSaveChunk([
                        new DataVaultSaveRequest(
                            new DateTimeOffset(2026, 5, 24, 14, 35, 0, TimeSpan.Zero),
                            "crm-bad",
                            [],
                            [],
                            [new(contact, parentHashKey, [new("Unexpected Payload", "bad")], "contact-hash-2")]),
                    ]),
                ])));

    var summary = Assert.Single(observer.SaveSummaries);
    Assert.Equal(DataVaultSaveTelemetryOperationKind.ChunkedRequest, summary.OperationKind);
    Assert.Equal(DataVaultTelemetryOutcome.Failed, summary.Outcome);
    Assert.Equal(2, summary.ChunkCount);
    Assert.Equal(2, summary.ProcessedChunkCount);
    Assert.Equal(0, summary.RowsWritten);
    Assert.Equal(0, summary.RetainedStateCurrentCount);
    Assert.Equal(1, summary.RetainedStateHighWaterCount);
    Assert.NotNull(summary.ChunkedTransactionExplanation);
    Assert.Contains(
        "current transaction",
        summary.ChunkedTransactionExplanation!.Explanation,
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task PublicAsyncChunkedSaveReleasesRetainedStateTelemetryOnFailureAndStopsEnumeration() {
    var observer = new CapturingTelemetryObserver();
    var requestedChunkOrdinals = new List<int>();
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var parentHashKey = "customer-hash";
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();
    services.AddSingleton<IDataVaultTelemetryObserver>(observer);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new ExplicitSaveServiceContext(options);
    await context.Database.EnsureCreatedAsync();

    await Assert.ThrowsAsync<ArgumentException>(() =>
        saveService.SaveAsync(
            context,
            CreateCountingAsyncChunks(
                [
                    new DataVaultSaveChunk([
                        new DataVaultSaveRequest(
                            new DateTimeOffset(2026, 5, 24, 14, 30, 0, TimeSpan.Zero),
                            "crm-import",
                            [],
                            [],
                            [new(contact, parentHashKey, [new("Email Address", "first@example.test")], "contact-hash-1")]),
                    ]),
                    new DataVaultSaveChunk([
                        new DataVaultSaveRequest(
                            new DateTimeOffset(2026, 5, 24, 14, 35, 0, TimeSpan.Zero),
                            "crm-bad",
                            [],
                            [],
                            [new(contact, parentHashKey, [new("Unexpected Payload", "bad")], "contact-hash-2")]),
                    ]),
                    new DataVaultSaveChunk([
                        new DataVaultSaveRequest(
                            new DateTimeOffset(2026, 5, 24, 14, 40, 0, TimeSpan.Zero),
                            "crm-later",
                            [],
                            [],
                            [new(contact, parentHashKey, [new("Email Address", "later@example.test")], "contact-hash-3")]),
                    ]),
                ],
                requestedChunkOrdinals.Add)));

    Assert.Equal([0, 1], requestedChunkOrdinals);
    var summary = Assert.Single(observer.SaveSummaries);
    Assert.Equal(DataVaultSaveTelemetryOperationKind.ChunkedRequest, summary.OperationKind);
    Assert.Equal(DataVaultTelemetryOutcome.Failed, summary.Outcome);
    Assert.Equal(2, summary.ChunkCount);
    Assert.Equal(2, summary.ProcessedChunkCount);
    Assert.Equal(0, summary.RowsWritten);
    Assert.Equal(0, summary.RetainedStateCurrentCount);
    Assert.Equal(1, summary.RetainedStateHighWaterCount);
    Assert.NotNull(summary.ChunkedTransactionExplanation);
  }

  [Fact]
  public async Task PublicChunkedSaveReleasesRetainedStateTelemetryOnCancellationBeforeLaterChunks() {
    var observer = new CapturingTelemetryObserver();
    using var cancellationSource = new CancellationTokenSource();
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var parentHashKey = "customer-hash";
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, ExplicitSaveServiceModelCacheKeyFactory>()
        .AddInterceptors(new CancelAfterFirstSaveChangesInterceptor(cancellationSource))
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();
    services.AddSingleton<IDataVaultTelemetryObserver>(observer);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new ExplicitSaveServiceContext(options);
    await context.Database.EnsureCreatedAsync();

    await Assert.ThrowsAsync<OperationCanceledException>(() =>
        saveService.SaveAsync(
            context,
            new DataVaultChunkedSaveRequest(
                [
                    new DataVaultSaveChunk([
                        new DataVaultSaveRequest(
                            new DateTimeOffset(2026, 5, 24, 15, 0, 0, TimeSpan.Zero),
                            "crm-import",
                            [],
                            [],
                            [new(contact, parentHashKey, [new("Email Address", "first@example.test")], "contact-hash-1")]),
                    ]),
                    new DataVaultSaveChunk([
                        new DataVaultSaveRequest(
                            new DateTimeOffset(2026, 5, 24, 15, 5, 0, TimeSpan.Zero),
                            "crm-change",
                            [],
                            [],
                            [new(contact, parentHashKey, [new("Email Address", "changed@example.test")], "contact-hash-2")]),
                    ]),
                ]),
            cancellationSource.Token));

    var summary = Assert.Single(observer.SaveSummaries);
    Assert.Equal(DataVaultTelemetryOutcome.Failed, summary.Outcome);
    Assert.Equal(2, summary.ChunkCount);
    Assert.Equal(1, summary.ProcessedChunkCount);
    Assert.Equal(0, summary.RetainedStateCurrentCount);
    Assert.Equal(1, summary.RetainedStateHighWaterCount);
    Assert.NotNull(summary.ChunkedTransactionExplanation);
    Assert.Contains(
        "all-or-nothing",
        summary.ChunkedTransactionExplanation!.Remediation,
        StringComparison.Ordinal);
  }

  [Fact]
  public async Task PublicChunkedSaveClassifiesRetainedStateLimitFallbackWithoutRawValues() {
    var observer = new CapturingTelemetryObserver();
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var saveService = new DefaultDataVaultSaveService(
        DefaultStableHashService.Instance,
        DefaultStableHashNormalizer.Instance,
        [DefaultDataVaultLoadTimestampResolver.Instance],
        [DefaultDataVaultRecordSourceResolver.Instance],
        [],
        [observer],
        chunkedRetainedSatelliteSeriesLimit: 1);

    await using var context = new ExplicitSaveServiceContext(options);
    await context.Database.EnsureCreatedAsync();

    var result = await saveService.SaveAsync(
        context,
        new DataVaultChunkedSaveRequest(
            [
                new DataVaultSaveChunk([
                    new DataVaultSaveRequest(
                        new DateTimeOffset(2026, 5, 24, 15, 30, 0, TimeSpan.Zero),
                        "crm-import",
                        [],
                        [],
                        [
                            new(contact, "customer-hash-1", [new("Email Address", "first@example.test")], "contact-hash-1"),
                            new(contact, "customer-hash-2", [new("Email Address", "second@example.test")], "contact-hash-2"),
                        ]),
                ]),
            ]));

    Assert.Equal(2, result.RowsWritten);
    var summary = Assert.Single(observer.SaveSummaries);
    Assert.Equal(0, summary.RetainedStateCurrentCount);
    Assert.Equal(1, summary.RetainedStateHighWaterCount);
    Assert.Equal(
        [DataVaultChunkedSaveStateFallbackCauseKind.RetainedSatelliteSeriesLimitReached],
        summary.ChunkedStateFallbackCauseKinds);
    Assert.Equal(
        [DataVaultChunkedSaveUnsupportedShapeKind.RetainedSatelliteSeriesLimitExceeded],
        summary.UnsupportedShapeKinds);
    var stateFallbackExplanation = Assert.Single(summary.ChunkedStateFallbackExplanations);
    Assert.Equal(
        DataVaultChunkedSaveStateFallbackCauseKind.RetainedSatelliteSeriesLimitReached,
        stateFallbackExplanation.Kind);
    Assert.Contains("10000", stateFallbackExplanation.Remediation, StringComparison.Ordinal);
    Assert.DoesNotContain("customer-hash", stateFallbackExplanation.Explanation, StringComparison.Ordinal);
    Assert.DoesNotContain("customer-hash", stateFallbackExplanation.Remediation, StringComparison.Ordinal);
    var unsupportedShapeExplanation = Assert.Single(summary.UnsupportedShapeExplanations);
    Assert.Equal(
        DataVaultChunkedSaveUnsupportedShapeKind.RetainedSatelliteSeriesLimitExceeded,
        unsupportedShapeExplanation.Kind);
    Assert.Contains("retained-state budget", unsupportedShapeExplanation.Remediation, StringComparison.Ordinal);
  }

  [Fact]
  public async Task DefaultSaveServicePersistsCustomerProfileSatelliteHistoryThroughSqlite() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["customer_name", "customer_status"]);
    var firstLoadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    var secondLoadTimestamp = new DateTimeOffset(2026, 4, 29, 11, 30, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string customerHashKey;
    DataVaultSaveResult firstSatelliteResult;

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var firstHubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "crm-import",
              [new(customer, [new("Customer Id", "C-100")])],
              []));
      customerHashKey = GetHashKey(firstHubResult, DataVaultTableKind.Hub, "Customer");

      firstSatelliteResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "crm-import",
              [],
              [],
              [
                  new(
                      profile,
                      customerHashKey,
                      [new("customer_name", "Alice Adams"), new("customer_status", "prospect")],
                      "profile-hash-1"),
              ]));

      Assert.Equal(1, firstHubResult.RowsWritten);
      Assert.Equal(1, firstSatelliteResult.RowsWritten);
    }

    DataVaultSaveResult secondHubResult;
    DataVaultSaveResult secondSatelliteResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      secondHubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              secondLoadTimestamp,
              "crm-change",
              [new(customer, [new("Customer Id", "C-100")])],
              []));

      secondSatelliteResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              secondLoadTimestamp,
              "crm-change",
              [],
              [],
              [
                  new(
                      profile,
                      customerHashKey,
                      [new("customer_name", "Alice Baker"), new("customer_status", "active")],
                      "profile-hash-2"),
              ]));
    }

    Assert.Equal(0, secondHubResult.RowsWritten);
    Assert.Equal(1, secondSatelliteResult.RowsWritten);
    AssertSingleSavedRecord(
        firstSatelliteResult,
        DataVaultTableKind.Satellite,
        "Profile",
        "SatCustomerProfile",
        customerHashKey);
    AssertSingleSavedRecord(
        secondSatelliteResult,
        DataVaultTableKind.Satellite,
        "Profile",
        "SatCustomerProfile",
        customerHashKey);

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var customerRows = await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().ToListAsync();
      var allProfileRows = await context.Set<Dictionary<string, object>>("SatCustomerProfile")
          .AsNoTracking()
          .ToListAsync();
      var profileRows = allProfileRows
          .Where(row => Assert.IsType<string>(row["CustomerHashKey"]) == customerHashKey)
          .OrderBy(row => (DateTimeOffset)row["LoadTimestamp"])
          .ToArray();
      var customerRow = Assert.Single(customerRows);

      Assert.Equal("C-100", customerRow["CustomerId"]);
      Assert.Equal(customerHashKey, customerRow["CustomerHashKey"]);
      Assert.Equal(2, allProfileRows.Count);
      Assert.Equal(2, profileRows.Length);
      AssertCustomerProfileSatelliteRow(
          profileRows[0],
          customerHashKey,
          "Alice Adams",
          "prospect",
          "profile-hash-1",
          firstLoadTimestamp,
          "crm-import");
      AssertCustomerProfileSatelliteRow(
          profileRows[1],
          customerHashKey,
          "Alice Baker",
          "active",
          "profile-hash-2",
          secondLoadTimestamp,
          "crm-change");
    }
  }

  [Fact]
  public async Task ReadServiceReadsLatestAndAsOfCustomerProfileSatelliteRowsThroughSqlite() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["customer_name", "customer_status"]);
    var firstLoadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    var secondLoadTimestamp = new DateTimeOffset(2026, 4, 29, 11, 30, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    string customerHashKey;

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "crm-import",
              [new(customer, [new("Customer Id", "C-100")])],
              []));
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "crm-import",
              [],
              [],
              [
                  new(
                      profile,
                      customerHashKey,
                      [new("customer_name", "Alice Adams"), new("customer_status", "prospect")],
                      "profile-hash-1"),
              ]));
      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              secondLoadTimestamp,
              "crm-change",
              [],
              [],
              [
                  new(
                      profile,
                      customerHashKey,
                      [new("customer_name", "Alice Baker"), new("customer_status", "active")],
                      "profile-hash-2"),
              ]));
    }

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var latestRows = await readService.ReadLatestSatelliteRowsAsync(
          context,
          new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey]));
      var currentRows = await readService.ReadCurrentSatelliteRowsAsync(
          context,
          profile,
          [customerHashKey]);
      var latestRow = Assert.Single(latestRows);
      var currentRow = Assert.Single(currentRows);

      Assert.Equal("Profile", latestRow.MetadataName);
      Assert.Equal("SatCustomerProfile", latestRow.TableName);
      Assert.Equal(customerHashKey, latestRow.ParentHashKey);
      Assert.Empty(latestRow.DrivingKeyValues);
      Assert.Equal("profile-hash-2", latestRow.HashDiff);
      Assert.Equal(secondLoadTimestamp, latestRow.LoadTimestamp);
      Assert.Equal("crm-change", latestRow.RecordSource);
      Assert.Equal("Alice Baker", latestRow.PayloadValues["customer_name"]);
      Assert.Equal("active", latestRow.PayloadValues["customer_status"]);
      Assert.Equal(latestRow.MetadataName, currentRow.MetadataName);
      Assert.Equal(latestRow.TableName, currentRow.TableName);
      Assert.Equal(latestRow.ParentHashKey, currentRow.ParentHashKey);
      Assert.Equal(latestRow.HashDiff, currentRow.HashDiff);
      Assert.Equal(latestRow.LoadTimestamp, currentRow.LoadTimestamp);
      Assert.Equal(latestRow.RecordSource, currentRow.RecordSource);
      Assert.Equal(latestRow.PayloadValues, currentRow.PayloadValues);

      var asOfRows = await readService.ReadLatestSatelliteRowsAsync(
          context,
          new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey], firstLoadTimestamp));
      var convenienceAsOfRows = await readService.ReadAsOfSatelliteRowsAsync(
          context,
          profile,
          [customerHashKey],
          firstLoadTimestamp);
      var asOfRow = Assert.Single(asOfRows);
      var convenienceAsOfRow = Assert.Single(convenienceAsOfRows);

      Assert.Equal("profile-hash-1", asOfRow.HashDiff);
      Assert.Equal(firstLoadTimestamp, asOfRow.LoadTimestamp);
      Assert.Equal("prospect", asOfRow.PayloadValues["customer_status"]);
      Assert.Equal(asOfRow.ParentHashKey, convenienceAsOfRow.ParentHashKey);
      Assert.Equal(asOfRow.HashDiff, convenienceAsOfRow.HashDiff);
      Assert.Equal(asOfRow.LoadTimestamp, convenienceAsOfRow.LoadTimestamp);
      Assert.Equal(asOfRow.RecordSource, convenienceAsOfRow.RecordSource);
      Assert.Equal(asOfRow.PayloadValues, convenienceAsOfRow.PayloadValues);

      Assert.Empty(await readService.ReadLatestSatelliteRowsAsync(
          context,
          new DataVaultLatestSatelliteReadRequest(profile, ["missing-hash-key"])));
      Assert.Empty(await readService.ReadCurrentSatelliteRowsAsync(
          context,
          profile,
          ["missing-hash-key"]));
    }
  }

  [Theory]
  [InlineData(DataVaultHashKeyStorageProfile.HexString, "text", StableHashDigestByteLength * 2)]
  [InlineData(DataVaultHashKeyStorageProfile.Binary, "blob", StableHashDigestByteLength)]
  public async Task SaveAndReadServicesRoundTripHashKeyStorageProfilesThroughSqlite(
      DataVaultHashKeyStorageProfile storageProfile,
      string expectedStorageClass,
      int expectedLength) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["customer_name", "customer_status"]);
    var firstLoadTimestamp = new DateTimeOffset(2026, 6, 1, 8, 0, 0, TimeSpan.Zero);
    var secondLoadTimestamp = new DateTimeOffset(2026, 6, 1, 9, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault(configure => configure.UseStableHashAlgorithm(StableHashAlgorithmId));
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    string customerHashKey;

    await using (var context = new ExplicitSaveServiceContext(options, storageProfile: storageProfile)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "crm-import",
              [new(customer, [new("Customer Id", "C-STORAGE")])],
              []));
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "crm-import",
              [],
              [],
              [
                  new(
                      profile,
                      customerHashKey,
                      [new("customer_name", "Alice Adams"), new("customer_status", "prospect")],
                      "profile-hash-1"),
              ]));
      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              secondLoadTimestamp,
              "crm-change",
              [],
              [],
              [
                  new(
                      profile,
                      customerHashKey,
                      [new("customer_name", "Alice Baker"), new("customer_status", "active")],
                      "profile-hash-2"),
              ]));
    }

    using (var connection = database.CreateOpenConnection()) {
      AssertSqliteHashStorage(
          connection,
          "HubCustomer",
          "CustomerHashKey",
          customerHashKey,
          expectedStorageClass,
          expectedLength);
      AssertSqliteHashStorage(
          connection,
          "SatCustomerProfile",
          "CustomerHashKey",
          customerHashKey,
          expectedStorageClass,
          expectedLength);
    }

    await using (var context = new ExplicitSaveServiceContext(options, storageProfile: storageProfile)) {
      var latestRows = await readService.ReadLatestSatelliteRowsAsync(
          context,
          new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey]));
      var currentRows = await readService.ReadCurrentSatelliteRowsAsync(
          context,
          profile,
          [customerHashKey]);
      var asOfRows = await readService.ReadAsOfSatelliteRowsAsync(
          context,
          profile,
          [customerHashKey],
          firstLoadTimestamp);
      var latestRow = Assert.Single(latestRows);
      var currentRow = Assert.Single(currentRows);
      var asOfRow = Assert.Single(asOfRows);

      Assert.Equal(customerHashKey, latestRow.ParentHashKey);
      Assert.Equal(customerHashKey, currentRow.ParentHashKey);
      Assert.Equal(customerHashKey, asOfRow.ParentHashKey);
      Assert.Equal("active", latestRow.PayloadValues["customer_status"]);
      Assert.Equal("active", currentRow.PayloadValues["customer_status"]);
      Assert.Equal("prospect", asOfRow.PayloadValues["customer_status"]);
    }
  }

  [Fact]
  public async Task BinaryHashKeyStorageProfileRejectsWrongDigestParticipantBeforeSqliteWrite() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var loadTimestamp = new DateTimeOffset(2026, 6, 1, 8, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateExplicitSaveServiceOptions(database);
    var services = new ServiceCollection();
    services.AddDVault(configure => configure.UseStableHashAlgorithm(StableHashAlgorithmId));
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using (var context = new ExplicitSaveServiceContext(options, storageProfile: DataVaultHashKeyStorageProfile.Binary)) {
      await context.Database.EnsureCreatedAsync();

      var exception = await Assert.ThrowsAsync<FormatException>(() =>
          saveService.SaveAsync(
              context,
              new DataVaultSaveRequest(
                  loadTimestamp,
                  "crm-import",
                  [],
                  [
                      new(
                          customerOrder,
                          [
                              new("Customer", CreateCanonicalHexDigest(StableHashDigestByteLength)),
                              new("Order", "abcd"),
                          ]),
                  ])));

      Assert.Contains("32 lowercase hexadecimal characters", exception.Message, StringComparison.Ordinal);
    }

    using var connection = database.CreateOpenConnection();
    Assert.Equal("0", connection.ExecuteScalarString("SELECT count(*) FROM \"LinkCustomerOrder\";"));
  }

  [Fact]
  public async Task RegistryBackedSaveAndReadResolveAppDefaultMetadataThroughDbContextOptions() {
    var firstLoadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    var secondLoadTimestamp = new DateTimeOffset(2026, 4, 29, 11, 30, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataModel(CreateMetadataModel()));
    services.AddDVaultSqlite();
    services.AddDbContext<RegistryBackedSaveServiceContext>(
        options => options
            .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
            .UseDataVaultMetadata());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    string customerHashKey;
    string orderHashKey;

    using (var scope = provider.CreateScope()) {
      var context = scope.ServiceProvider.GetRequiredService<RegistryBackedSaveServiceContext>();
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

      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
      orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");

      var detailResult = await saveService.SaveAsync(
          context,
          new DataVaultRegistrySaveRequest(
              firstLoadTimestamp,
              "crm-import",
              [],
              [
                  new("CustomerOrder", [new("Customer", customerHashKey), new("Order", orderHashKey)]),
              ],
              [
                  new(
                      DataVaultMetadataReference.Hub("Customer"),
                      "Profile",
                      customerHashKey,
                      [new("customer_name", "Alice Adams"), new("customer_status", "prospect")],
                      "profile-hash-1"),
              ]));

      var changedProfileResult = await saveService.SaveAsync(
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
              ]));

      Assert.Equal(2, hubResult.RowsWritten);
      Assert.Equal(2, detailResult.RowsWritten);
      Assert.Equal(1, changedProfileResult.RowsWritten);
    }

    using (var scope = provider.CreateScope()) {
      var context = scope.ServiceProvider.GetRequiredService<RegistryBackedSaveServiceContext>();
      var latestRows = await readService.ReadLatestSatelliteRowsAsync(
          context,
          new DataVaultRegistryLatestSatelliteReadRequest(
              DataVaultMetadataReference.Hub("Customer"),
              "Profile",
              [customerHashKey]));
      var currentRows = await readService.ReadCurrentSatelliteRowsAsync(
          context,
          DataVaultMetadataReference.Hub("Customer"),
          "Profile",
          [customerHashKey]);
      var asOfRows = await readService.ReadLatestSatelliteRowsAsync(
          context,
          new DataVaultRegistryLatestSatelliteReadRequest(
              DataVaultMetadataReference.Hub("Customer"),
              "Profile",
              [customerHashKey],
              firstLoadTimestamp));
      var convenienceAsOfRows = await readService.ReadAsOfSatelliteRowsAsync(
          context,
          DataVaultMetadataReference.Hub("Customer"),
          "Profile",
          [customerHashKey],
          firstLoadTimestamp);
      var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().SingleAsync();
      var latestRow = Assert.Single(latestRows);
      var currentRow = Assert.Single(currentRows);
      var asOfRow = Assert.Single(asOfRows);
      var convenienceAsOfRow = Assert.Single(convenienceAsOfRows);

      Assert.Equal(customerHashKey, linkRow["CustomerHashKey"]);
      Assert.Equal(orderHashKey, linkRow["OrderHashKey"]);
      Assert.Equal("profile-hash-2", latestRow.HashDiff);
      Assert.Equal("active", latestRow.PayloadValues["customer_status"]);
      Assert.Equal(latestRow.ParentHashKey, currentRow.ParentHashKey);
      Assert.Equal(latestRow.HashDiff, currentRow.HashDiff);
      Assert.Equal(latestRow.LoadTimestamp, currentRow.LoadTimestamp);
      Assert.Equal(latestRow.PayloadValues, currentRow.PayloadValues);
      Assert.Equal("profile-hash-1", asOfRow.HashDiff);
      Assert.Equal("prospect", asOfRow.PayloadValues["customer_status"]);
      Assert.Equal(asOfRow.ParentHashKey, convenienceAsOfRow.ParentHashKey);
      Assert.Equal(asOfRow.HashDiff, convenienceAsOfRow.HashDiff);
      Assert.Equal(asOfRow.LoadTimestamp, convenienceAsOfRow.LoadTimestamp);
      Assert.Equal(asOfRow.PayloadValues, convenienceAsOfRow.PayloadValues);
    }
  }

  [Fact]
  public async Task RegistryBackedSaveUsesContextScopedRegistryOverride() {
    var loadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataModel(CreateCustomerOnlyMetadataModel()));
    services.AddDVaultSqlite();
    services.AddDbContext<RegistryBackedSaveServiceContext>(
        options => options
            .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
            .UseDataVaultMetadata(DataVaultMetadataRegistry.Create(CreateOrderOnlyMetadataModel())));

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    using (var scope = provider.CreateScope()) {
      var context = scope.ServiceProvider.GetRequiredService<RegistryBackedSaveServiceContext>();
      await context.Database.EnsureCreatedAsync();

      var result = await saveService.SaveAsync(
          context,
          new DataVaultRegistrySaveRequest(
              loadTimestamp,
              "order-import",
              [new("Order", [new("Order Id", "O-200")])],
              []));

      AssertSingleSavedRecord(
          result,
          DataVaultTableKind.Hub,
          "Order",
          "HubOrder",
          GetHashKey(result, DataVaultTableKind.Hub, "Order"));
      Assert.Contains("HubOrder", context.Model.GetEntityTypes().Select(entity => entity.Name));
      Assert.DoesNotContain("HubCustomer", context.Model.GetEntityTypes().Select(entity => entity.Name));
    }
  }

  [Fact]
  public async Task RegistryBackedSaveFailsBeforeWritesWhenDbContextHasNoAuthoritativeRegistry() {
    var loadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new ExplicitSaveServiceContext(options);
    await context.Database.EnsureCreatedAsync();

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        saveService.SaveAsync(
            context,
            new DataVaultRegistrySaveRequest(
                loadTimestamp,
                "crm-import",
                [new("Customer", [new("Customer Id", "C-100")])],
                [])));

    Assert.Contains("UseDataVaultMetadata", exception.Message, StringComparison.Ordinal);
    Assert.Empty(await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().ToListAsync());
  }

  [Fact]
  public async Task RegistryBackedSaveAndReadFailBeforeOrchestrationWhenMetadataEntryIsMissing() {
    var loadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataModel(CreateCustomerOnlyMetadataModel()));
    services.AddDVaultSqlite();
    services.AddDbContext<RegistryBackedSaveServiceContext>(
        options => options
            .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
            .UseDataVaultMetadata());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();

    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RegistryBackedSaveServiceContext>();
    await context.Database.EnsureCreatedAsync();

    var saveException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        saveService.SaveAsync(
            context,
            new DataVaultRegistrySaveRequest(
                loadTimestamp,
                "crm-import",
                [new("Customer", [new("Customer Id", "C-100")])],
                [new("MissingLink", [new("Customer", "customer-hash")])])));
    var readException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadLatestSatelliteRowsAsync(
            context,
            new DataVaultRegistryLatestSatelliteReadRequest(
                DataVaultMetadataReference.Hub("Customer"),
                "MissingProfile",
                ["customer-hash"])));
    var currentReadException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadCurrentSatelliteRowsAsync(
            context,
            DataVaultMetadataReference.Hub("Customer"),
            "MissingProfile",
            ["customer-hash"]));
    var asOfReadException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadAsOfSatelliteRowsAsync(
            context,
            DataVaultMetadataReference.Hub("Customer"),
            "MissingProfile",
            ["customer-hash"],
            loadTimestamp));

    Assert.Contains("link metadata 'MissingLink'", saveException.Message, StringComparison.Ordinal);
    Assert.Contains("satellite metadata 'MissingProfile'", readException.Message, StringComparison.Ordinal);
    Assert.Contains("satellite metadata 'MissingProfile'", currentReadException.Message, StringComparison.Ordinal);
    Assert.Contains("satellite metadata 'MissingProfile'", asOfReadException.Message, StringComparison.Ordinal);
    Assert.Empty(await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().ToListAsync());
  }

  [Fact]
  public async Task AddDVaultSqliteRegistersOptimizedStrategyForCleanSqliteContexts() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var loadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    var request = new DataVaultSaveRequest(
        loadTimestamp,
        "crm-import",
        [new(customer, [new("Customer Id", "C-100")])],
        []);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var strategy = Assert.Single(provider.GetServices<IDataVaultProviderSaveStrategy>());

    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      Assert.True(strategy.CanSave(context, [request]));

      var result = await saveService.SaveAsync(context, request);

      Assert.Equal(1, result.RowsWritten);
      AssertSingleSavedRecord(
          result,
          DataVaultTableKind.Hub,
          "Customer",
          "HubCustomer",
          GetHashKey(result, DataVaultTableKind.Hub, "Customer"));
    }

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var customerRow = await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().SingleAsync();

      Assert.Equal("C-100", customerRow["CustomerId"]);
      Assert.Equal("crm-import", customerRow["RecordSource"]);
      Assert.Equal(loadTimestamp, customerRow["LoadTimestamp"]);
    }
  }

  [Theory]
  [InlineData(DataVaultLoadTimestampStorage.Iso8601UtcText)]
  [InlineData(DataVaultLoadTimestampStorage.UtcTicks)]
  public async Task AddDVaultSqliteOptimizedStrategySupportsConfiguredLoadTimestampStorage(
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var firstLoadTimestamp = new DateTimeOffset(2026, 5, 8, 8, 0, 0, TimeSpan.Zero);
    var replayLoadTimestamp = new DateTimeOffset(2026, 5, 8, 8, 30, 0, TimeSpan.Zero);
    var changedLoadTimestamp = new DateTimeOffset(2026, 5, 8, 9, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, ExplicitSaveServiceModelCacheKeyFactory>()
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string customerHashKey;

    await using (var context = new ExplicitSaveServiceContext(options, loadTimestampStorage)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "sqlite-storage-test",
              [new(customer, [new("Customer Id", "C-STORAGE")])],
              []));

      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
    }

    DataVaultSaveResult firstResult;
    await using (var context = new ExplicitSaveServiceContext(options, loadTimestampStorage)) {
      firstResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstLoadTimestamp,
              "sqlite-storage-test",
              [],
              [],
              [new(contact, customerHashKey, [new("Email Address", "first@example.test")], "contact-hash-1")]));
    }

    DataVaultSaveResult replayResult;
    await using (var context = new ExplicitSaveServiceContext(options, loadTimestampStorage)) {
      replayResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              replayLoadTimestamp,
              "sqlite-storage-test",
              [],
              [],
              [new(contact, customerHashKey, [new("Email Address", "ignored@example.test")], "contact-hash-1")]));
    }

    DataVaultSaveResult changedResult;
    await using (var context = new ExplicitSaveServiceContext(options, loadTimestampStorage)) {
      changedResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              changedLoadTimestamp,
              "sqlite-storage-test",
              [],
              [],
              [new(contact, customerHashKey, [new("Email Address", "changed@example.test")], "contact-hash-2")]));
    }

    Assert.Equal(1, firstResult.RowsWritten);
    Assert.Equal(0, replayResult.RowsWritten);
    Assert.Equal(1, changedResult.RowsWritten);

    await using (var context = new ExplicitSaveServiceContext(options, loadTimestampStorage)) {
      var rows = (await context.Set<Dictionary<string, object>>("SatCustomerContact").AsNoTracking().ToListAsync())
          .OrderBy(row => DataVaultLoadTimestampValueConverter.ReadProviderValue(row["LoadTimestamp"]))
          .ToArray();

      Assert.Equal(2, rows.Length);
      Assert.Equal(firstLoadTimestamp, DataVaultLoadTimestampValueConverter.ReadProviderValue(rows[0]["LoadTimestamp"]));
      Assert.Equal(changedLoadTimestamp, DataVaultLoadTimestampValueConverter.ReadProviderValue(rows[1]["LoadTimestamp"]));
      if (loadTimestampStorage == DataVaultLoadTimestampStorage.UtcTicks) {
        Assert.IsType<long>(rows[0]["LoadTimestamp"]);
      }
      else {
        Assert.IsType<string>(rows[0]["LoadTimestamp"]);
      }
    }
  }

  [Fact]
  public async Task DefaultSaveServicePersistsMultiActiveSatelliteRowsByCanonicalDrivingKeysThroughSqlite() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contactChannel = new DataVaultSatelliteMetadata(
        "ContactChannel",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type", "Region Code"]);
    var parentHashKey = "customer-hash";
    var firstLoadTimestamp = new DateTimeOffset(2026, 5, 6, 10, 0, 0, TimeSpan.Zero);
    var replayLoadTimestamp = new DateTimeOffset(2026, 5, 6, 10, 30, 0, TimeSpan.Zero);
    var changedLoadTimestamp = new DateTimeOffset(2026, 5, 6, 11, 0, 0, TimeSpan.Zero);
    var firstRequest = new DataVaultSaveRequest(
        firstLoadTimestamp,
        "crm-import",
        [],
        [],
        [
            new(
                contactChannel,
                parentHashKey,
                [new("Region Code", "DE"), new("Contact Type", "billing")],
                [new("Email Address", "billing-de@example.test")],
                "contact-channel-hash-1"),
            new(
                contactChannel,
                parentHashKey,
                [new("Contact Type", "shipping"), new("Region Code", "DE")],
                [new("Email Address", "shipping-de@example.test")],
                "contact-channel-hash-2"),
        ]);
    var replayRequest = new DataVaultSaveRequest(
        replayLoadTimestamp,
        "crm-replay",
        [],
        [],
        [
            new(
                contactChannel,
                parentHashKey,
                [new("Region Code", "DE"), new("Contact Type", "billing")],
                [new("Email Address", "ignored-replay@example.test")],
                "contact-channel-hash-1"),
        ]);
    var changedRequest = new DataVaultSaveRequest(
        changedLoadTimestamp,
        "crm-change",
        [],
        [],
        [
            new(
                contactChannel,
                parentHashKey,
                [new("Contact Type", "billing"), new("Region Code", "DE")],
                [new("Email Address", "billing-de-new@example.test")],
                "contact-channel-hash-3"),
        ]);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var strategy = Assert.Single(provider.GetServices<IDataVaultProviderSaveStrategy>());

    DataVaultSaveResult firstResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      await context.Database.EnsureCreatedAsync();

      Assert.False(strategy.CanSave(context, [firstRequest]));

      firstResult = await saveService.SaveAsync(context, firstRequest);
    }

    DataVaultSaveResult replayResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      replayResult = await saveService.SaveAsync(context, replayRequest);
    }

    DataVaultSaveResult changedResult;
    await using (var context = new ExplicitSaveServiceContext(options)) {
      changedResult = await saveService.SaveAsync(context, changedRequest);
    }

    Assert.Equal(2, firstResult.RowsWritten);
    Assert.Equal(0, replayResult.RowsWritten);
    Assert.Equal(1, changedResult.RowsWritten);
    Assert.Collection(
        firstResult.SavedRecords,
        record => AssertMultiActiveSavedRecord(record, parentHashKey, "billing", "DE"),
        record => AssertMultiActiveSavedRecord(record, parentHashKey, "shipping", "DE"));
    AssertMultiActiveSavedRecord(Assert.Single(replayResult.SavedRecords), parentHashKey, "billing", "DE");
    AssertMultiActiveSavedRecord(Assert.Single(changedResult.SavedRecords), parentHashKey, "billing", "DE");

    await using (var context = new ExplicitSaveServiceContext(options)) {
      var rows = await context.Set<Dictionary<string, object>>("SatCustomerContactChannel")
          .AsNoTracking()
          .ToListAsync();
      var billingRows = rows
          .Where(row =>
              Assert.IsType<string>(row["ContactType"]) == "billing" &&
              Assert.IsType<string>(row["RegionCode"]) == "DE")
          .OrderBy(row => (DateTimeOffset)row["LoadTimestamp"])
          .ToArray();
      var shippingRow = Assert.Single(rows.Where(row =>
          Assert.IsType<string>(row["ContactType"]) == "shipping" &&
          Assert.IsType<string>(row["RegionCode"]) == "DE"));

      Assert.Equal(3, rows.Count);
      Assert.Equal(2, billingRows.Length);
      AssertMultiActiveSatelliteRow(
          billingRows[0],
          parentHashKey,
          "billing",
          "DE",
          "billing-de@example.test",
          "contact-channel-hash-1",
          firstLoadTimestamp,
          "crm-import");
      AssertMultiActiveSatelliteRow(
          billingRows[1],
          parentHashKey,
          "billing",
          "DE",
          "billing-de-new@example.test",
          "contact-channel-hash-3",
          changedLoadTimestamp,
          "crm-change");
      AssertMultiActiveSatelliteRow(
          shippingRow,
          parentHashKey,
          "shipping",
          "DE",
          "shipping-de@example.test",
          "contact-channel-hash-2",
          firstLoadTimestamp,
          "crm-import");
    }
  }

  private static string GetHashKey(DataVaultSaveResult result, DataVaultTableKind kind, string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && record.MetadataName == metadataName)
        .HashKey;
  }

  private static DbContextOptions<ExplicitSaveServiceContext> CreateExplicitSaveServiceOptions(SqliteTestDatabase database) {
    return new DbContextOptionsBuilder<ExplicitSaveServiceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, ExplicitSaveServiceModelCacheKeyFactory>()
        .Options;
  }

  private static DataVaultProviderCapabilityProfile CreateSqliteProfile(DataVaultHashKeyStorageProfile storageProfile) {
    return DataVaultProviderCapabilityProfiles.Sqlite.WithHashKeyStorageProfile(
        storageProfile,
        StableHashAlgorithmId,
        StableHashDigestByteLength);
  }

  private static void AssertSqliteHashStorage(
      SqliteTestConnection connection,
      string tableName,
      string columnName,
      string expectedHashKey,
      string expectedStorageClass,
      int expectedLength) {
    Assert.Equal(
        expectedStorageClass,
        connection.ExecuteScalarString(
            "SELECT typeof(" + QuoteSqliteIdentifier(columnName) + ") FROM " + QuoteSqliteIdentifier(tableName) + " ORDER BY rowid LIMIT 1;"));
    Assert.Equal(
        expectedLength.ToString(CultureInfo.InvariantCulture),
        connection.ExecuteScalarString(
            "SELECT length(" + QuoteSqliteIdentifier(columnName) + ") FROM " + QuoteSqliteIdentifier(tableName) + " ORDER BY rowid LIMIT 1;"));

    if (string.Equals(expectedStorageClass, "blob", StringComparison.Ordinal)) {
      Assert.Equal(
          expectedHashKey.ToUpperInvariant(),
          connection.ExecuteScalarString(
              "SELECT hex(" + QuoteSqliteIdentifier(columnName) + ") FROM " + QuoteSqliteIdentifier(tableName) + " ORDER BY rowid LIMIT 1;"));
      return;
    }

    Assert.Equal(
        expectedHashKey,
        connection.ExecuteScalarString(
            "SELECT " + QuoteSqliteIdentifier(columnName) + " FROM " + QuoteSqliteIdentifier(tableName) + " ORDER BY rowid LIMIT 1;"));
  }

  private static string CreateCanonicalHexDigest(int digestByteLength, int seed = 0) {
    return Convert.ToHexString(Enumerable
        .Range(0, digestByteLength)
        .Select(value => (byte)((value + seed) % 256))
        .ToArray()).ToLowerInvariant();
  }

  private static string QuoteSqliteIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static void AssertSingleSavedRecord(
      DataVaultSaveResult result,
      DataVaultTableKind kind,
      string metadataName,
      string tableName,
      string hashKey) {
    var record = Assert.Single(result.SavedRecords);

    Assert.Equal(kind, record.Kind);
    Assert.Equal(metadataName, record.MetadataName);
    Assert.Equal(tableName, record.TableName);
    Assert.Equal(hashKey, record.HashKey);
  }

  private static void AssertMultiActiveSavedRecord(
      DataVaultSavedRecord record,
      string parentHashKey,
      string contactType,
      string regionCode) {
    Assert.Equal(DataVaultTableKind.Satellite, record.Kind);
    Assert.Equal("ContactChannel", record.MetadataName);
    Assert.Equal("SatCustomerContactChannel", record.TableName);
    Assert.Equal(parentHashKey, record.HashKey);
    var expectedDrivingKeyValues = new[]
    {
        new KeyValuePair<string, string>("Contact Type", contactType),
        new KeyValuePair<string, string>("Region Code", regionCode),
    };

    Assert.Equal(expectedDrivingKeyValues, record.DrivingKeyValues.ToArray());
  }

  private static void AssertSatelliteRow(
      Dictionary<string, object> row,
      string parentHashKey,
      string emailAddress,
      string hashDiff,
      DateTimeOffset loadTimestamp,
      string recordSource) {
    Assert.Equal(parentHashKey, row["CustomerHashKey"]);
    Assert.Equal(emailAddress, row["EmailAddress"]);
    Assert.Equal(hashDiff, row["HashDiff"]);
    Assert.Equal(loadTimestamp, row["LoadTimestamp"]);
    Assert.Equal(recordSource, row["RecordSource"]);
  }

  private static void AssertCustomerProfileSatelliteRow(
      Dictionary<string, object> row,
      string parentHashKey,
      string customerName,
      string customerStatus,
      string hashDiff,
      DateTimeOffset loadTimestamp,
      string recordSource) {
    Assert.Equal(parentHashKey, row["CustomerHashKey"]);
    Assert.Equal(customerName, row["CustomerName"]);
    Assert.Equal(customerStatus, row["CustomerStatus"]);
    Assert.Equal(hashDiff, row["HashDiff"]);
    Assert.Equal(loadTimestamp, row["LoadTimestamp"]);
    Assert.Equal(recordSource, row["RecordSource"]);
  }

  private static void AssertMultiActiveSatelliteRow(
      Dictionary<string, object> row,
      string parentHashKey,
      string contactType,
      string regionCode,
      string emailAddress,
      string hashDiff,
      DateTimeOffset loadTimestamp,
      string recordSource) {
    Assert.Equal(parentHashKey, row["CustomerHashKey"]);
    Assert.Equal(contactType, row["ContactType"]);
    Assert.Equal(regionCode, row["RegionCode"]);
    Assert.Equal(emailAddress, row["EmailAddress"]);
    Assert.Equal(hashDiff, row["HashDiff"]);
    Assert.Equal(loadTimestamp, row["LoadTimestamp"]);
    Assert.Equal(recordSource, row["RecordSource"]);
  }

  private static void AssertSavedRecordsEqual(
      IReadOnlyList<DataVaultSavedRecord> expected,
      IReadOnlyList<DataVaultSavedRecord> actual) {
    Assert.Equal(expected.Count, actual.Count);
    for (var index = 0; index < expected.Count; index++) {
      Assert.Equal(expected[index].Kind, actual[index].Kind);
      Assert.Equal(expected[index].MetadataName, actual[index].MetadataName);
      Assert.Equal(expected[index].TableName, actual[index].TableName);
      Assert.Equal(expected[index].HashKey, actual[index].HashKey);
      Assert.Equal(expected[index].DrivingKeyValues.ToArray(), actual[index].DrivingKeyValues.ToArray());
    }
  }

  private sealed class CountingLoadTimestampResolver(DateTimeOffset loadTimestamp) : IDataVaultLoadTimestampResolver {
    public int CallCount { get; private set; }

    public DateTimeOffset? ResolveLoadTimestamp(DataVaultLoadTimestampResolutionContext context) {
      ArgumentNullException.ThrowIfNull(context);

      CallCount++;
      return loadTimestamp;
    }
  }

  private sealed class CountingRecordSourceResolver(string recordSource) : IDataVaultRecordSourceResolver {
    public int CallCount { get; private set; }

    public string? ResolveRecordSource(DataVaultRecordSourceResolutionContext context) {
      ArgumentNullException.ThrowIfNull(context);

      CallCount++;
      return recordSource;
    }
  }

  private sealed class TableSelectCommandCounter(string tableName) : DbCommandInterceptor {
    public int SelectCount { get; private set; }

    public void Reset() {
      SelectCount = 0;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default) {
      CountSelect(command);
      return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    private void CountSelect(DbCommand command) {
      if (command.CommandText.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase) &&
          command.CommandText.Contains(tableName, StringComparison.Ordinal)) {
        SelectCount++;
      }
    }
  }

  private sealed class CancelAfterFirstSaveChangesInterceptor(CancellationTokenSource cancellationSource) : SaveChangesInterceptor {
    private int _savedChangesCount;

    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default) {
      _savedChangesCount++;
      if (_savedChangesCount == 1) {
        cancellationSource.Cancel();
      }

      return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
  }

  private static async IAsyncEnumerable<DataVaultSaveChunk> CreateAsyncChunks(
      IReadOnlyList<DataVaultSaveChunk> chunks,
      [EnumeratorCancellation] CancellationToken cancellationToken = default) {
    await foreach (var chunk in CreateCountingAsyncChunks(chunks, _ => { }, cancellationToken).ConfigureAwait(false)) {
      yield return chunk;
    }
  }

  private static async IAsyncEnumerable<DataVaultSaveChunk> CreateCountingAsyncChunks(
      IReadOnlyList<DataVaultSaveChunk> chunks,
      Action<int> onChunkRequested,
      [EnumeratorCancellation] CancellationToken cancellationToken = default) {
    for (var index = 0; index < chunks.Count; index++) {
      cancellationToken.ThrowIfCancellationRequested();
      onChunkRequested(index);
      await Task.Yield();
      yield return chunks[index];
    }
  }

  private sealed class CapturingTelemetryObserver : IDataVaultTelemetryObserver {
    public List<DataVaultSaveTelemetrySummary> SaveSummaries { get; } = [];

    public void RecordSave(DataVaultSaveTelemetrySummary summary) {
      SaveSummaries.Add(summary);
    }

    public void RecordRead(DataVaultReadTelemetrySummary summary) {
    }
  }

  private sealed class CancelAfterFirstChunkSaveStrategy(CancellationTokenSource cancellationSource) : IDataVaultProviderSaveStrategy {
    private readonly List<int> _requestCounts = [];

    public int Priority => int.MaxValue;

    public int SaveCallCount => _requestCounts.Count;

    public IReadOnlyList<int> RequestCounts => _requestCounts;

    public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(requests);

      return true;
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DataVaultProviderSaveStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      _requestCounts.Add(context.Requests.Count);
      cancellationSource.Cancel();

      return Task.FromResult(new DataVaultSaveResult(0, []));
    }
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
                "Profile",
                DataVaultMetadataReference.Hub("Customer"),
                ["customer_name", "customer_status"]),
            new DataVaultSatelliteMetadata(
                "ContactChannel",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"],
                ["Contact Type", "Region Code"]),
            new DataVaultSatelliteMetadata(
                "State",
                DataVaultMetadataReference.Link("CustomerOrder"),
                ["State Code"]),
        ]);
  }

  private static DataVaultMetadataModel CreateDependentChildKeyMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrderLine = new DataVaultLinkMetadata(
        "CustomerOrderLine",
        [customer.ToReference(), order.ToReference()],
        ["Line Number"]);

    return new DataVaultMetadataModel([customer, order], [customerOrderLine], []);
  }

  private static DataVaultMetadataModel CreateCustomerOnlyMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        []);
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

  private static DataVaultMetadataModel CreateOrderOnlyMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Order", ["Order Id"])],
        [],
        []);
  }

  private sealed class RegistryBackedSaveServiceContext(
      DbContextOptions<RegistryBackedSaveServiceContext> options) : DbContext(options) {
  }

  private sealed class SameAsSaveServiceContext(
      DbContextOptions<SameAsSaveServiceContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          vault => {
            vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
            vault.Link("CustomerIdentityMatch", link => {
              link.Participant<Customer>("SourceCustomer");
              link.Participant<Customer>("MatchedCustomer");
            });
          },
          DataVaultProviderCapabilityProfiles.Sqlite);
    }
  }

  private sealed class DependentChildKeySaveServiceContext(
      DbContextOptions<DependentChildKeySaveServiceContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateDependentChildKeyMetadataModel(),
          CreateSqliteProfile(DataVaultHashKeyStorageProfile.HexString));
    }
  }

  private sealed class ExplicitSaveServiceContext(
      DbContextOptions<ExplicitSaveServiceContext> options,
      DataVaultLoadTimestampStorage loadTimestampStorage = DataVaultLoadTimestampStorage.ProviderDefault,
      DataVaultHashKeyStorageProfile storageProfile = DataVaultHashKeyStorageProfile.HexString) : DbContext(options) {
    public DataVaultLoadTimestampStorage LoadTimestampStorage { get; } = loadTimestampStorage;

    public DataVaultHashKeyStorageProfile StorageProfile { get; } = storageProfile;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateMetadataModel(),
          CreateSqliteProfile(StorageProfile),
          LoadTimestampStorage);
    }
  }

  private sealed class Customer {
    public string CustomerId { get; init; } = string.Empty;
  }

  private sealed class ExplicitSaveServiceModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is ExplicitSaveServiceContext explicitSaveServiceContext
          ? (
              context.GetType(),
              explicitSaveServiceContext.LoadTimestampStorage,
              explicitSaveServiceContext.StorageProfile,
              StableHashAlgorithmId,
              StableHashDigestByteLength,
              designTime)
          : (object)(context.GetType(), designTime);
    }
  }
}
