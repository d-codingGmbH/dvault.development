using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
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

  private static string CreateConnectionString(SqliteTestDatabase database) {
    return "Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False";
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
