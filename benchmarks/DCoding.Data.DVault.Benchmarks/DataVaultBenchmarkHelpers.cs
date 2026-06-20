using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
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
  public const string Db2OptimizedStrategyFamily = "db2-optimized-dvault";

  public static void AddDataVaultServices(IServiceCollection services, DataVaultBenchmarkStrategy strategy) {
    AddDataVaultServices(services, strategy, BenchmarkHashKeyVariant.Default);
  }

  public static void AddDataVaultServices(
      IServiceCollection services,
      DataVaultBenchmarkStrategy strategy,
      BenchmarkHashKeyVariant hashKeyVariant) {
    ArgumentNullException.ThrowIfNull(services);
    ArgumentNullException.ThrowIfNull(hashKeyVariant);

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
      case DataVaultBenchmarkStrategy.Db2Optimized:
        Db2BenchmarkReflection.AddDVaultDb2(services);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy.");
    }

    services.AddDVault(options => options.UseStableHashAlgorithm(hashKeyVariant.StableHashAlgorithmId));
  }

  public static string GetDataVaultBaselineName(DataVaultBenchmarkStrategy strategy) {
    return strategy switch {
      DataVaultBenchmarkStrategy.ProviderNeutralFallback => "dvault-adddvault-fallback",
      DataVaultBenchmarkStrategy.SqliteOptimized => "dvault-adddvaultsqlite-optimized",
      DataVaultBenchmarkStrategy.PostgresOptimized => "dvault-adddvaultpostgres-optimized",
      DataVaultBenchmarkStrategy.SqlServerOptimized => "dvault-adddvaultsqlserver-optimized",
      DataVaultBenchmarkStrategy.MySqlOptimized => "dvault-adddvaultmysql-optimized",
      DataVaultBenchmarkStrategy.OracleOptimized => "dvault-adddvaultoracle-optimized",
      DataVaultBenchmarkStrategy.Db2Optimized => "dvault-adddvaultdb2-optimized",
      _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy."),
    };
  }

  public static string GetDataVaultBaselineName(
      DataVaultBenchmarkStrategy strategy,
      BenchmarkHashKeyVariant hashKeyVariant) {
    ArgumentNullException.ThrowIfNull(hashKeyVariant);

    var baselineName = GetDataVaultBaselineName(strategy);
    return hashKeyVariant == BenchmarkHashKeyVariant.Default
        ? baselineName
        : baselineName + "/" + hashKeyVariant.Label;
  }

  public static string GetDataVaultStrategyFamily(DataVaultBenchmarkStrategy strategy) {
    return strategy switch {
      DataVaultBenchmarkStrategy.ProviderNeutralFallback => ProviderNeutralFallbackStrategyFamily,
      DataVaultBenchmarkStrategy.SqliteOptimized => SqliteOptimizedStrategyFamily,
      DataVaultBenchmarkStrategy.PostgresOptimized => PostgresOptimizedStrategyFamily,
      DataVaultBenchmarkStrategy.SqlServerOptimized => SqlServerOptimizedStrategyFamily,
      DataVaultBenchmarkStrategy.MySqlOptimized => MySqlOptimizedStrategyFamily,
      DataVaultBenchmarkStrategy.OracleOptimized => OracleOptimizedStrategyFamily,
      DataVaultBenchmarkStrategy.Db2Optimized => Db2OptimizedStrategyFamily,
      _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy."),
    };
  }

  public static string? GetProviderSaveStrategyName(DataVaultBenchmarkStrategy strategy) {
    return strategy switch {
      DataVaultBenchmarkStrategy.ProviderNeutralFallback => null,
      DataVaultBenchmarkStrategy.SqliteOptimized => "SqliteDataVaultSaveStrategy",
      DataVaultBenchmarkStrategy.PostgresOptimized => "PostgresDataVaultSaveStrategy",
      DataVaultBenchmarkStrategy.SqlServerOptimized => "SqlServerDataVaultSaveStrategy",
      DataVaultBenchmarkStrategy.MySqlOptimized => "MySqlStagedDataVaultSaveStrategy",
      DataVaultBenchmarkStrategy.OracleOptimized => "OracleDataVaultSaveStrategy",
      DataVaultBenchmarkStrategy.Db2Optimized => "Db2DataVaultSaveStrategy",
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
      DataVaultBenchmarkStrategy.Db2Optimized => null,
      _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy."),
    };
  }

  public static string? GetProviderReadStrategyName(
      DataVaultBenchmarkStrategy strategy,
      string scenarioName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(scenarioName);

    if (string.Equals(scenarioName, "latest-satellite-read", StringComparison.Ordinal)) {
      return strategy switch {
        DataVaultBenchmarkStrategy.SqliteOptimized => "SqliteDataVaultReadStrategy",
        DataVaultBenchmarkStrategy.PostgresOptimized => "PostgresDataVaultReadStrategy",
        DataVaultBenchmarkStrategy.SqlServerOptimized => "SqlServerDataVaultReadStrategy",
        DataVaultBenchmarkStrategy.MySqlOptimized => "MySqlDataVaultReadStrategy",
        DataVaultBenchmarkStrategy.OracleOptimized => "OracleDataVaultReadStrategy",
        DataVaultBenchmarkStrategy.Db2Optimized => "Db2DataVaultReadStrategy",
        _ => null,
      };
    }

    if (scenarioName is "pit-as-of-read" or "bridge-traversal-read") {
      return strategy switch {
        DataVaultBenchmarkStrategy.ProviderNeutralFallback => null,
        DataVaultBenchmarkStrategy.SqliteOptimized => "SqliteDataVaultReadStrategy",
        DataVaultBenchmarkStrategy.PostgresOptimized => "PostgresDataVaultReadStrategy",
        DataVaultBenchmarkStrategy.SqlServerOptimized => "SqlServerDataVaultReadStrategy",
        DataVaultBenchmarkStrategy.MySqlOptimized => "MySqlDataVaultReadStrategy",
        DataVaultBenchmarkStrategy.OracleOptimized => "OracleDataVaultReadStrategy",
        DataVaultBenchmarkStrategy.Db2Optimized => "Db2DataVaultReadStrategy",
        _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy."),
      };
    }

    return GetProviderReadStrategyName(strategy);
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

  public static bool IsLowercaseHexDigest(string value, int digestByteLength) {
    return value.Length == digestByteLength * 2 && value.All(static character =>
        character is >= '0' and <= '9' or >= 'a' and <= 'f');
  }

  public static void AssertStableHashKey(
      string value,
      DataVaultProviderCapabilityProfile providerCapabilities,
      string description) {
    ArgumentNullException.ThrowIfNull(value);
    ArgumentNullException.ThrowIfNull(providerCapabilities);
    ArgumentException.ThrowIfNullOrWhiteSpace(description);

    var mapping = providerCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.HashKey);
    if (mapping.DigestByteLength is not { } digestByteLength) {
      throw new InvalidOperationException(description + " The active provider profile does not declare a digest byte length.");
    }

    BenchmarkAssert.True(
        IsLowercaseHexDigest(value, digestByteLength),
        description +
        " Expected a canonical lowercase hexadecimal " +
        digestByteLength.ToString(CultureInfo.InvariantCulture) +
        "-byte stable-hash digest for algorithm '" +
        (mapping.StableHashAlgorithmId ?? "<unspecified>") +
        "'.");
  }

  public static void AssertHashKeyStorageMapping(
      DbContext context,
      string entityName,
      string propertyName,
      DataVaultProviderCapabilityProfile providerCapabilities,
      string description) {
    ArgumentNullException.ThrowIfNull(context);
    ArgumentException.ThrowIfNullOrWhiteSpace(entityName);
    ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
    ArgumentNullException.ThrowIfNull(providerCapabilities);
    ArgumentException.ThrowIfNullOrWhiteSpace(description);

    var expectedMapping = providerCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.HashKey);
    var property = context.Model.FindEntityType(entityName)?.FindProperty(propertyName) ??
        throw new InvalidOperationException(description + " The EF model is missing " + entityName + "." + propertyName + ".");

    BenchmarkAssert.Equal(
        expectedMapping.HashKeyStorageProfile,
        property.FindAnnotation(DataVaultAnnotationNames.HashKeyStorageProfile)?.Value as DataVaultHashKeyStorageProfile?,
        description + " The EF model hash-key storage profile drifted.");
    BenchmarkAssert.Equal(
        expectedMapping.StableHashAlgorithmId,
        property.FindAnnotation(DataVaultAnnotationNames.StableHashAlgorithmId)?.Value as string,
        description + " The EF model stable-hash algorithm drifted.");
    BenchmarkAssert.Equal(
        expectedMapping.DigestByteLength,
        property.FindAnnotation(DataVaultAnnotationNames.StableHashDigestByteLength)?.Value as int?,
        description + " The EF model stable-hash digest length drifted.");
    BenchmarkAssert.Equal(
        expectedMapping.ValueFormat,
        property.FindAnnotation(DataVaultAnnotationNames.ProviderValueFormat)?.Value as DataVaultProviderValueFormat?,
        description + " The EF model provider value format drifted.");
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
