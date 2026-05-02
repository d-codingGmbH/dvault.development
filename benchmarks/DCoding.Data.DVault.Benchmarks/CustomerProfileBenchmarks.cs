using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CustomerProfilePlainEfBenchmark : IScenarioBenchmark {
  public string ScenarioName => "customer-profile-history";

  public string BaselineName => "conventional-ef";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = TempSqliteDatabase.Create();
    var options = new DbContextOptionsBuilder<CustomerProfileHistoryContext>()
        .UseSqlite(database.ConnectionString)
        .Options;

    await using (var context = new CustomerProfileHistoryContext(options)) {
      await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
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

internal sealed class CustomerProfileDataVaultBenchmark : IScenarioBenchmark {
  public string ScenarioName => "customer-profile-history";

  public string BaselineName => "dvault-explicit-save";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = TempSqliteDatabase.Create();
    var options = new DbContextOptionsBuilder<CustomerProfileDataVaultContext>()
        .UseSqlite(database.ConnectionString)
        .Options;
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using (var context = new CustomerProfileDataVaultContext(options)) {
      await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
    }

    var elapsed = await BenchmarkClock.MeasureAsync(async () => {
      string customerHashKey;
      var firstEvent = ScenarioContracts.CustomerProfileEvents[0];
      await using (var context = new CustomerProfileDataVaultContext(options)) {
        var firstHubResult = await saveService.SaveAsync(
            context,
            new DataVaultSaveRequest(
                firstEvent.ChangedAtUtc,
                firstEvent.RecordSource,
                [new(ScenarioContracts.CustomerHub, [new("Customer Id", firstEvent.CustomerBusinessKey)])],
                []),
            cancellationToken).ConfigureAwait(false);
        customerHashKey = DataVaultBenchmarkHelpers.GetHashKey(firstHubResult, DataVaultTableKind.Hub, "Customer");

        await saveService.SaveAsync(
            context,
            new DataVaultSaveRequest(
                firstEvent.ChangedAtUtc,
                firstEvent.RecordSource,
                [],
                [],
                [CreateSatelliteSaveOperation(firstEvent, customerHashKey)]),
            cancellationToken).ConfigureAwait(false);
      }

      var secondEvent = ScenarioContracts.CustomerProfileEvents[1];
      await using (var context = new CustomerProfileDataVaultContext(options)) {
        await saveService.SaveAsync(
            context,
            new DataVaultSaveRequest(
                secondEvent.ChangedAtUtc,
                secondEvent.RecordSource,
                [new(ScenarioContracts.CustomerHub, [new("Customer Id", secondEvent.CustomerBusinessKey)])],
                []),
            cancellationToken).ConfigureAwait(false);

        await saveService.SaveAsync(
            context,
            new DataVaultSaveRequest(
                secondEvent.ChangedAtUtc,
                secondEvent.RecordSource,
                [],
                [],
                [CreateSatelliteSaveOperation(secondEvent, customerHashKey)]),
            cancellationToken).ConfigureAwait(false);
      }
    }).ConfigureAwait(false);

    await VerifyOutcomeAsync(options, cancellationToken).ConfigureAwait(false);

    return new ScenarioBenchmarkResult(elapsed, "1 customer hub row and 2 profile satellite rows for C-100");
  }

  private static DataVaultSatelliteSaveOperation CreateSatelliteSaveOperation(
      CustomerProfileEvent customerProfileEvent,
      string customerHashKey) {
    return new DataVaultSatelliteSaveOperation(
        ScenarioContracts.CustomerProfileSatellite,
        customerHashKey,
        [
            new("customer_name", customerProfileEvent.CustomerName),
            new("customer_status", customerProfileEvent.CustomerStatus),
        ],
        customerProfileEvent.HashDiff);
  }

  private static async Task VerifyOutcomeAsync(
      DbContextOptions<CustomerProfileDataVaultContext> options,
      CancellationToken cancellationToken) {
    await using var context = new CustomerProfileDataVaultContext(options);
    var customerRows = await context.Set<Dictionary<string, object>>("HubCustomer")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
    var profileRows = (await context.Set<Dictionary<string, object>>("SatCustomerProfile")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false))
        .OrderBy(row => (DateTimeOffset)row["LoadTimestamp"])
        .ToArray();
    var customerRow = BenchmarkAssert.Single(customerRows, "The DVault customer benchmark must persist one customer hub row.");
    var customerHashKey = (string)customerRow["CustomerHashKey"];

    BenchmarkAssert.Equal(ScenarioContracts.CustomerBusinessKey, (string)customerRow["CustomerId"], "Customer hub business key drifted.");
    BenchmarkAssert.True(DataVaultBenchmarkHelpers.IsLowercaseSha256(customerHashKey), "Customer hub hash key must use the stable SHA-256 shape.");
    BenchmarkAssert.Equal(ScenarioContracts.CustomerProfileEvents.Length, profileRows.Length, "The DVault customer benchmark must persist two profile satellite rows.");

    for (var index = 0; index < ScenarioContracts.CustomerProfileEvents.Length; index++) {
      AssertProfileSatelliteRow(profileRows[index], customerHashKey, ScenarioContracts.CustomerProfileEvents[index]);
    }
  }

  private static void AssertProfileSatelliteRow(
      Dictionary<string, object> row,
      string customerHashKey,
      CustomerProfileEvent expected) {
    BenchmarkAssert.Equal(customerHashKey, (string)row["CustomerHashKey"], "Profile satellite parent hash key drifted.");
    BenchmarkAssert.Equal(expected.CustomerName, (string)row["CustomerName"], "Profile satellite customer name drifted.");
    BenchmarkAssert.Equal(expected.CustomerStatus, (string)row["CustomerStatus"], "Profile satellite status drifted.");
    BenchmarkAssert.Equal(expected.HashDiff, (string)row["HashDiff"], "Profile satellite hash diff drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, (DateTimeOffset)row["LoadTimestamp"], "Profile satellite load timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, (string)row["RecordSource"], "Profile satellite record source drifted.");
  }

  private sealed class CustomerProfileDataVaultContext(DbContextOptions<CustomerProfileDataVaultContext> options)
      : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateCustomerProfileDataVaultModel());
    }
  }
}
