using System.Data;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.PostgresProvider)]
public sealed class PostgresOptimizedDataVaultSaveServiceTests {
  private const int MinimumPostgresStagedBulkOperationCount = 60;
  private const string PostgresProviderName = "Npgsql.EntityFrameworkCore.PostgreSQL";
  private const string PostgresStrategyRegistrationDiagnostic =
      "PostgreSQL optimized dispatch expected AddDVaultPostgres to register a compatible IDataVaultProviderSaveStrategy " +
          "for a clean Npgsql-backed context; no registered strategy accepted the request.";
  private const string PostgresOptimizedPathDiagnostic =
      "PostgreSQL optimized dispatch expected the provider strategy to persist without fallback-tracked Data Vault rows; " +
          "tracked rows were present, so AddDVaultPostgres may have fallen back to the provider-neutral EF writer.";

  [Fact]
  public async Task AddDVaultPostgresOptimizedStrategyPersistsHubLinkAndSatelliteRowsWhenConfigured() {
    var configuration = PostgresIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(PostgresIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var schemaName = "dvault_test_" + Guid.NewGuid().ToString("N");
    var options = CreatePostgresOptions(configuration.ConnectionString!);
    var services = new ServiceCollection();
    services.AddDVaultPostgres();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new OptimizedPostgresSaveContext(options, schemaName);
    await context.Database.ExecuteSqlRawAsync("CREATE SCHEMA " + QuoteIdentifier(schemaName) + ";");

    try {
      await context.Database.ExecuteSqlRawAsync(context.Database.GenerateCreateScript());

      Assert.Equal(PostgresProviderName, context.Database.ProviderName);

      var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
      var order = new DataVaultHubMetadata("Order", ["Order Id"]);
      var customerOrder = new DataVaultLinkMetadata(
          "CustomerOrder",
          [customer.ToReference(), order.ToReference()]);
      var contact = new DataVaultSatelliteMetadata(
          "Contact",
          customer.ToReference(),
          ["Email Address"]);
      var hubLoadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
      var linkLoadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 30, 0, TimeSpan.Zero);
      var firstSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 10, 45, 0, TimeSpan.Zero);
      var unchangedSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 11, 0, 0, TimeSpan.Zero);
      var changedSatelliteTimestamp = new DateTimeOffset(2026, 4, 29, 11, 15, 0, TimeSpan.Zero);
      var hubRequest = new DataVaultSaveRequest(
          hubLoadTimestamp,
          "crm-import",
          [
              new(customer, [new("Customer Id", "C-100")]),
              new(order, [new("Order Id", "O-200")]),
          ],
          []);

      AssertCompatiblePostgresStrategy(provider, context, hubRequest);

      var hubResult = await saveService.SaveAsync(context, hubRequest);

      Assert.Equal(2, hubResult.RowsWritten);
      AssertOptimizedPathObserved(context);

      var customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
      var orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");
      var linkRequest = new DataVaultSaveRequest(
          linkLoadTimestamp,
          "crm-import",
          [],
          [
              new(customerOrder, [new("Customer", customerHashKey), new("Order", orderHashKey)]),
          ]);

      AssertCompatiblePostgresStrategy(provider, context, linkRequest);

      var linkResult = await saveService.SaveAsync(context, linkRequest);

      Assert.Equal(1, linkResult.RowsWritten);
      AssertOptimizedPathObserved(context);

      var firstSatelliteRequest = new DataVaultSaveRequest(
          firstSatelliteTimestamp,
          "crm-import",
          [],
          [],
          [
              new(contact, customerHashKey, [new("Email Address", "first@example.test")], "contact-hash-1"),
          ]);

      AssertCompatiblePostgresStrategy(provider, context, firstSatelliteRequest);

      var firstSatelliteResult = await saveService.SaveAsync(context, firstSatelliteRequest);

      Assert.Equal(1, firstSatelliteResult.RowsWritten);
      AssertOptimizedPathObserved(context);

      var unchangedSatelliteRequest = new DataVaultSaveRequest(
          unchangedSatelliteTimestamp,
          "crm-replay",
          [],
          [],
          [
              new(contact, customerHashKey, [new("Email Address", "ignored@example.test")], "contact-hash-1"),
          ]);

      AssertCompatiblePostgresStrategy(provider, context, unchangedSatelliteRequest);

      var unchangedSatelliteResult = await saveService.SaveAsync(context, unchangedSatelliteRequest);

      Assert.Equal(0, unchangedSatelliteResult.RowsWritten);
      AssertOptimizedPathObserved(context);

      var changedSatelliteRequest = new DataVaultSaveRequest(
          changedSatelliteTimestamp,
          "crm-change",
          [],
          [],
          [
              new(contact, customerHashKey, [new("Email Address", "changed@example.test")], "contact-hash-2"),
          ]);

