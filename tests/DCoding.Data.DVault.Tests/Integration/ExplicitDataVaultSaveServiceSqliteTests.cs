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

  private static string GetHashKey(DataVaultSaveResult result, DataVaultTableKind kind, string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && record.MetadataName == metadataName)
        .HashKey;
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
        []);
  }

  private sealed class ExplicitSaveServiceContext(DbContextOptions<ExplicitSaveServiceContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());
    }
  }
}
