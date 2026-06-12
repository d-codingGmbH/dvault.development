using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class BridgeTraversalReadBenchmark : IScenarioBenchmark, IBenchmarkHashKeyVariantSource {
  private const int DescendantCount = 100;
  private const int MaximumDepth = 3;
  private const int DepthCycle = 5;

  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;
  private readonly BenchmarkHashKeyVariant _hashKeyVariant;

  public BridgeTraversalReadBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage)
      : this(provider, strategy, loadTimestampStorage, BenchmarkHashKeyVariant.Default) {
  }

  public BridgeTraversalReadBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      BenchmarkHashKeyVariant hashKeyVariant) {
    ArgumentNullException.ThrowIfNull(provider);
    ArgumentNullException.ThrowIfNull(hashKeyVariant);

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
    _hashKeyVariant = hashKeyVariant;
  }

  public string ScenarioName => "bridge-traversal-read";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy, _hashKeyVariant);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public BenchmarkHashKeyVariant HashKeyVariant => _hashKeyVariant;

  public string DatasetSize => "1 hierarchy ancestor with 100 descendant bridge rows";

  public string ChangeRatio => "maximum depth 3 of 5";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<BridgeTraversalReadContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage, _hashKeyVariant);
    using var provider = ReadBenchmarkServices.CreateProvider(_strategy, _hashKeyVariant);
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    var ancestorHashKey = _hashKeyVariant.CreateDeterministicHashKey("region-root");

    try {
      await using (var context = new BridgeTraversalReadContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
        await SeedBridgeRowsAsync(context, ancestorHashKey, _hashKeyVariant, cancellationToken).ConfigureAwait(false);
      }

      IReadOnlyList<DataVaultBridgeReadRecord> readRows = [];
      var request = new DataVaultBridgeReadRequest(
          BridgeReadScenario.Metadata.Bridge,
          DataVaultBridgeTraversalEndpoint.Ancestor,
          [ancestorHashKey],
          MaximumDepth);
      DataVaultDiagnosticsResult diagnostics;
      await using (var diagnosticsContext = new BridgeTraversalReadContext(options, providerCapabilities)) {
        diagnostics = readDiagnostics.Analyze(diagnosticsContext, request);
        ReadBenchmarkServices.AssertReadStrategySelection(
            _strategy,
            ScenarioName,
            diagnostics);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new BridgeTraversalReadContext(options, providerCapabilities);
        readRows = await readService
            .ReadBridgeRowsAsync(
                context,
                request,
                cancellationToken)
            .ConfigureAwait(false);
      }).ConfigureAwait(false);

      VerifyBridgeRows(readRows, ancestorHashKey, providerCapabilities);

      return new ScenarioBenchmarkResult(
          elapsed,
          ExpectedDepthBoundedRowCount().ToString(CultureInfo.InvariantCulture) +
          " bridge traversal rows read from " +
          DescendantCount.ToString(CultureInfo.InvariantCulture) +
          " seeded hierarchy rows",
          BenchmarkExecutionDetails.CreateReadStrategyDetail(this, diagnostics));
    }
    finally {
      await using var cleanupContext = new BridgeTraversalReadContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private static async Task SeedBridgeRowsAsync(
      BridgeTraversalReadContext context,
      string ancestorHashKey,
      BenchmarkHashKeyVariant hashKeyVariant,
      CancellationToken cancellationToken) {
    ArgumentException.ThrowIfNullOrWhiteSpace(ancestorHashKey);
    ArgumentNullException.ThrowIfNull(hashKeyVariant);

    var rows = context.Set<Dictionary<string, object>>("BridgeSalesRegionHierarchy");

    for (var index = 1; index <= DescendantCount; index++) {
      rows.Add(new Dictionary<string, object>(StringComparer.Ordinal) {
        ["AncestorSalesRegionHashKey"] = ancestorHashKey,
        ["DescendantSalesRegionHashKey"] = hashKeyVariant.CreateDeterministicHashKey(
            "region-" + index.ToString("000", CultureInfo.InvariantCulture)),
        ["TraversalDepth"] = ((index - 1) % DepthCycle) + 1,
      });
    }

    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
  }

  private static void VerifyBridgeRows(
      IReadOnlyList<DataVaultBridgeReadRecord> readRows,
      string ancestorHashKey,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    BenchmarkAssert.Equal(
        ExpectedDepthBoundedRowCount(),
        readRows.Count,
        "The bridge traversal benchmark must return only rows within the requested maximum depth.");
    BenchmarkAssert.True(
        readRows.All(row => row.TraversalDepth is > 0 and <= MaximumDepth),
        "The bridge traversal benchmark returned a row outside the requested depth bound.");

    var firstRow = readRows[0];
    BenchmarkAssert.Equal("SalesRegionHierarchy", firstRow.MetadataName, "The bridge traversal metadata name drifted.");
    BenchmarkAssert.Equal("BridgeSalesRegionHierarchy", firstRow.TableName, "The bridge traversal table name drifted.");
    BenchmarkAssert.Equal(ancestorHashKey, firstRow.EndpointHashKeys[0].HashKey, "The bridge traversal ancestor hash key drifted.");
    DataVaultBenchmarkHelpers.AssertStableHashKey(
        firstRow.EndpointHashKeys[0].HashKey,
        providerCapabilities,
        "The bridge traversal ancestor hash key must use the active stable-hash shape.");
  }

  private static int ExpectedDepthBoundedRowCount() {
    return Enumerable.Range(1, DescendantCount)
        .Count(index => ((index - 1) % DepthCycle) + 1 <= MaximumDepth);
  }
}
