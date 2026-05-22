using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultCompiledCompatibilitySqliteTests {
  private static readonly Func<CompiledCompatibilityContext, string, CompiledHubOrderRead> ReadHubOrderByHashKey =
      EF.CompileQuery((CompiledCompatibilityContext context, string orderHashKey) =>
          context.Set<Dictionary<string, object>>("HubOrder")
              .AsNoTracking()
              .Where(row => EF.Property<string>(row, "OrderHashKey") == orderHashKey)
              .Select(row => new CompiledHubOrderRead(
                  EF.Property<string>(row, "OrderHashKey"),
                  EF.Property<string>(row, "OrderId"),
                  EF.Property<string>(row, "RecordSource")))
              .Single());

  [Fact]
  public void CompiledModelKeepsDataVaultMetadataAnnotationsAfterRuntimeModelInitialization() {
    var metadataModel = CreateOrderMetadataModel();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var designOptions = CreateOptions(database);

    using var designContext = new CompiledCompatibilityContext(designOptions, metadataModel);
    var compiledRuntimeModel = CreateCompiledRuntimeModel(designContext);
    var compiledOptions = new DbContextOptionsBuilder<CompiledCompatibilityContext>()
        .UseSqlite(CreateConnectionString(database))
        .UseModel(compiledRuntimeModel)
        .Options;

    using var compiledContext = new CompiledCompatibilityContext(compiledOptions, metadataModel);
    var compiledModel = compiledContext.Model;
    var hub = AssertEntity(compiledModel, "HubOrder", "compiled model metadata entity availability");
    var satellite = AssertEntity(compiledModel, "SatOrderFulfillment", "compiled model metadata entity availability");
    var orderId = AssertProperty(hub, "OrderId", "compiled model metadata property availability");
    var recordSource = AssertProperty(satellite, "RecordSource", "compiled model metadata property availability");

    Assert.Equal(
        "model-metadata",
        AssertAnnotation<string>(compiledModel, DataVaultAnnotationNames.MetadataSourceKind, "compiled model DVault metadata source availability"));
    Assert.Equal(
        DataVaultTableKind.Hub,
        AssertAnnotation<DataVaultTableKind>(hub, DataVaultAnnotationNames.EntityKind, "compiled model hub entity kind availability"));
    Assert.Equal(
        "Order",
        AssertAnnotation<string>(hub, DataVaultAnnotationNames.MetadataName, "compiled model hub metadata name availability"));
    Assert.Equal(
        "HubOrder",
        AssertAnnotation<string>(hub, DataVaultAnnotationNames.ProducedName, "compiled model hub produced name availability"));
    Assert.Equal(
        DataVaultPropertyRole.BusinessKey,
        AssertAnnotation<DataVaultPropertyRole>(orderId, DataVaultAnnotationNames.PropertyRole, "compiled model business-key role availability"));
    Assert.Equal(
        DataVaultPropertyRole.Technical,
        AssertAnnotation<DataVaultPropertyRole>(recordSource, DataVaultAnnotationNames.PropertyRole, "compiled model technical role availability"));
    Assert.Equal(
        TechnicalMetadataColumnRole.RecordSource,
        AssertAnnotation<TechnicalMetadataColumnRole>(recordSource, DataVaultAnnotationNames.TechnicalColumnRole, "compiled model technical column role availability"));
  }

  [Fact]
  public void ModelDriftPreflightComparesCompiledRuntimeModelAgainstExplicitSnapshotModelWithoutDatabaseConnection() {
    var metadataModel = CreateOrderMetadataModel();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var designOptions = CreateOptions(database);

    using var designContext = new CompiledCompatibilityContext(designOptions, metadataModel);
    var snapshotModel = designContext.GetService<IDesignTimeModel>().Model;
    var compiledRuntimeModel = CreateCompiledRuntimeModel(designContext);
    var compiledOptions = new DbContextOptionsBuilder<CompiledCompatibilityContext>()
        .UseSqlite(CreateConnectionString(database))
        .UseModel(compiledRuntimeModel)
        .Options;

    using var compiledContext = new CompiledCompatibilityContext(compiledOptions, metadataModel);

    var report = DataVaultModelDriftPreflightReporter.Compare(metadataModel, compiledContext, snapshotModel);

    Assert.False(report.HasBlockingDifferences, report.ToDisplayString());
    Assert.Empty(report.MetadataVersusRuntime.Differences);
    Assert.Empty(report.MetadataVersusSnapshotModel.Differences);
    Assert.Empty(report.RuntimeVersusSnapshotModel.Differences);
  }

  [Fact]
  public async Task CompiledQueryReadsGeneratedSharedTypeProjectionWithDeterministicValuesThroughSqlite() {
    var metadataModel = CreateOrderMetadataModel();
    var order = metadataModel.Hubs.Single(hub => hub.Name == "Order");
    var loadTimestamp = new DateTimeOffset(2026, 5, 13, 12, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateOptions(database);
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string orderHashKey;

    await using (var context = new CompiledCompatibilityContext(options, metadataModel)) {
      await context.Database.EnsureCreatedAsync();

      var saveResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "compiled-query-seed",
              [new(order, [new("Order Id", "O-COMPILED-100")])],
              []));

      orderHashKey = saveResult.SavedRecords
          .Single(record => record.Kind == DataVaultTableKind.Hub && record.MetadataName == "Order")
          .HashKey;
    }

    await using (var context = new CompiledCompatibilityContext(options, metadataModel)) {
      var row = ReadHubOrderByHashKey(context, orderHashKey);

      Assert.Equal(orderHashKey, row.OrderHashKey);
      Assert.Equal("O-COMPILED-100", row.OrderId);
      Assert.Equal("compiled-query-seed", row.RecordSource);
    }
  }

  private static DbContextOptions<CompiledCompatibilityContext> CreateOptions(SqliteTestDatabase database) {
    return new DbContextOptionsBuilder<CompiledCompatibilityContext>()
        .UseSqlite(CreateConnectionString(database))
        .ReplaceService<IModelCacheKeyFactory, CompiledCompatibilityModelCacheKeyFactory>()
        .Options;
  }

  private static IModel CreateCompiledRuntimeModel(DbContext context) {
    var designModel = context.GetService<IDesignTimeModel>().Model;

    return context.GetService<IModelRuntimeInitializer>()
        .Initialize(designModel, designTime: false, validationLogger: null);
  }

  private static DataVaultMetadataModel CreateOrderMetadataModel() {
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var fulfillment = new DataVaultSatelliteMetadata(
        "Fulfillment",
        order.ToReference(),
        ["Status Code"]);

    return new DataVaultMetadataModel([order], [], [fulfillment]);
  }

  private static string CreateConnectionString(SqliteTestDatabase database) {
    return "Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False";
  }

  private static IEntityType AssertEntity(
      IModel model,
      string entityName,
      string diagnosticScope) {
    return model.FindEntityType(entityName) ??
        throw new InvalidOperationException(
            diagnosticScope + " failed: expected entity '" + entityName + "' was not available.");
  }

  private static IProperty AssertProperty(
      IEntityType entityType,
      string propertyName,
      string diagnosticScope) {
    return entityType.FindProperty(propertyName) ??
        throw new InvalidOperationException(
            diagnosticScope + " failed: expected property '" + propertyName + "' was not available on entity '" + entityType.Name + "'.");
  }

  private static T AssertAnnotation<T>(
      IReadOnlyAnnotatable annotatable,
      string annotationName,
      string diagnosticScope) {
    var annotation = annotatable.FindAnnotation(annotationName);

    if (annotation?.Value is T typedValue) {
      return typedValue;
    }

    throw new InvalidOperationException(
        diagnosticScope + " failed: expected annotation '" + annotationName + "' with value type '" + typeof(T).Name + "'.");
  }

  private sealed class CompiledCompatibilityContext(
      DbContextOptions<CompiledCompatibilityContext> options,
      DataVaultMetadataModel metadataModel) : DbContext(options) {
    public DataVaultMetadataModel MetadataModel { get; } = metadataModel;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(MetadataModel);
    }
  }

  private sealed class CompiledCompatibilityModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is CompiledCompatibilityContext compatibilityContext
          ? (context.GetType(), compatibilityContext.MetadataModel, designTime)
          : (object)(context.GetType(), designTime);
    }
  }

  private sealed record CompiledHubOrderRead(
      string OrderHashKey,
      string OrderId,
      string RecordSource);
}
