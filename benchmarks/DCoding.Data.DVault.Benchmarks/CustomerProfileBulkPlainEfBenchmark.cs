using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CustomerProfileBulkPlainEfBenchmark : IScenarioBenchmark {
  private readonly BenchmarkDatabaseProvider _provider;
  private readonly CustomerProfileBulkScenarioDefinition _scenario;

  public CustomerProfileBulkPlainEfBenchmark(CustomerProfileBulkScenarioDefinition scenario) {
    ArgumentNullException.ThrowIfNull(scenario);

    _provider = BenchmarkDatabaseProviders.Sqlite;
    _scenario = scenario;
  }

  public CustomerProfileBulkPlainEfBenchmark(
      BenchmarkDatabaseProvider provider,
      CustomerProfileBulkScenarioDefinition scenario) {
    ArgumentNullException.ThrowIfNull(provider);
    ArgumentNullException.ThrowIfNull(scenario);

    _provider = provider;
    _scenario = scenario;
  }

  public string ScenarioName => _scenario.ScenarioName;

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => "conventional-ef-bulk";

  public string StrategyFamily => DataVaultBenchmarkHelpers.ClassicEfStrategyFamily;

  public string DatasetSize => _scenario.DatasetSize;

  public string ChangeRatio => _scenario.ChangeRatio;

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<CustomerProfileBulkHistoryContext>();

    try {
      await using (var context = new CustomerProfileBulkHistoryContext(options)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new CustomerProfileBulkHistoryContext(options);
        var rowId = 0L;

        foreach (var customerProfileEvent in _scenario.CreateEvents()) {
          context.CustomerProfileHistoryRows.Add(new CustomerProfileBulkHistoryRow {
            Id = ++rowId,
            CustomerBusinessKey = customerProfileEvent.CustomerBusinessKey,
            CustomerName = customerProfileEvent.CustomerName,
            CustomerStatus = customerProfileEvent.CustomerStatus,
            ChangedAtUtc = customerProfileEvent.ChangedAtUtc,
            RecordSource = customerProfileEvent.RecordSource,
          });
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      }).ConfigureAwait(false);

      await VerifyOutcomeAsync(options, _scenario, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          _scenario.TotalChangeCount.ToString(CultureInfo.InvariantCulture) +
          " customer profile history rows for " +
          _scenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " customers");
    }
    finally {
      await using var cleanupContext = new CustomerProfileBulkHistoryContext(options);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private static async Task VerifyOutcomeAsync(
      DbContextOptions<CustomerProfileBulkHistoryContext> options,
      CustomerProfileBulkScenarioDefinition scenario,
      CancellationToken cancellationToken) {
    await using var context = new CustomerProfileBulkHistoryContext(options);
    var sampleBusinessKey = scenario.CreateBusinessKey(scenario.SampleCustomerIndex);
    var rows = (await context.CustomerProfileHistoryRows
        .AsNoTracking()
        .Where(row => row.CustomerBusinessKey == sampleBusinessKey)
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false))
        .OrderBy(row => row.ChangedAtUtc)
        .ThenBy(row => row.Id)
        .ToArray();

    BenchmarkAssert.Equal(
        scenario.TotalChangeCount,
        await context.CustomerProfileHistoryRows.AsNoTracking().CountAsync(cancellationToken).ConfigureAwait(false),
        "The conventional EF bulk customer profile benchmark must persist every contracted history row.");
    BenchmarkAssert.Equal(
        scenario.ChangeCount,
        rows.Length,
        "The conventional EF bulk customer profile benchmark must persist every sample customer state.");

    AssertHistoryRow(rows[0], scenario.CreateEvent(scenario.SampleCustomerIndex, 0));
    AssertHistoryRow(
        rows[^1],
        scenario.CreateEvent(
            scenario.SampleCustomerIndex,
            scenario.ChangeCount - 1));
  }

  private static void AssertHistoryRow(CustomerProfileBulkHistoryRow row, CustomerProfileBulkEvent expected) {
    BenchmarkAssert.Equal(expected.CustomerBusinessKey, row.CustomerBusinessKey, "Bulk customer profile business key drifted.");
    BenchmarkAssert.Equal(expected.CustomerName, row.CustomerName, "Bulk customer profile name drifted.");
    BenchmarkAssert.Equal(expected.CustomerStatus, row.CustomerStatus, "Bulk customer profile status drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, row.ChangedAtUtc, "Bulk customer profile timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, row.RecordSource, "Bulk customer profile record source drifted.");
  }

  private sealed class CustomerProfileBulkHistoryContext(DbContextOptions<CustomerProfileBulkHistoryContext> options)
      : DbContext(options) {
    public DbSet<CustomerProfileBulkHistoryRow> CustomerProfileHistoryRows => Set<CustomerProfileBulkHistoryRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<CustomerProfileBulkHistoryRow>(entity => {
        entity.ToTable("CustomerProfileBulkHistory");
        entity.HasKey(row => row.Id);
        entity.Property(row => row.Id).ValueGeneratedNever();
        entity.Property(row => row.CustomerBusinessKey).IsRequired();
        entity.Property(row => row.CustomerName).IsRequired();
        entity.Property(row => row.CustomerStatus).IsRequired();
        entity.Property(row => row.ChangedAtUtc).IsRequired();
        entity.Property(row => row.RecordSource).IsRequired();
        entity.HasIndex(row => new { row.CustomerBusinessKey, row.ChangedAtUtc });
      });
    }
  }

  private sealed class CustomerProfileBulkHistoryRow {
    public long Id { get; set; }

    public string CustomerBusinessKey { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerStatus { get; set; } = string.Empty;

    public DateTimeOffset ChangedAtUtc { get; set; }

    public string RecordSource { get; set; } = string.Empty;
  }
}
