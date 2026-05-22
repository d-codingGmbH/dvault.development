using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CompiledModelBenchmark : IScenarioBenchmark {
  private const string OrderBusinessKey = "O-COMPILED-PERF-100";
  private readonly bool _useRuntimeModel;

  public CompiledModelBenchmark(bool useRuntimeModel) {
    _useRuntimeModel = useRuntimeModel;
  }

  public string ScenarioName => "compiled-model-startup";

  public string ProviderName => BenchmarkArtifacts.RequiredProviderName;

  public string BaselineName => _useRuntimeModel
      ? "dvault-usemodel-runtime-model"
      : "dvault-design-model";

  public string StrategyFamily => _useRuntimeModel
      ? "ef-usemodel-runtime-model"
      : "ef-model-build";

  public string DatasetSize => "1 generated order hub row";

  public string ChangeRatio => "runtime model precomputed outside measured operation";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = TempSqliteDatabase.Create();
    var metadataModel = CompiledEvidenceScenario.CreateOrderMetadataModel();
    var designOptions = CompiledEvidenceScenario.CreateOptions(database.ConnectionString);
    string orderHashKey;

    await using (var context = new CompiledEvidenceContext(
        designOptions,
        metadataModel,
        "compiled-model-seed")) {
      await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      orderHashKey = await CompiledEvidenceScenario
          .SeedOrderHubAsync(context, OrderBusinessKey, "compiled-model-seed", cancellationToken)
          .ConfigureAwait(false);
    }

    IModel? runtimeModel = null;
    if (_useRuntimeModel) {
      await using var designContext = new CompiledEvidenceContext(
          designOptions,
          metadataModel,
          "compiled-model-design");
      runtimeModel = CompiledEvidenceScenario.CreateRuntimeModel(designContext);
    }

    var measuredModelCacheKey = "compiled-model-measured-" + Guid.NewGuid().ToString("N");
    var measuredOptions = _useRuntimeModel
        ? CompiledEvidenceScenario.CreateRuntimeModelOptions(database.ConnectionString, runtimeModel!)
        : CompiledEvidenceScenario.CreateOptions(database.ConnectionString);
    CompiledHubOrderRead? row = null;
    var elapsed = await BenchmarkClock.MeasureAsync(() => {
      using var context = new CompiledEvidenceContext(
          measuredOptions,
          metadataModel,
          measuredModelCacheKey);
      row = CompiledEvidenceScenario.ReadHubOrder(context, orderHashKey);

      return Task.CompletedTask;
    }).ConfigureAwait(false);

    CompiledEvidenceScenario.AssertHubOrder(row, orderHashKey, OrderBusinessKey, "compiled-model-seed");

    return new ScenarioBenchmarkResult(
        elapsed,
        _useRuntimeModel
            ? "1 generated order hub row read through precomputed UseModel(runtimeModel)"
            : "1 generated order hub row read through ordinary DVault model building");
  }
}

internal sealed class CompiledQueryBenchmark : IScenarioBenchmark {
  private const string OrderBusinessKey = "O-COMPILED-QUERY-PERF-100";
  private readonly bool _compiledQuery;

  public CompiledQueryBenchmark(bool compiledQuery) {
    _compiledQuery = compiledQuery;
  }

  public string ScenarioName => "compiled-query-hub-read";

  public string ProviderName => BenchmarkArtifacts.RequiredProviderName;

  public string BaselineName => _compiledQuery
      ? "ef-compilequery"
      : "ordinary-ef-query";

  public string StrategyFamily => _compiledQuery
      ? "compiled-ef-query"
      : "direct-ef-query";

  public string DatasetSize => "1 generated order hub row";

  public string ChangeRatio => "stable shared-type table projection";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = TempSqliteDatabase.Create();
    var metadataModel = CompiledEvidenceScenario.CreateOrderMetadataModel();
    var options = CompiledEvidenceScenario.CreateOptions(database.ConnectionString);
    string orderHashKey;

    await using (var context = new CompiledEvidenceContext(options, metadataModel, "compiled-query")) {
      await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      orderHashKey = await CompiledEvidenceScenario
          .SeedOrderHubAsync(context, OrderBusinessKey, "compiled-query-seed", cancellationToken)
          .ConfigureAwait(false);
    }

    CompiledHubOrderRead? row = null;
    var elapsed = await BenchmarkClock.MeasureAsync(() => {
      using var context = new CompiledEvidenceContext(options, metadataModel, "compiled-query");
      row = _compiledQuery
          ? CompiledEvidenceScenario.ReadHubOrderCompiled(context, orderHashKey)
          : CompiledEvidenceScenario.ReadHubOrder(context, orderHashKey);

      return Task.CompletedTask;
    }).ConfigureAwait(false);

    CompiledEvidenceScenario.AssertHubOrder(row, orderHashKey, OrderBusinessKey, "compiled-query-seed");

    return new ScenarioBenchmarkResult(
        elapsed,
        _compiledQuery
            ? "1 generated order hub row read through EF.CompileQuery stable projection"
            : "1 generated order hub row read through equivalent ordinary EF projection");
  }
}

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

