using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultSaveChangesGuardInterceptorSqliteTests {
  [Fact]
  public async Task BlockingGuardRejectsModifiedAndDeletedGeneratedRows() {
    var cancellationToken = TestContext.Current.CancellationToken;
    var loadTimestamp = new DateTimeOffset(2026, 5, 21, 9, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateGeneratedOptions(database, guardOptions => guardOptions.UseBlockingMode());

    await using (var context = new GeneratedMetadataContext(options)) {
      await context.Database.EnsureCreatedAsync(cancellationToken);

      context.Set<Dictionary<string, object>>("HubCustomer").Add(CreateCustomerHubRow(loadTimestamp, "initial"));
      context.Set<Dictionary<string, object>>("LinkCustomerOrder").Add(CreateCustomerOrderLinkRow(loadTimestamp, "initial"));
      context.Set<Dictionary<string, object>>("SatCustomerContact").Add(CreateCustomerContactSatelliteRow(loadTimestamp, "initial"));

      await context.SaveChangesAsync(cancellationToken);
    }

    await using (var context = new GeneratedMetadataContext(options)) {
      var hubRow = await context.Set<Dictionary<string, object>>("HubCustomer").SingleAsync(cancellationToken);
      var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").SingleAsync(cancellationToken);
      var satelliteRow = await context.Set<Dictionary<string, object>>("SatCustomerContact").SingleAsync(cancellationToken);

      hubRow["CustomerId"] = "C-100-modified";
      context.Entry(hubRow).State = EntityState.Modified;
      context.Set<Dictionary<string, object>>("LinkCustomerOrder").Remove(linkRow);
      context.Set<Dictionary<string, object>>("SatCustomerContact").Remove(satelliteRow);

      var exception = await Assert.ThrowsAsync<DataVaultSaveChangesGuardException>(() => context.SaveChangesAsync(cancellationToken));

      Assert.Equal(3, exception.Report.Findings.Count);
      Assert.Contains(exception.Report.Findings, finding =>
          finding.EntityKind == DataVaultTableKind.Hub &&
          finding.State == EntityState.Modified &&
          finding.TableName == "HubCustomer");
      Assert.Contains(exception.Report.Findings, finding =>
          finding.EntityKind == DataVaultTableKind.Link &&
          finding.State == EntityState.Deleted &&
          finding.TableName == "LinkCustomerOrder");
      Assert.Contains(exception.Report.Findings, finding =>
          finding.EntityKind == DataVaultTableKind.Satellite &&
          finding.State == EntityState.Deleted &&
          finding.TableName == "SatCustomerContact");
      Assert.Contains("Hub 'Customer' mapped to 'HubCustomer' in Modified state", exception.Report.ToDisplayString(), StringComparison.Ordinal);
      Assert.Contains("Link 'CustomerOrder' mapped to 'LinkCustomerOrder' in Deleted state", exception.Report.ToDisplayString(), StringComparison.Ordinal);
      Assert.Contains("Satellite 'Contact' mapped to 'SatCustomerContact' in Deleted state", exception.Report.ToDisplayString(), StringComparison.Ordinal);
    }
  }

  [Fact]
  public async Task BlockingGuardRejectsAddedRowsMissingRequiredStructuralValues() {
    var cancellationToken = TestContext.Current.CancellationToken;
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateGeneratedOptions(database, guardOptions => guardOptions.UseBlockingMode());

    await using var context = new GeneratedMetadataContext(options);
    await context.Database.EnsureCreatedAsync(cancellationToken);

    context.Set<Dictionary<string, object>>("HubCustomer").Add(new Dictionary<string, object> {
      ["CustomerHashKey"] = " ",
      ["CustomerId"] = "C-100",
      ["LoadTimestamp"] = new DateTimeOffset(2026, 5, 21, 9, 0, 0, TimeSpan.Zero),
      ["RecordSource"] = "direct-save",
    });
    context.Set<Dictionary<string, object>>("LinkCustomerOrder").Add(new Dictionary<string, object> {
      ["CustomerOrderHashKey"] = "customer-order-hash",
      ["LoadTimestamp"] = new DateTimeOffset(2026, 5, 21, 9, 0, 0, TimeSpan.Zero),
      ["RecordSource"] = "direct-save",
      ["CustomerHashKey"] = "customer-hash",
    });
    context.Set<Dictionary<string, object>>("SatCustomerContact").Add(new Dictionary<string, object> {
      ["CustomerHashKey"] = "customer-hash",
      ["LoadTimestamp"] = new DateTimeOffset(2026, 5, 21, 9, 0, 0, TimeSpan.Zero),
      ["RecordSource"] = "direct-save",
      ["EmailAddress"] = "first@example.test",
    });

    var exception = await Assert.ThrowsAsync<DataVaultSaveChangesGuardException>(() => context.SaveChangesAsync(cancellationToken));
    var explanation = exception.Report.ToDisplayString();

    Assert.Equal(3, exception.Report.Findings.Count);
    Assert.Contains("Hub 'Customer' mapped to 'HubCustomer' in Added state", explanation, StringComparison.Ordinal);
    Assert.Contains("Required structural property 'CustomerHashKey' is missing.", explanation, StringComparison.Ordinal);
    Assert.Contains("Link 'CustomerOrder' mapped to 'LinkCustomerOrder' in Added state", explanation, StringComparison.Ordinal);
    Assert.Contains("Required structural property 'OrderHashKey' is missing.", explanation, StringComparison.Ordinal);
    Assert.Contains("Satellite 'Contact' mapped to 'SatCustomerContact' in Added state", explanation, StringComparison.Ordinal);
    Assert.Contains("Required structural property 'HashDiff' is missing.", explanation, StringComparison.Ordinal);
  }

  [Fact]
  public async Task WarningGuardReportsFindingsAndAllowsSaveChangesWithoutMutation() {
    var cancellationToken = TestContext.Current.CancellationToken;
    var reports = new List<DataVaultSaveChangesGuardReport>();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var optionsBuilder = new DbContextOptionsBuilder<AnnotatedGuardContext>()
        .UseSqlite(CreateConnectionString(database));
    optionsBuilder.UseDataVaultSaveChangesGuardInterceptor(guard => guard.UseWarningMode(reports.Add));

    await using (var context = new AnnotatedGuardContext(optionsBuilder.Options)) {
      await context.Database.EnsureCreatedAsync(cancellationToken);

      context.Set<Dictionary<string, object>>("AnnotatedLink").Add(new Dictionary<string, object> {
        ["SyntheticRelationshipKey"] = "link-hash",
        ["SourceCustomerKey"] = "customer-hash",
        ["LoadedAtUtc"] = new DateTimeOffset(2026, 5, 21, 9, 0, 0, TimeSpan.Zero),
        ["SourceSystem"] = "direct-save",
      });

      await context.SaveChangesAsync(cancellationToken);
    }

    Assert.Single(reports);
    var report = Assert.Single(reports);
    var finding = Assert.Single(report.Findings);

    Assert.Equal(DataVaultTableKind.Link, finding.EntityKind);
    Assert.Equal(EntityState.Added, finding.State);
    Assert.Contains("TargetOrderKey", finding.Reasons[0], StringComparison.Ordinal);

    await using (var context = new AnnotatedGuardContext(optionsBuilder.Options)) {
      var row = await context.Set<Dictionary<string, object>>("AnnotatedLink").AsNoTracking().SingleAsync(cancellationToken);

      Assert.True(!row.TryGetValue("TargetOrderKey", out var value) || value is null);
      Assert.Equal("link-hash", row["SyntheticRelationshipKey"]);
    }
  }

  [Fact]
  public async Task GuardAndMetadataFillCoexistForCallerOwnedGeneratedRows() {
    var cancellationToken = TestContext.Current.CancellationToken;
    var configuredLoadTimestamp = new DateTimeOffset(2026, 5, 21, 10, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var optionsBuilder = CreateGeneratedOptionsBuilder(database);
    optionsBuilder.UseDataVaultSaveChangesMetadataInterceptor(interceptor => interceptor
        .UseLoadTimestamp(configuredLoadTimestamp)
        .UseRecordSource("metadata-fill"));
    optionsBuilder.UseDataVaultSaveChangesGuardInterceptor(guard => guard.UseBlockingMode());
    var options = optionsBuilder.Options;

    await using (var context = new GeneratedMetadataContext(options)) {
      await context.Database.EnsureCreatedAsync(cancellationToken);

      context.Set<Dictionary<string, object>>("HubCustomer").Add(new Dictionary<string, object> {
        ["CustomerHashKey"] = "customer-hash",
        ["CustomerId"] = "C-100",
      });
      context.Set<Dictionary<string, object>>("LinkCustomerOrder").Add(new Dictionary<string, object> {
        ["CustomerOrderHashKey"] = "customer-order-hash",
        ["CustomerHashKey"] = "customer-hash",
        ["OrderHashKey"] = "order-hash",
      });
      context.Set<Dictionary<string, object>>("SatCustomerContact").Add(new Dictionary<string, object> {
        ["CustomerHashKey"] = "customer-hash",
        ["HashDiff"] = "contact-hash-diff",
        ["EmailAddress"] = "first@example.test",
      });

      await context.SaveChangesAsync(cancellationToken);
    }

    await using (var context = new GeneratedMetadataContext(options)) {
      var hubRow = await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().SingleAsync(cancellationToken);
      var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().SingleAsync(cancellationToken);
      var satelliteRow = await context.Set<Dictionary<string, object>>("SatCustomerContact").AsNoTracking().SingleAsync(cancellationToken);

      Assert.Equal(configuredLoadTimestamp, hubRow["LoadTimestamp"]);
      Assert.Equal(configuredLoadTimestamp, linkRow["LoadTimestamp"]);
      Assert.Equal(configuredLoadTimestamp, satelliteRow["LoadTimestamp"]);
      Assert.Equal("metadata-fill", hubRow["RecordSource"]);
      Assert.Equal("metadata-fill", linkRow["RecordSource"]);
      Assert.Equal("metadata-fill", satelliteRow["RecordSource"]);
    }
  }

  [Fact]
  public async Task ExplicitSaveServiceSucceedsUnderOptInGuard() {
    var cancellationToken = TestContext.Current.CancellationToken;
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 21, 11, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateGeneratedOptions(database, guardOptions => guardOptions.UseBlockingMode());
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using (var context = new GeneratedMetadataContext(options)) {
      await context.Database.EnsureCreatedAsync(cancellationToken);

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "explicit-save",
              [
                  new(customer, [new("Customer Id", "C-100")]),
                  new(order, [new("Order Id", "O-200")]),
              ],
              []),
          cancellationToken);
      var customerHashKey = hubResult.SavedRecords.Single(record => record.MetadataName == "Customer").HashKey;
      var orderHashKey = hubResult.SavedRecords.Single(record => record.MetadataName == "Order").HashKey;

      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              loadTimestamp,
              "explicit-save",
              [],
              [new(customerOrder, [new("Customer", customerHashKey), new("Order", orderHashKey)])],
              [new(contact, customerHashKey, [new("Email Address", "first@example.test")], "contact-hash-diff")]),
          cancellationToken);
    }

    await using (var context = new GeneratedMetadataContext(options)) {
      Assert.Equal(1, await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().CountAsync(cancellationToken));
      Assert.Equal(1, await context.Set<Dictionary<string, object>>("HubOrder").AsNoTracking().CountAsync(cancellationToken));
      Assert.Equal(1, await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().CountAsync(cancellationToken));
      Assert.Equal(1, await context.Set<Dictionary<string, object>>("SatCustomerContact").AsNoTracking().CountAsync(cancellationToken));
    }
  }

  [Fact]
  public async Task GuardUsesAnnotationsInsteadOfHardCodedNames() {
    var cancellationToken = TestContext.Current.CancellationToken;
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var reports = new List<DataVaultSaveChangesGuardReport>();
    var optionsBuilder = new DbContextOptionsBuilder<AnnotatedGuardContext>()
        .UseSqlite(CreateConnectionString(database));
    optionsBuilder.UseDataVaultSaveChangesGuardInterceptor(guard => guard.UseWarningMode(reports.Add));

    await using (var context = new AnnotatedGuardContext(optionsBuilder.Options)) {
      await context.Database.EnsureCreatedAsync(cancellationToken);

      context.Set<Dictionary<string, object>>("AnnotatedLink").Add(new Dictionary<string, object> {
        ["SyntheticRelationshipKey"] = "link-hash",
        ["SourceCustomerKey"] = "customer-hash",
        ["LoadedAtUtc"] = new DateTimeOffset(2026, 5, 21, 12, 0, 0, TimeSpan.Zero),
        ["SourceSystem"] = "direct-save",
      });

      await context.SaveChangesAsync(cancellationToken);
    }

    var report = Assert.Single(reports);
    var finding = Assert.Single(report.Findings);

    Assert.Equal("AnnotatedLink", finding.TableName);
    Assert.Equal(DataVaultTableKind.Link, finding.EntityKind);
    Assert.Contains("Required structural property 'TargetOrderKey' is missing.", finding.Reasons);
  }

  private static DbContextOptions<GeneratedMetadataContext> CreateGeneratedOptions(
      SqliteTestDatabase database,
      Action<DataVaultSaveChangesGuardOptions> configureGuard) {
    var optionsBuilder = CreateGeneratedOptionsBuilder(database);
    optionsBuilder.UseDataVaultSaveChangesGuardInterceptor(configureGuard);

    return optionsBuilder.Options;
  }

  private static DbContextOptionsBuilder<GeneratedMetadataContext> CreateGeneratedOptionsBuilder(SqliteTestDatabase database) {
    var optionsBuilder = new DbContextOptionsBuilder<GeneratedMetadataContext>()
        .UseSqlite(CreateConnectionString(database));
    optionsBuilder.UseDataVaultMetadata(CreateMetadataModel());

    return optionsBuilder;
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

  private static Dictionary<string, object> CreateCustomerHubRow(DateTimeOffset loadTimestamp, string recordSource) {
    return new Dictionary<string, object> {
      ["CustomerHashKey"] = "customer-hash",
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = recordSource,
      ["CustomerId"] = "C-100",
    };
  }

  private static Dictionary<string, object> CreateCustomerOrderLinkRow(DateTimeOffset loadTimestamp, string recordSource) {
    return new Dictionary<string, object> {
      ["CustomerOrderHashKey"] = "customer-order-hash",
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = recordSource,
      ["CustomerHashKey"] = "customer-hash",
      ["OrderHashKey"] = "order-hash",
    };
  }

  private static Dictionary<string, object> CreateCustomerContactSatelliteRow(DateTimeOffset loadTimestamp, string recordSource) {
    return new Dictionary<string, object> {
      ["CustomerHashKey"] = "customer-hash",
      ["HashDiff"] = "contact-hash-diff",
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = recordSource,
      ["EmailAddress"] = "first@example.test",
    };
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

  private sealed class AnnotatedGuardContext(DbContextOptions<AnnotatedGuardContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.SharedTypeEntity<Dictionary<string, object>>("AnnotatedLink", entityBuilder => {
        entityBuilder.ToTable("AnnotatedLink");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, DataVaultTableKind.Link);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "CustomRelationship");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, "AnnotatedLink");

        AnnotateProperty(
            entityBuilder.IndexerProperty<string>("SyntheticRelationshipKey"),
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.HashKey);
        AnnotateProperty(
            entityBuilder.IndexerProperty<string>("SourceCustomerKey"),
            DataVaultPropertyRole.ParticipantReference,
            TechnicalMetadataColumnRole.HashKey);
        var targetOrderKey = entityBuilder.IndexerProperty<string?>("TargetOrderKey");
        targetOrderKey.IsRequired(false);
        AnnotateProperty(
            targetOrderKey,
            DataVaultPropertyRole.ParticipantReference,
            TechnicalMetadataColumnRole.HashKey);
        AnnotateProperty(
            entityBuilder.IndexerProperty<DateTimeOffset>("LoadedAtUtc"),
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.LoadTimestamp);
        AnnotateProperty(
            entityBuilder.IndexerProperty<string>("SourceSystem"),
            DataVaultPropertyRole.Technical,
            TechnicalMetadataColumnRole.RecordSource);

        entityBuilder.HasKey("SyntheticRelationshipKey");
      });
    }
  }
}