      AssertCompatiblePostgresStrategy(provider, context, changedSatelliteRequest);

      var changedSatelliteResult = await saveService.SaveAsync(context, changedSatelliteRequest);

      Assert.Equal(1, changedSatelliteResult.RowsWritten);
      AssertOptimizedPathObserved(context);
      AssertSingleSavedRecord(
          linkResult,
          DataVaultTableKind.Link,
          "CustomerOrder",
          "LinkCustomerOrder",
          GetHashKey(linkResult, DataVaultTableKind.Link, "CustomerOrder"));
      AssertSingleSavedRecord(
          firstSatelliteResult,
          DataVaultTableKind.Satellite,
          "Contact",
          "SatCustomerContact",
          customerHashKey);
      AssertSingleSavedRecord(
          unchangedSatelliteResult,
          DataVaultTableKind.Satellite,
          "Contact",
          "SatCustomerContact",
          customerHashKey);
      AssertSingleSavedRecord(
          changedSatelliteResult,
          DataVaultTableKind.Satellite,
          "Contact",
          "SatCustomerContact",
          customerHashKey);

      var customerRow = await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().SingleAsync();
      var orderRow = await context.Set<Dictionary<string, object>>("HubOrder").AsNoTracking().SingleAsync();
      var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().SingleAsync();
      var satelliteRows = (await context.Set<Dictionary<string, object>>("SatCustomerContact")
          .AsNoTracking()
          .ToListAsync())
          .OrderBy(row => Assert.IsType<DateTimeOffset>(row["LoadTimestamp"]))
          .ToArray();

