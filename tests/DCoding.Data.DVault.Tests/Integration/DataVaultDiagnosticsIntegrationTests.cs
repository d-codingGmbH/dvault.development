using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultDiagnosticsIntegrationTests {
  [Fact]
  public void AnalyzeSqliteDbContextWithoutSaveRequestKeepsStrategyNotEvaluated() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    using var context = new DiagnosticsContext(CreateOptions(database));

    var result = diagnostics.Analyze(context);

    Assert.True(result.Validation.IsValid);
    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated, result.SaveStrategy.Status);
    Assert.Equal(KnownProviderNames.Sqlite, result.SaveStrategy.ProviderName);
    Assert.Empty(result.SaveStrategy.Candidates);
    Assert.Equal("sqlite-v1", result.Explain.CapabilityProfileName);
    Assert.Equal("sqlite-provider-v1", result.Explain.ProviderBehaviorProfileName);
    Assert.Equal(["HubCustomer", "SatCustomerProfile"], result.Explain.Entities.Select(entity => entity.TableName).ToArray());
  }

  [Fact]
  public void AnalyzeSqliteDbContextSingleSaveRequestSelectsSqliteStrategy() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    using var context = new DiagnosticsContext(CreateOptions(database));

    var result = diagnostics.Analyze(context, CreateCustomerSaveRequest("single-request", "C-100"));

    Assert.True(result.Validation.IsValid);
    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected, result.SaveStrategy.Status);
    Assert.Equal(KnownProviderNames.Sqlite, result.SaveStrategy.ProviderName);
    Assert.Equal("SqliteDataVaultSaveStrategy", result.SaveStrategy.SelectedStrategyName);
    Assert.Equal(100, result.SaveStrategy.SelectedStrategyPriority);

    var candidate = Assert.Single(result.SaveStrategy.Candidates);
    Assert.Equal(0, candidate.Ordinal);
    Assert.Equal("SqliteDataVaultSaveStrategy", candidate.StrategyName);
    Assert.True(candidate.CanSave);
    Assert.Empty(candidate.FallbackCauses);
    Assert.Empty(result.SaveStrategy.FallbackCauses);
  }

  [Fact]
  public void AnalyzeBulkSaveRequestPassesOrderedBatchToStrategyEvaluation() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var recorder = new RecordingProviderSaveStrategy(
        priority: 200,
        requests => requests.Select(request => request.RecordSource).SequenceEqual(["bulk-first", "bulk-second"]));
    var services = new ServiceCollection();
    services.AddSingleton<IDataVaultProviderSaveStrategy>(recorder);
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    using var context = new DiagnosticsContext(CreateOptions(database));
    var bulkRequest = new DataVaultBulkSaveRequest([
        CreateCustomerSaveRequest("bulk-first", "C-200"),
        CreateCustomerSaveRequest("bulk-second", "C-201"),
    ]);

    var result = diagnostics.Analyze(context, bulkRequest);

    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected, result.SaveStrategy.Status);
    Assert.Equal(nameof(RecordingProviderSaveStrategy), result.SaveStrategy.SelectedStrategyName);
    Assert.Equal(["bulk-first", "bulk-second"], recorder.CapturedRequests.Select(request => request.RecordSource).ToArray());
    Assert.Equal(["C-200", "C-201"], recorder.CapturedRequests
        .Select(request => request.HubOperations.Single().BusinessKeyValues["CustomerNumber"])
        .ToArray());
  }

  [Fact]
  public void AnalyzeRegistrySaveRequestResolvesMetadataBeforeStrategyEvaluation() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var recorder = new RecordingProviderSaveStrategy(
        priority: 200,
        requests => requests.Single().HubOperations.Single().Metadata.Name == "Customer");
    var services = new ServiceCollection();
    services.AddSingleton<IDataVaultProviderSaveStrategy>(recorder);
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    using var context = new DiagnosticsContext(CreateOptions(database));
    var registryRequest = new DataVaultRegistrySaveRequest(
        LoadTimestamp,
        "registry-request",
        [new DataVaultRegistryHubSaveOperation("Customer", [new("CustomerNumber", "C-300")])],
        []);

    var result = diagnostics.Analyze(context, registryRequest);

    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected, result.SaveStrategy.Status);
    Assert.Equal(nameof(RecordingProviderSaveStrategy), result.SaveStrategy.SelectedStrategyName);

    var resolvedOperation = recorder.CapturedRequests.Single().HubOperations.Single();
    Assert.Equal("Customer", resolvedOperation.Metadata.Name);
    Assert.Equal("C-300", resolvedOperation.BusinessKeyValues["CustomerNumber"]);
  }

  [Fact]
  public void AnalyzeSingleSaveRequestReportsDirtyContextFallbackCause() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    using var context = new DiagnosticsContext(CreateOptions(database));
    context.Set<TrackedCustomer>().Add(new TrackedCustomer { Id = 1 });

    var result = diagnostics.Analyze(context, CreateCustomerSaveRequest("dirty-context", "C-350"));

    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback, result.SaveStrategy.Status);
    Assert.Null(result.SaveStrategy.SelectedStrategyName);

    var candidate = Assert.Single(result.SaveStrategy.Candidates);
    Assert.False(candidate.CanSave);
    Assert.Contains(
        candidate.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.DirtyDbContext);
    Assert.Contains(
        result.SaveStrategy.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.DirtyDbContext);
  }

  [Fact]
  public void AnalyzeSingleSaveRequestKeepsCandidateOrderingWhenHigherPriorityStrategyDeclines() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var rejectingStrategy = new RecordingProviderSaveStrategy(priority: 200, _ => false);
    var services = new ServiceCollection();
    services.AddSingleton<IDataVaultProviderSaveStrategy>(rejectingStrategy);
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    using var context = new DiagnosticsContext(CreateOptions(database));

    var result = diagnostics.Analyze(context, CreateCustomerSaveRequest("ordered-candidates", "C-400"));

    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected, result.SaveStrategy.Status);
    Assert.Equal("SqliteDataVaultSaveStrategy", result.SaveStrategy.SelectedStrategyName);
    Assert.Collection(
        result.SaveStrategy.Candidates,
        candidate => {
          Assert.Equal(0, candidate.Ordinal);
          Assert.Equal(nameof(RecordingProviderSaveStrategy), candidate.StrategyName);
          Assert.False(candidate.CanSave);
          Assert.Contains(
              candidate.FallbackCauses,
              cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined);
        },
        candidate => {
          Assert.Equal(1, candidate.Ordinal);
          Assert.Equal("SqliteDataVaultSaveStrategy", candidate.StrategyName);
          Assert.True(candidate.CanSave);
        });
  }

  private static readonly DateTimeOffset LoadTimestamp = new(2026, 5, 10, 0, 0, 0, TimeSpan.Zero);

  private static DbContextOptions<DiagnosticsContext> CreateOptions(SqliteTestDatabase database) {
    var optionsBuilder = new DbContextOptionsBuilder<DiagnosticsContext>();
    optionsBuilder
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .UseDataVaultMetadata(DataVaultMetadataRegistry.Create(CreateCustomerMetadataModel()));

    return optionsBuilder.Options;
  }

  private static DataVaultMetadataModel CreateCustomerMetadataModel() {
    var customerHub = new DataVaultHubMetadata("Customer", ["CustomerNumber"]);
    var customerSatellite = new DataVaultSatelliteMetadata(
        "Profile",
        DataVaultMetadataReference.Hub("Customer"),
        ["Name"]);

    return new DataVaultMetadataModel([customerHub], [], [customerSatellite]);
  }

  private static DataVaultSaveRequest CreateCustomerSaveRequest(
      string recordSource,
      string customerNumber) {
    var customerHub = new DataVaultHubMetadata("Customer", ["CustomerNumber"]);

    return new DataVaultSaveRequest(
        LoadTimestamp,
        recordSource,
        [new DataVaultHubSaveOperation(
            customerHub,
            [new("CustomerNumber", customerNumber)])],
        []);
  }

  private sealed class DiagnosticsContext(DbContextOptions<DiagnosticsContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<TrackedCustomer>();
    }
  }

  private sealed class TrackedCustomer {
    public int Id { get; init; }
  }

  private sealed class RecordingProviderSaveStrategy(
      int priority,
      Func<IReadOnlyList<DataVaultSaveRequest>, bool> canSave) : IDataVaultProviderSaveStrategy {
    public IReadOnlyList<DataVaultSaveRequest> CapturedRequests { get; private set; } = [];

    public int Priority { get; } = priority;

    public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(requests);

      CapturedRequests = requests.ToArray();

      return canSave(CapturedRequests);
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DataVaultProviderSaveStrategyContext context,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException("Recording diagnostics strategy is never used for persistence.");
    }
  }
}
