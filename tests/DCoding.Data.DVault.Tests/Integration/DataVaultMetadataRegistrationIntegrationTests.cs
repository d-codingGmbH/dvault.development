using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
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

  private static string[] EntityNames(IModel model) {
    return model.GetEntityTypes()
        .Select(entityType => entityType.Name)
        .Order(StringComparer.Ordinal)
        .ToArray();
  }

  private sealed class RegistryProjectionContext(DbContextOptions<RegistryProjectionContext> options) : DbContext(options) {
  }

  private sealed class ExplicitCustomerMetadataContext(DbContextOptions<ExplicitCustomerMetadataContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreateCustomerMetadataModel());
    }
  }
}
