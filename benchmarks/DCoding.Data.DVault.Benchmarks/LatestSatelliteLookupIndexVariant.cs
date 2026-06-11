using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable EF1003 // Benchmark index variants use fixed produced table and index names with local quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class LatestSatelliteLookupIndexVariant {
  private const string ParentDescIndexName = "IX_DV_LATEST_DESC";
  private const string CoveringIndexName = "IX_DV_LATEST_COVER";
  private const string CompressedIndexName = "IX_DV_LATEST_COMP";

  private LatestSatelliteLookupIndexVariant(
      string baselineName,
      string description,
      Func<DbContext, CancellationToken, Task> applyAsync) {
    BaselineName = baselineName;
    Description = description;
    ApplyAsync = applyAsync;
  }

  public string BaselineName { get; }

  public string Description { get; }

  public Func<DbContext, CancellationToken, Task> ApplyAsync { get; }

  public static IReadOnlyList<LatestSatelliteLookupIndexVariant> GetVariants(string providerName) {
    var variants = new List<LatestSatelliteLookupIndexVariant>
    {
        new("latest-index-default", "current model index", (_, _) => Task.CompletedTask),
        new("latest-index-parent-desc", "parent plus descending timestamp index", ApplyParentDescIndexAsync),
        new("latest-index-covering", "parent plus descending timestamp plus hash-diff index", ApplyCoveringIndexAsync),
    };

    if (string.Equals(providerName, BenchmarkExternalProviderDefinitions.Oracle.ProviderName, StringComparison.Ordinal)) {
      variants.Add(new(
          "latest-index-covering-compress1",
          "Oracle compressed parent plus descending timestamp plus hash-diff index",
          ApplyOracleCompressedIndexAsync));
    }

    return variants;
  }

  private static async Task ApplyParentDescIndexAsync(DbContext context, CancellationToken cancellationToken) {
    await DropDefaultSatelliteParentIndexAsync(context, cancellationToken).ConfigureAwait(false);
    await ExecuteProviderSqlAsync(
        context,
        cancellationToken,
        sqlite: "CREATE INDEX " + QuoteSqlite(ParentDescIndexName) + " ON " + QuoteSqlite("SatCustomerProfile") +
            " (" + QuoteSqlite("CustomerHashKey") + ", " + QuoteSqlite("LoadTimestamp") + " DESC);",
        postgres: "CREATE INDEX " + QuotePostgres(ParentDescIndexName) + " ON " + QuotePostgres("SatCustomerProfile") +
            " (" + QuotePostgres("CustomerHashKey") + ", " + QuotePostgres("LoadTimestamp") + " DESC);",
        sqlServer: "CREATE INDEX " + QuoteSqlServer(ParentDescIndexName) + " ON " + QuoteSqlServer("SatCustomerProfile") +
            " (" + QuoteSqlServer("CustomerHashKey") + ", " + QuoteSqlServer("LoadTimestamp") + " DESC);",
        mySql: "CREATE INDEX " + QuoteMySql(ParentDescIndexName) + " ON " + QuoteMySql("SatCustomerProfile") +
            " (" + QuoteMySql("CustomerHashKey") + ", " + QuoteMySql("LoadTimestamp") + " DESC);",
        oracle: "CREATE INDEX " + QuoteOracle(ParentDescIndexName) + " ON " + QuoteOracle("SatCustomerProfile") +
            " (" + QuoteOracle("CustomerHashKey") + ", " + QuoteOracle("LoadTimestamp") + " DESC)").ConfigureAwait(false);
  }

  private static async Task ApplyCoveringIndexAsync(DbContext context, CancellationToken cancellationToken) {
    await DropDefaultSatelliteParentIndexAsync(context, cancellationToken).ConfigureAwait(false);
    await ExecuteProviderSqlAsync(
        context,
        cancellationToken,
        sqlite: "CREATE INDEX " + QuoteSqlite(CoveringIndexName) + " ON " + QuoteSqlite("SatCustomerProfile") +
            " (" + QuoteSqlite("CustomerHashKey") + ", " + QuoteSqlite("LoadTimestamp") + " DESC, " + QuoteSqlite("HashDiff") + ");",
        postgres: "CREATE INDEX " + QuotePostgres(CoveringIndexName) + " ON " + QuotePostgres("SatCustomerProfile") +
            " (" + QuotePostgres("CustomerHashKey") + ", " + QuotePostgres("LoadTimestamp") + " DESC) INCLUDE (" + QuotePostgres("HashDiff") + ");",
        sqlServer: "CREATE INDEX " + QuoteSqlServer(CoveringIndexName) + " ON " + QuoteSqlServer("SatCustomerProfile") +
            " (" + QuoteSqlServer("CustomerHashKey") + ", " + QuoteSqlServer("LoadTimestamp") + " DESC) INCLUDE (" + QuoteSqlServer("HashDiff") + ");",
        mySql: "CREATE INDEX " + QuoteMySql(CoveringIndexName) + " ON " + QuoteMySql("SatCustomerProfile") +
            " (" + QuoteMySql("CustomerHashKey") + ", " + QuoteMySql("LoadTimestamp") + " DESC, " + QuoteMySql("HashDiff") + ");",
        oracle: "CREATE INDEX " + QuoteOracle(CoveringIndexName) + " ON " + QuoteOracle("SatCustomerProfile") +
            " (" + QuoteOracle("CustomerHashKey") + ", " + QuoteOracle("LoadTimestamp") + " DESC, " + QuoteOracle("HashDiff") + ")")
        .ConfigureAwait(false);
  }

  private static async Task ApplyOracleCompressedIndexAsync(DbContext context, CancellationToken cancellationToken) {
    await DropDefaultSatelliteParentIndexAsync(context, cancellationToken).ConfigureAwait(false);
    await ExecuteProviderSqlAsync(
        context,
        cancellationToken,
        sqlite: null,
        postgres: null,
        sqlServer: null,
        mySql: null,
        oracle: "CREATE INDEX " + QuoteOracle(CompressedIndexName) + " ON " + QuoteOracle("SatCustomerProfile") +
            " (" + QuoteOracle("CustomerHashKey") + ", " + QuoteOracle("LoadTimestamp") + " DESC, " + QuoteOracle("HashDiff") + ") COMPRESS 1")
        .ConfigureAwait(false);
  }

  private static Task DropDefaultSatelliteParentIndexAsync(DbContext context, CancellationToken cancellationToken) {
    return ExecuteProviderSqlAsync(
        context,
        cancellationToken,
        sqlite: "DROP INDEX IF EXISTS " + QuoteSqlite("IxSatCustomerProfileSatelliteParentCustomerHashKeyLoadTimestamp") + ";",
        postgres: "DROP INDEX IF EXISTS " + QuotePostgres("IxSatCustomerProfileSatelliteParentCustomerHashKeyLoadTimestamp") + ";",
        sqlServer: "DROP INDEX IF EXISTS " + QuoteSqlServer("IxSatCustomerProfileSatelliteParentCustomerHashKeyLoadTimestamp") +
            " ON " + QuoteSqlServer("SatCustomerProfile") + ";",
        mySql: "DROP INDEX " + QuoteMySql("IxSatCustomerProfileSatelliteParentCustomerHashKeyLoadTimestamp") +
            " ON " + QuoteMySql("SatCustomerProfile") + ";",
        oracle: "BEGIN " +
            "FOR index_record IN (" +
            "SELECT index_name FROM user_indexes WHERE table_name = 'SatCustomerProfile' AND index_type <> 'LOB' AND index_name NOT IN (" +
            "SELECT constraint_name FROM user_constraints WHERE table_name = 'SatCustomerProfile' AND constraint_type = 'P'" +
            ")) LOOP " +
            "EXECUTE IMMEDIATE 'DROP INDEX \"' || REPLACE(index_record.index_name, '\"', '\"\"') || '\"'; " +
            "END LOOP; " +
            "END;");
  }

  private static async Task ExecuteProviderSqlAsync(
      DbContext context,
      CancellationToken cancellationToken,
      string? sqlite,
      string? postgres,
      string? sqlServer,
      string? mySql,
      string? oracle) {
    var commandText = context.Database.ProviderName switch {
      "Microsoft.EntityFrameworkCore.Sqlite" => sqlite,
      "Npgsql.EntityFrameworkCore.PostgreSQL" => postgres,
      "Microsoft.EntityFrameworkCore.SqlServer" => sqlServer,
      "Pomelo.EntityFrameworkCore.MySql" or "MySql.EntityFrameworkCore" => mySql,
      "Oracle.EntityFrameworkCore" => oracle,
      _ => throw new NotSupportedException(
          "Latest satellite lookup index benchmarks do not support provider '" + context.Database.ProviderName + "'."),
    };

    if (string.IsNullOrWhiteSpace(commandText)) {
      return;
    }

    await context.Database.ExecuteSqlRawAsync(commandText, cancellationToken).ConfigureAwait(false);
  }

  private static string QuoteSqlite(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string QuotePostgres(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string QuoteSqlServer(string identifier) {
    return "[" + identifier.Replace("]", "]]", StringComparison.Ordinal) + "]";
  }

  private static string QuoteMySql(string identifier) {
    return "`" + identifier.Replace("`", "``", StringComparison.Ordinal) + "`";
  }

  private static string QuoteOracle(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }
}

#pragma warning restore EF1003
