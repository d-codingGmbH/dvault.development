using System.Diagnostics;
using System.Globalization;
using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

internal static class BenchmarkExecutionDetails {
  public static string CreatePlanned(IScenarioBenchmark benchmark) {
    ArgumentNullException.ThrowIfNull(benchmark);

    return CreateDetail(benchmark, GetExecutionPath(benchmark));
  }

  public static string CreateSaveStrategyDetail(
      IScenarioBenchmark benchmark,
      DataVaultDiagnosticsResult diagnostics,
      int requestCount,
      int hubOperationCount,
      int linkOperationCount,
      int satelliteOperationCount) {
    ArgumentNullException.ThrowIfNull(benchmark);
    ArgumentNullException.ThrowIfNull(diagnostics);

    return CreateDetail(benchmark, GetSaveExecutionPath(benchmark, diagnostics)) +
        "; saveStrategyStatus=" + diagnostics.SaveStrategy.Status +
        "; provider=" + (diagnostics.SaveStrategy.ProviderName ?? "<none>") +
        "; selectedStrategy=" + (diagnostics.SaveStrategy.SelectedStrategyName ?? "<none>") +
        "; candidateStrategies=" + FormatSaveStrategyCandidates(diagnostics.SaveStrategy.Candidates) +
        "; candidates=" + diagnostics.SaveStrategy.Candidates.Count.ToString(CultureInfo.InvariantCulture) +
        "; fallbackCauses=" + FormatFallbackCauses(diagnostics.SaveStrategy.FallbackCauses) +
        "; requestCount=" + requestCount.ToString(CultureInfo.InvariantCulture) +
        "; hubOperations=" + hubOperationCount.ToString(CultureInfo.InvariantCulture) +
        "; linkOperations=" + linkOperationCount.ToString(CultureInfo.InvariantCulture) +
        "; satelliteOperations=" + satelliteOperationCount.ToString(CultureInfo.InvariantCulture) +
        "; nativeBulkGate=clean-context,no-multi-active-satellites,provider-eligible-bulk-request" +
        FormatStagedProviderBulk(diagnostics.SaveStrategy.StagedProviderBulk);
  }

  public static string CreateReadStrategyDetail(
      IScenarioBenchmark benchmark,
      DataVaultDiagnosticsResult diagnostics) {
    ArgumentNullException.ThrowIfNull(benchmark);
    ArgumentNullException.ThrowIfNull(diagnostics);

    return CreatePlanned(benchmark) +
        "; readStrategyStatus=" + diagnostics.ReadStrategy.Status +
        "; provider=" + (diagnostics.ReadStrategy.ProviderName ?? "<none>") +
        "; selectedStrategy=" + (diagnostics.ReadStrategy.SelectedStrategyName ?? "<none>") +
        "; candidates=" + diagnostics.ReadStrategy.Candidates.Count.ToString(CultureInfo.InvariantCulture) +
        "; fallbackCauses=" + FormatFallbackCauses(diagnostics.ReadStrategy.FallbackCauses) +
        "; readShape=" + (diagnostics.ReadShape?.Kind.ToString() ?? "<none>") +
        "; readShapeProviderStatus=" + (diagnostics.ReadShape?.Provider.ReadStrategyStatus.ToString() ?? "<none>") +
        "; readShapeFallbackCauses=" + FormatFallbackCauses(
            diagnostics.ReadShape?.Provider.ReadStrategyFallbackCauses ?? Array.Empty<DataVaultReadStrategyFallbackCause>());
  }

