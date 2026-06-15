using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal enum DataVaultBenchmarkStrategy {
  ProviderNeutralFallback,
  SqliteOptimized,
  PostgresOptimized,
  SqlServerOptimized,
  MySqlOptimized,
  OracleOptimized,
  Db2Optimized,
}
