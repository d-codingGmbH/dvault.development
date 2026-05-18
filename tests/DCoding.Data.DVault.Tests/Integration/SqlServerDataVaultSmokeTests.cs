using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
public sealed class SqlServerDataVaultSmokeTests {
  [Fact]
  public async Task AddDVaultSqlServerPersistsRepresentativeHubSaveWhenConfigured() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 4, 9, 15, 0, TimeSpan.Zero);
    var request = new DataVaultSaveRequest(
        loadTimestamp,
        "sqlserver-smoke",
        [new(customer, [new("Customer Id", "C-SQL-100")])],
        []);
    await using var database = await SqlServerSmokeDatabase.CreateAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using (var context = database.CreateContext()) {
      var result = await saveService.SaveAsync(context, request);

      Assert.Equal(1, result.RowsWritten);
      AssertSingleSavedRecord(
          result,
          DataVaultTableKind.Hub,
          "Customer",
          "HubCustomer",
          GetHashKey(result, DataVaultTableKind.Hub, "Customer"));
    }

    await using (var context = database.CreateContext()) {
      var row = await context.Set<Dictionary<string, object>>("HubCustomer").AsNoTracking().SingleAsync();

      Assert.Equal("C-SQL-100", row["CustomerId"]);
      Assert.Equal("sqlserver-smoke", row["RecordSource"]);
      Assert.Equal(loadTimestamp, row["LoadTimestamp"]);
      Assert.Matches("^[0-9a-f]{64}$", Assert.IsType<string>(row["CustomerHashKey"]));
    }
  }

  [Fact]
  public async Task AddDVaultSqlServerPersistsRepresentativeLinkSaveWhenConfigured() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var loadTimestamp = new DateTimeOffset(2026, 5, 4, 9, 30, 0, TimeSpan.Zero);
    var hubRequest = new DataVaultSaveRequest(
        loadTimestamp,
        "sqlserver-smoke",
        [
            new(customer, [new("Customer Id", "C-SQL-200")]),
            new(order, [new("Order Id", "O-SQL-200")]),
        ],
        []);
    await using var database = await SqlServerSmokeDatabase.CreateAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string customerHashKey;
    string orderHashKey;
    DataVaultSaveResult linkResult;

    await using (var context = database.CreateContext()) {
      var hubResult = await saveService.SaveAsync(context, hubRequest);

      Assert.Equal(2, hubResult.RowsWritten);
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
      orderHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");

      var linkRequest = new DataVaultSaveRequest(
          loadTimestamp,
          "sqlserver-smoke",
          [],
          [
              new(customerOrder, [new("Customer", customerHashKey), new("Order", orderHashKey)]),
          ]);
      linkResult = await saveService.SaveAsync(context, linkRequest);

      Assert.Equal(1, linkResult.RowsWritten);
      AssertSingleSavedRecord(
          linkResult,
          DataVaultTableKind.Link,
          "CustomerOrder",
          "LinkCustomerOrder",
          GetHashKey(linkResult, DataVaultTableKind.Link, "CustomerOrder"));
    }

    await using (var context = database.CreateContext()) {
      var linkRow = await context.Set<Dictionary<string, object>>("LinkCustomerOrder").AsNoTracking().SingleAsync();

      Assert.Equal(customerHashKey, linkRow["CustomerHashKey"]);
      Assert.Equal(orderHashKey, linkRow["OrderHashKey"]);
      Assert.Equal("sqlserver-smoke", linkRow["RecordSource"]);
      Assert.Equal(loadTimestamp, linkRow["LoadTimestamp"]);
      Assert.Matches("^[0-9a-f]{64}$", Assert.IsType<string>(linkRow["CustomerOrderHashKey"]));
    }
  }

  [Fact]
  public async Task AddDVaultSqlServerPersistsRepresentativeSatelliteSaveWhenConfigured() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"]);
    var hubLoadTimestamp = new DateTimeOffset(2026, 5, 4, 9, 45, 0, TimeSpan.Zero);
    var satelliteLoadTimestamp = new DateTimeOffset(2026, 5, 4, 10, 0, 0, TimeSpan.Zero);
    var hubRequest = new DataVaultSaveRequest(
        hubLoadTimestamp,
        "sqlserver-smoke",
        [new(customer, [new("Customer Id", "C-SQL-300")])],
        []);
    await using var database = await SqlServerSmokeDatabase.CreateAsync();
    using var provider = CreateServiceProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    string customerHashKey;
    DataVaultSaveResult satelliteResult;

    await using (var context = database.CreateContext()) {
      var hubResult = await saveService.SaveAsync(context, hubRequest);

      Assert.Equal(1, hubResult.RowsWritten);
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      var satelliteRequest = new DataVaultSaveRequest(
          satelliteLoadTimestamp,
          "sqlserver-smoke",
          [],
          [],
          [
              new(contact, customerHashKey, [new("Email Address", "sqlserver@example.test")], "contact-hash-sqlserver-1"),
          ]);
      satelliteResult = await saveService.SaveAsync(context, satelliteRequest);

      Assert.Equal(1, satelliteResult.RowsWritten);
      AssertSingleSavedRecord(
          satelliteResult,
          DataVaultTableKind.Satellite,
          "Contact",
          "SatCustomerContact",
          customerHashKey);
    }

    await using (var context = database.CreateContext()) {
      var row = await context.Set<Dictionary<string, object>>("SatCustomerContact").AsNoTracking().SingleAsync();

      Assert.Equal(customerHashKey, row["CustomerHashKey"]);
      Assert.Equal("sqlserver@example.test", row["EmailAddress"]);
      Assert.Equal("contact-hash-sqlserver-1", row["HashDiff"]);
      Assert.Equal(satelliteLoadTimestamp, row["LoadTimestamp"]);
      Assert.Equal("sqlserver-smoke", row["RecordSource"]);
    }
  }

  [Fact]
  public async Task AddDVaultSqlServerBulkStrategyPersistsOrderedHubLinkAndSatelliteBatchWhenConfigured() {
    await ExternalProviderBulkSaveAssertions.AssertProviderBulkSaveAsync(
        ExternalProviderLiveSchemaFixture.CreateSqlServerAsync,
        services => services.AddDVaultSqlServer(),
        "SqlServerDataVaultSaveStrategy");
  }

  private static ServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();
    services.AddDVaultSqlServer();

    return services.BuildServiceProvider(validateScopes: true);
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
            new DataVaultSatelliteMetadata(
                "State",
                DataVaultMetadataReference.Link("CustomerOrder"),
                ["State Code"]),
        ]);
  }

  private static DbContextOptions<SqlServerSmokeContext> CreateSqlServerOptions(string connectionString) {
    var optionsBuilder = new DbContextOptionsBuilder<SqlServerSmokeContext>();

    SqlServerProviderReflection.UseSqlServer(optionsBuilder, connectionString);
    optionsBuilder.ReplaceService<IModelCacheKeyFactory, SqlServerSmokeModelCacheKeyFactory>();

    return optionsBuilder.Options;
  }

  private static string QuoteIdentifier(string value) {
    return "[" + value.Replace("]", "]]", StringComparison.Ordinal) + "]";
  }

  private static string SqlLiteral(string value) {
    return "N'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }

  private sealed class SqlServerSmokeDatabase : IAsyncDisposable {
    private static readonly string[] ProducedTables = [
        "SatCustomerContact",
        "SatCustomerOrderState",
        "LinkCustomerOrder",
        "HubOrder",
        "HubCustomer",
    ];

    private readonly DbContextOptions<SqlServerSmokeContext> _options;
    private readonly string _schemaName;

    private SqlServerSmokeDatabase(DbContextOptions<SqlServerSmokeContext> options, string schemaName) {
      _options = options;
      _schemaName = schemaName;
    }

    public static async Task<SqlServerSmokeDatabase> CreateAsync() {
      var configuration = SqlServerIntegrationTestConfiguration.FromEnvironment();
      if (!configuration.IsConfigured) {
        Assert.Skip(SqlServerIntegrationTestConfiguration.MissingConfigurationSkipMessage);
      }

      var schemaName = "dvault_test_" + Guid.NewGuid().ToString("N");
      var database = new SqlServerSmokeDatabase(CreateSqlServerOptions(configuration.ConnectionString!), schemaName);

      try {
        await using var context = database.CreateContext();
        await database.CreateSchemaAsync(context);
      }
      catch {
        await database.DisposeAsync();
        throw;
      }

      return database;
    }

    public SqlServerSmokeContext CreateContext() {
      return new SqlServerSmokeContext(_options, _schemaName);
    }

    public async ValueTask DisposeAsync() {
      await using var context = CreateContext();
      await DropSchemaAsync(context).ConfigureAwait(false);
    }

    private async Task CreateSchemaAsync(SqlServerSmokeContext context) {
      await context.Database.ExecuteSqlRawAsync(
          "IF SCHEMA_ID(" + SqlLiteral(_schemaName) + ") IS NULL EXEC(N'CREATE SCHEMA " + QuoteIdentifier(_schemaName) + "');");
      foreach (var batch in SqlServerBatchScript.SplitBatches(context.Database.GenerateCreateScript())) {
        await context.Database.ExecuteSqlRawAsync(batch);
      }
    }

    private async Task DropSchemaAsync(SqlServerSmokeContext context) {
      foreach (var tableName in ProducedTables) {
        await context.Database.ExecuteSqlRawAsync(
            "DROP TABLE IF EXISTS " + QuoteIdentifier(_schemaName) + "." + QuoteIdentifier(tableName) + ";");
      }

      await context.Database.ExecuteSqlRawAsync("DROP SCHEMA IF EXISTS " + QuoteIdentifier(_schemaName) + ";");
    }
  }

  private sealed class SqlServerSmokeContext(
      DbContextOptions<SqlServerSmokeContext> options,
      string schemaName) : DbContext(options) {
    public string SchemaName { get; } = schemaName;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.HasDefaultSchema(SchemaName);
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());
    }
  }

  private sealed class SqlServerSmokeModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      if (context is SqlServerSmokeContext smokeContext) {
        return (context.GetType(), smokeContext.SchemaName, designTime);
      }

      return (context.GetType(), designTime);
    }
  }
}
