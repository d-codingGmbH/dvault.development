using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal static class DataVaultBenchmarkHelpers {
  public const string ClassicEfStrategyFamily = "classic-ef";
  public const string ProviderNeutralFallbackStrategyFamily = "provider-neutral-dvault-fallback";
  public const string SqliteOptimizedStrategyFamily = "sqlite-optimized-dvault";

  public static void AddDataVaultServices(IServiceCollection services, DataVaultBenchmarkStrategy strategy) {
    switch (strategy) {
      case DataVaultBenchmarkStrategy.ProviderNeutralFallback:
        services.AddDVault();
        break;
      case DataVaultBenchmarkStrategy.SqliteOptimized:
        services.AddDVaultSqlite();
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy.");
    }
  }

  public static string GetDataVaultBaselineName(DataVaultBenchmarkStrategy strategy) {
    return strategy switch {
      DataVaultBenchmarkStrategy.ProviderNeutralFallback => "dvault-adddvault-fallback",
      DataVaultBenchmarkStrategy.SqliteOptimized => "dvault-adddvaultsqlite-optimized",
      _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "Unsupported benchmark strategy."),
    };
  }

  public static string GetDataVaultStrategyFamily(DataVaultBenchmarkStrategy strategy) {
    return strategy switch {
      DataVaultBenchmarkStrategy.ProviderNeutralFallback => ProviderNeutralFallbackStrategyFamily,
      DataVaultBenchmarkStrategy.SqliteOptimized => SqliteOptimizedStrategyFamily,
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
}

internal enum DataVaultBenchmarkStrategy {
  ProviderNeutralFallback,
  SqliteOptimized,
}