internal static class CompiledEvidenceScenario {
  public static readonly DataVaultHubMetadata OrderHub = new("Order", ["Order Id"]);

  private static readonly DataVaultSatelliteMetadata OrderFulfillmentSatellite = new(
      "Fulfillment",
      OrderHub.ToReference(),
      ["Status Code"]);
  private static readonly DataVaultMetadataModel OrderMetadataModel = new(
      [OrderHub],
      [],
      [OrderFulfillmentSatellite]);

  private static readonly Func<CompiledEvidenceContext, string, CompiledHubOrderRead> ReadHubOrderByHashKey =
      EF.CompileQuery((CompiledEvidenceContext context, string orderHashKey) =>
          context.Set<Dictionary<string, object>>("HubOrder")
              .AsNoTracking()
              .Where(row => EF.Property<string>(row, "OrderHashKey") == orderHashKey)
              .Select(row => new CompiledHubOrderRead(
                  EF.Property<string>(row, "OrderHashKey"),
                  EF.Property<string>(row, "OrderId"),
                  EF.Property<string>(row, "RecordSource")))
              .Single());

  public static DataVaultMetadataModel CreateOrderMetadataModel() {
    return OrderMetadataModel;
  }

  public static DbContextOptions<CompiledEvidenceContext> CreateOptions(string connectionString) {
    return new DbContextOptionsBuilder<CompiledEvidenceContext>()
        .UseSqlite(connectionString)
        .ReplaceService<IModelCacheKeyFactory, CompiledEvidenceModelCacheKeyFactory>()
        .Options;
  }

  public static DbContextOptions<CompiledEvidenceContext> CreateRuntimeModelOptions(
      string connectionString,
      IModel runtimeModel) {
    return new DbContextOptionsBuilder<CompiledEvidenceContext>()
        .UseSqlite(connectionString)
        .UseModel(runtimeModel)
        .Options;
  }

  public static IModel CreateRuntimeModel(DbContext context) {
    var designModel = context.GetService<IDesignTimeModel>().Model;

    return context.GetService<IModelRuntimeInitializer>()
        .Initialize(designModel, designTime: false, validationLogger: null);
  }

  public static async Task<string> SeedOrderHubAsync(
      CompiledEvidenceContext context,
      string orderBusinessKey,
      string recordSource,
      CancellationToken cancellationToken) {
    var services = new ServiceCollection();
    services.AddDVaultSqlite();
    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var saveResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 13, 12, 0, 0, TimeSpan.Zero),
            recordSource,
            [new(OrderHub, [new("Order Id", orderBusinessKey)])],
            []),
        cancellationToken).ConfigureAwait(false);

    return saveResult.SavedRecords
        .Single(record => record.Kind == DataVaultTableKind.Hub && record.MetadataName == "Order")
        .HashKey;
  }

  public static CompiledHubOrderRead ReadHubOrderCompiled(
      CompiledEvidenceContext context,
      string orderHashKey) {
    return ReadHubOrderByHashKey(context, orderHashKey);
  }

  public static CompiledHubOrderRead ReadHubOrder(
      DbContext context,
      string orderHashKey) {
    return context.Set<Dictionary<string, object>>("HubOrder")
        .AsNoTracking()
        .Where(row => EF.Property<string>(row, "OrderHashKey") == orderHashKey)
        .Select(row => new CompiledHubOrderRead(
            EF.Property<string>(row, "OrderHashKey"),
            EF.Property<string>(row, "OrderId"),
            EF.Property<string>(row, "RecordSource")))
        .Single();
  }

  public static void AssertHubOrder(
      CompiledHubOrderRead? row,
      string orderHashKey,
      string orderBusinessKey,
      string recordSource) {
    ArgumentNullException.ThrowIfNull(row);

    BenchmarkAssert.Equal(orderHashKey, row.OrderHashKey, "The compiled evidence order hash key drifted.");
    BenchmarkAssert.Equal(orderBusinessKey, row.OrderId, "The compiled evidence order business key drifted.");
    BenchmarkAssert.Equal(recordSource, row.RecordSource, "The compiled evidence record source drifted.");
  }
}

internal sealed class CompiledEvidenceContext(
    DbContextOptions<CompiledEvidenceContext> options,
    DataVaultMetadataModel metadataModel,
    object modelCacheKey) : DbContext(options) {
  public DataVaultMetadataModel MetadataModel { get; } = metadataModel;

  public object ModelCacheKey { get; } = modelCacheKey;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyDataVaultMetadata(MetadataModel);
  }
}

internal sealed class PooledEvidenceContext(DbContextOptions<PooledEvidenceContext> options)
    : DbContext(options) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyDataVaultMetadata(CompiledEvidenceScenario.CreateOrderMetadataModel());
  }
}

internal sealed class CompiledEvidenceModelCacheKeyFactory : IModelCacheKeyFactory {
  public object Create(DbContext context, bool designTime) {
    return context is CompiledEvidenceContext evidenceContext
        ? (context.GetType(), evidenceContext.MetadataModel, evidenceContext.ModelCacheKey, designTime)
        : (object)(context.GetType(), designTime);
  }
}

internal sealed record CompiledHubOrderRead(
    string OrderHashKey,
    string OrderId,
    string RecordSource);
