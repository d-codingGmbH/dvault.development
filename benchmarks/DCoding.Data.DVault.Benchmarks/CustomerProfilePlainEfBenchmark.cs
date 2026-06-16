using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CustomerProfilePlainEfBenchmark : IScenarioBenchmark {
  public string ScenarioName => "customer-profile-history";

  public string ProviderName => BenchmarkArtifacts.RequiredProviderName;

  public string BaselineName => "conventional-ef";

  public string StrategyFamily => DataVaultBenchmarkHelpers.ClassicEfStrategyFamily;

  public string DatasetSize => "1 customer, 2 profile states";

  public string ChangeRatio => "50% repeat-change history";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = TempSqliteDatabase.Create();
    var options = new DbContextOptionsBuilder<CustomerProfileHistoryContext>()
        .UseSqlite(database.ConnectionString)
        .Options;

    await using (var context = new CustomerProfileHistoryContext(options)) {
      await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
    }

    var elapsed = await BenchmarkClock.MeasureAsync(async () => {
      await using var context = new CustomerProfileHistoryContext(options);

      foreach (var customerProfileEvent in ScenarioContracts.CustomerProfileEvents) {
        context.CustomerProfileHistoryRows.Add(new CustomerProfileHistoryRow {
          CustomerBusinessKey = customerProfileEvent.CustomerBusinessKey,
          CustomerName = customerProfileEvent.CustomerName,
          CustomerStatus = customerProfileEvent.CustomerStatus,
          ChangedAtUtc = customerProfileEvent.ChangedAtUtc,
          RecordSource = customerProfileEvent.RecordSource,
        });

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      }
    }).ConfigureAwait(false);

    await VerifyOutcomeAsync(options, cancellationToken).ConfigureAwait(false);

    return new ScenarioBenchmarkResult(elapsed, "2 customer profile history rows for C-100");
  }

  private static async Task VerifyOutcomeAsync(
      DbContextOptions<CustomerProfileHistoryContext> options,
      CancellationToken cancellationToken) {
    await using var context = new CustomerProfileHistoryContext(options);
    var rows = (await context.CustomerProfileHistoryRows
        .AsNoTracking()
        .Where(row => row.CustomerBusinessKey == ScenarioContracts.CustomerBusinessKey)
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false))
        .OrderBy(row => row.ChangedAtUtc)
        .ThenBy(row => row.Id)
        .ToArray();

    BenchmarkAssert.Equal(
        ScenarioContracts.CustomerProfileEvents.Length,
        await context.CustomerProfileHistoryRows.AsNoTracking().CountAsync(cancellationToken).ConfigureAwait(false),
        "The conventional EF customer profile benchmark must persist exactly the contracted rows.");
    BenchmarkAssert.Equal(
        ScenarioContracts.CustomerProfileEvents.Length,
        rows.Length,
        "The conventional EF customer profile benchmark must persist rows for C-100.");

    for (var index = 0; index < ScenarioContracts.CustomerProfileEvents.Length; index++) {
      AssertHistoryRow(rows[index], ScenarioContracts.CustomerProfileEvents[index]);
    }
  }

  private static void AssertHistoryRow(CustomerProfileHistoryRow row, CustomerProfileEvent expected) {
    BenchmarkAssert.Equal(expected.CustomerBusinessKey, row.CustomerBusinessKey, "Customer profile business key drifted.");
    BenchmarkAssert.Equal(expected.CustomerName, row.CustomerName, "Customer profile name drifted.");
    BenchmarkAssert.Equal(expected.CustomerStatus, row.CustomerStatus, "Customer profile status drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, row.ChangedAtUtc, "Customer profile timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, row.RecordSource, "Customer profile record source drifted.");
  }

  private sealed class CustomerProfileHistoryContext(DbContextOptions<CustomerProfileHistoryContext> options)
      : DbContext(options) {
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
