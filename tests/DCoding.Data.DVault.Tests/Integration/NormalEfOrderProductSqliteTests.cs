using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

public sealed class NormalEfOrderProductSqliteTests {
  [Fact]
  public async Task ConventionalOrderProductLineModelPersistsRelationshipPayloadThroughSqlite() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<NormalOrderProductContext>()
        .UseSqlite(CreateConnectionString(database))
        .Options;

    await using (var context = new NormalOrderProductContext(options)) {
      await context.Database.EnsureCreatedAsync();

      context.Products.AddRange(
          new Product {
            ProductId = 100,
            Sku = "SKU-COFFEE",
            Name = "Coffee subscription",
            CurrentUnitPriceCents = 1299
          },
          new Product {
            ProductId = 200,
            Sku = "SKU-MUG",
            Name = "Stoneware mug",
            CurrentUnitPriceCents = 1899
          });

      context.Orders.AddRange(
          new Order {
            OrderId = 1000,
            OrderNumber = "O-1000",
            OrderedAtUtc = new DateTimeOffset(2026, 5, 1, 9, 30, 0, TimeSpan.Zero),
            Lines = [
                new OrderLine {
                  OrderLineId = 10001,
                  LineNumber = 1,
                  ProductId = 100,
                  Quantity = 2,
                  UnitPriceCents = 1299,
                  ProductNameSnapshot = "Coffee subscription"
                },
                new OrderLine {
                  OrderLineId = 10002,
                  LineNumber = 2,
                  ProductId = 200,
                  Quantity = 1,
                  UnitPriceCents = 1899,
                  ProductNameSnapshot = "Stoneware mug"
                },
            ]
          },
          new Order {
            OrderId = 1001,
            OrderNumber = "O-1001",
            OrderedAtUtc = new DateTimeOffset(2026, 5, 1, 10, 0, 0, TimeSpan.Zero),
            Lines = [
                new OrderLine {
                  OrderLineId = 10011,
                  LineNumber = 1,
                  ProductId = 100,
                  Quantity = 3,
                  UnitPriceCents = 1299,
                  ProductNameSnapshot = "Coffee subscription"
                },
            ]
          });

      await context.SaveChangesAsync();
    }

