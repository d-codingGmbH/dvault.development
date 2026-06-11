using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class DbContextPoolingBenchmark : IScenarioBenchmark {
  private const string OrderBusinessKey = "O-POOLED-PERF-100";
  private readonly bool _pooled;

  public DbContextPoolingBenchmark(bool pooled) {
    _pooled = pooled;
  }

  public string ScenarioName => "dbcontext-pooling-dvault-operation";

  public string ProviderName => BenchmarkArtifacts.RequiredProviderName;

  public string BaselineName => _pooled
      ? "adddbcontextpool"
      : "adddbcontext";

  public string StrategyFamily => _pooled
      ? "pooled-dvault-context"
      : "non-pooled-dvault-context";

  public string DatasetSize => "1 generated order hub row";

  public string ChangeRatio => "fixed metadata source and options-only context";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = TempSqliteDatabase.Create();
    using var provider = CreateProvider(database.ConnectionString);

    await using (var scope = provider.CreateAsyncScope()) {
      var context = scope.ServiceProvider.GetRequiredService<PooledEvidenceContext>();
      await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
    }

    string orderHashKey = string.Empty;
    var elapsed = await BenchmarkClock.MeasureAsync(async () => {
      await using var scope = provider.CreateAsyncScope();
      var context = scope.ServiceProvider.GetRequiredService<PooledEvidenceContext>();
      var saveService = scope.ServiceProvider.GetRequiredService<IDataVaultSaveService>();

      var saveResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              new DateTimeOffset(2026, 5, 13, 12, 30, 0, TimeSpan.Zero),
              "dbcontext-pooling-seed",
              [new(CompiledEvidenceScenario.OrderHub, [new("Order Id", OrderBusinessKey)])],
              []),
          cancellationToken).ConfigureAwait(false);
      orderHashKey = saveResult.SavedRecords
          .Single(record => record.Kind == DataVaultTableKind.Hub && record.MetadataName == "Order")
          .HashKey;

      _ = CompiledEvidenceScenario.ReadHubOrder(context, orderHashKey);
    }).ConfigureAwait(false);

    await using (var scope = provider.CreateAsyncScope()) {
      var context = scope.ServiceProvider.GetRequiredService<PooledEvidenceContext>();
      var row = CompiledEvidenceScenario.ReadHubOrder(context, orderHashKey);
      CompiledEvidenceScenario.AssertHubOrder(row, orderHashKey, OrderBusinessKey, "dbcontext-pooling-seed");
      BenchmarkAssert.Equal(
          1,
          await context.Set<Dictionary<string, object>>("HubOrder")
              .AsNoTracking()
              .CountAsync(cancellationToken)
              .ConfigureAwait(false),
          "The DbContext pooling benchmark must persist exactly one generated order hub row.");
    }

    return new ScenarioBenchmarkResult(
        elapsed,
        _pooled
            ? "1 generated order hub row saved and read through AddDbContextPool fixed-model configuration"
            : "1 generated order hub row saved and read through AddDbContext fixed-model configuration");
  }

  private ServiceProvider CreateProvider(string connectionString) {
    var services = new ServiceCollection();
    services.AddDVaultSqlite();
    if (_pooled) {
      services.AddDbContextPool<PooledEvidenceContext>(builder => {
        builder.UseSqlite(connectionString);
      });
    }
    else {
      services.AddDbContext<PooledEvidenceContext>(builder => {
        builder.UseSqlite(connectionString);
      });
    }

    return services.BuildServiceProvider(validateScopes: true);
  }
}
