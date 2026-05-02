using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

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

  private static void AssertSavedRecordsEqual(
      IReadOnlyList<DataVaultSavedRecord> expected,
      IReadOnlyList<DataVaultSavedRecord> actual) {
    Assert.Equal(expected.Count, actual.Count);
    for (var index = 0; index < expected.Count; index++) {
      Assert.Equal(expected[index].Kind, actual[index].Kind);
      Assert.Equal(expected[index].MetadataName, actual[index].MetadataName);
      Assert.Equal(expected[index].TableName, actual[index].TableName);
      Assert.Equal(expected[index].HashKey, actual[index].HashKey);
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
                "State",
                DataVaultMetadataReference.Link("CustomerOrder"),
                ["State Code"]),
        ]);
  }

  private sealed class ExplicitSaveServiceContext(DbContextOptions<ExplicitSaveServiceContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());
    }
  }
}