  private static string GetExecutionPath(IScenarioBenchmark benchmark) {
    if (benchmark is ProviderNativeBulkIngestionBenchmark providerNativeBulkIngestionBenchmark) {
      return providerNativeBulkIngestionBenchmark.ExecutionPathDetail;
    }

    if (IsReadModelScenario(benchmark.ScenarioName)) {
      return GetReadExecutionPath(benchmark);
    }

    return benchmark.StrategyFamily switch {
      DataVaultBenchmarkHelpers.ClassicEfStrategyFamily => "classic EF baseline",
      DataVaultBenchmarkHelpers.ProviderNeutralFallbackStrategyFamily =>
          "DVault provider-neutral fallback path; selectedStrategy=<none>",
      DataVaultBenchmarkHelpers.SqliteOptimizedStrategyFamily =>
          "DVault SQLite optimized path; selectedStrategy=" + GetSqliteStrategyName(benchmark.ScenarioName),
      DataVaultBenchmarkHelpers.PostgresOptimizedStrategyFamily =>
          "DVault PostgreSQL staged bulk save path; transfer=COPY; selectedStrategy=PostgresDataVaultSaveStrategy; " +
          "smallBatchBoundary=direct-or-unnest",
      DataVaultBenchmarkHelpers.SqlServerOptimizedStrategyFamily =>
          "DVault SQL Server staged native bulk save path; transfer=SqlBulkCopy; selectedStrategy=SqlServerDataVaultSaveStrategy",
      DataVaultBenchmarkHelpers.MySqlOptimizedStrategyFamily =>
          "DVault MySQL retained multi-row save path; selectedStrategy=MySqlDataVaultSaveStrategy",
      DataVaultBenchmarkHelpers.OracleOptimizedStrategyFamily =>
          "DVault Oracle direct optimized save path; selectedStrategy=OracleDataVaultSaveStrategy; " +
          "oracleBulkBoundary=direct-oracle-batching; stagedOracleBulk=not-selected-no-measured-win",
      DataVaultBenchmarkHelpers.Db2OptimizedStrategyFamily =>
          "DVault DB2 optimized save path; selectedStrategy=Db2DataVaultSaveStrategy; " +
          "db2SaveBoundary=clean-context-set-based; stagedBulkBoundary=not-supported",
      "ef-model-build" => "ordinary EF model-building startup path",
      "ef-usemodel-runtime-model" => "precomputed EF runtime model path",
      "direct-ef-query" => "ordinary direct EF query path",
      "compiled-ef-query" => "EF.CompileQuery path",
      "non-pooled-dvault-context" => "AddDbContext DVault context path",
      "pooled-dvault-context" => "AddDbContextPool DVault context path",
      _ => "benchmark-defined path",
    };
  }

  private static string CreateDetail(IScenarioBenchmark benchmark, string executionPath) {
    var hashKeyVariantDetail = benchmark is IBenchmarkHashKeyVariantSource variantSource
        ? "; " + variantSource.HashKeyVariant.CreateExecutionDetail()
        : string.Empty;

    return "scenario=" + benchmark.ScenarioName +
        "; provider=" + benchmark.ProviderName +
        "; baseline=" + benchmark.BaselineName +
        "; strategyFamily=" + benchmark.StrategyFamily +
        "; executionPath=" + executionPath +
        hashKeyVariantDetail;
  }

  private static string GetSaveExecutionPath(
      IScenarioBenchmark benchmark,
      DataVaultDiagnosticsResult diagnostics) {
    if (diagnostics.SaveStrategy.Status == DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback) {
      var plannedExecutionPath = GetExecutionPath(benchmark);
      if (plannedExecutionPath.Contains("DVault provider-neutral fallback path", StringComparison.Ordinal)) {
        return plannedExecutionPath;
      }

      return "DVault provider-neutral fallback path; selectedStrategy=<none>; providerSpecificSaveStrategy=fallback";
    }

    return diagnostics.SaveStrategy.SelectedStrategyName switch {
      "PostgresDataVaultSaveStrategy" => IsNativeStagedProviderBulk(diagnostics.SaveStrategy.StagedProviderBulk)
          ? "DVault PostgreSQL staged bulk save path; transfer=COPY; selectedStrategy=PostgresDataVaultSaveStrategy; " +
              "stagedBulkBoundary=60-plus-operations; smallBatchBoundary=direct-or-UNNEST; cleanupBoundary=temporary-staging-table"
          : "DVault PostgreSQL retained direct or UNNEST save path; transfer=direct-or-UNNEST; " +
              "selectedStrategy=PostgresDataVaultSaveStrategy; stagedBulkBoundary=below-60-operations; cleanupBoundary=no-staging-table",
      "MySqlStagedDataVaultSaveStrategy" =>
          "DVault MySQL staged bulk save path; selectedStrategy=MySqlStagedDataVaultSaveStrategy; " +
          "nativeBulkBoundary=50-plus-operations; " +
          "stagedBulkBoundary=100-plus-satellite-only-or-100-to-303-mixed-operations; " +
          "cleanupBoundary=temporary-staging-tables",
      "MySqlDataVaultSaveStrategy" =>
          "DVault MySQL retained multi-row save path; selectedStrategy=MySqlDataVaultSaveStrategy; " +
          "nativeBulkBoundary=50-plus-operations; " +
          "stagedBulkBoundary=below-100-mixed-operations; largeMixedBoundary=above-303-provider-neutral; " +
          "cleanupBoundary=no-staging-table",
      "SqlServerDataVaultSaveStrategy" =>
          "DVault SQL Server staged native bulk save path; transfer=SqlBulkCopy; selectedStrategy=SqlServerDataVaultSaveStrategy; " +
          "nativeBulkBoundary=100-plus-operations; mixedBatchBoundary=900-plus-operations; cleanupBoundary=temporary-staging-table",
      "OracleDataVaultSaveStrategy" =>
          "DVault Oracle direct optimized save path; selectedStrategy=OracleDataVaultSaveStrategy; " +
          "oracleBulkBoundary=direct-oracle-batching; stagedOracleBulk=not-selected-no-measured-win; cleanupBoundary=direct-provider-transaction",
      "Db2DataVaultSaveStrategy" =>
          "DVault DB2 optimized save path; selectedStrategy=Db2DataVaultSaveStrategy; " +
          "db2SaveBoundary=clean-context-set-based; stagedBulkBoundary=not-supported; cleanupBoundary=direct-provider-transaction",
      "SqliteDataVaultSaveStrategy" =>
          "DVault SQLite optimized path; selectedStrategy=SqliteDataVaultSaveStrategy",
      _ => GetExecutionPath(benchmark),
    };
  }

