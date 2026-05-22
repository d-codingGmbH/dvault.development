using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
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
    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.NotEvaluated, result.ReadStrategy.Status);
    Assert.Null(result.ReadShape);
    Assert.Equal(KnownProviderNames.Sqlite, result.SaveStrategy.ProviderName);
    Assert.Equal(KnownProviderNames.Sqlite, result.ReadStrategy.ProviderName);
    Assert.Empty(result.SaveStrategy.Candidates);
    Assert.Empty(result.ReadStrategy.Candidates);
    Assert.Equal("sqlite-v1", result.Explain.CapabilityProfileName);
    Assert.Equal("sqlite-provider-v1", result.Explain.ProviderBehaviorProfileName);
    Assert.Equal(["HubCustomer", "SatCustomerProfile"], result.Explain.Entities.Select(entity => entity.TableName).ToArray());
  }

  [Fact]
  public void AnalyzeSqliteDbContextMigrationOperationsSurfacesFindingsThroughResultIssues() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    using var context = new DiagnosticsContext(CreateOptions(database));

    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(
        diagnostics,
        context,
        [
            new DropTableOperation {
              Name = "HubCustomer",
            },
        ]);

    var issue = Assert.Single(report.Issues);
    var summary = Assert.Single(report.OperationSummaries);
    Assert.Equal("DVM2006", issue.Code);
    Assert.Equal(DataVaultDiagnosticsIssueSeverity.Error, issue.Severity);
    Assert.Equal("migration/DropTable/HubCustomer", issue.Path);
    Assert.Equal(DataVaultMigrationGuardrailOperationOutcome.Incompatible, summary.Outcome);
    Assert.Equal("migration/DropTable/HubCustomer", summary.Path);
    Assert.Equal(issue, Assert.Single(summary.Issues));
    Assert.False(report.IsValid);
    Assert.Same(report.Diagnostics.Issues.Single(), Assert.Single(report.Diagnostics.Validation.Issues));
    Assert.NotEmpty(issue.Remediation);
    Assert.Contains("DVault migration guardrails: invalid", report.ToDisplayString(), StringComparison.Ordinal);
    Assert.Contains(
        "provider Microsoft.EntityFrameworkCore.Sqlite, capability sqlite-v1, provider behavior sqlite-provider-v1",
        report.ToDisplayString(),
        StringComparison.Ordinal);
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
    Assert.Contains(
        "save strategy ProviderStrategySelected (SqliteDataVaultSaveStrategy)",
        result.ToDisplayString(),
        StringComparison.Ordinal);

    var candidate = Assert.Single(result.SaveStrategy.Candidates);
    Assert.Equal(0, candidate.Ordinal);
    Assert.Equal("SqliteDataVaultSaveStrategy", candidate.StrategyName);
    Assert.True(candidate.CanSave);
    Assert.Equal([KnownProviderNames.Sqlite], candidate.SupportedProviderNames);
    Assert.Contains(
        candidate.GateRequirements,
        requirement => requirement.Kind == DataVaultSaveStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        candidate.GateRequirements,
        requirement => requirement.Kind == DataVaultSaveStrategyFallbackCauseKind.DirtyDbContext);
    Assert.Empty(candidate.FallbackCauses);
    Assert.Empty(result.SaveStrategy.FallbackCauses);
  }

  [Fact]
  public void AnalyzeSqliteDbContextLatestSatelliteReadRequestSelectsSqliteStrategy() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVaultSqlite();
    var fallbackServices = new ServiceCollection();
    fallbackServices.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    using var fallbackProvider = fallbackServices.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    var fallbackDiagnostics = fallbackProvider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    using var context = new DiagnosticsContext(CreateOptions(database));
    var request = CreateProfileReadRequest(["customer-hk"]);

    var result = diagnostics.Analyze(context, request);
    var fallbackResult = fallbackDiagnostics.Analyze(context, request);

    Assert.True(result.Validation.IsValid);
    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated, result.SaveStrategy.Status);
    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected, result.ReadStrategy.Status);
    Assert.Equal(KnownProviderNames.Sqlite, result.ReadStrategy.ProviderName);
    Assert.Equal("SqliteDataVaultReadStrategy", result.ReadStrategy.SelectedStrategyName);
    Assert.Equal(100, result.ReadStrategy.SelectedStrategyPriority);
    Assert.Contains(
        "read strategy ProviderStrategySelected (SqliteDataVaultReadStrategy)",
        result.ToDisplayString(),
        StringComparison.Ordinal);

    var candidate = Assert.Single(result.ReadStrategy.Candidates);
    Assert.Equal(0, candidate.Ordinal);
    Assert.Equal("SqliteDataVaultReadStrategy", candidate.StrategyName);
    Assert.True(candidate.CanRead);
    Assert.Equal([KnownProviderNames.Sqlite], candidate.SupportedProviderNames);
    Assert.Contains(
        candidate.GateRequirements,
        requirement => requirement.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent);
    Assert.Contains(
        candidate.GateRequirements,
        requirement => requirement.Kind == DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported);
    Assert.Empty(candidate.FallbackCauses);
    Assert.Empty(result.ReadStrategy.FallbackCauses);
    Assert.NotNull(result.ReadShape);
    var readShape = result.ReadShape!;
    Assert.Equal(DataVaultReadShapeKind.LatestSatellite, readShape.Kind);
    Assert.NotNull(readShape.Satellite);
    var satelliteShape = readShape.Satellite!;
    Assert.Equal(DataVaultSatelliteReadSemantics.Current, satelliteShape.Semantics);
    Assert.Equal("SatCustomerProfile", satelliteShape.Satellite.TableName);
    Assert.Equal(["CustomerHashKey"], satelliteShape.FilterColumns.Single().ColumnNames);
    Assert.Contains(
        satelliteShape.ExpectedIndexBaseline,
        index => index.Kind == "secondary-index" && index.ColumnNames.Contains("CustomerHashKey"));

    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback, fallbackResult.ReadShape!.Provider.ReadStrategyStatus);
    Assert.Contains(
        fallbackResult.ReadShape.Provider.ReadStrategyFallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered);
  }

  [Fact]
  public void AnalyzeSqliteDbContextRegistryReadRequestsPopulateEquivalentReadShape() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var metadataModel = CreateReadShapeMetadataModel();
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    var optionsBuilder = new DbContextOptionsBuilder<ReadShapeDiagnosticsContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False");
    optionsBuilder.UseDataVaultMetadata(DataVaultMetadataRegistry.Create(metadataModel));
    var options = optionsBuilder.Options;
    using var context = new ReadShapeDiagnosticsContext(options);
    var profile = metadataModel.Satellites.Single(satellite => satellite.Name == "Profile");
    var bridge = metadataModel.Bridges.Single();
    var asOf = new DateTimeOffset(2026, 5, 11, 12, 0, 0, TimeSpan.Zero);

    var explicitLatest = diagnostics.Analyze(
        context,
        new DataVaultLatestSatelliteReadRequest(profile, ["customer-hk"], asOf));
    var registryLatest = diagnostics.Analyze(
        context,
        new DataVaultRegistryLatestSatelliteReadRequest(
            DataVaultMetadataReference.Hub("Customer"),
            "Profile",
            ["other-customer-hk"],
            asOf));
    var explicitBridge = diagnostics.Analyze(
        context,
        new DataVaultBridgeReadRequest(
            bridge,
            DataVaultBridgeTraversalEndpoint.From,
            ["customer-hk"]));
    var registryBridge = diagnostics.Analyze(
        context,
        new DataVaultRegistryBridgeReadRequest(
            "CustomerOrder",
            DataVaultBridgeTraversalEndpoint.From,
            ["other-customer-hk"]));

    Assert.NotNull(explicitLatest.ReadShape);
    Assert.NotNull(registryLatest.ReadShape);
    Assert.NotNull(explicitLatest.ReadShape!.Satellite);
    Assert.NotNull(registryLatest.ReadShape!.Satellite);
    Assert.Equal(explicitLatest.ReadShape.Satellite!.Satellite, registryLatest.ReadShape.Satellite!.Satellite);
    Assert.Equal(explicitLatest.ReadShape.Satellite.FilterColumns.SelectMany(columns => columns.ColumnNames),
        registryLatest.ReadShape.Satellite.FilterColumns.SelectMany(columns => columns.ColumnNames));
    Assert.Equal(DataVaultSatelliteReadSemantics.AsOf, registryLatest.ReadShape.Satellite.Semantics);
    Assert.NotNull(explicitBridge.ReadShape);
    Assert.NotNull(registryBridge.ReadShape);
    Assert.NotNull(explicitBridge.ReadShape!.Bridge);
    Assert.NotNull(registryBridge.ReadShape!.Bridge);
    Assert.Equal(explicitBridge.ReadShape.Bridge!.Bridge, registryBridge.ReadShape.Bridge!.Bridge);
    Assert.Equal(explicitBridge.ReadShape.Bridge.EndpointFilter.ColumnNames, registryBridge.ReadShape.Bridge.EndpointFilter.ColumnNames);
    Assert.Equal(DataVaultBridgeKind.ManyToMany, registryBridge.ReadShape.Bridge.BridgeKind);
  }

  [Fact]
  public void PreflightAggregatesSqliteValidationMigrationAndReadDiagnostics() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    using var context = new DiagnosticsContext(CreateOptions(database));

    var report = DataVaultPreflight.Run(
        diagnostics,
        new DataVaultPreflightRequest(context, CreateCustomerMetadataModel()) {
          MigrationOperations = Array.Empty<MigrationOperation>(),
          RepresentativeDiagnosticsRequests = [
            new DataVaultPreflightRepresentativeDiagnosticsRequest(
                "latest-profile",
                dbContext => readDiagnostics.Analyze(dbContext, CreateProfileReadRequest(["customer-hk"]))),
          ],
        });

    Assert.Equal(DataVaultPreflightStatus.Passed, report.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Passed, report.ValidationProvider.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Passed, report.MigrationGuardrail.Status);
    Assert.Equal(DataVaultPreflightSectionStatus.Passed, report.RequestDiagnostics.Status);
    Assert.Equal(KnownProviderNames.Sqlite, report.ValidationProvider.Report!.Explain.ProviderName);
    Assert.Empty(report.MigrationGuardrail.Report!.OperationSummaries);

    var requestDiagnostics = Assert.Single(report.RequestDiagnostics.Report!.Results);
    Assert.Equal("latest-profile", requestDiagnostics.Name);
    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected, requestDiagnostics.Diagnostics.ReadStrategy.Status);
    Assert.Equal("SqliteDataVaultReadStrategy", requestDiagnostics.Diagnostics.ReadStrategy.SelectedStrategyName);
    Assert.Contains("DVault preflight: passed", report.ToDisplayString(), StringComparison.Ordinal);
    Assert.Contains("request-diagnostics: passed", report.ToDisplayString(), StringComparison.Ordinal);
  }

  [Fact]
  public void AnalyzeSqliteDbContextReadRequestReportsDeclineFallbackCause() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    using var context = new DiagnosticsContext(CreateOptions(database));
    var customer = new DataVaultHubMetadata("Customer", ["CustomerNumber"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["EmailAddress"],
        ["ContactType"]);

    var result = diagnostics.Analyze(
        context,
        new DataVaultLatestSatelliteReadRequest(contact, ["customer-hk"]));

    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback, result.ReadStrategy.Status);
    Assert.Null(result.ReadStrategy.SelectedStrategyName);
    Assert.Contains("read strategy ProviderNeutralFallback", result.ToDisplayString(), StringComparison.Ordinal);

    var candidate = Assert.Single(result.ReadStrategy.Candidates);
    Assert.False(candidate.CanRead);
    Assert.Equal([KnownProviderNames.Sqlite], candidate.SupportedProviderNames);
    Assert.Contains(
        candidate.GateRequirements,
        requirement => requirement.Kind == DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported);
    Assert.Contains(
        candidate.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported);
    Assert.Contains(
        result.ReadStrategy.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported);
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
    Assert.Contains("save strategy ProviderNeutralFallback", result.ToDisplayString(), StringComparison.Ordinal);

    var candidate = Assert.Single(result.SaveStrategy.Candidates);
    Assert.False(candidate.CanSave);
    Assert.Equal([KnownProviderNames.Sqlite], candidate.SupportedProviderNames);
    Assert.Contains(
        candidate.GateRequirements,
        requirement => requirement.Kind == DataVaultSaveStrategyFallbackCauseKind.DirtyDbContext);
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
          Assert.Equal([KnownProviderNames.Sqlite], candidate.SupportedProviderNames);
        });
  }

  [Fact]
  public void AnalyzeReadRequestKeepsCandidateOrderingWhenHigherPriorityStrategyDeclines() {
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var rejectingStrategy = new RecordingProviderReadStrategy(priority: 200, _ => false);
    var services = new ServiceCollection();
    services.AddSingleton<IDataVaultProviderReadStrategy>(rejectingStrategy);
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var diagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    using var context = new DiagnosticsContext(CreateOptions(database));

    var result = diagnostics.Analyze(context, CreateProfileReadRequest(["customer-hk"]));

    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected, result.ReadStrategy.Status);
    Assert.Equal("SqliteDataVaultReadStrategy", result.ReadStrategy.SelectedStrategyName);
    Assert.Collection(
        result.ReadStrategy.Candidates,
        candidate => {
          Assert.Equal(0, candidate.Ordinal);
          Assert.Equal(nameof(RecordingProviderReadStrategy), candidate.StrategyName);
          Assert.False(candidate.CanRead);
          Assert.Contains(
              candidate.FallbackCauses,
              cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.StrategyDeclined);
        },
        candidate => {
          Assert.Equal(1, candidate.Ordinal);
          Assert.Equal("SqliteDataVaultReadStrategy", candidate.StrategyName);
          Assert.True(candidate.CanRead);
          Assert.Equal([KnownProviderNames.Sqlite], candidate.SupportedProviderNames);
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

  private static DataVaultMetadataModel CreateReadShapeMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerNumber"]);
    var order = new DataVaultHubMetadata("Order", ["OrderNumber"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Name"]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference());

    return new DataVaultMetadataModel(
        [customer, order],
        [customerOrder],
        [profile],
        Array.Empty<DataVaultPointInTimeMetadata>(),
        [bridge],
        Array.Empty<DataVaultPitMetadata>());
  }

  private static DataVaultLatestSatelliteReadRequest CreateProfileReadRequest(IEnumerable<string> parentHashKeys) {
    var customerHub = new DataVaultHubMetadata("Customer", ["CustomerNumber"]);
    var customerSatellite = new DataVaultSatelliteMetadata(
        "Profile",
        customerHub.ToReference(),
        ["Name"]);

    return new DataVaultLatestSatelliteReadRequest(customerSatellite, parentHashKeys);
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

  private sealed class ReadShapeDiagnosticsContext(DbContextOptions<ReadShapeDiagnosticsContext> options) : DbContext(options) {
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

  private sealed class RecordingProviderReadStrategy(
      int priority,
      Func<DataVaultLatestSatelliteReadRequest, bool> canRead) : IDataVaultProviderReadStrategy {
    public int Priority { get; } = priority;

    public bool CanReadLatestSatelliteRows(
        DbContext dbContext,
        DataVaultLatestSatelliteReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      return canRead(request);
    }

    public Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException("Recording diagnostics strategy is never used for reads.");
    }

    public Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException("Recording diagnostics strategy is never used for reads.");
    }
  }
}
