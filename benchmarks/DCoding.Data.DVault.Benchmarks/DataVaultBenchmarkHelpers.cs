using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal static class DataVaultBenchmarkHelpers {
  public const string ClassicEfStrategyFamily = "classic-ef";
  public const string ProviderNeutralFallbackStrategyFamily = "provider-neutral-dvault-fallback";
  public const string SqliteOptimizedStrategyFamily = "sqlite-optimized-dvault";
  public const string PostgresOptimizedStrategyFamily = "postgres-optimized-dvault";
  public const string SqlServerOptimizedStrategyFamily = "sqlserver-optimized-dvault";
  public const string MySqlOptimizedStrategyFamily = "mysql-optimized-dvault";
  public const string OracleOptimizedStrategyFamily = "oracle-optimized-dvault";

  public static void AddDataVaultServices(IServiceCollection services, DataVaultBenchmarkStrategy strategy) {
    switch (strategy) {
      case DataVaultBenchmarkStrategy.ProviderNeutralFallback:
        services.AddDVault();
        break;
      case DataVaultBenchmarkStrategy.SqliteOptimized:
        services.AddDVaultSqlite();
        break;
      case DataVaultBenchmarkStrategy.PostgresOptimized:
        services.AddDVaultPostgres();
        break;
      case DataVaultBenchmarkStrategy.SqlServerOptimized:
        services.AddDVaultSqlServer();
        break;
      case DataVaultBenchmarkStrategy.MySqlOptimized:
        services.AddDVaultMySql();
        break;
      case DataVaultBenchmarkStrategy.OracleOptimized:
        services.AddDVaultOracle();
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy.");
    }
  }

  public static string GetDataVaultBaselineName(DataVaultBenchmarkStrategy strategy) {
    return strategy switch {
      DataVaultBenchmarkStrategy.ProviderNeutralFallback => "dvault-adddvault-fallback",
      DataVaultBenchmarkStrategy.SqliteOptimized => "dvault-adddvaultsqlite-optimized",
      DataVaultBenchmarkStrategy.PostgresOptimized => "dvault-adddvaultpostgres-optimized",
      DataVaultBenchmarkStrategy.SqlServerOptimized => "dvault-adddvaultsqlserver-optimized",
      DataVaultBenchmarkStrategy.MySqlOptimized => "dvault-adddvaultmysql-optimized",
      DataVaultBenchmarkStrategy.OracleOptimized => "dvault-adddvaultoracle-optimized",
      _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy."),
    };
  }

  public static string GetDataVaultStrategyFamily(DataVaultBenchmarkStrategy strategy) {
    return strategy switch {
      DataVaultBenchmarkStrategy.ProviderNeutralFallback => ProviderNeutralFallbackStrategyFamily,
      DataVaultBenchmarkStrategy.SqliteOptimized => SqliteOptimizedStrategyFamily,
      DataVaultBenchmarkStrategy.PostgresOptimized => PostgresOptimizedStrategyFamily,
      DataVaultBenchmarkStrategy.SqlServerOptimized => SqlServerOptimizedStrategyFamily,
      DataVaultBenchmarkStrategy.MySqlOptimized => MySqlOptimizedStrategyFamily,
      DataVaultBenchmarkStrategy.OracleOptimized => OracleOptimizedStrategyFamily,
      _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy."),
    };
  }

  public static string GetHashKey(DataVaultSaveResult result, DataVaultTableKind kind, string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && string.Equals(record.MetadataName, metadataName, StringComparison.Ordinal))
        .HashKey;
  }

  public static bool IsLowercaseSha256(string value) {
    return value.Length == 64 && value.All(static character =>
        character is >= '0' and <= '9' or >= 'a' and <= 'f');
  }

  public static DateTimeOffset ReadLoadTimestamp(
      IReadOnlyDictionary<string, object> row,
      string columnName = "LoadTimestamp") {
    ArgumentNullException.ThrowIfNull(row);
    ArgumentException.ThrowIfNullOrWhiteSpace(columnName);

    var value = row[columnName];
    return value switch {
      DateTimeOffset dateTimeOffset => dateTimeOffset.ToUniversalTime(),
      DateTime dateTime => new DateTimeOffset(
          dateTime.Kind == DateTimeKind.Unspecified
              ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
              : dateTime).ToUniversalTime(),
      string text => DateTimeOffset.Parse(
          text,
          CultureInfo.InvariantCulture,
          DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal),
      _ => DateTimeOffset.Parse(
          Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty,
          CultureInfo.InvariantCulture,
          DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal),
    };
  }
}

internal enum DataVaultBenchmarkStrategy {
  ProviderNeutralFallback,
  SqliteOptimized,
  PostgresOptimized,
  SqlServerOptimized,
  MySqlOptimized,
  OracleOptimized,
}
