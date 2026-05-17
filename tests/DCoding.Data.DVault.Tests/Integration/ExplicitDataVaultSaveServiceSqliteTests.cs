using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class ExplicitDataVaultSaveServiceSqliteTests {
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
      var latestRow = Assert.Single(latestRows);

      Assert.Equal("Profile", latestRow.MetadataName);
      Assert.Equal("SatCustomerProfile", latestRow.TableName);
      Assert.Equal(customerHashKey, latestRow.ParentHashKey);
      Assert.Empty(latestRow.DrivingKeyValues);
      Assert.Equal("profile-hash-2", latestRow.HashDiff);
      Assert.Equal(secondLoadTimestamp, latestRow.LoadTimestamp);
      Assert.Equal("crm-change", latestRow.RecordSource);
      Assert.Equal("Alice Baker", latestRow.PayloadValues["customer_name"]);
      Assert.Equal("active", latestRow.PayloadValues["customer_status"]);

      var asOfRows = await readService.ReadLatestSatelliteRowsAsync(
          context,
          new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey], firstLoadTimestamp));
      var asOfRow = Assert.Single(asOfRows);

      Assert.Equal("profile-hash-1", asOfRow.HashDiff);
      Assert.Equal(firstLoadTimestamp, asOfRow.LoadTimestamp);
      Assert.Equal("prospect", asOfRow.PayloadValues["customer_status"]);

      Assert.Empty(await readService.ReadLatestSatelliteRowsAsync(
          context,
          new DataVaultLatestSatelliteReadRequest(profile, ["missing-hash-key"])));
    }
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
      var asOfRows = await readService.ReadLatestSatelliteRowsAsync(
          context,
          new DataVaultRegistryLatestSatelliteReadRequest(
              DataVaultMetadataReference.Hub("Customer"),
              "Profile",
              [customerHashKey],
              firstLoadTimestamp));
      var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().SingleAsync();
      var latestRow = Assert.Single(latestRows);
      var asOfRow = Assert.Single(asOfRows);

      Assert.Equal(customerHashKey, linkRow["CustomerHashKey"]);
      Assert.Equal(orderHashKey, linkRow["OrderHashKey"]);
      Assert.Equal("profile-hash-2", latestRow.HashDiff);
      Assert.Equal("active", latestRow.PayloadValues["customer_status"]);
      Assert.Equal("profile-hash-1", asOfRow.HashDiff);
      Assert.Equal("prospect", asOfRow.PayloadValues["customer_status"]);
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

    Assert.Contains("link metadata 'MissingLink'", saveException.Message, StringComparison.Ordinal);
    Assert.Contains("satellite metadata 'MissingProfile'", readException.Message, StringComparison.Ordinal);
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

  private sealed class ExplicitSaveServiceContext(
      DbContextOptions<ExplicitSaveServiceContext> options,
      DataVaultLoadTimestampStorage loadTimestampStorage = DataVaultLoadTimestampStorage.ProviderDefault) : DbContext(options) {
    public DataVaultLoadTimestampStorage LoadTimestampStorage { get; } = loadTimestampStorage;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateMetadataModel(),
          DataVaultProviderCapabilityProfiles.Sqlite,
          LoadTimestampStorage);
    }
  }

  private sealed class Customer {
    public string CustomerId { get; init; } = string.Empty;
  }

  private sealed class ExplicitSaveServiceModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is ExplicitSaveServiceContext explicitSaveServiceContext
          ? (context.GetType(), explicitSaveServiceContext.LoadTimestampStorage, designTime)
          : (object)(context.GetType(), designTime);
    }
  }
}
