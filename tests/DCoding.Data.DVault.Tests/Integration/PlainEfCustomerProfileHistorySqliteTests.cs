using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class PlainEfCustomerProfileHistorySqliteTests {
  private const string CustomerBusinessKey = "C-100";

  private static readonly CustomerProfileEvent[] SharedCustomerProfileEvents = [
      new(
          CustomerBusinessKey,
          "Alice Adams",
          "prospect",
          new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero),
          "crm-import"),
      new(
          CustomerBusinessKey,
          "Alice Baker",
          "active",
          new DateTimeOffset(2026, 4, 29, 11, 30, 0, TimeSpan.Zero),
          "crm-change"),
  ];

  [Fact]
  public async Task ConventionalEfBaselinePersistsExactCustomerProfileHistoryRowsThroughSqlite() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<CustomerProfileHistoryContext>()
        .UseSqlite(CreateConnectionString(database))
        .Options;

    await using (var context = new CustomerProfileHistoryContext(options)) {
      await context.Database.EnsureCreatedAsync();

      foreach (var customerProfileEvent in SharedCustomerProfileEvents) {
        await ApplyCustomerProfileEventAsync(context, customerProfileEvent);
      }
    }

    await using (var context = new CustomerProfileHistoryContext(options)) {
      var rows = (await context.CustomerProfileHistoryRows
          .AsNoTracking()
          .Where(row => row.CustomerBusinessKey == CustomerBusinessKey)
          .ToListAsync())
          .OrderBy(row => row.ChangedAtUtc)
          .ThenBy(row => row.Id)
          .ToArray();

      Assert.Equal(2, await context.CustomerProfileHistoryRows.AsNoTracking().CountAsync());
      Assert.Equal(SharedCustomerProfileEvents.Length, rows.Length);
      AssertHistoryRow(rows[0], SharedCustomerProfileEvents[0]);
      AssertHistoryRow(rows[1], SharedCustomerProfileEvents[1]);
    }
  }

  private static Task ApplyCustomerProfileEventAsync(
      CustomerProfileHistoryContext context,
      CustomerProfileEvent customerProfileEvent) {
    context.CustomerProfileHistoryRows.Add(new CustomerProfileHistoryRow {
      CustomerBusinessKey = customerProfileEvent.CustomerBusinessKey,
      CustomerName = customerProfileEvent.CustomerName,
      CustomerStatus = customerProfileEvent.CustomerStatus,
      ChangedAtUtc = customerProfileEvent.ChangedAtUtc,
      RecordSource = customerProfileEvent.RecordSource,
    });

    return context.SaveChangesAsync();
  }

  private static void AssertHistoryRow(CustomerProfileHistoryRow row, CustomerProfileEvent expected) {
    Assert.Equal(expected.CustomerBusinessKey, row.CustomerBusinessKey);
    Assert.Equal(expected.CustomerName, row.CustomerName);
    Assert.Equal(expected.CustomerStatus, row.CustomerStatus);
    Assert.Equal(expected.ChangedAtUtc, row.ChangedAtUtc);
    Assert.Equal(expected.RecordSource, row.RecordSource);
  }

  private static string CreateConnectionString(SqliteTestDatabase database) {
    return "Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False";
  }

  private sealed record CustomerProfileEvent(
      string CustomerBusinessKey,
      string CustomerName,
      string CustomerStatus,
      DateTimeOffset ChangedAtUtc,
      string RecordSource);

  private sealed class CustomerProfileHistoryContext(DbContextOptions<CustomerProfileHistoryContext> options) : DbContext(options) {
    public DbSet<CustomerProfileHistoryRow> CustomerProfileHistoryRows => Set<CustomerProfileHistoryRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<CustomerProfileHistoryRow>(entity => {
        entity.ToTable("CustomerProfileHistory");
        entity.HasKey(row => row.Id);
        entity.Property(row => row.CustomerBusinessKey).IsRequired();
        entity.Property(row => row.CustomerName).IsRequired();
        entity.Property(row => row.CustomerStatus).IsRequired();
        entity.Property(row => row.ChangedAtUtc).IsRequired();
        entity.Property(row => row.RecordSource).IsRequired();
        entity.HasIndex(row => new { row.CustomerBusinessKey, row.ChangedAtUtc });
      });
    }
  }

  private sealed class CustomerProfileHistoryRow {
    public long Id { get; set; }

    public string CustomerBusinessKey { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerStatus { get; set; } = string.Empty;

    public DateTimeOffset ChangedAtUtc { get; set; }

    public string RecordSource { get; set; } = string.Empty;
  }
}