      Assert.Equal("C-100", customerRow["CustomerId"]);
      Assert.Equal("O-200", orderRow["OrderId"]);
      Assert.Equal(customerHashKey, customerRow["CustomerHashKey"]);
      Assert.Equal(orderHashKey, orderRow["OrderHashKey"]);
      Assert.Equal("crm-import", customerRow["RecordSource"]);
      Assert.Equal("crm-import", orderRow["RecordSource"]);
      Assert.Equal("crm-import", linkRow["RecordSource"]);
      Assert.Equal(hubLoadTimestamp, customerRow["LoadTimestamp"]);
      Assert.Equal(hubLoadTimestamp, orderRow["LoadTimestamp"]);
      Assert.Equal(linkLoadTimestamp, linkRow["LoadTimestamp"]);
      Assert.Equal(customerHashKey, linkRow["CustomerHashKey"]);
      Assert.Equal(orderHashKey, linkRow["OrderHashKey"]);
      Assert.Matches("^[0-9a-f]{64}$", Assert.IsType<string>(linkRow["CustomerOrderHashKey"]));
      Assert.Equal(2, satelliteRows.Length);
      AssertSatelliteRow(
          satelliteRows[0],
          customerHashKey,
          "first@example.test",
          "contact-hash-1",
          firstSatelliteTimestamp,
          "crm-import");
      AssertSatelliteRow(
          satelliteRows[1],
          customerHashKey,
          "changed@example.test",
          "contact-hash-2",
          changedSatelliteTimestamp,
          "crm-change");
    }
    finally {
      await context.Database.ExecuteSqlRawAsync("DROP SCHEMA IF EXISTS " + QuoteIdentifier(schemaName) + " CASCADE;");
    }
  }

  [Fact]
  public async Task AddDVaultPostgresBulkStrategyPersistsOrderedHubLinkAndSatelliteBatchWhenConfigured() {
    await ExternalProviderBulkSaveAssertions.AssertProviderBulkSaveAsync(
        ExternalProviderLiveSchemaFixture.CreatePostgresAsync,
        services => services.AddDVaultPostgres(),
        "PostgresDataVaultSaveStrategy",
        AssertPostgresStagedBulkBoundary);
  }

  [Fact]
  public async Task AddDVaultPostgresStagedBulkStrategyRollsBackFailureAndCleansUpWhenConfigured() {
    await ExternalProviderBulkSaveAssertions.AssertProviderBulkSaveFailureRollsBackAsync(
        ExternalProviderLiveSchemaFixture.CreatePostgresAsync,
        services => services.AddDVaultPostgres(),
        "PostgresDataVaultSaveStrategy",
        AssertPostgresStagedBulkBoundary,
        AssertNoPostgresStagingTablesAsync);
  }

  private static DbContextOptions<OptimizedPostgresSaveContext> CreatePostgresOptions(string connectionString) {
    var optionsBuilder = new DbContextOptionsBuilder<OptimizedPostgresSaveContext>();

    NpgsqlProviderReflection.UseNpgsql(optionsBuilder, connectionString);

    return optionsBuilder.Options;
  }

  private static void AssertCompatiblePostgresStrategy(
      IServiceProvider provider,
      OptimizedPostgresSaveContext context,
      DataVaultSaveRequest request) {
    var strategies = provider.GetServices<IDataVaultProviderSaveStrategy>().ToArray();

    Assert.True(
        strategies.Any(strategy => strategy.CanSave(context, [request])),
        PostgresStrategyRegistrationDiagnostic);
  }

  private static void AssertOptimizedPathObserved(OptimizedPostgresSaveContext context) {
    var trackedEntries = context.ChangeTracker.Entries().ToArray();

    Assert.True(
        trackedEntries.Length == 0,
        PostgresOptimizedPathDiagnostic + " Actual tracked entries: " + FormatTrackedEntries(trackedEntries));
  }

  private static void AssertPostgresStagedBulkBoundary(
      DataVaultBulkSaveRequest request,
      DataVaultDiagnosticsResult diagnostics) {
    var staged = diagnostics.SaveStrategy.StagedProviderBulk;

    Assert.NotNull(staged);
    Assert.Equal(DataVaultStagedProviderBulkLifecyclePhase.NativeBulkApplication, staged!.LifecyclePhase);
    Assert.Equal(DataVaultStagedProviderBulkProviderCaveatKind.None, staged.ProviderCaveatKind);
    Assert.Equal(request.Requests.Count, staged.RequestCount);
    Assert.Equal(request.Requests.Sum(current => current.HubOperations.Count), staged.HubOperationCount);
    Assert.Equal(request.Requests.Sum(current => current.LinkOperations.Count), staged.LinkOperationCount);
    Assert.Equal(request.Requests.Sum(current => current.SatelliteOperations.Count), staged.SatelliteOperationCount);
    Assert.True(staged.OperationCount >= MinimumPostgresStagedBulkOperationCount);
    Assert.Empty(staged.FallbackCauseKinds);
  }

  private static async Task AssertNoPostgresStagingTablesAsync(DbContext context) {
    var connection = context.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;
    if (shouldCloseConnection) {
      await connection.OpenAsync().ConfigureAwait(false);
    }

    try {
      await using var command = connection.CreateCommand();
      command.CommandText =
          "SELECT COUNT(*) FROM pg_catalog.pg_class c " +
          "JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace " +
          "WHERE n.nspname LIKE 'pg_temp_%' AND c.relname LIKE '__dvault_stage_%'";
      var count = Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false));

      Assert.Equal(0, count);
    }
    finally {
      if (shouldCloseConnection) {
        await connection.CloseAsync().ConfigureAwait(false);
      }
    }
  }

  private static string FormatTrackedEntries(IReadOnlyList<EntityEntry> trackedEntries) {
    if (trackedEntries.Count == 0) {
      return "<none>";
    }

    return string.Join(
        ", ",
        trackedEntries
            .Select(entry => (entry.Metadata.GetTableName() ?? entry.Metadata.Name) + ":" + entry.State)
            .OrderBy(entry => entry, StringComparer.Ordinal));
  }

  private static string GetHashKey(DataVaultSaveResult result, DataVaultTableKind kind, string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && record.MetadataName == metadataName)
        .HashKey;
  }

  private static void AssertSingleSavedRecord(
      DataVaultSaveResult result,
      DataVaultTableKind kind,
      string metadataName,
      string tableName,
      string hashKey) {
    var record = Assert.Single(result.SavedRecords);

    Assert.Equal(kind, record.Kind);
    Assert.Equal(metadataName, record.MetadataName);
    Assert.Equal(tableName, record.TableName);
    Assert.Equal(hashKey, record.HashKey);
  }

  private static void AssertSatelliteRow(
      Dictionary<string, object> row,
      string parentHashKey,
      string emailAddress,
      string hashDiff,
      DateTimeOffset loadTimestamp,
      string recordSource) {
    Assert.Equal(parentHashKey, row["CustomerHashKey"]);
    Assert.Equal(emailAddress, row["EmailAddress"]);
    Assert.Equal(hashDiff, row["HashDiff"]);
    Assert.Equal(loadTimestamp, row["LoadTimestamp"]);
    Assert.Equal(recordSource, row["RecordSource"]);
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    return new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata("Customer", ["Customer Id"]),
            new DataVaultHubMetadata("Order", ["Order Id"]),
        ],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]),
        ],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"]),
        ]);
  }

  private static string QuoteIdentifier(string value) {
    return "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private sealed class OptimizedPostgresSaveContext(
      DbContextOptions<OptimizedPostgresSaveContext> options,
      string schemaName) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.HasDefaultSchema(schemaName);
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());
    }
  }
}
