using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultSaveStrategySelectionTests {
  private const string SqliteProviderName = "Microsoft.EntityFrameworkCore.Sqlite";
  private const string ProviderNeutralStrategyRegistrationDiagnostic =
      "Provider-neutral AddDVault fallback scenario expected no provider-specific save strategies to be registered; " +
          "a registered strategy would make this test stop proving the compatibility fallback baseline.";
  private const string ProviderNeutralFallbackDiagnostic =
      "Provider-neutral AddDVault fallback dispatch expected the built-in EF writer to leave a tracked HubCustomer row; " +
          "no tracked row was found, which suggests an optimized provider strategy was selected unexpectedly.";
  private const string SqliteStrategyRegistrationDiagnostic =
      "SQLite optimized dispatch expected AddDVaultSqlite to register a compatible IDataVaultProviderSaveStrategy for " +
          "a clean Microsoft.EntityFrameworkCore.Sqlite context; no registered strategy accepted the request.";
  private const string SqliteOptimizedPathDiagnostic =
      "SQLite optimized dispatch expected SqliteDataVaultSaveStrategy to persist through raw SQL without tracked fallback rows; " +
          "tracked rows were present, so the SQLite capability gate or registration path may be broken.";
  private const string MissingSqliteCapabilityDiagnostic =
      "Missing SQLite capability registration scenario expected AddDVault without AddDVaultSqlite to expose no compatible " +
          "provider save strategy; if this fails, the test no longer proves fallback selection for absent wiring.";
  private const string MissingSqliteFallbackDiagnostic =
      "Missing SQLite capability registration should fall back through IDataVaultSaveService instead of implicitly selecting " +
          "an optimized provider path.";
  private const string UnknownProviderFallbackDiagnostic =
      "Unknown provider dispatch should evaluate but reject incompatible optimized strategies, then select the fallback writer.";
  private const string UnknownProviderSelectedDiagnostic =
      "Unknown provider strategy was selected even though its provider gate did not match the current DbContext provider.";
  private const string PriorityDispatchDiagnostic =
      "Provider strategy dispatch must evaluate registrations by descending Priority, skip incompatible strategies, and " +
          "stop at the first compatible strategy.";
  private const string EqualPriorityTieDispatchDiagnostic =
      "Equal-priority provider strategy dispatch must keep dependency-injection registration order as the deterministic tie-break.";

  [Fact]
  public async Task ProviderNeutralAddDVaultSelectsFallbackWhenNoProviderStrategyIsRegistered() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var strategies = provider.GetServices<IDataVaultProviderSaveStrategy>().ToArray();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    Assert.True(
        strategies.Length == 0,
        ProviderNeutralStrategyRegistrationDiagnostic);

    await using var context = new StrategySelectionContext(options);
    await context.Database.EnsureCreatedAsync();

    var result = await saveService.SaveAsync(context, CreateCustomerSaveRequest("fallback-baseline"));

    AssertSavedCustomer(result);
    await AssertCustomerRowAsync(context, "C-100", "fallback-baseline");
    AssertFallbackPathObserved(
        context,
        ProviderNeutralFallbackDiagnostic);
  }

  [Fact]
  public async Task AddDVaultSqliteSelectsOptimizedStrategyWhenSqliteWiringIsCompatible() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateOptions(database);
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var request = CreateCustomerSaveRequest("sqlite-optimized");

    await using var context = new StrategySelectionContext(options);
    await context.Database.EnsureCreatedAsync();

    var strategies = provider.GetServices<IDataVaultProviderSaveStrategy>().ToArray();
    Assert.True(
        strategies.Any(strategy => strategy.CanSave(context, [request])),
        SqliteStrategyRegistrationDiagnostic);

    var result = await saveService.SaveAsync(context, request);

    AssertSavedCustomer(result);
    await AssertCustomerRowAsync(context, "C-100", "sqlite-optimized");
    AssertOptimizedPathObserved(
        context,
        SqliteOptimizedPathDiagnostic);
  }

  [Fact]
  public async Task SqliteContextFallsBackWhenSqliteCapabilityRegistrationIsMissing() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateOptions(database);
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new StrategySelectionContext(options);
    await context.Database.EnsureCreatedAsync();

    Assert.Equal(SqliteProviderName, context.Database.ProviderName);
    Assert.True(
        !provider.GetServices<IDataVaultProviderSaveStrategy>().Any(),
        MissingSqliteCapabilityDiagnostic);

    var result = await saveService.SaveAsync(context, CreateCustomerSaveRequest("missing-sqlite-capability"));

    AssertSavedCustomer(result);
    await AssertCustomerRowAsync(context, "C-100", "missing-sqlite-capability");
    AssertFallbackPathObserved(
        context,
        MissingSqliteFallbackDiagnostic);
  }

  [Fact]
  public async Task UnknownProviderStrategyDoesNotOverrideFallbackSelection() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateOptions(database);
    var unknownProviderStrategy = new ProviderNameProbeSaveStrategy("Contoso.UnknownProvider", priority: 100);
    var services = new ServiceCollection();
    services.AddDVault();
    services.AddSingleton<IDataVaultProviderSaveStrategy>(unknownProviderStrategy);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new StrategySelectionContext(options);
    await context.Database.EnsureCreatedAsync();

    var result = await saveService.SaveAsync(context, CreateCustomerSaveRequest("unknown-provider-fallback"));

    Assert.Equal(SqliteProviderName, unknownProviderStrategy.LastProviderName);
    Assert.Equal(1, unknownProviderStrategy.CanSaveCallCount);
    Assert.Equal(0, unknownProviderStrategy.SaveCallCount);
    AssertSavedCustomer(result);
    await AssertCustomerRowAsync(context, "C-100", "unknown-provider-fallback");
    AssertFallbackPathObserved(
        context,
        UnknownProviderFallbackDiagnostic);
  }

  [Fact]
  public async Task DispatchEvaluatesStrategiesByDescendingPriorityUntilFirstCompatibleStrategy() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateOptions(database);
    var evaluationOrder = new List<string>();
    var lowPriorityCompatible = new DispatchProbeSaveStrategy(
        "low-priority-compatible",
        priority: 10,
        canSave: true,
        evaluationOrder);
    var selectedCompatible = new DispatchProbeSaveStrategy(
        "selected-compatible",
        priority: 100,
        canSave: true,
        evaluationOrder);
    var highPriorityIncompatible = new DispatchProbeSaveStrategy(
        "high-priority-incompatible",
        priority: 200,
        canSave: false,
        evaluationOrder);
    var services = new ServiceCollection();
    services.AddDVault();
    services.AddSingleton<IDataVaultProviderSaveStrategy>(lowPriorityCompatible);
    services.AddSingleton<IDataVaultProviderSaveStrategy>(selectedCompatible);
    services.AddSingleton<IDataVaultProviderSaveStrategy>(highPriorityIncompatible);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new StrategySelectionContext(options);
    var result = await saveService.SaveAsync(context, CreateCustomerSaveRequest("priority-dispatch"));

    Assert.Equal(
        new[] { "high-priority-incompatible", "selected-compatible" },
        evaluationOrder);
    Assert.Equal(1, highPriorityIncompatible.CanSaveCallCount);
    Assert.Equal(0, highPriorityIncompatible.SaveCallCount);
    Assert.Equal(1, selectedCompatible.CanSaveCallCount);
    Assert.Equal(1, selectedCompatible.SaveCallCount);
    Assert.Equal(0, lowPriorityCompatible.CanSaveCallCount);
    Assert.Equal(0, lowPriorityCompatible.SaveCallCount);
    AssertDispatchProbeResult(
        result,
        "selected-compatible",
        PriorityDispatchDiagnostic);
  }

  [Fact]
  public async Task DispatchKeepsRegistrationOrderWhenCompatibleStrategiesSharePriority() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateOptions(database);
    var evaluationOrder = new List<string>();
    var firstRegistered = new DispatchProbeSaveStrategy(
        "first-registered",
        priority: 100,
        canSave: true,
        evaluationOrder);
    var secondRegistered = new DispatchProbeSaveStrategy(
        "second-registered",
        priority: 100,
        canSave: true,
        evaluationOrder);
    var services = new ServiceCollection();
    services.AddDVault();
    services.AddSingleton<IDataVaultProviderSaveStrategy>(firstRegistered);
    services.AddSingleton<IDataVaultProviderSaveStrategy>(secondRegistered);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new StrategySelectionContext(options);
    var result = await saveService.SaveAsync(context, CreateCustomerSaveRequest("equal-priority-dispatch"));

    Assert.Equal(new[] { "first-registered" }, evaluationOrder);
    Assert.Equal(1, firstRegistered.CanSaveCallCount);
    Assert.Equal(1, firstRegistered.SaveCallCount);
    Assert.Equal(0, secondRegistered.CanSaveCallCount);
    Assert.Equal(0, secondRegistered.SaveCallCount);
    AssertDispatchProbeResult(
        result,
        "first-registered",
        EqualPriorityTieDispatchDiagnostic);
  }

  [Fact]
  public void StrategySelectionFailureDiagnosticsIdentifyDispatchRegressions() {
    AssertDiagnosticContains(
        nameof(ProviderNeutralAddDVaultSelectsFallbackWhenNoProviderStrategyIsRegistered),
        ProviderNeutralStrategyRegistrationDiagnostic,
        "provider-specific save strategies",
        "compatibility fallback baseline");
    AssertDiagnosticContains(
        nameof(ProviderNeutralAddDVaultSelectsFallbackWhenNoProviderStrategyIsRegistered),
        ProviderNeutralFallbackDiagnostic + " Actual tracked entries: <none>",
        "optimized provider strategy was selected unexpectedly",
        "Actual tracked entries");
    AssertDiagnosticContains(
        nameof(AddDVaultSqliteSelectsOptimizedStrategyWhenSqliteWiringIsCompatible),
        SqliteStrategyRegistrationDiagnostic,
        "AddDVaultSqlite",
        "compatible IDataVaultProviderSaveStrategy",
        "no registered strategy accepted");
    AssertDiagnosticContains(
        nameof(AddDVaultSqliteSelectsOptimizedStrategyWhenSqliteWiringIsCompatible),
        SqliteOptimizedPathDiagnostic + " Actual tracked entries: HubCustomer:Unchanged",
        "SqliteDataVaultSaveStrategy",
        "SQLite capability gate or registration path may be broken",
        "Actual tracked entries");
    AssertDiagnosticContains(
        nameof(SqliteContextFallsBackWhenSqliteCapabilityRegistrationIsMissing),
        MissingSqliteCapabilityDiagnostic,
        "Missing SQLite capability registration",
        "no compatible provider save strategy",
        "fallback selection for absent wiring");
    AssertDiagnosticContains(
        nameof(SqliteContextFallsBackWhenSqliteCapabilityRegistrationIsMissing),
        MissingSqliteFallbackDiagnostic + " Actual tracked entries: <none>",
        "Missing SQLite capability registration",
        "fall back through IDataVaultSaveService",
        "optimized provider path");
    AssertDiagnosticContains(
        nameof(UnknownProviderStrategyDoesNotOverrideFallbackSelection),
        UnknownProviderFallbackDiagnostic + " Actual tracked entries: <none>",
        "Unknown provider dispatch",
        "reject incompatible optimized strategies",
        "fallback writer");
    AssertDiagnosticContains(
        nameof(UnknownProviderStrategyDoesNotOverrideFallbackSelection),
        UnknownProviderSelectedDiagnostic,
        "Unknown provider strategy was selected",
        "provider gate did not match");
    AssertDiagnosticContains(
        nameof(DispatchEvaluatesStrategiesByDescendingPriorityUntilFirstCompatibleStrategy),
        PriorityDispatchDiagnostic,
        "descending Priority",
        "skip incompatible strategies",
        "first compatible strategy");
    AssertDiagnosticContains(
        nameof(DispatchKeepsRegistrationOrderWhenCompatibleStrategiesSharePriority),
        EqualPriorityTieDispatchDiagnostic,
        "Equal-priority provider strategy dispatch",
        "registration order",
        "deterministic tie-break");
  }

  private static DbContextOptions<StrategySelectionContext> CreateOptions(SqliteTestDatabase database) {
    return new DbContextOptionsBuilder<StrategySelectionContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
  }

  private static DataVaultSaveRequest CreateCustomerSaveRequest(string recordSource) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    return new DataVaultSaveRequest(
        new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero),
        recordSource,
        [new(customer, [new("Customer Id", "C-100")])],
        []);
  }

  private static void AssertSavedCustomer(DataVaultSaveResult result) {
    Assert.Equal(1, result.RowsWritten);

    var record = Assert.Single(result.SavedRecords);
    Assert.Equal(DataVaultTableKind.Hub, record.Kind);
    Assert.Equal("Customer", record.MetadataName);
    Assert.Equal("HubCustomer", record.TableName);
    Assert.Matches("^[0-9a-f]{64}$", record.HashKey);
  }

  private static async Task AssertCustomerRowAsync(
      StrategySelectionContext context,
      string customerId,
      string recordSource) {
    var row = await context.Set<Dictionary<string, object>>("HubCustomer")
        .AsNoTracking()
        .SingleAsync();

    Assert.Equal(customerId, row["CustomerId"]);
    Assert.Equal(recordSource, row["RecordSource"]);
  }

  private static void AssertFallbackPathObserved(
      StrategySelectionContext context,
      string failureMessage) {
    var trackedEntries = context.ChangeTracker.Entries().ToArray();

    Assert.True(
        trackedEntries.Any(entry =>
            string.Equals(entry.Metadata.GetTableName(), "HubCustomer", StringComparison.Ordinal) &&
            entry.State == EntityState.Unchanged),
        failureMessage + " Actual tracked entries: " + FormatTrackedEntries(trackedEntries));
  }

  private static void AssertOptimizedPathObserved(
      StrategySelectionContext context,
      string failureMessage) {
    var trackedEntries = context.ChangeTracker.Entries().ToArray();

    Assert.True(
        trackedEntries.Length == 0,
        failureMessage + " Actual tracked entries: " + FormatTrackedEntries(trackedEntries));
  }

  private static void AssertDispatchProbeResult(
      DataVaultSaveResult result,
      string expectedStrategyName,
      string failureMessage) {
    var record = Assert.Single(result.SavedRecords);

    Assert.True(
        result.RowsWritten == 0 &&
            string.Equals(record.MetadataName, expectedStrategyName, StringComparison.Ordinal) &&
            string.Equals(record.TableName, "StrategyProbe", StringComparison.Ordinal),
        failureMessage);
  }

  private static string FormatTrackedEntries(IReadOnlyList<EntityEntry> trackedEntries) {
    if (trackedEntries.Count == 0) {
      return "<none>";
    }

    return string.Join(
        ", ",
        trackedEntries
            .Select(entry => (entry.Metadata.GetTableName() ?? entry.Metadata.Name) + ":" + entry.State)
            .OrderBy(entry => entry, StringComparer.Ordinal));
  }

  private static void AssertDiagnosticContains(
      string expectationName,
      string diagnostic,
      params string[] requiredFragments) {
    foreach (var requiredFragment in requiredFragments) {
      Assert.Contains(
          requiredFragment,
          diagnostic,
          StringComparison.Ordinal);
    }

    Assert.False(
        string.IsNullOrWhiteSpace(expectationName),
        "Strategy selection diagnostic catalog entries must be tied to the dispatch expectation they protect.");
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        []);
  }

  private sealed class StrategySelectionContext(DbContextOptions<StrategySelectionContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());
    }
  }

  private sealed class ProviderNameProbeSaveStrategy(string supportedProviderName, int priority) : IDataVaultProviderSaveStrategy {
    public int CanSaveCallCount { get; private set; }

    public int SaveCallCount { get; private set; }

    public string? LastProviderName { get; private set; }

    public int Priority { get; } = priority;

    public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(requests);

      CanSaveCallCount++;
      LastProviderName = dbContext.Database.ProviderName;

      return string.Equals(LastProviderName, supportedProviderName, StringComparison.Ordinal);
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DataVaultProviderSaveStrategyContext context,
        CancellationToken cancellationToken = default) {
      SaveCallCount++;

      throw new InvalidOperationException(
          UnknownProviderSelectedDiagnostic);
    }
  }

  private sealed class DispatchProbeSaveStrategy(
      string strategyName,
      int priority,
      bool canSave,
      ICollection<string> evaluationOrder) : IDataVaultProviderSaveStrategy {
    public int CanSaveCallCount { get; private set; }

    public int SaveCallCount { get; private set; }

    public int Priority { get; } = priority;

    public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(requests);

      CanSaveCallCount++;
      evaluationOrder.Add(strategyName);

      return canSave;
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DataVaultProviderSaveStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      SaveCallCount++;

      return Task.FromResult(new DataVaultSaveResult(
          0,
          [
            new DataVaultSavedRecord(
                DataVaultTableKind.Hub,
                strategyName,
                "StrategyProbe",
                "0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdef"),
          ]));
    }
  }
}
