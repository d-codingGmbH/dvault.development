using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.OracleProvider)]
public sealed class OracleDataVaultSmokeTests {
  private const string CustomerHubEntityName = "HubCustomer";
  private const string CustomerHashKeyColumnName = "CustomerHashKey";
  private const string CustomerIdColumnName = "CustomerId";
  private const string RecordSourceColumnName = "RecordSource";
  private const string CustomerId = "C-ORACLE-100";
  private const string RecordSource = "oracle-smoke";

  private static readonly DateTimeOffset LoadTimestamp =
      new(2026, 5, 4, 0, 0, 0, TimeSpan.Zero);

  [Fact]
  public async Task AddDVaultOraclePersistsSingleCustomerHubWhenConfigured() {
    var configuration = OracleIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(OracleIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var databaseNames = OracleSmokeDatabaseNames.Create();
    var options = CreateOracleOptions(configuration.ConnectionString!);
    var services = new ServiceCollection();
    services.AddDVaultOracle();

    using var serviceProvider = services.BuildServiceProvider(validateScopes: true);
    var saveService = serviceProvider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new OracleSmokeContext(options, databaseNames);
    try {
      await context.GetService<IRelationalDatabaseCreator>().CreateTablesAsync();

      var result = await saveService.SaveAsync(context, CreateCustomerHubRequest());

      Assert.Equal(1, result.RowsWritten);
      var record = Assert.Single(result.SavedRecords);
      Assert.Equal(DataVaultTableKind.Hub, record.Kind);
      Assert.Equal("Customer", record.MetadataName);
      Assert.Equal(CustomerHubEntityName, record.TableName);
      Assert.Matches("^[0-9a-f]{64}$", record.HashKey);

      var row = await context.Set<Dictionary<string, object>>(CustomerHubEntityName)
          .AsNoTracking()
          .SingleAsync();

      Assert.Equal(CustomerId, row[CustomerIdColumnName]);
      Assert.Equal(RecordSource, row[RecordSourceColumnName]);
      Assert.Equal(record.HashKey, row[CustomerHashKeyColumnName]);
    }
    finally {
      await DropOracleTableIfExistsAsync(context, databaseNames.CustomerHubTableName);
    }
  }

  private static DbContextOptions<OracleSmokeContext> CreateOracleOptions(string connectionString) {
    var optionsBuilder = new DbContextOptionsBuilder<OracleSmokeContext>();

    OracleProviderReflection.UseOracle(optionsBuilder, connectionString);

    return optionsBuilder.Options;
  }

  private static DataVaultSaveRequest CreateCustomerHubRequest() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    return new DataVaultSaveRequest(
        LoadTimestamp,
        RecordSource,
        [new DataVaultHubSaveOperation(customer, [new("Customer Id", CustomerId)])],
        []);
  }

  private static async Task DropOracleTableIfExistsAsync(DbContext context, string tableName) {
    await context.Database.ExecuteSqlRawAsync(
        "BEGIN " +
        "EXECUTE IMMEDIATE " + SqlLiteral("DROP TABLE " + QuoteIdentifier(tableName) + " PURGE") + "; " +
        "EXCEPTION WHEN OTHERS THEN IF SQLCODE != -942 THEN RAISE; END IF; END;");
  }

  private static string QuoteIdentifier(string value) {
    return "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string SqlLiteral(string value) {
    return "'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }

  private sealed class OracleSmokeContext(
      DbContextOptions<OracleSmokeContext> options,
      OracleSmokeDatabaseNames databaseNames) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);

      modelBuilder.ApplyDataVaultMetadata(new DataVaultMetadataModel([customer], [], []));
      modelBuilder.SharedTypeEntity<Dictionary<string, object>>(CustomerHubEntityName, entity => {
        entity.ToTable(databaseNames.CustomerHubTableName);
        entity.Property<string>(CustomerHashKeyColumnName).HasColumnType("VARCHAR2(64)");
        entity.Property<DateTimeOffset>("LoadTimestamp").HasColumnType("TIMESTAMP WITH TIME ZONE");
        entity.Property<string>(RecordSourceColumnName).HasColumnType("VARCHAR2(256)");
        entity.Property<string>(CustomerIdColumnName).HasColumnType("VARCHAR2(256)");
        entity.HasKey(CustomerHashKeyColumnName).HasName(databaseNames.CustomerPrimaryKeyName);
        entity
            .HasIndex(CustomerIdColumnName)
            .IsUnique()
            .HasDatabaseName(databaseNames.CustomerBusinessKeyIndexName);
      });
    }
  }

  private sealed class OracleSmokeDatabaseNames {
    private OracleSmokeDatabaseNames(string suffix) {
      CustomerHubTableName = "DVH" + suffix;
      CustomerPrimaryKeyName = "PK_DVH" + suffix;
      CustomerBusinessKeyIndexName = "IX_DVH" + suffix;
    }

    public string CustomerHubTableName { get; }

    public string CustomerPrimaryKeyName { get; }

    public string CustomerBusinessKeyIndexName { get; }

    public static OracleSmokeDatabaseNames Create() {
      return new OracleSmokeDatabaseNames(Guid.NewGuid().ToString("N")[..16].ToUpperInvariant());
    }
  }
}
