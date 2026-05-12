using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultProviderReadStrategyTests {
  [Fact]
  public async Task ReadDispatchEvaluatesStrategiesByDescendingPriorityUntilFirstCompatibleStrategy() {
    var evaluationOrder = new List<string>();
    var lowPriorityCompatible = new DispatchProbeReadStrategy(
        "low-priority-compatible",
        priority: 10,
        canRead: true,
        evaluationOrder);
    var selectedCompatible = new DispatchProbeReadStrategy(
        "selected-compatible",
        priority: 100,
        canRead: true,
        evaluationOrder);
    var highPriorityIncompatible = new DispatchProbeReadStrategy(
        "high-priority-incompatible",
        priority: 200,
        canRead: false,
        evaluationOrder);
    var readService = new DefaultDataVaultReadService([
        lowPriorityCompatible,
        selectedCompatible,
        highPriorityIncompatible,
    ]);

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var rows = await readService.ReadLatestSatelliteRowsAsync(context, CreateReadRequest(["customer-hk"]));

    Assert.Equal(
        ["high-priority-incompatible", "selected-compatible"],
        evaluationOrder);
    Assert.Equal(1, highPriorityIncompatible.CanReadCallCount);
    Assert.Equal(0, highPriorityIncompatible.ReadCallCount);
    Assert.Equal(1, selectedCompatible.CanReadCallCount);
    Assert.Equal(1, selectedCompatible.ReadCallCount);
    Assert.Equal(0, lowPriorityCompatible.CanReadCallCount);
    Assert.Equal(0, lowPriorityCompatible.ReadCallCount);

    var row = Assert.Single(rows);
    Assert.Equal("selected-compatible", row.MetadataName);
    Assert.Equal("StrategyProbe", row.TableName);
  }

  [Fact]
  public async Task ReadDispatchKeepsRegistrationOrderWhenCompatibleStrategiesSharePriority() {
    var evaluationOrder = new List<string>();
    var firstRegistered = new DispatchProbeReadStrategy(
        "first-registered",
        priority: 100,
        canRead: true,
        evaluationOrder);
    var secondRegistered = new DispatchProbeReadStrategy(
        "second-registered",
        priority: 100,
        canRead: true,
        evaluationOrder);
    var readService = new DefaultDataVaultReadService([firstRegistered, secondRegistered]);

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var rows = await readService.ReadLatestSatelliteRowsAsync(context, CreateReadRequest(["customer-hk"]));

    Assert.Equal(["first-registered"], evaluationOrder);
    Assert.Equal(1, firstRegistered.CanReadCallCount);
    Assert.Equal(1, firstRegistered.ReadCallCount);
    Assert.Equal(0, secondRegistered.CanReadCallCount);
    Assert.Equal(0, secondRegistered.ReadCallCount);
    Assert.Equal("first-registered", Assert.Single(rows).MetadataName);
  }

  [Fact]
  public async Task TypedProjectionReadUsesSelectedProviderStrategy() {
    var strategy = new DispatchProbeReadStrategy(
        "projection-selected",
        priority: 100,
        canRead: true,
        []);
    var readService = new DefaultDataVaultReadService([strategy]);

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var projections = await readService.ReadLatestSatelliteAsync(
        context,
        CreateReadRequest(["customer-hk"]),
        row => new {
          ParentHashKey = row.RequiredString("ParentHashKey"),
          HashDiff = row.RequiredString("HashDiff"),
          LoadTimestamp = row.RequiredDateTimeOffset("LoadTimestamp"),
          RecordSource = row.RequiredString("RecordSource"),
          Name = row.RequiredString("Name"),
        });

    var projection = Assert.Single(projections);
    Assert.Equal("customer-hk", projection.ParentHashKey);
    Assert.Equal("projection-selected-hash", projection.HashDiff);
    Assert.Equal("projection-selected", projection.RecordSource);
    Assert.Equal("projection-selected name", projection.Name);
    Assert.Equal(1, strategy.ProjectionReadCallCount);
    Assert.Equal(0, strategy.ReadCallCount);
  }

  [Fact]
  public async Task ReadDispatchFallsBackWhenNoProviderStrategyIsRegistered() {
    var readService = new DefaultDataVaultReadService();

    await using var context = new DbContext(new DbContextOptionsBuilder().Options);
    var rows = await readService.ReadLatestSatelliteRowsAsync(context, CreateReadRequest([]));

    Assert.Empty(rows);
  }

  private static DataVaultLatestSatelliteReadRequest CreateReadRequest(IEnumerable<string> parentHashKeys) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Name"]);

    return new DataVaultLatestSatelliteReadRequest(profile, parentHashKeys);
  }

  private sealed class DispatchProbeReadStrategy(
      string strategyName,
      int priority,
      bool canRead,
      List<string> evaluationOrder) : IDataVaultProviderReadStrategy {
    private static readonly DateTimeOffset LoadTimestamp = new(2026, 5, 11, 12, 0, 0, TimeSpan.Zero);

    public int CanReadCallCount { get; private set; }

    public int ReadCallCount { get; private set; }

    public int ProjectionReadCallCount { get; private set; }

    public int Priority { get; } = priority;

    public bool CanReadLatestSatelliteRows(
        DbContext dbContext,
        DataVaultLatestSatelliteReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      CanReadCallCount++;
      evaluationOrder.Add(strategyName);

      return canRead;
    }

    public Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      ReadCallCount++;

      return Task.FromResult<IReadOnlyList<DataVaultSatelliteReadRecord>>([
          new DataVaultSatelliteReadRecord(
              strategyName,
              "StrategyProbe",
              "customer-hk",
              new Dictionary<string, string>(StringComparer.Ordinal),
              strategyName + "-hash",
              LoadTimestamp,
              strategyName,
              new Dictionary<string, string>(StringComparer.Ordinal) {
                ["Name"] = strategyName + " name",
              }),
      ]);
    }

    public Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(context);

      ProjectionReadCallCount++;

      return Task.FromResult<IReadOnlyList<DataVaultSatelliteProjectionRow>>([
          new DataVaultSatelliteProjectionRow(
              strategyName,
              new Dictionary<string, DataVaultSatelliteProjectionValue>(StringComparer.Ordinal) {
                ["ParentHashKey"] = DataVaultSatelliteProjectionValue.Present("customer-hk"),
                ["HashDiff"] = DataVaultSatelliteProjectionValue.Present(strategyName + "-hash"),
                ["LoadTimestamp"] = DataVaultSatelliteProjectionValue.Present(LoadTimestamp),
                ["RecordSource"] = DataVaultSatelliteProjectionValue.Present(strategyName),
                ["Name"] = DataVaultSatelliteProjectionValue.Present(strategyName + " name"),
              }),
      ]);
    }
  }
}