  private static bool IsNativeStagedProviderBulk(DataVaultStagedProviderBulkDiagnostics? stagedProviderBulk) {
    return stagedProviderBulk?.LifecyclePhase == DataVaultStagedProviderBulkLifecyclePhase.NativeBulkApplication;
  }

  private static string GetSqliteStrategyName(string scenarioName) {
    return scenarioName is "latest-satellite-read" or "pit-as-of-read" or "bridge-traversal-read"
        ? "SqliteDataVaultReadStrategy"
        : "SqliteDataVaultSaveStrategy";
  }

  private static bool IsReadModelScenario(string scenarioName) {
    return scenarioName is "latest-satellite-read" or "pit-as-of-read" or "bridge-traversal-read";
  }

  private static string GetReadExecutionPath(IScenarioBenchmark benchmark) {
    if (benchmark.StrategyFamily == DataVaultBenchmarkHelpers.ProviderNeutralFallbackStrategyFamily) {
      return "DVault provider-neutral fallback path; selectedStrategy=<none>";
    }

    var readShape = GetReadShapeName(benchmark.ScenarioName);
    var strategyName = GetReadStrategyName(benchmark.StrategyFamily, benchmark.ScenarioName);
    var providerName = GetReadProviderDisplayName(benchmark.StrategyFamily);
    if (strategyName is null) {
      return "DVault " + providerName + " provider package latest satellite read path; selectedStrategy=<none>; " +
          "plannedReadStrategy=<none>; providerSpecificReadStrategy=not registered for latest satellite reads; " +
          "readShape=" + readShape;
    }

    return "DVault " + providerName + " optimized " + GetReadShapeDisplayName(benchmark.ScenarioName) +
        " read path; selectedStrategy=" + strategyName +
        "; plannedReadStrategy=" + strategyName +
        "; readShape=" + readShape +
        GetPostgresLatestSatelliteSqlShapeDetail(benchmark);
  }

  private static string GetPostgresLatestSatelliteSqlShapeDetail(IScenarioBenchmark benchmark) {
    return benchmark.StrategyFamily == DataVaultBenchmarkHelpers.PostgresOptimizedStrategyFamily &&
        string.Equals(benchmark.ScenarioName, "latest-satellite-read", StringComparison.Ordinal)
        ? "; latestSatelliteSqlShape=windowed-row-number"
        : string.Empty;
  }

