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
public sealed class DataVaultMetadataRegistrationIntegrationTests {
  [Fact]
  public void DbContextOptionsProjectAppDefaultRegistryWithoutOnModelCreatingServiceLocation() {
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataModel(CreateCustomerMetadataModel()));
    services.AddDbContext<RegistryProjectionContext>(
        options => options
            .UseSqlite("Data Source=:memory:")
            .UseDataVaultMetadata());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    using var scope = provider.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<RegistryProjectionContext>();

    Assert.Contains("HubCustomer", EntityNames(context.Model));
    Assert.Equal(
        "app-default-registry",
        Assert.IsType<string>(context.Model.FindAnnotation(DataVaultAnnotationNames.MetadataSourceKind)?.Value));
  }

  [Fact]
  public void DbContextOptionsProjectAppDefaultBinaryFirstProfileThroughMetadataTranslation() {
    var services = new ServiceCollection();
    services.AddDVault(options => options
        .UseBinaryFirstProfile()
        .UseMetadataModel(CreateRelationshipMetadataModel()));
    services.AddDbContext<RegistryProjectionContext>(
        options => options
            .UseSqlite("Data Source=:memory:")
            .UseDataVaultMetadata());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    using var scope = provider.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<RegistryProjectionContext>();
    var conventions = Assert.IsType<DataVaultConventions>(
        context.Model.FindAnnotation(DataVaultAnnotationNames.Conventions)?.Value);

    Assert.Equal("binary-first", conventions.ProfileName);
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, conventions.HashKeyStorageProfile);
    AssertBinaryHashKeyProperty(context.Model, "HubCustomer", "CustomerHashKey", DataVaultLogicalPropertyKind.HashKey);
    AssertBinaryHashKeyProperty(context.Model, "LinkCustomerOrder", "CustomerHashKey", DataVaultLogicalPropertyKind.ParticipantReference);
  }

  [Fact]
  public void DbContextOptionsAppDefaultRegistryParticipatesInModelCacheKey() {
    using var customerProvider = CreateAppDefaultProvider(CreateCustomerMetadataModel());
    using var orderProvider = CreateAppDefaultProvider(CreateOrderMetadataModel());
    using var customerScope = customerProvider.CreateScope();
    using var orderScope = orderProvider.CreateScope();
    using var customerContext = customerScope.ServiceProvider.GetRequiredService<RegistryProjectionContext>();
    using var orderContext = orderScope.ServiceProvider.GetRequiredService<RegistryProjectionContext>();

    Assert.Contains("HubCustomer", EntityNames(customerContext.Model));
    Assert.DoesNotContain("HubOrder", EntityNames(customerContext.Model));
    Assert.Contains("HubOrder", EntityNames(orderContext.Model));
    Assert.DoesNotContain("HubCustomer", EntityNames(orderContext.Model));
    Assert.Equal("app-default-registry", MetadataSourceKind(customerContext.Model));
    Assert.Equal("app-default-registry", MetadataSourceKind(orderContext.Model));
    Assert.NotEqual(MetadataSourceFingerprint(customerContext.Model), MetadataSourceFingerprint(orderContext.Model));
  }

  [Fact]
  public void DbContextOptionsExplicitRegistryParticipatesInModelCacheKey() {
    var customerOptionsBuilder = new DbContextOptionsBuilder<RegistryProjectionContext>();
    customerOptionsBuilder
        .UseSqlite("Data Source=:memory:")
        .UseDataVaultMetadata(DataVaultMetadataRegistry.Create(CreateCustomerMetadataModel()));
    var orderOptionsBuilder = new DbContextOptionsBuilder<RegistryProjectionContext>();
    orderOptionsBuilder
        .UseSqlite("Data Source=:memory:")
        .UseDataVaultMetadata(DataVaultMetadataRegistry.Create(CreateOrderMetadataModel()));

    using var customerContext = new RegistryProjectionContext(customerOptionsBuilder.Options);
    using var orderContext = new RegistryProjectionContext(orderOptionsBuilder.Options);

    Assert.Contains("HubCustomer", EntityNames(customerContext.Model));
    Assert.DoesNotContain("HubOrder", EntityNames(customerContext.Model));
    Assert.Contains("HubOrder", EntityNames(orderContext.Model));
    Assert.DoesNotContain("HubCustomer", EntityNames(orderContext.Model));
    Assert.Equal("dbcontext-registry", MetadataSourceKind(customerContext.Model));
    Assert.Equal("dbcontext-registry", MetadataSourceKind(orderContext.Model));
    Assert.NotEqual(MetadataSourceFingerprint(customerContext.Model), MetadataSourceFingerprint(orderContext.Model));
  }

  [Fact]
  public void DbContextOptionsImportResultParticipatesInModelCacheKey() {
    var customerImportResult = ImportModelArtifact("Customer", "Customer Id");
    var orderImportResult = ImportModelArtifact("Order", "Order Id");
    var customerOptionsBuilder = new DbContextOptionsBuilder<RegistryProjectionContext>();
    customerOptionsBuilder
        .UseSqlite("Data Source=:memory:")
        .UseDataVaultMetadata(customerImportResult);
    var orderOptionsBuilder = new DbContextOptionsBuilder<RegistryProjectionContext>();
    orderOptionsBuilder
        .UseSqlite("Data Source=:memory:")
        .UseDataVaultMetadata(orderImportResult);

    using var customerContext = new RegistryProjectionContext(customerOptionsBuilder.Options);
    using var orderContext = new RegistryProjectionContext(orderOptionsBuilder.Options);

    Assert.Contains("HubCustomer", EntityNames(customerContext.Model));
    Assert.DoesNotContain("HubOrder", EntityNames(customerContext.Model));
    Assert.Contains("HubOrder", EntityNames(orderContext.Model));
    Assert.DoesNotContain("HubCustomer", EntityNames(orderContext.Model));
    Assert.Equal("dbcontext-registry", MetadataSourceKind(customerContext.Model));
    Assert.Equal("dbcontext-registry", MetadataSourceKind(orderContext.Model));
    Assert.NotEqual(MetadataSourceFingerprint(customerContext.Model), MetadataSourceFingerprint(orderContext.Model));
  }

  [Fact]
  public void DbContextExplicitRegistryOverridesAppDefaultRegistryForThatContext() {
    var services = new ServiceCollection();
    var orderRegistry = DataVaultMetadataRegistry.Create(CreateOrderMetadataModel());
    services.AddDVault(options => options.UseMetadataModel(CreateCustomerMetadataModel()));
    services.AddDbContext<RegistryProjectionContext>(
        options => options
            .UseSqlite("Data Source=:memory:")
            .UseDataVaultMetadata(orderRegistry));

    using var provider = services.BuildServiceProvider(validateScopes: true);
    using var scope = provider.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<RegistryProjectionContext>();

    Assert.Contains("HubOrder", EntityNames(context.Model));
    Assert.DoesNotContain("HubCustomer", EntityNames(context.Model));
    Assert.Equal(
        "dbcontext-registry",
        Assert.IsType<string>(context.Model.FindAnnotation(DataVaultAnnotationNames.MetadataSourceKind)?.Value));
  }

  [Fact]
  public void DbContextRegistryConflictsWithDifferentExplicitModelMetadata() {
    var optionsBuilder = new DbContextOptionsBuilder<ExplicitCustomerMetadataContext>();
    optionsBuilder
        .UseSqlite("Data Source=:memory:")
        .UseDataVaultMetadata(DataVaultMetadataRegistry.Create(CreateOrderMetadataModel()));

    using var context = new ExplicitCustomerMetadataContext(optionsBuilder.Options);
    var exception = Assert.Throws<InvalidOperationException>(() => _ = context.Model);

    Assert.Contains("model-metadata", exception.Message, StringComparison.Ordinal);
    Assert.Contains("dbcontext-registry", exception.Message, StringComparison.Ordinal);
    Assert.Contains("one authoritative DVault metadata source", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public void CallerOwnedModelShapeStateIsolatesCacheWhenCustomKeyIncludesDiscriminator() {
    var optionsBuilder = new DbContextOptionsBuilder<CallerOwnedProjectionContext>();
    optionsBuilder
        .UseSqlite("Data Source=:memory:")
        .ReplaceService<IModelCacheKeyFactory, CallerOwnedProjectionModelCacheKeyFactory>();

    using var archiveContext = new CallerOwnedProjectionContext(optionsBuilder.Options, "Archive_");
    using var liveContext = new CallerOwnedProjectionContext(optionsBuilder.Options, "Live_");

    Assert.Equal("Archive_HubCustomer", TableName(archiveContext.Model, "HubCustomer"));
    Assert.Equal("Live_HubCustomer", TableName(liveContext.Model, "HubCustomer"));
    Assert.Equal("model-metadata", MetadataSourceKind(archiveContext.Model));
    Assert.Equal("model-metadata", MetadataSourceKind(liveContext.Model));
    Assert.Equal(MetadataSourceFingerprint(archiveContext.Model), MetadataSourceFingerprint(liveContext.Model));
  }

  private static ServiceProvider CreateAppDefaultProvider(DataVaultMetadataModel metadataModel) {
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataModel(metadataModel));
    services.AddDbContext<RegistryProjectionContext>(
        options => options
            .UseSqlite("Data Source=:memory:")
            .UseDataVaultMetadata());

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static DataVaultMetadataModel CreateCustomerMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Customer", ["Customer Id"])],
        [],
        []);
  }

  private static DataVaultMetadataModel CreateOrderMetadataModel() {
    return new DataVaultMetadataModel(
        [new DataVaultHubMetadata("Order", ["Order Id"])],
        [],
        []);
  }

  private static DataVaultMetadataModel CreateRelationshipMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);

    return new DataVaultMetadataModel(
        [customer, order],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [customer.ToReference(), order.ToReference()]),
        ],
        []);
  }

  private static DataVaultModelImportResult ImportModelArtifact(
      string hubName,
      string businessKeyName) {
    var importResult = DataVaultModelArtifactImporter.ImportJson(
        "{" + Environment.NewLine +
        "  \"schemaVersion\": \"dvault.model.v1\"," + Environment.NewLine +
        "  \"hubs\": [" + Environment.NewLine +
        "    {" + Environment.NewLine +
        "      \"name\": \"" + hubName + "\"," + Environment.NewLine +
        "      \"businessKeys\": [\"" + businessKeyName + "\"]" + Environment.NewLine +
        "    }" + Environment.NewLine +
        "  ]" + Environment.NewLine +
        "}");

    Assert.True(importResult.IsValid, DataVaultModelImportResult.FormatDiagnostics(importResult.Diagnostics));

    return importResult;
  }

  private static string[] EntityNames(IModel model) {
    return model.GetEntityTypes()
        .Select(entityType => entityType.Name)
        .Order(StringComparer.Ordinal)
        .ToArray();
  }

  private static string MetadataSourceKind(IModel model) {
    return Assert.IsType<string>(model.FindAnnotation(DataVaultAnnotationNames.MetadataSourceKind)?.Value);
  }

  private static string MetadataSourceFingerprint(IModel model) {
    return Assert.IsType<string>(model.FindAnnotation(DataVaultAnnotationNames.MetadataSourceFingerprint)?.Value);
  }

  private static string TableName(IModel model, string entityName) {
    var entityType = model.FindEntityType(entityName);

    Assert.NotNull(entityType);

    return entityType!.GetTableName() ?? entityType.Name;
  }

  private static void AssertBinaryHashKeyProperty(
      IModel model,
      string entityName,
      string propertyName,
      DataVaultLogicalPropertyKind expectedLogicalPropertyKind) {
    var property = model.FindEntityType(entityName)?.FindProperty(propertyName);

    Assert.NotNull(property);
    Assert.Equal(typeof(string), property!.ClrType);
    Assert.Equal("BLOB", property.GetColumnType());
    Assert.Equal("BLOB", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderStorageType));
    Assert.Equal(expectedLogicalPropertyKind, AnnotationValue<DataVaultLogicalPropertyKind>(
        property,
        DataVaultAnnotationNames.ProviderLogicalPropertyKind));
    Assert.Equal(DataVaultProviderValueFormat.LowercaseHexBinary, AnnotationValue<DataVaultProviderValueFormat>(
        property,
        DataVaultAnnotationNames.ProviderValueFormat));
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, AnnotationValue<DataVaultHashKeyStorageProfile>(
        property,
        DataVaultAnnotationNames.HashKeyStorageProfile));
    Assert.Equal("lowercase-hex-string-to-bytes", AnnotationValue<string>(
        property,
        DataVaultAnnotationNames.HashKeyConversionBehavior));
  }

  private static T AnnotationValue<T>(IProperty property, string annotationName) {
    var annotation = property.FindAnnotation(annotationName);

    Assert.NotNull(annotation);

    return Assert.IsType<T>(annotation!.Value);
  }

  private sealed class RegistryProjectionContext(DbContextOptions<RegistryProjectionContext> options) : DbContext(options) {
  }

  private sealed class ExplicitCustomerMetadataContext(DbContextOptions<ExplicitCustomerMetadataContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateCustomerMetadataModel());
    }
  }

  private sealed class CallerOwnedProjectionContext(
      DbContextOptions<CallerOwnedProjectionContext> options,
      string tableNamePrefix) : DbContext(options) {
    public string TableNamePrefix { get; } = tableNamePrefix;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateCustomerMetadataModel());

      modelBuilder.SharedTypeEntity<Dictionary<string, object>>("HubCustomer", entity => {
        entity.ToTable(TableNamePrefix + "HubCustomer");
      });
    }
  }

  private sealed class CallerOwnedProjectionModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is CallerOwnedProjectionContext projectionContext
          ? (context.GetType(), projectionContext.TableNamePrefix, designTime)
          : (object)(context.GetType(), designTime);
    }
  }
}
