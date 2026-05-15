using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal sealed class ExternalProviderLiveSchemaFixture : IAsyncDisposable {
  private readonly Func<ExternalProviderLiveSchemaContext, Task> _dropAsync;
  private readonly DbContextOptions<ExternalProviderLiveSchemaContext> _options;

  private ExternalProviderLiveSchemaFixture(
      string providerName,
      DbContextOptions<ExternalProviderLiveSchemaContext> options,
      ExternalProviderLiveSchemaModelOptions modelOptions,
      Func<ExternalProviderLiveSchemaContext, Task> dropAsync) {
    ProviderName = providerName;
    _options = options;
    ModelOptions = modelOptions;
    _dropAsync = dropAsync;
  }

  public string ProviderName { get; }

  public ExternalProviderLiveSchemaModelOptions ModelOptions { get; }

  public DataVaultLiveSchemaSnapshot ExpectedSnapshot => ModelOptions.ExpectedSnapshot;

  public ExternalProviderLiveSchemaContext CreateContext() {
    return new ExternalProviderLiveSchemaContext(_options, ModelOptions);
  }

  public static async Task<ExternalProviderLiveSchemaFixture> CreatePostgresAsync() {
    var configuration = PostgresIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(PostgresIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var schemaName = "dvault_test_" + Guid.NewGuid().ToString("N");
    var modelOptions = ExternalProviderLiveSchemaModelOptions.ForPostgres(schemaName);
    var optionsBuilder = CreateOptionsBuilder();
    NpgsqlProviderReflection.UseNpgsql(optionsBuilder, configuration.ConnectionString!);

    var fixture = new ExternalProviderLiveSchemaFixture(
        ProviderTestCategories.PostgresProvider,
        optionsBuilder.Options,
        modelOptions,
        context => context.Database.ExecuteSqlRawAsync(
            "DROP SCHEMA IF EXISTS " + QuotePostgresIdentifier(schemaName) + " CASCADE;"));

    await using var context = fixture.CreateContext();
    await context.Database.ExecuteSqlRawAsync("CREATE SCHEMA " + QuotePostgresIdentifier(schemaName) + ";");
    await context.Database.ExecuteSqlRawAsync(context.Database.GenerateCreateScript());

    return fixture;
  }

  public static async Task<ExternalProviderLiveSchemaFixture> CreateSqlServerAsync() {
    var configuration = SqlServerIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(SqlServerIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var schemaName = "dvault_test_" + Guid.NewGuid().ToString("N");
    var modelOptions = ExternalProviderLiveSchemaModelOptions.ForSqlServer(schemaName);
    var optionsBuilder = CreateOptionsBuilder();
    SqlServerProviderReflection.UseSqlServer(optionsBuilder, configuration.ConnectionString!);

    var fixture = new ExternalProviderLiveSchemaFixture(
        ProviderTestCategories.SqlServerProvider,
        optionsBuilder.Options,
        modelOptions,
        DropSqlServerObjectsAsync);

    await using var context = fixture.CreateContext();
    await context.Database.ExecuteSqlRawAsync(
        "IF SCHEMA_ID(" +
        SqlServerLiteral(schemaName) +
        ") IS NULL EXEC(N'CREATE SCHEMA " +
        QuoteSqlServerIdentifier(schemaName) +
        "');");
    foreach (var batch in SqlServerBatchScript.SplitBatches(context.Database.GenerateCreateScript())) {
      await context.Database.ExecuteSqlRawAsync(batch);
    }

    return fixture;
  }

  public static async Task<ExternalProviderLiveSchemaFixture> CreateOracleAsync() {
    var configuration = OracleIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(OracleIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var suffix = Guid.NewGuid().ToString("N")[..16].ToUpperInvariant();
    var modelOptions = ExternalProviderLiveSchemaModelOptions.ForOracle(suffix);
    var optionsBuilder = CreateOptionsBuilder();
    OracleProviderReflection.UseOracle(optionsBuilder, configuration.ConnectionString!);

    var fixture = new ExternalProviderLiveSchemaFixture(
        ProviderTestCategories.OracleProvider,
        optionsBuilder.Options,
        modelOptions,
        DropOracleObjectsAsync);

    await using var context = fixture.CreateContext();
    await context.GetService<IRelationalDatabaseCreator>().CreateTablesAsync();

    return fixture;
  }

  public static async Task<ExternalProviderLiveSchemaFixture> CreateMySqlAsync() {
    var configuration = MySqlIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(MySqlIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var tableNamePrefix = "DVL" + Guid.NewGuid().ToString("N")[..12] + "_";
    var modelOptions = ExternalProviderLiveSchemaModelOptions.ForMySql(tableNamePrefix);
    var optionsBuilder = CreateOptionsBuilder();
    MySqlProviderReflection.UseMySql(optionsBuilder, configuration.ConnectionString!);

    var fixture = new ExternalProviderLiveSchemaFixture(
        ProviderTestCategories.MySqlProvider,
        optionsBuilder.Options,
        modelOptions,
        DropMySqlObjectsAsync);

    await using var context = fixture.CreateContext();
    await DropMySqlObjectsAsync(context);
    await context.Database.ExecuteSqlRawAsync(context.Database.GenerateCreateScript());

    return fixture;
  }

  public async ValueTask DisposeAsync() {
    await using var context = CreateContext();
    await _dropAsync(context).ConfigureAwait(false);
  }

  private static DbContextOptionsBuilder<ExternalProviderLiveSchemaContext> CreateOptionsBuilder() {
    var optionsBuilder = new DbContextOptionsBuilder<ExternalProviderLiveSchemaContext>();
    optionsBuilder.ReplaceService<IModelCacheKeyFactory, ExternalProviderLiveSchemaModelCacheKeyFactory>();

    return optionsBuilder;
  }

  private static async Task DropSqlServerObjectsAsync(ExternalProviderLiveSchemaContext context) {
    foreach (var table in context.ModelOptions.ExpectedSnapshot.Tables.Reverse()) {
      await context.Database.ExecuteSqlRawAsync(
          "DROP TABLE IF EXISTS " +
          QuoteSqlServerIdentifier(context.ModelOptions.DefaultSchema!) +
          "." +
          QuoteSqlServerIdentifier(table.TableName) +
          ";");
    }

    await context.Database.ExecuteSqlRawAsync(
        "DROP SCHEMA IF EXISTS " + QuoteSqlServerIdentifier(context.ModelOptions.DefaultSchema!) + ";");
  }

  private static async Task DropOracleObjectsAsync(ExternalProviderLiveSchemaContext context) {
    foreach (var table in context.ModelOptions.ExpectedSnapshot.Tables.Reverse()) {
      await context.Database.ExecuteSqlRawAsync(
          "BEGIN " +
          "EXECUTE IMMEDIATE " + OracleLiteral("DROP TABLE " + QuoteOracleIdentifier(table.TableName) + " PURGE") + "; " +
          "EXCEPTION WHEN OTHERS THEN IF SQLCODE != -942 THEN RAISE; END IF; END;");
    }
  }

  private static async Task DropMySqlObjectsAsync(ExternalProviderLiveSchemaContext context) {
    foreach (var table in context.ModelOptions.ExpectedSnapshot.Tables.Reverse()) {
      await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS " + QuoteMySqlIdentifier(table.TableName) + ";");
    }
  }

  private static string QuotePostgresIdentifier(string value) {
    return "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string QuoteSqlServerIdentifier(string value) {
    return "[" + value.Replace("]", "]]", StringComparison.Ordinal) + "]";
  }

  private static string QuoteOracleIdentifier(string value) {
    return "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string QuoteMySqlIdentifier(string value) {
    return "`" + value.Replace("`", "``", StringComparison.Ordinal) + "`";
  }

  private static string SqlServerLiteral(string value) {
    return "N'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }

  private static string OracleLiteral(string value) {
    return "'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }
}

internal sealed class ExternalProviderLiveSchemaModelOptions {
  private ExternalProviderLiveSchemaModelOptions(
      DataVaultProviderCapabilityProfile providerCapabilities,
      string? defaultSchema,
      string tableNamePrefix,
      IReadOnlyDictionary<string, string> tableNameOverrides,
      IReadOnlyDictionary<string, string> identifierNameOverrides) {
    ProviderCapabilities = providerCapabilities;
    DefaultSchema = defaultSchema;
    TableNamePrefix = tableNamePrefix;
    TableNameOverrides = tableNameOverrides;
    IdentifierNameOverrides = identifierNameOverrides;
    ExpectedSnapshot = LiveSchemaReaderContractFixture.CreateExpectedSnapshot(
        providerCapabilities,
        ResolveTableName,
        ResolveIdentifierName);
  }

  public DataVaultProviderCapabilityProfile ProviderCapabilities { get; }

  public string? DefaultSchema { get; }

  public string TableNamePrefix { get; }

  public IReadOnlyDictionary<string, string> TableNameOverrides { get; }

  public IReadOnlyDictionary<string, string> IdentifierNameOverrides { get; }

  public DataVaultLiveSchemaSnapshot ExpectedSnapshot { get; }

  public static ExternalProviderLiveSchemaModelOptions ForPostgres(string schemaName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(schemaName);

    return new ExternalProviderLiveSchemaModelOptions(
        DataVaultProviderCapabilityProfiles.Postgres,
        schemaName,
        tableNamePrefix: string.Empty,
        tableNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal),
        identifierNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal));
  }

  public static ExternalProviderLiveSchemaModelOptions ForSqlServer(string schemaName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(schemaName);

    return new ExternalProviderLiveSchemaModelOptions(
        DataVaultProviderCapabilityProfiles.SqlServer,
        schemaName,
        tableNamePrefix: string.Empty,
        tableNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal),
        identifierNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal));
  }

  public static ExternalProviderLiveSchemaModelOptions ForMySql(string tableNamePrefix) {
    ArgumentException.ThrowIfNullOrWhiteSpace(tableNamePrefix);

    return new ExternalProviderLiveSchemaModelOptions(
        DataVaultProviderCapabilityProfiles.MySql,
        defaultSchema: null,
        tableNamePrefix,
        tableNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal),
        identifierNameOverrides: new Dictionary<string, string>(StringComparer.Ordinal));
  }

  public static ExternalProviderLiveSchemaModelOptions ForOracle(string suffix) {
    ArgumentException.ThrowIfNullOrWhiteSpace(suffix);

    var tableNameOverrides = new Dictionary<string, string>(StringComparer.Ordinal) {
      ["HubCustomer"] = "DVHCU" + suffix,
      ["HubOrder"] = "DVHOR" + suffix,
      ["LinkCustomerOrder"] = "DVLCO" + suffix,
      ["SatCustomerContact"] = "DVSCC" + suffix,
      ["SatCustomerOrderState"] = "DVSCOS" + suffix,
    };
    var identifierNameOverrides = new Dictionary<string, string>(StringComparer.Ordinal) {
      ["PkHubCustomerCustomerHashKey"] = "DPKHC" + suffix,
      ["IxHubCustomerBusinessKeyCustomerId"] = "DIXHCBK" + suffix,
      ["PkHubOrderOrderHashKey"] = "DPKHO" + suffix,
      ["IxHubOrderBusinessKeyOrderId"] = "DIXHOBK" + suffix,
      ["PkLinkCustomerOrderCustomerOrderHashKey"] = "DPKCO" + suffix,
      ["IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey"] = "DIXCOREL" + suffix,
      ["PkSatCustomerContactCustomerHashKeyLoadTimestamp"] = "DPKSCC" + suffix,
      ["IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp"] = "DIXSCCP" + suffix,
      ["PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp"] = "DPKSCOS" + suffix,
      ["IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp"] = "DIXSCOSP" + suffix,
    };

    return new ExternalProviderLiveSchemaModelOptions(
        DataVaultProviderCapabilityProfiles.Oracle,
        defaultSchema: null,
        tableNamePrefix: string.Empty,
        tableNameOverrides,
        identifierNameOverrides);
  }

  public string ResolveTableName(string producedName) {
    return TableNameOverrides.TryGetValue(producedName, out var tableName)
        ? tableName
        : TableNamePrefix + producedName;
  }

  public string ResolveIdentifierName(string producedName) {
    return IdentifierNameOverrides.TryGetValue(producedName, out var identifierName)
        ? identifierName
        : LiveSchemaReaderContractFixture.ResolvePhysicalIdentifierName(producedName, ProviderCapabilities);
  }
}

internal sealed class ExternalProviderLiveSchemaContext(
    DbContextOptions<ExternalProviderLiveSchemaContext> options,
    ExternalProviderLiveSchemaModelOptions modelOptions) : DbContext(options) {
  public ExternalProviderLiveSchemaModelOptions ModelOptions { get; } = modelOptions;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    if (ModelOptions.DefaultSchema is not null) {
      modelBuilder.HasDefaultSchema(ModelOptions.DefaultSchema);
    }

    modelBuilder.ApplyDataVaultMetadata(
        LiveSchemaReaderContractFixture.CreateCanonicalMetadataModel(),
        ModelOptions.ProviderCapabilities);

    ConfigureProducedTable(
        modelBuilder,
        "HubCustomer",
        ["CustomerHashKey"],
        "PkHubCustomerCustomerHashKey",
        [
            new IndexOverride(
                ["CustomerId"],
                "IxHubCustomerBusinessKeyCustomerId"),
        ]);
    ConfigureProducedTable(
        modelBuilder,
        "HubOrder",
        ["OrderHashKey"],
        "PkHubOrderOrderHashKey",
        [
            new IndexOverride(
                ["OrderId"],
                "IxHubOrderBusinessKeyOrderId"),
        ]);
    ConfigureProducedTable(
        modelBuilder,
        "LinkCustomerOrder",
        ["CustomerOrderHashKey"],
        "PkLinkCustomerOrderCustomerOrderHashKey",
        [
            new IndexOverride(
                ["CustomerHashKey", "OrderHashKey"],
                "IxLinkCustomerOrderRelationshipCustomerHashKeyOrderHashKey"),
        ]);
    ConfigureProducedTable(
        modelBuilder,
        "SatCustomerContact",
        ["CustomerHashKey", "LoadTimestamp"],
        "PkSatCustomerContactCustomerHashKeyLoadTimestamp",
        [
            new IndexOverride(
                FindExpectedIndexColumns("SatCustomerContact"),
                "IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp"),
        ]);
    ConfigureProducedTable(
        modelBuilder,
        "SatCustomerOrderState",
        ["CustomerOrderHashKey", "LoadTimestamp"],
        "PkSatCustomerOrderStateCustomerOrderHashKeyLoadTimestamp",
        [
            new IndexOverride(
                FindExpectedIndexColumns("SatCustomerOrderState"),
                "IxSatCustomerOrderStateSatelliteParentCustomerOrderHashKeyLoadTimestamp"),
        ]);
  }

  private void ConfigureProducedTable(
      ModelBuilder modelBuilder,
      string producedTableName,
      IReadOnlyList<string> primaryKeyColumnNames,
      string producedPrimaryKeyName,
      IReadOnlyList<IndexOverride> indexes) {
    var physicalTableName = ModelOptions.ResolveTableName(producedTableName);
    var shouldOverrideTableName = !string.Equals(physicalTableName, producedTableName, StringComparison.Ordinal);
    var shouldOverrideIdentifierNames =
        !string.Equals(
            ModelOptions.ResolveIdentifierName(producedPrimaryKeyName),
            producedPrimaryKeyName,
            StringComparison.Ordinal) ||
        indexes.Any(index => !string.Equals(
            ModelOptions.ResolveIdentifierName(index.ProducedIndexName),
            index.ProducedIndexName,
            StringComparison.Ordinal));

    if (!shouldOverrideTableName && !shouldOverrideIdentifierNames) {
      return;
    }

    modelBuilder.SharedTypeEntity<Dictionary<string, object>>(producedTableName, entity => {
      if (shouldOverrideTableName) {
        ConfigureTableName(entity, physicalTableName);
      }

      if (shouldOverrideIdentifierNames) {
        entity
            .HasKey(primaryKeyColumnNames.ToArray())
            .HasName(ModelOptions.ResolveIdentifierName(producedPrimaryKeyName));

        foreach (var index in indexes) {
          entity
              .HasIndex(index.ColumnNames.ToArray())
              .HasDatabaseName(ModelOptions.ResolveIdentifierName(index.ProducedIndexName));
        }
      }
    });
  }

  private void ConfigureTableName(
      EntityTypeBuilder<Dictionary<string, object>> entity,
      string physicalTableName) {
    if (ModelOptions.DefaultSchema is null) {
      entity.ToTable(physicalTableName);
    }
    else {
      entity.ToTable(physicalTableName, ModelOptions.DefaultSchema);
    }
  }

  private IReadOnlyList<string> FindExpectedIndexColumns(string tableName) {
    return ModelOptions.ExpectedSnapshot.Tables
        .Single(table => string.Equals(table.TableName, ModelOptions.ResolveTableName(tableName), StringComparison.Ordinal))
        .Indexes
        .Single()
        .ColumnNames;
  }

  private sealed record IndexOverride(IReadOnlyList<string> ColumnNames, string ProducedIndexName);
}

internal sealed class ExternalProviderLiveSchemaModelCacheKeyFactory : IModelCacheKeyFactory {
  public object Create(DbContext context, bool designTime) {
    if (context is ExternalProviderLiveSchemaContext liveSchemaContext) {
      var options = liveSchemaContext.ModelOptions;
      return (
          context.GetType(),
          options.ProviderCapabilities.ProfileName,
          options.DefaultSchema ?? string.Empty,
          options.TableNamePrefix,
          string.Join("|", options.TableNameOverrides.OrderBy(item => item.Key, StringComparer.Ordinal)),
          string.Join("|", options.IdentifierNameOverrides.OrderBy(item => item.Key, StringComparer.Ordinal)),
          designTime);
    }

    return (context.GetType(), designTime);
  }
}
