using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultSaveChangesMetadataInterceptorSqliteTests {
  [Theory]
  [InlineData(false)]
  [InlineData(true)]
  public async Task OptedInInterceptorPopulatesMissingMetadataAndPreservesManualValuesForSyncAndAsync(bool useAsync) {
    var configuredLoadTimestamp = new DateTimeOffset(2026, 5, 14, 12, 30, 0, TimeSpan.Zero);
    var manualLinkLoadTimestamp = new DateTimeOffset(2026, 5, 13, 8, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateGeneratedOptions(database, configuredLoadTimestamp, "interceptor-source");

    await using (var context = new GeneratedMetadataContext(options)) {
      await context.Database.EnsureCreatedAsync();

      context.Set<Dictionary<string, object>>("HubCustomer").Add(new Dictionary<string, object> {
        ["CustomerHashKey"] = "customer-hash",
        ["CustomerId"] = "C-100",
      });
      context.Set<Dictionary<string, object>>("LinkCustomerOrder").Add(new Dictionary<string, object> {
        ["CustomerOrderHashKey"] = "customer-order-hash",
        ["LoadTimestamp"] = manualLinkLoadTimestamp,
        ["CustomerHashKey"] = "customer-hash",
        ["OrderHashKey"] = "order-hash",
      });
      context.Set<Dictionary<string, object>>("SatCustomerContact").Add(new Dictionary<string, object> {
        ["CustomerHashKey"] = "customer-hash",
        ["HashDiff"] = "contact-hash-diff",
        ["RecordSource"] = "manual-satellite-source",
        ["EmailAddress"] = "first@example.test",
      });

      if (useAsync) {
        await context.SaveChangesAsync();
      }
      else {
        context.SaveChanges();
      }
    }

    await using (var context = new GeneratedMetadataContext(options)) {
      var hubRow = await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().SingleAsync();
      var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().SingleAsync();
      var satelliteRow = await context.Set<Dictionary<string, object>>("SatCustomerContact").AsNoTracking().SingleAsync();

      Assert.Equal("customer-hash", hubRow["CustomerHashKey"]);
      Assert.Equal(configuredLoadTimestamp, hubRow["LoadTimestamp"]);
      Assert.Equal("interceptor-source", hubRow["RecordSource"]);

      Assert.Equal("customer-order-hash", linkRow["CustomerOrderHashKey"]);
      Assert.Equal(manualLinkLoadTimestamp, linkRow["LoadTimestamp"]);
      Assert.Equal("interceptor-source", linkRow["RecordSource"]);

      Assert.Equal("contact-hash-diff", satelliteRow["HashDiff"]);
      Assert.Equal(configuredLoadTimestamp, satelliteRow["LoadTimestamp"]);
      Assert.Equal("manual-satellite-source", satelliteRow["RecordSource"]);
    }
  }

  [Fact]
  public async Task InterceptorDiscoversTechnicalMetadataFromAnnotationsInsteadOfPropertyNames() {
    var configuredLoadTimestamp = new DateTimeOffset(2026, 5, 14, 15, 45, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var optionsBuilder = new DbContextOptionsBuilder<AnnotatedMetadataContext>()
        .UseSqlite(CreateConnectionString(database));
    optionsBuilder.UseDataVaultSaveChangesMetadataInterceptor(interceptor => interceptor
        .UseLoadTimestamp(configuredLoadTimestamp)
        .UseRecordSource("annotated-source"));
    var options = optionsBuilder.Options;

    await using (var context = new AnnotatedMetadataContext(options)) {
      await context.Database.EnsureCreatedAsync();

      context.Set<Dictionary<string, object>>("AnnotatedHubCustomer").Add(new Dictionary<string, object> {
        ["CustomerKeyValue"] = "customer-hash",
        ["CustomerId"] = "C-200",
      });

      await context.SaveChangesAsync();
    }

    await using (var context = new AnnotatedMetadataContext(options)) {
      var row = await context.Set<Dictionary<string, object>>("AnnotatedHubCustomer").AsNoTracking().SingleAsync();

      Assert.Equal("customer-hash", row["CustomerKeyValue"]);
      Assert.Equal(configuredLoadTimestamp, row["LoadedAtUtc"]);
      Assert.Equal("annotated-source", row["SourceSystem"]);
    }
  }

  private static DbContextOptions<GeneratedMetadataContext> CreateGeneratedOptions(
      SqliteTestDatabase database,
      DateTimeOffset loadTimestamp,
      string recordSource) {
    var optionsBuilder = new DbContextOptionsBuilder<GeneratedMetadataContext>()
        .UseSqlite(CreateConnectionString(database));
    optionsBuilder.UseDataVaultMetadata(CreateMetadataModel());
    optionsBuilder.UseDataVaultSaveChangesMetadataInterceptor(options => options
        .UseLoadTimestamp(loadTimestamp)
        .UseRecordSource(recordSource));

    return optionsBuilder.Options;
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);

    return new DataVaultMetadataModel([customer, order], [customerOrder], [contact]);
  }

  private static string CreateConnectionString(SqliteTestDatabase database) {
    return "Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False";
  }

  private static void AnnotateProperty(
      PropertyBuilder propertyBuilder,
      DataVaultPropertyRole propertyRole,
      TechnicalMetadataColumnRole? technicalRole) {
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, propertyRole);
    if (technicalRole is not null) {
      propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, technicalRole);
    }
  }

  private sealed class GeneratedMetadataContext(DbContextOptions<GeneratedMetadataContext> options) : DbContext(options) {
  }

  private sealed class AnnotatedMetadataContext(DbContextOptions<AnnotatedMetadataContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.SharedTypeEntity<Dictionary<string, object>>("AnnotatedHubCustomer", entityBuilder => {
        entityBuilder.ToTable("AnnotatedHubCustomer");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, DataVaultTableKind.Hub);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "Customer");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, "AnnotatedHubCustomer");

        AnnotateProperty(
            entityBuilder.IndexerProperty<string>("CustomerKeyValue"),
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.HashKey);
        AnnotateProperty(
            entityBuilder.IndexerProperty<DateTimeOffset>("LoadedAtUtc"),
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.LoadTimestamp);
        AnnotateProperty(
            entityBuilder.IndexerProperty<string>("SourceSystem"),
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.RecordSource);
        AnnotateProperty(
            entityBuilder.IndexerProperty<string>("CustomerId"),
            DataVaultPropertyRole.BusinessKey,
            technicalRole: null);

        entityBuilder.HasKey("CustomerKeyValue");
      });
    }
  }
}