    await using (var context = new NormalOrderProductContext(options)) {
      var order = await context.Orders
          .AsNoTracking()
          .Include(value => value.Lines.OrderBy(line => line.LineNumber))
          .ThenInclude(line => line.Product)
          .SingleAsync(value => value.OrderNumber == "O-1000");
      var reusedProduct = await context.Products
          .AsNoTracking()
          .Include(value => value.OrderLines)
          .SingleAsync(value => value.Sku == "SKU-COFFEE");

      Assert.Equal(2, order.Lines.Count);
      AssertLine(
          order.Lines[0],
          expectedLineNumber: 1,
          expectedProductSku: "SKU-COFFEE",
          expectedQuantity: 2,
          expectedUnitPriceCents: 1299,
          expectedProductNameSnapshot: "Coffee subscription");
      AssertLine(
          order.Lines[1],
          expectedLineNumber: 2,
          expectedProductSku: "SKU-MUG",
          expectedQuantity: 1,
          expectedUnitPriceCents: 1899,
          expectedProductNameSnapshot: "Stoneware mug");
      Assert.Equal(2, reusedProduct.OrderLines.Count);
      Assert.Equal(5, reusedProduct.OrderLines.Sum(line => line.Quantity));
      Assert.Equal(3, await context.OrderLines.CountAsync());
    }
  }

  [Fact]
  public async Task DataVaultOrderProductModelPersistsRelationshipSatelliteHistoryThroughSqlite() {
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var product = new DataVaultHubMetadata("Product", ["Sku"]);
    var orderProduct = new DataVaultLinkMetadata(
        "OrderProduct",
        [order.ToReference(), product.ToReference()]);
    var fulfillment = new DataVaultSatelliteMetadata(
        "Fulfillment",
        orderProduct.ToReference(),
        ["Allocation Status", "Warehouse Code"]);
    var relationshipLoadTimestamp = new DateTimeOffset(2026, 5, 1, 9, 30, 0, TimeSpan.Zero);
    var firstFulfillmentTimestamp = new DateTimeOffset(2026, 5, 1, 10, 0, 0, TimeSpan.Zero);
    var changedFulfillmentTimestamp = new DateTimeOffset(2026, 5, 1, 10, 45, 0, TimeSpan.Zero);
    var unchangedFulfillmentTimestamp = new DateTimeOffset(2026, 5, 1, 11, 15, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<OrderProductDataVaultContext>()
        .UseSqlite(CreateConnectionString(database))
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string orderHashKey;
    string productHashKey;
    string orderProductHashKey;

    await using (var context = new OrderProductDataVaultContext(options)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              relationshipLoadTimestamp,
              "order-entry",
              [
                  new(order, [new("Order Id", "O-1000")]),
                  new(product, [new("Sku", "SKU-COFFEE")]),
              ],
              []));

      orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");
      productHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Product");

      var linkResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              relationshipLoadTimestamp,
              "order-entry",
              [],
              [
                  new(orderProduct, [new("Order", orderHashKey), new("Product", productHashKey)]),
              ]));

      orderProductHashKey = GetHashKey(linkResult, DataVaultTableKind.Link, "OrderProduct");

      Assert.Equal(2, hubResult.RowsWritten);
      Assert.Equal(1, linkResult.RowsWritten);
    }

    DataVaultSaveResult firstFulfillmentResult;
    await using (var context = new OrderProductDataVaultContext(options)) {
      firstFulfillmentResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              firstFulfillmentTimestamp,
              "warehouse-allocation",
              [],
              [],
              [
                  new(
                      fulfillment,
                      orderProductHashKey,
                      [new("Allocation Status", "Backordered"), new("Warehouse Code", "NORTH-1")],
                      "fulfillment-backordered-north-1"),
              ]));
    }

    DataVaultSaveResult changedFulfillmentResult;
    await using (var context = new OrderProductDataVaultContext(options)) {
      changedFulfillmentResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              changedFulfillmentTimestamp,
              "warehouse-allocation",
              [],
              [],
              [
                  new(
                      fulfillment,
                      orderProductHashKey,
                      [new("Allocation Status", "Allocated"), new("Warehouse Code", "NORTH-1")],
                      "fulfillment-allocated-north-1"),
              ]));
    }

    DataVaultSaveResult unchangedFulfillmentResult;
    await using (var context = new OrderProductDataVaultContext(options)) {
      unchangedFulfillmentResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              unchangedFulfillmentTimestamp,
              "warehouse-replay",
              [],
              [],
              [
                  new(
                      fulfillment,
                      orderProductHashKey,
                      [new("Allocation Status", "Allocated"), new("Warehouse Code", "NORTH-1")],
                      "fulfillment-allocated-north-1"),
              ]));
    }

    Assert.Equal(1, firstFulfillmentResult.RowsWritten);
    Assert.Equal(1, changedFulfillmentResult.RowsWritten);
    Assert.Equal(0, unchangedFulfillmentResult.RowsWritten);
    AssertSingleSavedRecord(
        firstFulfillmentResult,
        DataVaultTableKind.Satellite,
        "Fulfillment",
        "SatOrderProductFulfillment",
        orderProductHashKey);
    AssertSingleSavedRecord(
        changedFulfillmentResult,
        DataVaultTableKind.Satellite,
        "Fulfillment",
        "SatOrderProductFulfillment",
        orderProductHashKey);

    await using (var context = new OrderProductDataVaultContext(options)) {
      var orderRow = await context.Set<Dictionary<string, object>>("HubOrder").AsNoTracking().SingleAsync();
      var productRow = await context.Set<Dictionary<string, object>>("HubProduct").AsNoTracking().SingleAsync();
      var linkRow = await context.Set<Dictionary<string, object>>("LinkOrderProduct").AsNoTracking().SingleAsync();
      var fulfillmentRows = (await context.Set<Dictionary<string, object>>("SatOrderProductFulfillment")
          .AsNoTracking()
          .ToListAsync())
          .OrderBy(row => (DateTimeOffset)row["LoadTimestamp"])
          .ToArray();

      Assert.Equal("O-1000", orderRow["OrderId"]);
      Assert.Equal("SKU-COFFEE", productRow["Sku"]);
      Assert.Equal(orderHashKey, orderRow["OrderHashKey"]);
      Assert.Equal(productHashKey, productRow["ProductHashKey"]);
      Assert.Equal(orderHashKey, linkRow["OrderHashKey"]);
      Assert.Equal(productHashKey, linkRow["ProductHashKey"]);
      Assert.Equal(orderProductHashKey, linkRow["OrderProductHashKey"]);
      Assert.Equal("order-entry", linkRow["RecordSource"]);
      Assert.Matches("^[0-9a-f]{64}$", Assert.IsType<string>(linkRow["OrderProductHashKey"]));
      Assert.Equal(2, fulfillmentRows.Length);
      AssertFulfillmentRow(
          fulfillmentRows[0],
          orderProductHashKey,
          "Backordered",
          "NORTH-1",
          "fulfillment-backordered-north-1",
          firstFulfillmentTimestamp,
          "warehouse-allocation");
      AssertFulfillmentRow(
          fulfillmentRows[1],
          orderProductHashKey,
          "Allocated",
          "NORTH-1",
          "fulfillment-allocated-north-1",
          changedFulfillmentTimestamp,
          "warehouse-allocation");
    }

    using var connection = database.CreateOpenConnection();

    Assert.Equal("HubOrder|HubProduct|LinkOrderProduct|SatOrderProductFulfillment", TableNames(connection));
    AssertTable(
        connection,
        "LinkOrderProduct",
        ["OrderProductHashKey", "LoadTimestamp", "RecordSource", "OrderHashKey", "ProductHashKey"],
        "PkLinkOrderProductOrderProductHashKey",
        ["OrderProductHashKey"],
        "IxLinkOrderProductRelationshipOrderHashKeyProductHashKey",
        ["OrderHashKey", "ProductHashKey"],
        expectedIndexUnique: false);
    AssertTable(
        connection,
        "SatOrderProductFulfillment",
        ["OrderProductHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "AllocationStatus", "WarehouseCode"],
        "PkSatOrderProductFulfillmentOrderProductHashKeyLoadTimestamp",
        ["OrderProductHashKey", "LoadTimestamp"],
        "IxSatOrderProductFulfillmentSatelliteParentOrderProductHashKeyLoadTimestamp",
        ["OrderProductHashKey", "LoadTimestamp"],
        expectedIndexUnique: false);
  }

  private static string CreateConnectionString(SqliteTestDatabase database) {
    return "Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False";
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

  private static void AssertLine(
      OrderLine line,
      int expectedLineNumber,
      string expectedProductSku,
      int expectedQuantity,
      int expectedUnitPriceCents,
      string expectedProductNameSnapshot) {
    Assert.Equal(expectedLineNumber, line.LineNumber);
    Assert.Equal(expectedProductSku, line.Product.Sku);
    Assert.Equal(expectedQuantity, line.Quantity);
    Assert.Equal(expectedUnitPriceCents, line.UnitPriceCents);
    Assert.Equal(expectedProductNameSnapshot, line.ProductNameSnapshot);
  }

  private static void AssertFulfillmentRow(
      Dictionary<string, object> row,
      string orderProductHashKey,
      string allocationStatus,
      string warehouseCode,
      string hashDiff,
      DateTimeOffset loadTimestamp,
      string recordSource) {
    Assert.Equal(orderProductHashKey, row["OrderProductHashKey"]);
    Assert.Equal(allocationStatus, row["AllocationStatus"]);
    Assert.Equal(warehouseCode, row["WarehouseCode"]);
    Assert.Equal(hashDiff, row["HashDiff"]);
    Assert.Equal(loadTimestamp, row["LoadTimestamp"]);
    Assert.Equal(recordSource, row["RecordSource"]);
  }

  private static void AssertTable(
      SqliteTestConnection connection,
      string tableName,
      string[] expectedColumnNames,
      string expectedPrimaryKeyName,
      string[] expectedPrimaryKeyColumnNames,
      string expectedIndexName,
      string[] expectedIndexColumnNames,
      bool expectedIndexUnique) {
    var createSql = connection.ExecuteScalarString(
        "SELECT sql FROM sqlite_master WHERE type = 'table' AND name = " + SqlLiteral(tableName) + ";");

    Assert.NotNull(createSql);
    Assert.Contains("CONSTRAINT \"" + expectedPrimaryKeyName + "\"", createSql!, StringComparison.Ordinal);
    Assert.Contains("PRIMARY KEY", createSql!, StringComparison.Ordinal);
    Assert.Equal(string.Join("|", expectedColumnNames), ColumnNames(connection, tableName));
    Assert.Equal(string.Join("|", expectedPrimaryKeyColumnNames), PrimaryKeyColumnNames(connection, tableName));
    Assert.Equal(expectedIndexUnique ? "1" : "0", IndexUniqueValue(connection, tableName, expectedIndexName));
    Assert.Equal(string.Join("|", expectedIndexColumnNames), IndexColumnNames(connection, expectedIndexName));
  }

  private static DataVaultMetadataModel CreateDataVaultMetadataModel() {
    return new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata("Order", ["Order Id"]),
            new DataVaultHubMetadata("Product", ["Sku"]),
        ],
        [
            new DataVaultLinkMetadata(
                "OrderProduct",
                [DataVaultMetadataReference.Hub("Order"), DataVaultMetadataReference.Hub("Product")]),
        ],
        [
            new DataVaultSatelliteMetadata(
                "Fulfillment",
                DataVaultMetadataReference.Link("OrderProduct"),
                ["Allocation Status", "Warehouse Code"]),
        ]);
  }

  private static string? TableNames(SqliteTestConnection connection) {
    return connection.ExecuteScalarString(
        "SELECT group_concat(name, '|') FROM (" +
        "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%' ORDER BY name);");
  }

  private static string? ColumnNames(SqliteTestConnection connection, string tableName) {
    return connection.ExecuteScalarString(
        "SELECT group_concat(name, '|') FROM (" +
        "SELECT name FROM pragma_table_info(" + SqlLiteral(tableName) + ") ORDER BY cid);");
  }

  private static string? PrimaryKeyColumnNames(SqliteTestConnection connection, string tableName) {
    return connection.ExecuteScalarString(
        "SELECT group_concat(name, '|') FROM (" +
        "SELECT name FROM pragma_table_info(" + SqlLiteral(tableName) + ") WHERE pk > 0 ORDER BY pk);");
  }

  private static string? IndexUniqueValue(SqliteTestConnection connection, string tableName, string indexName) {
    return connection.ExecuteScalarString(
        "SELECT \"unique\" FROM pragma_index_list(" + SqlLiteral(tableName) + ") " +
        "WHERE name = " + SqlLiteral(indexName) + ";");
  }

  private static string? IndexColumnNames(SqliteTestConnection connection, string indexName) {
    return connection.ExecuteScalarString(
        "SELECT group_concat(name, '|') FROM (" +
        "SELECT name FROM pragma_index_info(" + SqlLiteral(indexName) + ") ORDER BY seqno);");
  }

  private static string SqlLiteral(string value) {
    return "'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }

  private sealed class NormalOrderProductContext(DbContextOptions<NormalOrderProductContext> options) : DbContext(options) {
    public DbSet<Order> Orders => Set<Order>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Order>(entity => {
        entity.ToTable("Orders");
        entity.HasKey(value => value.OrderId);
        entity.Property(value => value.OrderId).ValueGeneratedNever();
        entity.Property(value => value.OrderNumber).HasMaxLength(32).IsRequired();
        entity.Property(value => value.OrderedAtUtc).IsRequired();
        entity.HasIndex(value => value.OrderNumber).IsUnique();
        entity.HasMany(value => value.Lines)
            .WithOne(value => value.Order)
            .HasForeignKey(value => value.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
      });

      modelBuilder.Entity<Product>(entity => {
        entity.ToTable("Products");
        entity.HasKey(value => value.ProductId);
        entity.Property(value => value.ProductId).ValueGeneratedNever();
        entity.Property(value => value.Sku).HasMaxLength(64).IsRequired();
        entity.Property(value => value.Name).HasMaxLength(128).IsRequired();
        entity.Property(value => value.CurrentUnitPriceCents).IsRequired();
        entity.HasIndex(value => value.Sku).IsUnique();
      });

      modelBuilder.Entity<OrderLine>(entity => {
        entity.ToTable("OrderLines");
        entity.HasKey(value => value.OrderLineId);
        entity.Property(value => value.OrderLineId).ValueGeneratedNever();
        entity.Property(value => value.LineNumber).IsRequired();
        entity.Property(value => value.Quantity).IsRequired();
        entity.Property(value => value.UnitPriceCents).IsRequired();
        entity.Property(value => value.ProductNameSnapshot).HasMaxLength(128).IsRequired();
        entity.HasIndex(value => new { value.OrderId, value.LineNumber }).IsUnique();
        entity.HasIndex(value => value.ProductId);
        entity.HasOne(value => value.Product)
            .WithMany(value => value.OrderLines)
            .HasForeignKey(value => value.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
      });
    }
  }

  private sealed class OrderProductDataVaultContext(DbContextOptions<OrderProductDataVaultContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateDataVaultMetadataModel());
    }
  }

  private sealed class Order {
    public int OrderId { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public DateTimeOffset OrderedAtUtc { get; set; }

    public List<OrderLine> Lines { get; set; } = [];
  }

  private sealed class Product {
    public int ProductId { get; set; }

    public string Sku { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int CurrentUnitPriceCents { get; set; }

    public List<OrderLine> OrderLines { get; set; } = [];
  }

  private sealed class OrderLine {
    public int OrderLineId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int LineNumber { get; set; }

    public int Quantity { get; set; }

    public int UnitPriceCents { get; set; }

    public string ProductNameSnapshot { get; set; } = string.Empty;

    public Order Order { get; set; } = null!;

    public Product Product { get; set; } = null!;
  }
}
