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

  public static string? GetProviderSaveStrategyName(DataVaultBenchmarkStrategy strategy) {
    return strategy switch {
      DataVaultBenchmarkStrategy.ProviderNeutralFallback => null,
      DataVaultBenchmarkStrategy.SqliteOptimized => "SqliteDataVaultSaveStrategy",
      DataVaultBenchmarkStrategy.PostgresOptimized => "PostgresDataVaultSaveStrategy",
      DataVaultBenchmarkStrategy.SqlServerOptimized => "SqlServerDataVaultSaveStrategy",
      DataVaultBenchmarkStrategy.MySqlOptimized => "MySqlDataVaultSaveStrategy",
      DataVaultBenchmarkStrategy.OracleOptimized => "OracleDataVaultSaveStrategy",
      _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy."),
    };
  }

  public static string? GetProviderReadStrategyName(DataVaultBenchmarkStrategy strategy) {
    return strategy switch {
      DataVaultBenchmarkStrategy.ProviderNeutralFallback => null,
      DataVaultBenchmarkStrategy.SqliteOptimized => "SqliteDataVaultReadStrategy",
      DataVaultBenchmarkStrategy.PostgresOptimized => null,
      DataVaultBenchmarkStrategy.SqlServerOptimized => null,
      DataVaultBenchmarkStrategy.MySqlOptimized => null,
      DataVaultBenchmarkStrategy.OracleOptimized => null,
      _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy."),
    };
  }

  public static void AssertProviderSaveStrategySelected(
      DataVaultDiagnosticsResult diagnostics,
      string expectedStrategyName) {
    ArgumentNullException.ThrowIfNull(diagnostics);
    ArgumentException.ThrowIfNullOrWhiteSpace(expectedStrategyName);

    BenchmarkAssert.Equal(
        DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected,
        diagnostics.SaveStrategy.Status,
        "The optimized benchmark row must select the provider-specific Data Vault save strategy.");
    BenchmarkAssert.Equal(
        expectedStrategyName,
        diagnostics.SaveStrategy.SelectedStrategyName,
        "The optimized benchmark row selected an unexpected provider-specific Data Vault save strategy.");
    BenchmarkAssert.Equal(0, diagnostics.SaveStrategy.FallbackCauses.Count, "The optimized benchmark row must not have fallback causes.");
    BenchmarkAssert.True(
        diagnostics.SaveStrategy.Candidates.Any(candidate => string.Equals(candidate.StrategyName, expectedStrategyName, StringComparison.Ordinal) && candidate.CanSave),
        "The optimized benchmark row must expose the selected provider strategy as an accepted diagnostics candidate.");
  }

  public static void AssertProviderReadStrategySelected(
      DataVaultDiagnosticsResult diagnostics,
      string expectedStrategyName) {
    ArgumentNullException.ThrowIfNull(diagnostics);
    ArgumentException.ThrowIfNullOrWhiteSpace(expectedStrategyName);

    BenchmarkAssert.Equal(
        DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected,
        diagnostics.ReadStrategy.Status,
        "The optimized benchmark row must select the provider-specific Data Vault read strategy.");
    BenchmarkAssert.Equal(
        expectedStrategyName,
        diagnostics.ReadStrategy.SelectedStrategyName,
        "The optimized benchmark row selected an unexpected provider-specific Data Vault read strategy.");
    BenchmarkAssert.Equal(0, diagnostics.ReadStrategy.FallbackCauses.Count, "The optimized benchmark row must not have fallback causes.");
    BenchmarkAssert.True(
        diagnostics.ReadStrategy.Candidates.Any(candidate => string.Equals(candidate.StrategyName, expectedStrategyName, StringComparison.Ordinal) && candidate.CanRead),
        "The optimized benchmark row must expose the selected provider read strategy as an accepted diagnostics candidate.");
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
      long ticks => new DateTimeOffset(new DateTime(ticks, DateTimeKind.Utc)),
      int ticks => new DateTimeOffset(new DateTime(ticks, DateTimeKind.Utc)),
      decimal ticks when ticks == decimal.Truncate(ticks) && ticks >= long.MinValue && ticks <= long.MaxValue =>
          new DateTimeOffset(new DateTime((long)ticks, DateTimeKind.Utc)),
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

  public static object ToStoredTimestamp(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      DateTimeOffset timestamp) {
    var utcTimestamp = timestamp.ToUniversalTime();
    return loadTimestampStorage switch {
      DataVaultLoadTimestampStorage.Iso8601UtcText => utcTimestamp.ToString("O", CultureInfo.InvariantCulture),
      DataVaultLoadTimestampStorage.UtcTicks => utcTimestamp.UtcDateTime.Ticks,
      _ => utcTimestamp,
    };
  }

  public static object ToStoredTimestamp(
      DataVaultProviderCapabilityProfile providerCapabilities,
      DataVaultLogicalPropertyKind logicalPropertyKind,
      DateTimeOffset timestamp) {
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    var mapping = providerCapabilities.GetRequiredTypeMapping(logicalPropertyKind);
    var utcTimestamp = timestamp.ToUniversalTime();
    return mapping.ValueFormat switch {
      DataVaultProviderValueFormat.UtcTicks => utcTimestamp.UtcDateTime.Ticks,
      DataVaultProviderValueFormat.Iso8601UtcText when mapping.ModelClrType == typeof(string) =>
          utcTimestamp.ToString("O", CultureInfo.InvariantCulture),
      _ => utcTimestamp,
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