  private static string? GetReadStrategyName(string strategyFamily, string scenarioName) {
    return strategyFamily switch {
      DataVaultBenchmarkHelpers.SqliteOptimizedStrategyFamily => "SqliteDataVaultReadStrategy",
      DataVaultBenchmarkHelpers.PostgresOptimizedStrategyFamily
          when scenarioName is "latest-satellite-read" or "pit-as-of-read" or "bridge-traversal-read" =>
          "PostgresDataVaultReadStrategy",
      DataVaultBenchmarkHelpers.SqlServerOptimizedStrategyFamily when scenarioName is "latest-satellite-read" or "pit-as-of-read" or "bridge-traversal-read" =>
          "SqlServerDataVaultReadStrategy",
      DataVaultBenchmarkHelpers.MySqlOptimizedStrategyFamily when scenarioName is "latest-satellite-read" or "pit-as-of-read" or "bridge-traversal-read" =>
          "MySqlDataVaultReadStrategy",
      DataVaultBenchmarkHelpers.OracleOptimizedStrategyFamily when scenarioName is "latest-satellite-read" or "pit-as-of-read" or "bridge-traversal-read" =>
          "OracleDataVaultReadStrategy",
      DataVaultBenchmarkHelpers.Db2OptimizedStrategyFamily when scenarioName is "latest-satellite-read" or "pit-as-of-read" or "bridge-traversal-read" =>
          "Db2DataVaultReadStrategy",
      _ => null,
    };
  }

  private static string GetReadProviderDisplayName(string strategyFamily) {
    return strategyFamily switch {
      DataVaultBenchmarkHelpers.SqliteOptimizedStrategyFamily => "SQLite",
      DataVaultBenchmarkHelpers.PostgresOptimizedStrategyFamily => "PostgreSQL",
      DataVaultBenchmarkHelpers.SqlServerOptimizedStrategyFamily => "SQL Server",
      DataVaultBenchmarkHelpers.MySqlOptimizedStrategyFamily => "MySQL",
      DataVaultBenchmarkHelpers.OracleOptimizedStrategyFamily => "Oracle",
      DataVaultBenchmarkHelpers.Db2OptimizedStrategyFamily => "DB2",
      _ => "provider-specific",
    };
  }

  private static string GetReadShapeName(string scenarioName) {
    return scenarioName switch {
      "latest-satellite-read" => "LatestSatellite",
      "pit-as-of-read" => "PitAsOf",
      "bridge-traversal-read" => "Bridge",
      _ => "Unknown",
    };
  }

  private static string GetReadShapeDisplayName(string scenarioName) {
    return scenarioName switch {
      "latest-satellite-read" => "latest satellite",
      "pit-as-of-read" => "PIT",
      "bridge-traversal-read" => "bridge",
      _ => "read-model",
    };
  }

  private static string FormatFallbackCauses(IReadOnlyList<DataVaultSaveStrategyFallbackCause> fallbackCauses) {
    if (fallbackCauses.Count == 0) {
      return "none";
    }

    return string.Join("|", fallbackCauses.Select(cause => cause.Kind.ToString()));
  }

  private static string FormatFallbackCauses(IReadOnlyList<DataVaultReadStrategyFallbackCause> fallbackCauses) {
    if (fallbackCauses.Count == 0) {
      return "none";
    }

    return string.Join("|", fallbackCauses.Select(cause => cause.Kind.ToString()));
  }

  private static string FormatSaveStrategyCandidates(
      IReadOnlyList<DataVaultSaveStrategyCandidateDiagnostics> candidates) {
    if (candidates.Count == 0) {
      return "none";
    }

    return string.Join("|", candidates
        .OrderBy(candidate => candidate.Ordinal)
        .Select(candidate => candidate.StrategyName));
  }

  private static string FormatStagedProviderBulk(DataVaultStagedProviderBulkDiagnostics? stagedProviderBulk) {
    if (stagedProviderBulk is null) {
      return string.Empty;
    }

    return "; stagedProviderBulkPhase=" + stagedProviderBulk.LifecyclePhase +
        "; stagedProviderBulkCaveat=" + stagedProviderBulk.ProviderCaveatKind +
        "; stagedProviderBulkOperations=" + stagedProviderBulk.OperationCount.ToString(CultureInfo.InvariantCulture) +
        "; stagedProviderBulkFallbackCauses=" + FormatStagedProviderBulkFallbackCauses(stagedProviderBulk);
  }

  private static string FormatStagedProviderBulkFallbackCauses(
      DataVaultStagedProviderBulkDiagnostics stagedProviderBulk) {
    if (stagedProviderBulk.FallbackCauseKinds.Count == 0) {
      return "none";
    }

    return string.Join("|", stagedProviderBulk.FallbackCauseKinds.Select(cause => cause.ToString()));
  }
}
