using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultTelemetryTests {
  [Fact]
  public void AddDVaultKeepsTelemetryOptIn() {
    var defaultServices = new ServiceCollection();
    defaultServices.AddDVault();

    using var defaultProvider = defaultServices.BuildServiceProvider(validateScopes: true);
    Assert.Empty(defaultProvider.GetServices<IDataVaultTelemetryObserver>());

    var telemetryServices = new ServiceCollection();
    telemetryServices.AddDVault();
    telemetryServices.AddDVaultTelemetry();
    telemetryServices.AddDVaultTelemetry();

    using var telemetryProvider = telemetryServices.BuildServiceProvider(validateScopes: true);
    Assert.IsType<DataVaultMeterTelemetryObserver>(Assert.Single(telemetryProvider.GetServices<IDataVaultTelemetryObserver>()));
  }

  [Fact]
  public async Task SaveTelemetryEmitsSingleSelectedStrategySummary() {
    var observer = new CapturingTelemetryObserver();
    var strategy = new ReturningSaveStrategy(canSave: true);
    var saveService = new DefaultDataVaultSaveService(
        new TestStableHashService(),
        new TestStableHashNormalizer(),
        [DefaultDataVaultLoadTimestampResolver.Instance],
        [DefaultDataVaultRecordSourceResolver.Instance],
        [strategy],
        [observer]);

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var result = await saveService.SaveAsync(context, CreateMixedSaveRequest("crm-import"));

    Assert.Equal(7, result.RowsWritten);
    var summary = Assert.Single(observer.SaveSummaries);
    Assert.Equal(DataVaultSaveTelemetryOperationKind.SingleRequest, summary.OperationKind);
    Assert.Equal(DataVaultTelemetryOutcome.Succeeded, summary.Outcome);
    Assert.Equal(1, summary.RequestCount);
    Assert.Equal(1, summary.HubOperationCount);
    Assert.Equal(1, summary.LinkOperationCount);
    Assert.Equal(1, summary.SatelliteOperationCount);
    Assert.Equal(7, summary.RowsWritten);
    Assert.Equal(1, summary.SavedRecordCount);
    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected, summary.StrategyStatus);
    Assert.Equal(nameof(ReturningSaveStrategy), summary.SelectedStrategyName);
    Assert.Empty(summary.FallbackCauseKinds);
    Assert.True(summary.Duration >= TimeSpan.Zero);
  }

  [Fact]
  public async Task SaveTelemetryEmitsBulkFailureWithFallbackClassification() {
    var observer = new CapturingTelemetryObserver();
    var saveService = new DefaultDataVaultSaveService(
        new TestStableHashService(),
        new TestStableHashNormalizer(),
        [DefaultDataVaultLoadTimestampResolver.Instance],
        [DefaultDataVaultRecordSourceResolver.Instance],
        [],
        [observer]);

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);

    await Assert.ThrowsAnyAsync<Exception>(() =>
        saveService.SaveAsync(
            context,
            new DataVaultBulkSaveRequest([
                CreateMixedSaveRequest("crm-import"),
                CreateHubOnlySaveRequest("crm-replay"),
            ])));

    var summary = Assert.Single(observer.SaveSummaries);
    Assert.Equal(DataVaultSaveTelemetryOperationKind.BulkRequest, summary.OperationKind);
    Assert.Equal(DataVaultTelemetryOutcome.Failed, summary.Outcome);
    Assert.Equal(2, summary.RequestCount);
    Assert.Equal(2, summary.HubOperationCount);
    Assert.Equal(1, summary.LinkOperationCount);
    Assert.Equal(1, summary.SatelliteOperationCount);
    Assert.Equal(0, summary.RowsWritten);
    Assert.Equal(DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback, summary.StrategyStatus);
    Assert.Contains(DataVaultSaveStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered, summary.FallbackCauseKinds);
    Assert.Null(summary.SelectedStrategyName);
  }

  [Fact]
  public async Task ReadTelemetryEmitsExactlyOnceForTypedSatelliteProjection() {
    var observer = new CapturingTelemetryObserver();
    var strategy = new ReturningReadStrategy();
    IDataVaultReadService readService = new DefaultDataVaultReadService(
        [strategy],
        [],
        [],
        [observer]);

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var projections = await readService.ReadLatestSatelliteAsync(
        context,
        CreateLatestSatelliteRequest(["customer-hk"]),
        row => new {
          ParentHashKey = row.RequiredString("ParentHashKey"),
          Name = row.RequiredString("Name"),
        });

    Assert.Equal("customer-hk", Assert.Single(projections).ParentHashKey);
    var summary = Assert.Single(observer.ReadSummaries);
    Assert.Equal(DataVaultReadTelemetryFamily.LatestSatellite, summary.Family);
    Assert.Equal(DataVaultTelemetryOutcome.Succeeded, summary.Outcome);
    Assert.Equal(1, summary.RequestedKeyCount);
    Assert.Equal(1, summary.ReturnedRowCount);
    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected, summary.StrategyStatus);
    Assert.Equal(nameof(ReturningReadStrategy), summary.SelectedStrategyName);
    Assert.Empty(summary.FallbackCauseKinds);
    Assert.True(summary.Duration >= TimeSpan.Zero);
  }

  [Fact]
  public async Task ReadTelemetryEmitsPitAndBridgeFallbackFailures() {
    var observer = new CapturingTelemetryObserver();
    var readService = new DefaultDataVaultReadService(
        [],
        [],
        [],
        [observer]);

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);

    await Assert.ThrowsAnyAsync<Exception>(() =>
        readService.ReadPitRowsAsync(
            context,
            CreatePitReadRequest(["customer-hk"])));
    await Assert.ThrowsAnyAsync<Exception>(() =>
        readService.ReadBridgeRowsAsync(
            context,
            CreateBridgeReadRequest(["customer-hk"])));

    Assert.Collection(
        observer.ReadSummaries,
        pit => {
          Assert.Equal(DataVaultReadTelemetryFamily.Pit, pit.Family);
          Assert.Equal(DataVaultTelemetryOutcome.Failed, pit.Outcome);
          Assert.Equal(1, pit.RequestedKeyCount);
          Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback, pit.StrategyStatus);
          Assert.Contains(DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered, pit.FallbackCauseKinds);
        },
        bridge => {
          Assert.Equal(DataVaultReadTelemetryFamily.Bridge, bridge.Family);
          Assert.Equal(DataVaultTelemetryOutcome.Failed, bridge.Outcome);
          Assert.Equal(1, bridge.RequestedKeyCount);
          Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback, bridge.StrategyStatus);
          Assert.Contains(DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered, bridge.FallbackCauseKinds);
        });
  }

  private static DataVaultSaveRequest CreateMixedSaveRequest(string recordSource) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var profile = new DataVaultSatelliteMetadata("Profile", customer.ToReference(), ["Name"]);

    return new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 20, 8, 0, 0, TimeSpan.Zero),
        recordSource,
        [new DataVaultHubSaveOperation(customer, [new("Customer Id", "C-100")])],
        [new DataVaultLinkSaveOperation(customerOrder, [new("Customer", "customer-hk"), new("Order", "order-hk")])],
        [new DataVaultSatelliteSaveOperation(profile, "customer-hk", [new("Name", "Alice")], "profile-hash")]);
  }

  private static DataVaultSaveRequest CreateHubOnlySaveRequest(string recordSource) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    return new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 20, 8, 5, 0, TimeSpan.Zero),
        recordSource,
        [new DataVaultHubSaveOperation(customer, [new("Customer Id", "C-200")])],
        []);
  }

  private static DataVaultLatestSatelliteReadRequest CreateLatestSatelliteRequest(IEnumerable<string> parentHashKeys) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata("Profile", customer.ToReference(), ["Name"]);

    return new DataVaultLatestSatelliteReadRequest(profile, parentHashKeys);
  }

  private static DataVaultPitAsOfReadRequest CreatePitReadRequest(IEnumerable<string> parentHashKeys) {
    var pit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);

    return new DataVaultPitAsOfReadRequest(
        pit,
        parentHashKeys,
        new DateTimeOffset(2026, 5, 20, 9, 0, 0, TimeSpan.Zero));
  }

  private static DataVaultBridgeReadRequest CreateBridgeReadRequest(IEnumerable<string> endpointHashKeys) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference());

    return new DataVaultBridgeReadRequest(
        bridge,
        DataVaultBridgeTraversalEndpoint.From,
        endpointHashKeys);
  }

  private sealed class CapturingTelemetryObserver : IDataVaultTelemetryObserver {
    public List<DataVaultSaveTelemetrySummary> SaveSummaries { get; } = [];

    public List<DataVaultReadTelemetrySummary> ReadSummaries { get; } = [];

    public void RecordSave(DataVaultSaveTelemetrySummary summary) {
      SaveSummaries.Add(summary);
    }

    public void RecordRead(DataVaultReadTelemetrySummary summary) {
      ReadSummaries.Add(summary);
    }
  }

  private sealed class ReturningSaveStrategy(bool canSave) : IDataVaultProviderSaveStrategy {
    public int Priority => 100;

    public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(requests);

      return canSave;
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DataVaultProviderSaveStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      return Task.FromResult(new DataVaultSaveResult(
          7,
          [new DataVaultSavedRecord(DataVaultTableKind.Hub, "Customer", "HubCustomer", "customer-hk")]));
    }
  }

  private sealed class ReturningReadStrategy : IDataVaultProviderReadStrategy {
    private static readonly DateTimeOffset LoadTimestamp = new(2026, 5, 20, 8, 30, 0, TimeSpan.Zero);

    public int Priority => 100;

    public bool CanReadLatestSatelliteRows(
        DbContext dbContext,
        DataVaultLatestSatelliteReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      return true;
    }

    public Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      return Task.FromResult<IReadOnlyList<DataVaultSatelliteReadRecord>>([
          new DataVaultSatelliteReadRecord(
              "Profile",
              "SatCustomerProfile",
              "customer-hk",
              new Dictionary<string, string>(StringComparer.Ordinal),
              "profile-hash",
              LoadTimestamp,
              "crm-import",
              new Dictionary<string, string>(StringComparer.Ordinal) {
                ["Name"] = "Alice",
              }),
      ]);
    }

    public Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      return Task.FromResult<IReadOnlyList<DataVaultSatelliteProjectionRow>>([
          new DataVaultSatelliteProjectionRow(
              "Profile",
              new Dictionary<string, DataVaultSatelliteProjectionValue>(StringComparer.Ordinal) {
                ["ParentHashKey"] = DataVaultSatelliteProjectionValue.Present("customer-hk"),
                ["HashDiff"] = DataVaultSatelliteProjectionValue.Present("profile-hash"),
                ["LoadTimestamp"] = DataVaultSatelliteProjectionValue.Present(LoadTimestamp),
                ["RecordSource"] = DataVaultSatelliteProjectionValue.Present("crm-import"),
                ["Name"] = DataVaultSatelliteProjectionValue.Present("Alice"),
              }),
      ]);
    }
  }

  private sealed class TestStableHashService : IStableHashService {
    public string AlgorithmId => "test-sha256-v1";

    public StableHashDigest ComputeHash(string normalizedInput) {
      ArgumentNullException.ThrowIfNull(normalizedInput);

      return new StableHashDigest(AlgorithmId, new string('a', 64));
    }
  }

  private sealed class TestStableHashNormalizer : IStableHashNormalizer {
    public string NormalizeValue(object? value) {
      return value?.ToString() ?? string.Empty;
    }

    public string NormalizeFields(IEnumerable<KeyValuePair<string, object?>> fields) {
      ArgumentNullException.ThrowIfNull(fields);

      return string.Join(
          "\n",
          fields.Select(field => field.Key + "=" + NormalizeValue(field.Value)));
    }
  }
}
