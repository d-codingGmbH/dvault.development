using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class OrderProductPlainEfBenchmark : IScenarioBenchmark {
  private const long OrderId = 1000;
  private const long ProductId = 100;
  private const long OrderProductRelationshipId = 10001;

  public string ScenarioName => "order-product-fulfillment-history";

  public string ProviderName => BenchmarkArtifacts.RequiredProviderName;

  public string BaselineName => "conventional-ef";

  public string StrategyFamily => DataVaultBenchmarkHelpers.ClassicEfStrategyFamily;

  public string DatasetSize => "1 order-product relationship, 2 fulfillment states";

  public string ChangeRatio => "50% repeat-change history";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = TempSqliteDatabase.Create();
    var options = new DbContextOptionsBuilder<OrderFulfillmentHistoryContext>()
        .UseSqlite(database.ConnectionString)
        .Options;

    await using (var context = new OrderFulfillmentHistoryContext(options)) {
      await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
    }

    var elapsed = await BenchmarkClock.MeasureAsync(async () => {
      await using var context = new OrderFulfillmentHistoryContext(options);
      await PersistRelationshipAsync(context, cancellationToken).ConfigureAwait(false);

      foreach (var fulfillmentEvent in ScenarioContracts.MeasuredOrderFulfillmentEvents) {
        await ApplyFulfillmentIfChangedAsync(context, fulfillmentEvent, cancellationToken).ConfigureAwait(false);
      }
    }).ConfigureAwait(false);

    await using (var context = new OrderFulfillmentHistoryContext(options)) {
      var rowsWritten = await ApplyFulfillmentIfChangedAsync(
          context,
          ScenarioContracts.UnchangedOrderFulfillmentReplay,
          cancellationToken).ConfigureAwait(false);
      BenchmarkAssert.Equal(0, rowsWritten, "The conventional EF replay operation must not persist a third history row.");
    }

    await VerifyOutcomeAsync(options, cancellationToken).ConfigureAwait(false);

    return new ScenarioBenchmarkResult(
        elapsed,
        "1 order, 1 product, 1 relationship, and 2 fulfillment history rows for O-1000/SKU-COFFEE");
  }

  private static async Task PersistRelationshipAsync(
      OrderFulfillmentHistoryContext context,
      CancellationToken cancellationToken) {
    var relationship = ScenarioContracts.OrderRelationship;

    context.Orders.Add(new OrderRow {
      OrderId = OrderId,
      OrderNumber = relationship.OrderBusinessKey,
      OrderedAtUtc = relationship.CreatedAtUtc,
      RecordSource = relationship.RecordSource,
    });
    context.Products.Add(new ProductRow {
      ProductId = ProductId,
      Sku = relationship.ProductBusinessKey,
      Name = relationship.ProductName,
    });
    context.OrderProductRelationships.Add(new OrderProductRelationshipRow {
      OrderProductRelationshipId = OrderProductRelationshipId,
      OrderId = OrderId,
      ProductId = ProductId,
      CreatedAtUtc = relationship.CreatedAtUtc,
      RecordSource = relationship.RecordSource,
    });

    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
  }

  private static async Task<int> ApplyFulfillmentIfChangedAsync(
      OrderFulfillmentHistoryContext context,
      OrderFulfillmentEvent fulfillmentEvent,
      CancellationToken cancellationToken) {
    var latestRow = (await context.FulfillmentHistoryRows
        .AsNoTracking()
        .Where(row => row.OrderProductRelationshipId == OrderProductRelationshipId)
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false))
        .OrderByDescending(row => row.ChangedAtUtc)
        .FirstOrDefault();

    if (latestRow is not null &&
        string.Equals(latestRow.AllocationStatus, fulfillmentEvent.AllocationStatus, StringComparison.Ordinal) &&
        string.Equals(latestRow.WarehouseCode, fulfillmentEvent.WarehouseCode, StringComparison.Ordinal)) {
      return 0;
    }

    context.FulfillmentHistoryRows.Add(new FulfillmentHistoryRow {
      OrderProductRelationshipId = OrderProductRelationshipId,
      AllocationStatus = fulfillmentEvent.AllocationStatus,
      WarehouseCode = fulfillmentEvent.WarehouseCode,
      ChangedAtUtc = fulfillmentEvent.ChangedAtUtc,
      RecordSource = fulfillmentEvent.RecordSource,
    });

    return await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
  }

  private static async Task VerifyOutcomeAsync(
      DbContextOptions<OrderFulfillmentHistoryContext> options,
      CancellationToken cancellationToken) {
    await using var context = new OrderFulfillmentHistoryContext(options);

    BenchmarkAssert.Equal(1, await context.Orders.AsNoTracking().CountAsync(cancellationToken).ConfigureAwait(false), "The conventional EF order benchmark must persist one order row.");
    BenchmarkAssert.Equal(1, await context.Products.AsNoTracking().CountAsync(cancellationToken).ConfigureAwait(false), "The conventional EF order benchmark must persist one product row.");
    BenchmarkAssert.Equal(1, await context.OrderProductRelationships.AsNoTracking().CountAsync(cancellationToken).ConfigureAwait(false), "The conventional EF order benchmark must persist one relationship row.");
    BenchmarkAssert.Equal(2, await context.FulfillmentHistoryRows.AsNoTracking().CountAsync(cancellationToken).ConfigureAwait(false), "The conventional EF order benchmark must persist exactly two fulfillment history rows.");

    var order = await context.Orders.AsNoTracking().SingleAsync(cancellationToken).ConfigureAwait(false);
    var product = await context.Products.AsNoTracking().SingleAsync(cancellationToken).ConfigureAwait(false);
    var relationship = await context.OrderProductRelationships.AsNoTracking().SingleAsync(cancellationToken).ConfigureAwait(false);
    var fulfillmentRows = (await context.FulfillmentHistoryRows
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false))
        .OrderBy(row => row.ChangedAtUtc)
        .ToArray();

    BenchmarkAssert.Equal(ScenarioContracts.OrderRelationship.OrderBusinessKey, order.OrderNumber, "Order business key drifted.");
    BenchmarkAssert.Equal(ScenarioContracts.OrderRelationship.ProductBusinessKey, product.Sku, "Product business key drifted.");
    BenchmarkAssert.Equal(OrderId, relationship.OrderId, "Relationship order reference drifted.");
    BenchmarkAssert.Equal(ProductId, relationship.ProductId, "Relationship product reference drifted.");
    BenchmarkAssert.Equal(ScenarioContracts.OrderRelationship.CreatedAtUtc, relationship.CreatedAtUtc, "Relationship creation timestamp drifted.");
    BenchmarkAssert.Equal(ScenarioContracts.OrderRelationship.RecordSource, relationship.RecordSource, "Relationship record source drifted.");

    for (var index = 0; index < ScenarioContracts.MeasuredOrderFulfillmentEvents.Length; index++) {
      AssertFulfillmentRow(fulfillmentRows[index], ScenarioContracts.MeasuredOrderFulfillmentEvents[index]);
    }
  }

  private static void AssertFulfillmentRow(FulfillmentHistoryRow row, OrderFulfillmentEvent expected) {
    BenchmarkAssert.Equal(OrderProductRelationshipId, row.OrderProductRelationshipId, "Fulfillment relationship reference drifted.");
    BenchmarkAssert.Equal(expected.AllocationStatus, row.AllocationStatus, "Fulfillment allocation status drifted.");
    BenchmarkAssert.Equal(expected.WarehouseCode, row.WarehouseCode, "Fulfillment warehouse code drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, row.ChangedAtUtc, "Fulfillment timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, row.RecordSource, "Fulfillment record source drifted.");
  }

  private sealed class OrderFulfillmentHistoryContext(DbContextOptions<OrderFulfillmentHistoryContext> options)
      : DbContext(options) {
    public DbSet<OrderRow> Orders => Set<OrderRow>();

    public DbSet<ProductRow> Products => Set<ProductRow>();

    public DbSet<OrderProductRelationshipRow> OrderProductRelationships => Set<OrderProductRelationshipRow>();

    public DbSet<FulfillmentHistoryRow> FulfillmentHistoryRows => Set<FulfillmentHistoryRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<OrderRow>(entity => {
        entity.ToTable("Orders");
        entity.HasKey(row => row.OrderId);
        entity.Property(row => row.OrderId).ValueGeneratedNever();
        entity.Property(row => row.OrderNumber).HasMaxLength(32).IsRequired();
        entity.Property(row => row.OrderedAtUtc).IsRequired();
        entity.Property(row => row.RecordSource).IsRequired();
        entity.HasIndex(row => row.OrderNumber).IsUnique();
      });

      modelBuilder.Entity<ProductRow>(entity => {
        entity.ToTable("Products");
        entity.HasKey(row => row.ProductId);
        entity.Property(row => row.ProductId).ValueGeneratedNever();
        entity.Property(row => row.Sku).HasMaxLength(64).IsRequired();
        entity.Property(row => row.Name).HasMaxLength(128).IsRequired();
        entity.HasIndex(row => row.Sku).IsUnique();
      });

      modelBuilder.Entity<OrderProductRelationshipRow>(entity => {
        entity.ToTable("OrderProducts");
        entity.HasKey(row => row.OrderProductRelationshipId);
        entity.Property(row => row.OrderProductRelationshipId).ValueGeneratedNever();
        entity.Property(row => row.CreatedAtUtc).IsRequired();
        entity.Property(row => row.RecordSource).IsRequired();
        entity.HasIndex(row => new { row.OrderId, row.ProductId }).IsUnique();
        entity.HasOne(row => row.Order)
            .WithMany(row => row.ProductRelationships)
            .HasForeignKey(row => row.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(row => row.Product)
            .WithMany(row => row.OrderRelationships)
            .HasForeignKey(row => row.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
      });

      modelBuilder.Entity<FulfillmentHistoryRow>(entity => {
        entity.ToTable("OrderProductFulfillmentHistory");
        entity.HasKey(row => row.FulfillmentHistoryRowId);
        entity.Property(row => row.AllocationStatus).HasMaxLength(64).IsRequired();
        entity.Property(row => row.WarehouseCode).HasMaxLength(64).IsRequired();
        entity.Property(row => row.ChangedAtUtc).IsRequired();
        entity.Property(row => row.RecordSource).IsRequired();
        entity.HasIndex(row => new { row.OrderProductRelationshipId, row.ChangedAtUtc });
        entity.HasOne(row => row.OrderProductRelationship)
            .WithMany(row => row.FulfillmentHistoryRows)
            .HasForeignKey(row => row.OrderProductRelationshipId)
            .OnDelete(DeleteBehavior.Cascade);
      });
    }
  }

  private sealed class OrderRow {
    public long OrderId { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public DateTimeOffset OrderedAtUtc { get; set; }

    public string RecordSource { get; set; } = string.Empty;

    public List<OrderProductRelationshipRow> ProductRelationships { get; set; } = [];
  }

  private sealed class ProductRow {
    public long ProductId { get; set; }

    public string Sku { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public List<OrderProductRelationshipRow> OrderRelationships { get; set; } = [];
  }

  private sealed class OrderProductRelationshipRow {
    public long OrderProductRelationshipId { get; set; }

    public long OrderId { get; set; }

    public long ProductId { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public string RecordSource { get; set; } = string.Empty;

    public OrderRow Order { get; set; } = null!;

    public ProductRow Product { get; set; } = null!;

    public List<FulfillmentHistoryRow> FulfillmentHistoryRows { get; set; } = [];
  }

  private sealed class FulfillmentHistoryRow {
    public long FulfillmentHistoryRowId { get; set; }

    public long OrderProductRelationshipId { get; set; }

    public string AllocationStatus { get; set; } = string.Empty;

    public string WarehouseCode { get; set; } = string.Empty;

    public DateTimeOffset ChangedAtUtc { get; set; }

    public string RecordSource { get; set; } = string.Empty;

    public OrderProductRelationshipRow OrderProductRelationship { get; set; } = null!;
  }
}
