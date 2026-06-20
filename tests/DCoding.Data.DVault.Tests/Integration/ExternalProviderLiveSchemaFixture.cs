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

  public static async Task<ExternalProviderLiveSchemaFixture> CreateDb2Async() {
    var configuration = Db2IntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(Db2IntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var suffix = Guid.NewGuid().ToString("N")[..16].ToUpperInvariant();
    var modelOptions = ExternalProviderLiveSchemaModelOptions.ForDb2(suffix);
    var optionsBuilder = CreateOptionsBuilder();
    Db2ProviderReflection.UseDb2(optionsBuilder, configuration.ConnectionString!);

    var fixture = new ExternalProviderLiveSchemaFixture(
        ProviderTestCategories.Db2Provider,
        optionsBuilder.Options,
        modelOptions,
        DropDb2ObjectsAsync);

    await using var context = fixture.CreateContext();
    await DropDb2ObjectsAsync(context);
    await context.GetService<IRelationalDatabaseCreator>().CreateTablesAsync();

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

  private static async Task DropDb2ObjectsAsync(ExternalProviderLiveSchemaContext context) {
    foreach (var table in context.ModelOptions.ExpectedSnapshot.Tables.Reverse()) {
      try {
        await context.Database.ExecuteSqlRawAsync("DROP TABLE " + QuoteDb2Identifier(table.TableName));
      }
      catch (Exception exception) when (IsUndefinedDb2Object(exception)) {
      }
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

  private static string QuoteDb2Identifier(string value) {
    var normalizedValue = value.ToUpperInvariant();
    return "\"" + normalizedValue.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string SqlServerLiteral(string value) {
    return "N'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }

  private static string OracleLiteral(string value) {
    return "'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }

  private static bool IsUndefinedDb2Object(Exception exception) {
    var message = exception.ToString();

    return message.Contains("SQLSTATE=42704", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("SQL0204N", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("undefined name", StringComparison.OrdinalIgnoreCase);
  }
}
