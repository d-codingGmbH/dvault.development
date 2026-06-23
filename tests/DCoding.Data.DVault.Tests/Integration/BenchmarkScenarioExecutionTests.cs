using System.Globalization;
using System.Text;
using System.Text.Json;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Benchmarks;
using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class BenchmarkScenarioExecutionTests {
  private const string SqliteProviderName = "SQLite local temporary files";
  private const string PostgresProviderName = "PostgreSQL external provider";
  private const string SqlServerProviderName = "SQL Server external provider";
  private const string MySqlProviderName = "MySQL external provider";
  private const string OracleProviderName = "Oracle external provider";
  private const string Db2ProviderName = "DB2 external provider";
  private const string ProviderEvidenceManifestSchemaVersion = "dvault.provider-evidence.v1";
  private const string BenchmarkCsvHeader = "scenario,provider,baseline,strategyFamily,datasetSize,changeRatio,executionStatus,skipReason,iterations,meanMilliseconds,minMilliseconds,maxMilliseconds,meanAllocatedBytes,minAllocatedBytes,maxAllocatedBytes,executionDetail,persistedOutcome";
  private const string BenchmarkMarkdownHeader = "| Scenario | Provider | Baseline | Strategy family | Dataset size | Change ratio | Execution status | Skip reason | Iterations | Mean ms | Min ms | Max ms | Mean allocated bytes | Min allocated bytes | Max allocated bytes | Execution detail | Persisted outcome |";
  private const string BenchmarkMarkdownSeparator = "| --- | --- | --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- | --- |";

  private static readonly ExpectedBenchmarkRow[] ExpectedRows =
  [
      CompletedSqlite(
          "customer-profile-history",
          "conventional-ef",
          "classic-ef",
          "1 customer, 2 profile states",
          "50% repeat-change history"),
      CompletedSqlite(
          "customer-profile-history",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "1 customer, 2 profile states",
          "50% repeat-change history"),
      CompletedSqlite(
          "customer-profile-history",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "1 customer, 2 profile states",
          "50% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-insert-only",
          "conventional-ef-bulk",
          "classic-ef",
          "100 customers, 1 profile state each",
          "0% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-insert-only",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "100 customers, 1 profile state each",
          "0% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-insert-only",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "100 customers, 1 profile state each",
          "0% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-history",
          "conventional-ef-bulk",
          "classic-ef",
          "100 customers, 10 profile states each",
          "90% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-history",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "100 customers, 10 profile states each",
          "90% repeat-change history"),
      CompletedSqlite(
          "customer-profile-bulk-history",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "100 customers, 10 profile states each",
          "90% repeat-change history"),
      CompletedSqlite(
          "customer-profile-streaming-save",
          "dvault-adddvault-fallback/materialized-explicit-bulk",
          "provider-neutral-dvault-fallback",
          "20 customers, 60 ordered explicit requests",
          "3 profile events per customer with one unchanged replay"),
      CompletedSqlite(
          "customer-profile-streaming-save",
          "dvault-adddvault-fallback/chunked-save-bounded-10",
          "provider-neutral-dvault-fallback",
          "20 customers, 60 ordered explicit requests",
          "3 profile events per customer with one unchanged replay"),
      CompletedSqlite(
          "customer-profile-streaming-save",
          "dvault-adddvault-fallback/async-source-bounded-10",
          "provider-neutral-dvault-fallback",
          "20 customers, 60 ordered explicit requests",
          "3 profile events per customer with one unchanged replay"),
      CompletedSqlite(
          "customer-profile-streaming-save",
          "dvault-adddvault-fallback/chunked-save-bounded-5",
          "provider-neutral-dvault-fallback",
          "20 customers, 60 ordered explicit requests",
          "3 profile events per customer with one unchanged replay"),
      CompletedSqlite(
          "order-product-fulfillment-history",
          "conventional-ef",
          "classic-ef",
          "1 order-product relationship, 2 fulfillment states",
          "50% repeat-change history"),
      CompletedSqlite(
          "order-product-fulfillment-history",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "1 order-product relationship, 2 fulfillment states",
          "50% repeat-change history"),
      CompletedSqlite(
          "order-product-fulfillment-history",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "1 order-product relationship, 2 fulfillment states",
          "50% repeat-change history"),
      CompletedSqlite(
          "latest-satellite-read",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "100 customers, 10 profile states each",
          "90% repeat-change history latest read"),
      CompletedSqlite(
          "latest-satellite-read",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "100 customers, 10 profile states each",
          "90% repeat-change history latest read"),
      CompletedSqlite(
          "pit-as-of-read",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "100 customers, 100 PIT rows, 2 satellite segments",
          "as-of read after latest profile/status snapshots"),
      CompletedSqlite(
          "pit-as-of-read",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "100 customers, 100 PIT rows, 2 satellite segments",
          "as-of read after latest profile/status snapshots"),
      CompletedSqlite(
          "bridge-traversal-read",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "1 hierarchy ancestor with 100 descendant bridge rows",
          "maximum depth 3 of 5"),
      CompletedSqlite(
          "bridge-traversal-read",
          "dvault-adddvaultsqlite-optimized",
          "sqlite-optimized-dvault",
          "1 hierarchy ancestor with 100 descendant bridge rows",
          "maximum depth 3 of 5"),
      CompletedSqlite(
          "compiled-model-startup",
          "dvault-design-model",
          "ef-model-build",
          "1 generated order hub row",
          "runtime model precomputed outside measured operation"),
      CompletedSqlite(
          "compiled-model-startup",
          "dvault-usemodel-runtime-model",
          "ef-usemodel-runtime-model",
          "1 generated order hub row",
          "runtime model precomputed outside measured operation"),
      CompletedSqlite(
          "compiled-query-hub-read",
          "ordinary-ef-query",
          "direct-ef-query",
          "1 generated order hub row",
          "stable shared-type table projection"),
      CompletedSqlite(
          "compiled-query-hub-read",
          "ef-compilequery",
          "compiled-ef-query",
          "1 generated order hub row",
          "stable shared-type table projection"),
      CompletedSqlite(
          "dbcontext-pooling-dvault-operation",
          "adddbcontext",
          "non-pooled-dvault-context",
          "1 generated order hub row",
          "fixed metadata source and options-only context"),
      CompletedSqlite(
          "dbcontext-pooling-dvault-operation",
          "adddbcontextpool",
          "pooled-dvault-context",
          "1 generated order hub row",
          "fixed metadata source and options-only context"),
      SkippedExternal(
          PostgresProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "300 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReason),
      SkippedExternal(
          PostgresProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultpostgres-direct-or-unnest",
          "postgres-optimized-dvault",
          "18 order-product pairs, 3 fulfillment satellite operations",
          "staged-ineligible provider-native batch below staged bulk boundary",
          NotConfiguredSkipReason),
      SkippedExternal(
          PostgresProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultpostgres-optimized",
          "postgres-optimized-dvault",
          "300 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReason),
      SkippedExternal(
          SqlServerProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "300 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          SqlServerProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultsqlserver-optimized",
          "sqlserver-optimized-dvault",
          "300 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          MySqlProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "300 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          MySqlProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultmysql-multi-row",
          "mysql-optimized-dvault",
          "18 order-product pairs, 3 fulfillment satellite operations",
          "staged-ineligible provider-native batch below staged bulk boundary",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          MySqlProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultmysql-staged",
          "mysql-optimized-dvault",
          "50 order-product pairs, 3 fulfillment satellite operations",
          "staged-eligible MySQL mixed hub/link/satellite bulk batch inside provider window",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          MySqlProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultmysql-optimized",
          "mysql-optimized-dvault",
          "300 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          OracleProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "300 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          OracleProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultoracle-optimized",
          "oracle-optimized-dvault",
          "300 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          Db2ProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvault-fallback",
          "provider-neutral-dvault-fallback",
          "300 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          Db2ProviderName,
          "provider-native-bulk-ingestion",
          "dvault-adddvaultdb2-optimized",
          "db2-optimized-dvault",
          "300 order-product pairs, 3 fulfillment satellite operations",
          "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          PostgresProviderName,
          "latest-satellite-read",
          "dvault-adddvaultpostgres-optimized",
          "postgres-optimized-dvault",
          "100 customers, 10 profile states each",
          "90% repeat-change history latest read",
          NotConfiguredSkipReason),
      SkippedExternal(
          PostgresProviderName,
          "pit-as-of-read",
          "dvault-adddvaultpostgres-optimized",
          "postgres-optimized-dvault",
          "100 customers, 100 PIT rows, 2 satellite segments",
          "as-of read after latest profile/status snapshots",
          NotConfiguredSkipReason),
      SkippedExternal(
          PostgresProviderName,
          "bridge-traversal-read",
          "dvault-adddvaultpostgres-optimized",
          "postgres-optimized-dvault",
          "1 hierarchy ancestor with 100 descendant bridge rows",
          "maximum depth 3 of 5",
          NotConfiguredSkipReason),
      SkippedExternal(
          SqlServerProviderName,
          "latest-satellite-read",
          "dvault-adddvaultsqlserver-optimized",
          "sqlserver-optimized-dvault",
          "100 customers, 10 profile states each",
          "90% repeat-change history latest read",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          SqlServerProviderName,
          "pit-as-of-read",
          "dvault-adddvaultsqlserver-optimized",
          "sqlserver-optimized-dvault",
          "100 customers, 100 PIT rows, 2 satellite segments",
          "as-of read after latest profile/status snapshots",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          SqlServerProviderName,
          "bridge-traversal-read",
          "dvault-adddvaultsqlserver-optimized",
          "sqlserver-optimized-dvault",
          "1 hierarchy ancestor with 100 descendant bridge rows",
          "maximum depth 3 of 5",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          MySqlProviderName,
          "latest-satellite-read",
          "dvault-adddvaultmysql-optimized",
          "mysql-optimized-dvault",
          "100 customers, 10 profile states each",
          "90% repeat-change history latest read",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          MySqlProviderName,
          "pit-as-of-read",
          "dvault-adddvaultmysql-optimized",
          "mysql-optimized-dvault",
          "100 customers, 100 PIT rows, 2 satellite segments",
          "as-of read after latest profile/status snapshots",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          MySqlProviderName,
          "bridge-traversal-read",
          "dvault-adddvaultmysql-optimized",
          "mysql-optimized-dvault",
          "1 hierarchy ancestor with 100 descendant bridge rows",
          "maximum depth 3 of 5",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          OracleProviderName,
          "latest-satellite-read",
          "dvault-adddvaultoracle-optimized",
          "oracle-optimized-dvault",
          "100 customers, 10 profile states each",
          "90% repeat-change history latest read",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          OracleProviderName,
          "pit-as-of-read",
          "dvault-adddvaultoracle-optimized",
          "oracle-optimized-dvault",
          "100 customers, 100 PIT rows, 2 satellite segments",
          "as-of read after latest profile/status snapshots",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          OracleProviderName,
          "bridge-traversal-read",
          "dvault-adddvaultoracle-optimized",
          "oracle-optimized-dvault",
          "1 hierarchy ancestor with 100 descendant bridge rows",
          "maximum depth 3 of 5",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          Db2ProviderName,
          "latest-satellite-read",
          "dvault-adddvaultdb2-optimized",
          "db2-optimized-dvault",
          "100 customers, 10 profile states each",
          "90% repeat-change history latest read",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          Db2ProviderName,
          "pit-as-of-read",
          "dvault-adddvaultdb2-optimized",
          "db2-optimized-dvault",
          "100 customers, 100 PIT rows, 2 satellite segments",
          "as-of read after latest profile/status snapshots",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable)),
      SkippedExternal(
          Db2ProviderName,
          "bridge-traversal-read",
          "dvault-adddvaultdb2-optimized",
          "db2-optimized-dvault",
          "1 hierarchy ancestor with 100 descendant bridge rows",
          "maximum depth 3 of 5",
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable)),
  ];

  private static readonly ExpectedPerformanceProfile[] ExpectedPerformanceProfiles =
  [
      new(DataVaultPerformanceProfileCategory.SmallAppLocalVault, "Small app-local vault", "Small App-Local Vault"),
      new(DataVaultPerformanceProfileCategory.MediumChunkedIngestion, "Medium chunked ingestion", "Medium Chunked Ingestion"),
      new(DataVaultPerformanceProfileCategory.StagedProviderIngestion, "Staged provider ingestion", "Staged Provider Ingestion"),
      new(DataVaultPerformanceProfileCategory.ReadModelHeavy, "Read-model heavy", "Read-Model Heavy"),
  ];

  private static readonly ExpectedGuidanceRow[] ExpectedPerformanceGuidanceRows =
  [
      new("customer-profile-history", "dvault-adddvault-fallback"),
      new("customer-profile-history", "dvault-adddvaultsqlite-optimized"),
      new("customer-profile-bulk-insert-only", "dvault-adddvault-fallback"),
      new("customer-profile-bulk-insert-only", "dvault-adddvaultsqlite-optimized"),
      new("customer-profile-bulk-history", "dvault-adddvault-fallback"),
      new("customer-profile-bulk-history", "dvault-adddvaultsqlite-optimized"),
      new("customer-profile-streaming-save", "dvault-adddvault-fallback/materialized-explicit-bulk"),
      new("customer-profile-streaming-save", "dvault-adddvault-fallback/chunked-save-bounded-10"),
      new("customer-profile-streaming-save", "dvault-adddvault-fallback/async-source-bounded-10"),
      new("customer-profile-streaming-save", "dvault-adddvault-fallback/chunked-save-bounded-5"),
      new("latest-satellite-read", "dvault-adddvault-fallback"),
      new("latest-satellite-read", "dvault-adddvaultsqlite-optimized"),
      new("pit-as-of-read", "dvault-adddvault-fallback"),
      new("pit-as-of-read", "dvault-adddvaultsqlite-optimized"),
      new("bridge-traversal-read", "dvault-adddvault-fallback"),
      new("bridge-traversal-read", "dvault-adddvaultsqlite-optimized"),
  ];

  private static readonly ExpectedProviderGuidanceRow[] ExpectedProviderGuidanceRows =
  [
      new(PostgresProviderName, "dvault-adddvaultpostgres-direct-or-unnest", ["stagedBulkBoundary=below-60-operations", "cleanupBoundary=no-staging-table"]),
      new(PostgresProviderName, "dvault-adddvaultpostgres-optimized", ["transfer=COPY", "stagedBulkBoundary=60-plus-operations", "smallBatchBoundary=direct-or-UNNEST"]),
      new(SqlServerProviderName, "dvault-adddvaultsqlserver-optimized", ["transfer=SqlBulkCopy", "nativeBulkBoundary=100-plus-operations", "mixedBatchBoundary=900-plus-operations"]),
      new(MySqlProviderName, "dvault-adddvaultmysql-multi-row", ["selectedStrategy=MySqlDataVaultSaveStrategy", "stagedBulkBoundary=below-100-operations"]),
      new(MySqlProviderName, "dvault-adddvaultmysql-staged", ["selectedStrategy=MySqlStagedDataVaultSaveStrategy", "stagedBulkBoundary=100-plus-satellite-only-or-100-to-303-mixed-operations", "cleanupBoundary=temporary-staging-tables"]),
      new(MySqlProviderName, "dvault-adddvaultmysql-optimized", ["selectedStrategy=<none>", "mysqlMixedBatchBoundary=above-303-provider-neutral"]),
      new(OracleProviderName, "dvault-adddvaultoracle-optimized", ["selectedStrategy=OracleDataVaultSaveStrategy", "stagedOracleBulk=not-selected-no-measured-win"]),
      new(Db2ProviderName, "dvault-adddvaultdb2-optimized", ["selectedStrategy=Db2DataVaultSaveStrategy", "db2SaveBoundary=clean-context-set-based", "stagedBulkBoundary=not-supported"]),
  ];

  private static readonly ExpectedProviderReadRow[] ExpectedProviderReadRows =
  [
      new(PostgresProviderName, "latest-satellite-read", "dvault-adddvaultpostgres-optimized", ["readShape=LatestSatellite", "selectedStrategy=PostgresDataVaultReadStrategy", "plannedReadStrategy=PostgresDataVaultReadStrategy", "latestSatelliteSqlShape=windowed-row-number"]),
      new(PostgresProviderName, "pit-as-of-read", "dvault-adddvaultpostgres-optimized", ["readShape=PitAsOf", "selectedStrategy=PostgresDataVaultReadStrategy", "plannedReadStrategy=PostgresDataVaultReadStrategy"]),
      new(PostgresProviderName, "bridge-traversal-read", "dvault-adddvaultpostgres-optimized", ["readShape=Bridge", "selectedStrategy=PostgresDataVaultReadStrategy", "plannedReadStrategy=PostgresDataVaultReadStrategy"]),
      new(SqlServerProviderName, "latest-satellite-read", "dvault-adddvaultsqlserver-optimized", ["readShape=LatestSatellite", "selectedStrategy=SqlServerDataVaultReadStrategy", "plannedReadStrategy=SqlServerDataVaultReadStrategy"]),
      new(SqlServerProviderName, "pit-as-of-read", "dvault-adddvaultsqlserver-optimized", ["readShape=PitAsOf", "selectedStrategy=SqlServerDataVaultReadStrategy", "plannedReadStrategy=SqlServerDataVaultReadStrategy"]),
      new(SqlServerProviderName, "bridge-traversal-read", "dvault-adddvaultsqlserver-optimized", ["readShape=Bridge", "selectedStrategy=SqlServerDataVaultReadStrategy", "plannedReadStrategy=SqlServerDataVaultReadStrategy"]),
      new(MySqlProviderName, "latest-satellite-read", "dvault-adddvaultmysql-optimized", ["readShape=LatestSatellite", "selectedStrategy=MySqlDataVaultReadStrategy", "plannedReadStrategy=MySqlDataVaultReadStrategy"]),
      new(MySqlProviderName, "pit-as-of-read", "dvault-adddvaultmysql-optimized", ["readShape=PitAsOf", "selectedStrategy=MySqlDataVaultReadStrategy", "plannedReadStrategy=MySqlDataVaultReadStrategy"]),
      new(MySqlProviderName, "bridge-traversal-read", "dvault-adddvaultmysql-optimized", ["readShape=Bridge", "selectedStrategy=MySqlDataVaultReadStrategy", "plannedReadStrategy=MySqlDataVaultReadStrategy"]),
      new(OracleProviderName, "latest-satellite-read", "dvault-adddvaultoracle-optimized", ["readShape=LatestSatellite", "selectedStrategy=OracleDataVaultReadStrategy", "plannedReadStrategy=OracleDataVaultReadStrategy"]),
      new(OracleProviderName, "pit-as-of-read", "dvault-adddvaultoracle-optimized", ["readShape=PitAsOf", "selectedStrategy=OracleDataVaultReadStrategy", "plannedReadStrategy=OracleDataVaultReadStrategy"]),
      new(OracleProviderName, "bridge-traversal-read", "dvault-adddvaultoracle-optimized", ["readShape=Bridge", "selectedStrategy=OracleDataVaultReadStrategy", "plannedReadStrategy=OracleDataVaultReadStrategy"]),
      new(Db2ProviderName, "latest-satellite-read", "dvault-adddvaultdb2-optimized", ["readShape=LatestSatellite", "selectedStrategy=Db2DataVaultReadStrategy", "plannedReadStrategy=Db2DataVaultReadStrategy"]),
      new(Db2ProviderName, "pit-as-of-read", "dvault-adddvaultdb2-optimized", ["readShape=PitAsOf", "selectedStrategy=Db2DataVaultReadStrategy", "plannedReadStrategy=Db2DataVaultReadStrategy"]),
      new(Db2ProviderName, "bridge-traversal-read", "dvault-adddvaultdb2-optimized", ["readShape=Bridge", "selectedStrategy=Db2DataVaultReadStrategy", "plannedReadStrategy=Db2DataVaultReadStrategy"]),
  ];

  private static readonly string[] RegressionBudgetRules =
  [
      "The targeted metric must improve or hold.",
      "For required SQLite rows, non-target mean-time and allocation regressions above 5% fail by default.",
      "For configured optional-provider rows, regressions above 10% must be explicitly called out and justified.",
  ];

  [Fact]
  public async Task LocalBenchmarkRunnerExecutesCustomerAndOrderComparisonsThroughSqlite() {
    var text = await RunBenchmarkAndCaptureOutputAsync(new BenchmarkOptions(1, 0)).ConfigureAwait(false);

    Assert.Contains("Required provider: " + SqliteProviderName, text);
    Assert.Contains(PostgresProviderName + ": skipped - " + NotConfiguredSkipReason, text);

    foreach (var expectedRow in ExpectedRows) {
      Assert.Contains(CreateMarkdownRowPrefix(expectedRow), text);
    }

    Assert.Contains("2 customer profile history rows for C-100", text);
    Assert.Contains("1 customer hub row and 2 profile satellite rows for C-100", text);
    Assert.Contains("100 customer profile history rows for 100 customers", text);
    Assert.Contains("100 customer hubs and 100 profile satellite rows", text);
    Assert.Contains("1000 customer profile history rows for 100 customers", text);
    Assert.Contains("100 customer hubs and 1000 profile satellite rows", text);
    Assert.Contains("20 customer hubs and 40 profile satellite rows from 60 materialized explicit requests", text);
    Assert.Contains("20 customer hubs and 40 profile satellite rows from 60 explicit requests across 6 chunks of 10", text);
    Assert.Contains("20 customer hubs and 40 profile satellite rows from 60 async-streamed explicit requests across 6 chunks of 10", text);
    Assert.Contains("20 customer hubs and 40 profile satellite rows from 60 explicit requests across 12 chunks of 5", text);
    Assert.Contains(
        "1 order, 1 product, 1 relationship, and 2 fulfillment history rows for O-1000/SKU-COFFEE",
        text);
    Assert.Contains(
        "1 order hub, 1 product hub, 1 link, and 2 fulfillment satellite rows for O-1000/SKU-COFFEE",
        text);
    Assert.Contains("100 latest profile satellite rows read from 1000 seeded profile states", text);
    Assert.Contains("100 PIT as-of rows read across profile and status satellite snapshots", text);
    Assert.Contains("60 bridge traversal rows read from 100 seeded hierarchy rows", text);
    Assert.Contains("1 generated order hub row read through ordinary DVault model building", text);
    Assert.Contains("1 generated order hub row read through precomputed UseModel(runtimeModel)", text);
    Assert.Contains("1 generated order hub row read through equivalent ordinary EF projection", text);
    Assert.Contains("1 generated order hub row read through EF.CompileQuery stable projection", text);
    Assert.Contains("1 generated order hub row saved and read through AddDbContext fixed-model configuration", text);
    Assert.Contains("1 generated order hub row saved and read through AddDbContextPool fixed-model configuration", text);
    Assert.Contains("Recorded " + ExpectedRows.Length.ToString(CultureInfo.InvariantCulture) + " benchmark report rows.", text);
    Assert.Contains("Executed " + ExpectedCompletedRowCount.ToString(CultureInfo.InvariantCulture) + " benchmark report rows.", text);
    Assert.Contains("Skipped " + ExpectedSkippedRowCount.ToString(CultureInfo.InvariantCulture) + " benchmark report rows.", text);
  }

  [Fact]
  public async Task LocalBenchmarkRunnerEmitsDocumentationArtifactsFromOneRun() {
    var artifactDirectory = Path.Combine(
        Path.GetTempPath(),
        "DVaultBenchmarkArtifacts-" + Guid.NewGuid().ToString("N"));

    try {
      var text = await RunBenchmarkAndCaptureOutputAsync(new BenchmarkOptions(1, 0, artifactDirectory))
          .ConfigureAwait(false);

      Assert.Contains("Wrote benchmark artifacts:", text);

      var markdownPath = Path.Combine(artifactDirectory, "benchmark-summary.md");
      var csvPath = Path.Combine(artifactDirectory, "benchmark-summary.csv");
      var jsonPath = Path.Combine(artifactDirectory, "benchmark-summary.json");

      Assert.True(File.Exists(markdownPath));
      Assert.True(File.Exists(csvPath));
      Assert.True(File.Exists(jsonPath));

      var markdown = await File.ReadAllTextAsync(markdownPath).ConfigureAwait(false);
      Assert.Contains("# DVault Benchmark Summary", markdown);
      Assert.Contains("- Required provider: " + SqliteProviderName, markdown);
      Assert.Contains("- Optional PostgreSQL provider: " + PostgresProviderName, markdown);
      Assert.Contains("- PostgreSQL execution status: skipped", markdown);
      Assert.Contains("- PostgreSQL skip reason: " + NotConfiguredSkipReason, markdown);
      Assert.Contains("- Iterations: 1", markdown);
      Assert.Contains("- Warmup iterations: 0", markdown);
      Assert.Contains("- Load timestamp storage: ProviderDefault", markdown);
      Assert.Contains("- Provider filter: all", markdown);
      Assert.Contains("- Hash key variants: sha256-v1-hex", markdown);
      Assert.Contains("- OS description: ", markdown);
      Assert.Contains("- OS architecture: ", markdown);
      Assert.Contains("- Process architecture: ", markdown);
      Assert.Contains("- Processor count: ", markdown);
      Assert.Contains("- .NET runtime version: ", markdown);
      Assert.Contains("| Scenario | Provider | Baseline | Strategy family | Dataset size | Change ratio | Execution status | Skip reason | Iterations | Mean ms | Min ms | Max ms | Mean allocated bytes | Min allocated bytes | Max allocated bytes | Execution detail | Persisted outcome |", markdown);

      foreach (var expectedRow in ExpectedRows) {
        Assert.Contains(CreateMarkdownRowPrefix(expectedRow), markdown);
      }

      var csv = await File.ReadAllTextAsync(csvPath).ConfigureAwait(false);
      var csvLines = csv.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      Assert.Equal(ExpectedRows.Length + 1, csvLines.Length);
      Assert.Equal(
          "scenario,provider,baseline,strategyFamily,datasetSize,changeRatio,executionStatus,skipReason,iterations,meanMilliseconds,minMilliseconds,maxMilliseconds,meanAllocatedBytes,minAllocatedBytes,maxAllocatedBytes,executionDetail,persistedOutcome",
          csvLines[0]);

      foreach (var expectedRow in ExpectedRows) {
        Assert.Contains(
            csvLines,
            line => line.StartsWith(CreateCsvRowPrefix(expectedRow), StringComparison.Ordinal));
      }

      using var json = JsonDocument.Parse(await File.ReadAllTextAsync(jsonPath).ConfigureAwait(false));
      var context = json.RootElement.GetProperty("context");
      Assert.Equal(SqliteProviderName, context.GetProperty("provider").GetString());
      Assert.Equal(PostgresProviderName, context.GetProperty("optionalPostgresProvider").GetString());
      Assert.Equal("skipped", context.GetProperty("postgresExecutionStatus").GetString());
      Assert.Equal(NotConfiguredSkipReason, context.GetProperty("postgresSkipReason").GetString());
      Assert.Equal(1, context.GetProperty("iterations").GetInt32());
      Assert.Equal(0, context.GetProperty("warmupIterations").GetInt32());
      Assert.Equal("ProviderDefault", context.GetProperty("loadTimestampStorage").GetString());
      Assert.Equal("all", context.GetProperty("providerFilter").GetString());
      AssertHashKeyVariantContext(
          ParseHashKeyVariantContext(context.GetProperty("hashKeyVariants")),
          [BenchmarkHashKeyVariant.Default]);
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("osDescription").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("osArchitecture").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("processArchitecture").GetString()));
      Assert.True(context.GetProperty("processorCount").GetInt32() > 0);
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("dotNetRuntimeDescription").GetString()));
      Assert.False(string.IsNullOrWhiteSpace(context.GetProperty("dotNetRuntimeVersion").GetString()));
      var optionalProviders = context.GetProperty("optionalProviders").EnumerateArray().ToArray();
      Assert.Equal(5, optionalProviders.Length);
      AssertOptionalProviderContext(
          optionalProviders,
          PostgresProviderName,
          BenchmarkExternalProviderDefinitions.Postgres.ConnectionStringEnvironmentVariable,
          NotConfiguredSkipReason);
      AssertOptionalProviderContext(
          optionalProviders,
          SqlServerProviderName,
          BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable,
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable));
      AssertOptionalProviderContext(
          optionalProviders,
          MySqlProviderName,
          BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable,
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable));
      AssertOptionalProviderContext(
          optionalProviders,
          OracleProviderName,
          BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable,
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable));
      AssertOptionalProviderContext(
          optionalProviders,
          Db2ProviderName,
          BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable,
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable));

      var results = json.RootElement.GetProperty("results").EnumerateArray().ToArray();
      Assert.Equal(ExpectedRows.Length, results.Length);

      foreach (var expectedRow in ExpectedRows) {
        var matchingResults = results.Where(result =>
            result.GetProperty("scenarioName").GetString() == expectedRow.ScenarioName &&
            result.GetProperty("provider").GetString() == expectedRow.ProviderName &&
            result.GetProperty("baselineName").GetString() == expectedRow.BaselineName &&
            result.GetProperty("strategyFamily").GetString() == expectedRow.StrategyFamily)
            .ToArray();

        var result = Assert.Single(matchingResults);
        Assert.Equal(expectedRow.DatasetSize, result.GetProperty("datasetSize").GetString());
        Assert.Equal(expectedRow.ChangeRatio, result.GetProperty("changeRatio").GetString());
        Assert.Equal(expectedRow.ExecutionStatus, result.GetProperty("executionStatus").GetString());
        Assert.Equal(expectedRow.SkipReason, result.GetProperty("skipReason").GetString());
        Assert.Equal(expectedRow.Iterations, result.GetProperty("iterations").GetInt32());
        var executionDetail = result.GetProperty("executionDetail").GetString();
        Assert.False(string.IsNullOrWhiteSpace(executionDetail));
        Assert.Contains("scenario=" + expectedRow.ScenarioName, executionDetail);
        if (expectedRow.StrategyFamily.EndsWith("-optimized-dvault", StringComparison.Ordinal)) {
          Assert.Contains("selectedStrategy=", executionDetail);
        }

        if (expectedRow.ExecutionStatus == "skipped") {
          Assert.Equal(JsonValueKind.Null, result.GetProperty("meanMilliseconds").ValueKind);
          Assert.Equal(JsonValueKind.Null, result.GetProperty("minMilliseconds").ValueKind);
          Assert.Equal(JsonValueKind.Null, result.GetProperty("maxMilliseconds").ValueKind);
          Assert.Equal(JsonValueKind.Null, result.GetProperty("meanAllocatedBytes").ValueKind);
          Assert.Equal(JsonValueKind.Null, result.GetProperty("minAllocatedBytes").ValueKind);
          Assert.Equal(JsonValueKind.Null, result.GetProperty("maxAllocatedBytes").ValueKind);
          Assert.Equal("not executed", result.GetProperty("persistedOutcome").GetString());
        }
        else {
          Assert.True(result.GetProperty("meanAllocatedBytes").GetDouble() >= 0);
          Assert.True(result.GetProperty("minAllocatedBytes").GetInt64() >= 0);
          Assert.True(result.GetProperty("maxAllocatedBytes").GetInt64() >= 0);
        }
      }

      var chunkedStreamingResults = results
          .Where(result =>
              result.GetProperty("scenarioName").GetString() == "customer-profile-streaming-save" &&
              result.GetProperty("baselineName").GetString()?.Contains("/chunked-save-bounded-", StringComparison.Ordinal) == true)
          .ToArray();
      Assert.Equal(2, chunkedStreamingResults.Length);
      foreach (var result in chunkedStreamingResults) {
        var executionDetail = result.GetProperty("executionDetail").GetString();
        Assert.Contains("savePath=IDataVaultSaveService.SaveAsync(ChunkedRequest)", executionDetail);
        Assert.Contains("chunkBoundary=bounded request chunks", executionDetail);
        Assert.Contains("chunkSize=", executionDetail);
        Assert.Contains("processedChunkCount=", executionDetail);
        Assert.Contains("retainedStateHighWater=", executionDetail);
      }

      var asyncStreamingResult = Assert.Single(results.Where(result =>
          result.GetProperty("scenarioName").GetString() == "customer-profile-streaming-save" &&
          result.GetProperty("baselineName").GetString() == "dvault-adddvault-fallback/async-source-bounded-10"));
      var asyncExecutionDetail = asyncStreamingResult.GetProperty("executionDetail").GetString();
      Assert.Contains(
          "savePath=IDataVaultSaveService.SaveAsync(IAsyncEnumerable<DataVaultSaveChunk>)",
          asyncExecutionDetail);
      Assert.Contains("operationKind=ChunkedRequest", asyncExecutionDetail);
      Assert.Contains("chunkBoundary=async bounded request chunks", asyncExecutionDetail);
      Assert.Contains("chunkSize=10", asyncExecutionDetail);
      Assert.Contains("chunkCount=6", asyncExecutionDetail);
      Assert.Contains("processedChunkCount=6", asyncExecutionDetail);
      Assert.Contains("retainedStateHighWater=", asyncExecutionDetail);
      Assert.Contains("sourceShape=IAsyncEnumerable<DataVaultSaveChunk>", asyncExecutionDetail);

      var sqlServerStagedBulkResult = Assert.Single(results.Where(result =>
          result.GetProperty("scenarioName").GetString() == "provider-native-bulk-ingestion" &&
          result.GetProperty("provider").GetString() == SqlServerProviderName &&
          result.GetProperty("baselineName").GetString() == "dvault-adddvaultsqlserver-optimized"));
      var sqlServerExecutionDetail = sqlServerStagedBulkResult.GetProperty("executionDetail").GetString();
      Assert.Contains("DVault SQL Server staged native bulk save path", sqlServerExecutionDetail);
      Assert.Contains("transfer=SqlBulkCopy", sqlServerExecutionDetail);
      Assert.Equal(
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable),
          sqlServerStagedBulkResult.GetProperty("skipReason").GetString());

      var postgresStagedBulkResult = Assert.Single(results.Where(result =>
          result.GetProperty("scenarioName").GetString() == "provider-native-bulk-ingestion" &&
          result.GetProperty("provider").GetString() == PostgresProviderName &&
          result.GetProperty("baselineName").GetString() == "dvault-adddvaultpostgres-optimized"));
      var postgresExecutionDetail = postgresStagedBulkResult.GetProperty("executionDetail").GetString();
      Assert.Contains("DVault PostgreSQL staged bulk save path", postgresExecutionDetail);
      Assert.Contains("transfer=COPY", postgresExecutionDetail);
      Assert.Contains("stagedBulkBoundary=60-plus-operations", postgresExecutionDetail);
      Assert.Contains("smallBatchBoundary=direct-or-UNNEST", postgresExecutionDetail);
      Assert.Equal(
          NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Postgres.ConnectionStringEnvironmentVariable),
          postgresStagedBulkResult.GetProperty("skipReason").GetString());

      var postgresRetainedPathResult = Assert.Single(results.Where(result =>
          result.GetProperty("scenarioName").GetString() == "provider-native-bulk-ingestion" &&
          result.GetProperty("provider").GetString() == PostgresProviderName &&
          result.GetProperty("baselineName").GetString() == "dvault-adddvaultpostgres-direct-or-unnest"));
      var postgresRetainedPathExecutionDetail = postgresRetainedPathResult.GetProperty("executionDetail").GetString();
      Assert.Contains("DVault PostgreSQL retained direct or UNNEST save path", postgresRetainedPathExecutionDetail);
      Assert.Contains("stagedBulkBoundary=below-60-operations", postgresRetainedPathExecutionDetail);
      Assert.Contains("cleanupBoundary=no-staging-table", postgresRetainedPathExecutionDetail);

      var mySqlRetainedPathResult = Assert.Single(results.Where(result =>
          result.GetProperty("scenarioName").GetString() == "provider-native-bulk-ingestion" &&
          result.GetProperty("provider").GetString() == MySqlProviderName &&
          result.GetProperty("baselineName").GetString() == "dvault-adddvaultmysql-multi-row"));
      var mySqlRetainedPathExecutionDetail = mySqlRetainedPathResult.GetProperty("executionDetail").GetString();
      Assert.Contains("DVault MySQL retained multi-row save path", mySqlRetainedPathExecutionDetail);
      Assert.Contains("selectedStrategy=MySqlDataVaultSaveStrategy", mySqlRetainedPathExecutionDetail);
      Assert.Contains("stagedBulkBoundary=below-100-operations", mySqlRetainedPathExecutionDetail);

      var mySqlStagedBulkResult = Assert.Single(results.Where(result =>
          result.GetProperty("scenarioName").GetString() == "provider-native-bulk-ingestion" &&
          result.GetProperty("provider").GetString() == MySqlProviderName &&
          result.GetProperty("baselineName").GetString() == "dvault-adddvaultmysql-staged"));
      var mySqlStagedBulkExecutionDetail = mySqlStagedBulkResult.GetProperty("executionDetail").GetString();
      Assert.Contains("DVault MySQL staged bulk save path", mySqlStagedBulkExecutionDetail);
      Assert.Contains("selectedStrategy=MySqlStagedDataVaultSaveStrategy", mySqlStagedBulkExecutionDetail);
      Assert.Contains("stagedBulkBoundary=100-plus-satellite-only-or-100-to-303-mixed-operations", mySqlStagedBulkExecutionDetail);
      Assert.Contains("cleanupBoundary=temporary-staging-tables", mySqlStagedBulkExecutionDetail);

      var mySqlLargeMixedMultiRowResult = Assert.Single(results.Where(result =>
          result.GetProperty("scenarioName").GetString() == "provider-native-bulk-ingestion" &&
          result.GetProperty("provider").GetString() == MySqlProviderName &&
          result.GetProperty("baselineName").GetString() == "dvault-adddvaultmysql-optimized"));
      var mySqlLargeMixedMultiRowExecutionDetail = mySqlLargeMixedMultiRowResult.GetProperty("executionDetail").GetString();
      Assert.Contains("DVault provider-neutral fallback path", mySqlLargeMixedMultiRowExecutionDetail);
      Assert.Contains("selectedStrategy=<none>", mySqlLargeMixedMultiRowExecutionDetail);
      Assert.Contains("mysqlMixedBatchBoundary=above-303-provider-neutral", mySqlLargeMixedMultiRowExecutionDetail);

      var oracleDirectBulkResult = Assert.Single(results.Where(result =>
          result.GetProperty("scenarioName").GetString() == "provider-native-bulk-ingestion" &&
          result.GetProperty("provider").GetString() == OracleProviderName &&
          result.GetProperty("baselineName").GetString() == "dvault-adddvaultoracle-optimized"));
      var oracleDirectBulkExecutionDetail = oracleDirectBulkResult.GetProperty("executionDetail").GetString();
      Assert.Contains("DVault Oracle direct optimized save path", oracleDirectBulkExecutionDetail);
      Assert.Contains("stagedOracleBulk=not-selected-no-measured-win", oracleDirectBulkExecutionDetail);

      var db2OptimizedResult = Assert.Single(results.Where(result =>
          result.GetProperty("scenarioName").GetString() == "provider-native-bulk-ingestion" &&
          result.GetProperty("provider").GetString() == Db2ProviderName &&
          result.GetProperty("baselineName").GetString() == "dvault-adddvaultdb2-optimized"));
      var db2OptimizedExecutionDetail = db2OptimizedResult.GetProperty("executionDetail").GetString();
      Assert.Contains("DVault DB2 optimized save path", db2OptimizedExecutionDetail);
      Assert.Contains("selectedStrategy=Db2DataVaultSaveStrategy", db2OptimizedExecutionDetail);
      Assert.Contains("stagedBulkBoundary=not-supported", db2OptimizedExecutionDetail);
    }
    finally {
      if (Directory.Exists(artifactDirectory)) {
        Directory.Delete(artifactDirectory, recursive: true);
      }
    }
  }

  [Fact]
  public void SaveStrategyExecutionDetailsUseSelectedProviderPathFromDiagnostics() {
    var postgresBenchmark = new BenchmarkExecutionDetailTestBenchmark(
        "customer-profile-scale-10x1",
        PostgresProviderName,
        "dvault-adddvaultpostgres-optimized",
        "postgres-optimized-dvault");
    var postgresDiagnostics = CreateDiagnosticsResult(new DataVaultSaveStrategyDiagnostics(
        DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected,
        KnownProviderNames.Postgres,
        "PostgresDataVaultSaveStrategy",
        100,
        [
            new DataVaultSaveStrategyCandidateDiagnostics(
                0,
                "PostgresDataVaultSaveStrategy",
                100,
                true,
                []),
        ],
        []) {
      StagedProviderBulk = new DataVaultStagedProviderBulkDiagnostics(
          DataVaultStagedProviderBulkLifecyclePhase.Declined,
          DataVaultStagedProviderBulkProviderCaveatKind.UnsupportedShape,
          requestCount: 1,
          hubOperationCount: 0,
          linkOperationCount: 0,
          satelliteOperationCount: 10,
          [DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkUnsupportedShape]),
    });

    var postgresDetail = BenchmarkExecutionDetails.CreateSaveStrategyDetail(
        postgresBenchmark,
        postgresDiagnostics,
        requestCount: 1,
        hubOperationCount: 0,
        linkOperationCount: 0,
        satelliteOperationCount: 10);

    Assert.Contains("executionPath=DVault PostgreSQL retained direct or UNNEST save path", postgresDetail);
    Assert.Contains("selectedStrategy=PostgresDataVaultSaveStrategy", postgresDetail);
    Assert.Contains("stagedProviderBulkPhase=Declined", postgresDetail);
    Assert.Contains("stagedProviderBulkFallbackCauses=StagedProviderBulkUnsupportedShape", postgresDetail);
    Assert.DoesNotContain("executionPath=DVault PostgreSQL staged bulk save path", postgresDetail);
    Assert.DoesNotContain("transfer=COPY", postgresDetail);

    var mySqlBenchmark = new BenchmarkExecutionDetailTestBenchmark(
        "customer-profile-scale-10x10",
        MySqlProviderName,
        "dvault-adddvaultmysql-optimized",
        "mysql-optimized-dvault");
    var mySqlDiagnostics = CreateDiagnosticsResult(new DataVaultSaveStrategyDiagnostics(
        DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback,
        KnownProviderNames.MySqlPomelo,
        SelectedStrategyName: null,
        SelectedStrategyPriority: null,
        [
            new DataVaultSaveStrategyCandidateDiagnostics(
                0,
                "MySqlStagedDataVaultSaveStrategy",
                110,
                false,
                [
                    new DataVaultSaveStrategyFallbackCause(
                        DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback,
                        "tiny satellite history batch"),
                ]),
        ],
        [
            new DataVaultSaveStrategyFallbackCause(
                DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback,
                "tiny satellite history batch"),
        ]));

    var mySqlDetail = BenchmarkExecutionDetails.CreateSaveStrategyDetail(
        mySqlBenchmark,
        mySqlDiagnostics,
        requestCount: 10,
        hubOperationCount: 0,
        linkOperationCount: 0,
        satelliteOperationCount: 100);

    Assert.Contains("executionPath=DVault provider-neutral fallback path", mySqlDetail);
    Assert.Contains("saveStrategyStatus=ProviderNeutralFallback", mySqlDetail);
    Assert.Contains("selectedStrategy=<none>", mySqlDetail);
    Assert.Contains("fallbackCauses=MySqlTinySatelliteHistoryProviderNeutralFallback", mySqlDetail);
    Assert.DoesNotContain("executionPath=DVault MySQL staged bulk save path", mySqlDetail);

    var mySqlSingleRequestTinyBenchmark = new BenchmarkExecutionDetailTestBenchmark(
        "customer-profile-scale-10x1",
        MySqlProviderName,
        "dvault-adddvaultmysql-optimized",
        "mysql-optimized-dvault");
    var mySqlSingleRequestTinyDiagnostics = CreateDiagnosticsResult(new DataVaultSaveStrategyDiagnostics(
        DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback,
        KnownProviderNames.MySqlPomelo,
        SelectedStrategyName: null,
        SelectedStrategyPriority: null,
        [
            new DataVaultSaveStrategyCandidateDiagnostics(
                0,
                "MySqlDataVaultSaveStrategy",
                100,
                false,
                [
                    new DataVaultSaveStrategyFallbackCause(
                        DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold,
                        "minimum threshold"),
                    new DataVaultSaveStrategyFallbackCause(
                        DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback,
                        "single-request tiny satellite batch"),
                ]),
        ],
        [
            new DataVaultSaveStrategyFallbackCause(
                DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold,
                "minimum threshold"),
            new DataVaultSaveStrategyFallbackCause(
                DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback,
                "single-request tiny satellite batch"),
        ]));

    var mySqlSingleRequestTinyDetail = BenchmarkExecutionDetails.CreateSaveStrategyDetail(
        mySqlSingleRequestTinyBenchmark,
        mySqlSingleRequestTinyDiagnostics,
        requestCount: 1,
        hubOperationCount: 0,
        linkOperationCount: 0,
        satelliteOperationCount: 10);

    Assert.Contains("executionPath=DVault provider-neutral fallback path", mySqlSingleRequestTinyDetail);
    Assert.Contains(
        "fallbackCauses=MySqlMinimumOperationThreshold|MySqlTinySatelliteHistoryProviderNeutralFallback",
        mySqlSingleRequestTinyDetail);
  }

  [Fact]
  public void CheckedInBenchmarkArtifactsAndPerformanceGuidanceStayInSync() {
    var markdown = ReadRepositoryText("benchmark-summary.md");
    var csv = ReadRepositoryText("benchmark-summary.csv");
    var json = ReadRepositoryText("benchmark-summary.json");
    var guidance = ReadRepositoryText(Path.Combine("docs", "performance-profiles.md"));
    var benchmarkContract = ReadRepositoryText(Path.Combine(
        "docs",
        "plans",
        "performance-evidence-benchmark-artifact-contract.md"));

    var artifacts = VerifyBenchmarkArtifactTriplet(markdown, csv, json);

    AssertExpectedRootBenchmarkRows(artifacts);
    AssertPerformanceGuidanceMatchesArtifacts(guidance, artifacts);
    AssertProviderTuningProfileCategoriesMatchGuidance(guidance);
    AssertRegressionBudgetDefaultsAreDocumented(benchmarkContract);
  }

  [Fact]
  public void ProviderEvidenceManifestContractMapsBenchmarkAndDocsEvidenceRows() {
    var markdown = ReadRepositoryText("benchmark-summary.md");
    var csv = ReadRepositoryText("benchmark-summary.csv");
    var json = ReadRepositoryText("benchmark-summary.json");
    var evidenceMatrix = ReadRepositoryText(Path.Combine(
        "docs",
        "plans",
        "provider-optimization-evidence-matrix.md"));
    var benchmarkContract = ReadRepositoryText(Path.Combine(
        "docs",
        "plans",
        "performance-evidence-benchmark-artifact-contract.md"));
    var benchmarkReadme = ReadRepositoryText(Path.Combine(
        "benchmarks",
        "DCoding.Data.DVault.Benchmarks",
        "README.md"));
    var performanceProfiles = ReadRepositoryText(Path.Combine("docs", "performance-profiles.md"));

    var artifacts = VerifyBenchmarkArtifactTriplet(markdown, csv, json);

    AssertProviderEvidenceManifestContractIsDocumented(
        evidenceMatrix,
        benchmarkContract,
        benchmarkReadme,
        performanceProfiles);

    var completedReadRow = FindArtifactRow(
        artifacts,
        SqliteProviderName,
        "latest-satellite-read",
        "dvault-adddvaultsqlite-optimized");
    var completedManifestRow = CreateBenchmarkBackedProviderEvidenceManifestRow(
        completedReadRow,
        "completed-timing");

    Assert.Equal("latest-satellite-read", completedManifestRow.Scenario);
    Assert.Equal(SqliteProviderName, completedManifestRow.Provider);
    Assert.Equal("dvault-adddvaultsqlite-optimized", completedManifestRow.Baseline);
    Assert.Equal("sqlite-optimized-dvault", completedManifestRow.StrategyFamily);
    Assert.Equal(
        new[] { "benchmark-summary.md", "benchmark-summary.csv", "benchmark-summary.json" },
        completedManifestRow.SourceArtifacts);
    Assert.Equal("completed-timing", completedManifestRow.EvidencePosture);
    Assert.Equal("completed", completedManifestRow.ExecutionStatus);
    Assert.Null(completedManifestRow.SkipReason);
    Assert.Null(completedManifestRow.WorkloadShape);
    Assert.Equal("LatestSatellite", completedManifestRow.ReadShape);
    Assert.Equal("DVault SQLite optimized latest satellite read path", completedManifestRow.SelectedPath);
    Assert.Null(completedManifestRow.PlannedPath);
    Assert.Equal("SqliteDataVaultReadStrategy", completedManifestRow.SelectedStrategy);
    Assert.Null(completedManifestRow.PlannedStrategy);
    Assert.Empty(completedManifestRow.FallbackCauses);
    Assert.Equal(artifacts.Context.Iterations, completedManifestRow.ResultSummary.Iterations);
    Assert.Equal("present", completedManifestRow.ResultSummary.MetricState);
    Assert.Equal(completedReadRow.PersistedOutcome, completedManifestRow.ResultSummary.PersistedOutcome);

    var skippedProviderRow = FindArtifactRow(
        artifacts,
        PostgresProviderName,
        "provider-native-bulk-ingestion",
        "dvault-adddvaultpostgres-optimized");
    var skippedManifestRow = CreateBenchmarkBackedProviderEvidenceManifestRow(
        skippedProviderRow,
        "skipped-placeholder");

    Assert.Equal("provider-native-bulk-ingestion", skippedManifestRow.Scenario);
    Assert.Equal(PostgresProviderName, skippedManifestRow.Provider);
    Assert.Equal("dvault-adddvaultpostgres-optimized", skippedManifestRow.Baseline);
    Assert.Equal("postgres-optimized-dvault", skippedManifestRow.StrategyFamily);
    Assert.Equal("skipped-placeholder", skippedManifestRow.EvidencePosture);
    Assert.Equal("skipped", skippedManifestRow.ExecutionStatus);
    Assert.Equal(NotConfiguredSkipReason, skippedManifestRow.SkipReason);
    Assert.Equal("provider-native-bulk-ingestion", skippedManifestRow.WorkloadShape);
    Assert.Null(skippedManifestRow.ReadShape);
    Assert.Null(skippedManifestRow.SelectedPath);
    Assert.Equal("DVault PostgreSQL staged bulk save path", skippedManifestRow.PlannedPath);
    Assert.Null(skippedManifestRow.SelectedStrategy);
    Assert.Equal("PostgresDataVaultSaveStrategy", skippedManifestRow.PlannedStrategy);
    Assert.Empty(skippedManifestRow.FallbackCauses);
    Assert.Equal(0, skippedManifestRow.ResultSummary.Iterations);
    Assert.Equal("not-executed", skippedManifestRow.ResultSummary.MetricState);
    Assert.Equal("not executed", skippedManifestRow.ResultSummary.PersistedOutcome);

    var docsOnlyManifestRow = CreateDocsOnlyProviderEvidenceManifestRow();

    Assert.Equal("pit-as-of-read", docsOnlyManifestRow.Scenario);
    Assert.Equal("DB2 external provider", docsOnlyManifestRow.Provider);
    Assert.Equal("AddDVaultDb2() / Db2DataVaultReadStrategy", docsOnlyManifestRow.Baseline);
    Assert.Equal("diagnostics-only", docsOnlyManifestRow.EvidencePosture);
    Assert.Null(docsOnlyManifestRow.ExecutionStatus);
    Assert.Null(docsOnlyManifestRow.SkipReason);
    Assert.Equal("PitAsOf", docsOnlyManifestRow.ReadShape);
    Assert.Equal("diagnostics-gated DB2 PIT read candidate", docsOnlyManifestRow.PlannedPath);
    Assert.Equal("Db2DataVaultReadStrategy", docsOnlyManifestRow.PlannedStrategy);
    Assert.Null(docsOnlyManifestRow.ResultSummary.Iterations);
    Assert.Equal("not-applicable", docsOnlyManifestRow.ResultSummary.MetricState);
    Assert.Null(docsOnlyManifestRow.ResultSummary.PersistedOutcome);
    Assert.Contains("\"provider\": \"DB2 external provider\"", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("\"evidencePosture\": \"diagnostics-only\"", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("\"metricState\": \"not-applicable\"", evidenceMatrix, StringComparison.Ordinal);

    var skippedDb2ReadRow = FindArtifactRow(
        artifacts,
        Db2ProviderName,
        "pit-as-of-read",
        "dvault-adddvaultdb2-optimized");
    var skippedDb2ManifestRow = CreateBenchmarkBackedProviderEvidenceManifestRow(
        skippedDb2ReadRow,
        "skipped-placeholder");

    Assert.Equal(Db2ProviderName, skippedDb2ManifestRow.Provider);
    Assert.Equal("dvault-adddvaultdb2-optimized", skippedDb2ManifestRow.Baseline);
    Assert.Equal("db2-optimized-dvault", skippedDb2ManifestRow.StrategyFamily);
    Assert.Equal("skipped-placeholder", skippedDb2ManifestRow.EvidencePosture);
    Assert.Equal("PitAsOf", skippedDb2ManifestRow.ReadShape);
    Assert.Equal("DVault DB2 optimized PIT read path", skippedDb2ManifestRow.PlannedPath);
    Assert.Equal("Db2DataVaultReadStrategy", skippedDb2ManifestRow.PlannedStrategy);
  }

  [Fact]
  public void ProviderEvidenceMatrixCitesProviderOptimizationClosureReadRows() {
    var artifactDirectory = Path.Combine(
        "artifacts",
        "benchmarks",
        "v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607");
    var markdown = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.md"));
    var csv = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.csv"));
    var json = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.json"));
    var evidenceMatrix = ReadRepositoryText(Path.Combine(
        "docs",
        "plans",
        "provider-optimization-evidence-matrix.md"));
    var gapMatrix = ReadRepositoryText(Path.Combine(
        "docs",
        "plans",
        "provider-optimization-gap-matrix.md"));
    var performanceProfiles = ReadRepositoryText(Path.Combine("docs", "performance-profiles.md"));

    var artifacts = VerifyBenchmarkArtifactTriplet(markdown, csv, json);
    var postgresPitRow = FindArtifactRow(
        artifacts,
        PostgresProviderName,
        "pit-as-of-read",
        "dvault-adddvaultpostgres-optimized");
    var postgresBridgeRow = FindArtifactRow(
        artifacts,
        PostgresProviderName,
        "bridge-traversal-read",
        "dvault-adddvaultpostgres-optimized");
    var sqlServerPitRow = FindArtifactRow(
        artifacts,
        SqlServerProviderName,
        "pit-as-of-read",
        "dvault-adddvaultsqlserver-optimized");
    var sqlServerBridgeRow = FindArtifactRow(
        artifacts,
        SqlServerProviderName,
        "bridge-traversal-read",
        "dvault-adddvaultsqlserver-optimized");
    var mySqlPitRow = FindArtifactRow(
        artifacts,
        MySqlProviderName,
        "pit-as-of-read",
        "dvault-adddvaultmysql-optimized");
    var mySqlBridgeRow = FindArtifactRow(
        artifacts,
        MySqlProviderName,
        "bridge-traversal-read",
        "dvault-adddvaultmysql-optimized");
    var oraclePitRow = FindArtifactRow(
        artifacts,
        OracleProviderName,
        "pit-as-of-read",
        "dvault-adddvaultoracle-optimized");
    var oracleBridgeRow = FindArtifactRow(
        artifacts,
        OracleProviderName,
        "bridge-traversal-read",
        "dvault-adddvaultoracle-optimized");

    AssertCompletedProviderReadRow(postgresPitRow, "PostgresDataVaultReadStrategy", "PitAsOf", "PostgreSQL");
    AssertCompletedProviderReadRow(postgresBridgeRow, "PostgresDataVaultReadStrategy", "Bridge", "PostgreSQL");
    AssertCompletedProviderReadRow(sqlServerPitRow, "SqlServerDataVaultReadStrategy", "PitAsOf", "SQL Server");
    AssertCompletedProviderReadRow(sqlServerBridgeRow, "SqlServerDataVaultReadStrategy", "Bridge", "SQL Server");
    AssertCompletedProviderReadRow(mySqlPitRow, "MySqlDataVaultReadStrategy", "PitAsOf", "MySQL");
    AssertCompletedProviderReadRow(mySqlBridgeRow, "MySqlDataVaultReadStrategy", "Bridge", "MySQL");
    AssertCompletedProviderReadRow(oraclePitRow, "OracleDataVaultReadStrategy", "PitAsOf", "Oracle");
    AssertCompletedProviderReadRow(oracleBridgeRow, "OracleDataVaultReadStrategy", "Bridge", "Oracle");
    Assert.Contains(
        "v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.md",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `latest-satellite-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `pit-as-of-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `bridge-traversal-read` | PostgreSQL external provider | `dvault-adddvaultpostgres-optimized` | `postgres-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `latest-satellite-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `pit-as-of-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `bridge-traversal-read` | SQL Server external provider | `dvault-adddvaultsqlserver-optimized` | `sqlserver-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `latest-satellite-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `pit-as-of-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `bridge-traversal-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `latest-satellite-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `pit-as-of-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `bridge-traversal-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains("## Closed Provider Read Evidence", gapMatrix, StringComparison.Ordinal);
    Assert.DoesNotContain(
        "| P2.01 | Evidence gap | PostgreSQL external provider | `pit-as-of-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.DoesNotContain(
        "| P3.01 | Evidence gap | PostgreSQL external provider | `bridge-traversal-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.DoesNotContain(
        "| P2.02 | Evidence gap | SQL Server external provider | `pit-as-of-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.DoesNotContain(
        "| P3.02 | Evidence gap | SQL Server external provider | `bridge-traversal-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.DoesNotContain(
        "| P2.03 | Evidence gap | MySQL external provider | `pit-as-of-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.DoesNotContain(
        "| P3.03 | Evidence gap | MySQL external provider | `bridge-traversal-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.DoesNotContain(
        "| P2.04 | Evidence gap | Oracle external provider | `pit-as-of-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.DoesNotContain(
        "| P3.04 | Evidence gap | Oracle external provider | `bridge-traversal-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "PostgreSQL, SQL Server, MySQL, Oracle, and DB2 provider-native save rows plus latest-satellite, PIT, and bridge read rows",
        performanceProfiles,
        StringComparison.Ordinal);
  }

  [Fact]
  public void MySqlLatestSatelliteEvidenceArtifactRecordsOptimizedReadSelectionWithoutImprovementClaim() {
    var artifactDirectory = Path.Combine(
        "artifacts",
        "benchmarks",
        "06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620");
    var markdown = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.md"));
    var csv = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.csv"));
    var json = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.json"));
    var evidenceMatrix = ReadRepositoryText(Path.Combine(
        "docs",
        "plans",
        "provider-optimization-evidence-matrix.md"));
    var gapMatrix = ReadRepositoryText(Path.Combine(
        "docs",
        "plans",
        "provider-optimization-gap-matrix.md"));
    var performanceProfiles = ReadRepositoryText(Path.Combine("docs", "performance-profiles.md"));
    var releaseNotes = ReadRepositoryText(Path.Combine("docs", "releases", "v0.42.0.md"));

    var artifacts = VerifyBenchmarkArtifactTriplet(markdown, csv, json);
    var latestSatellite = FindArtifactRow(
        artifacts,
        MySqlProviderName,
        "latest-satellite-read",
        "dvault-adddvaultmysql-optimized");
    var manifestRow = CreateBenchmarkBackedProviderEvidenceManifestRow(latestSatellite, "completed-timing");

    Assert.Equal(6, artifacts.RowsByKey.Count);
    AssertCompletedProviderReadRow(latestSatellite, "MySqlDataVaultReadStrategy", "LatestSatellite", "MySQL");
    Assert.Equal("19.113", latestSatellite.MeanMilliseconds);
    Assert.Equal("mysql-optimized-dvault", latestSatellite.StrategyFamily);
    Assert.Equal("100 latest profile satellite rows read from 1000 seeded profile states", latestSatellite.PersistedOutcome);
    Assert.Equal("completed-timing", manifestRow.EvidencePosture);
    Assert.Equal("present", manifestRow.ResultSummary.MetricState);
    Assert.Equal("LatestSatellite", manifestRow.ReadShape);
    Assert.Equal("DVault MySQL optimized latest satellite read path", manifestRow.SelectedPath);
    Assert.Null(manifestRow.PlannedPath);
    Assert.Equal("MySqlDataVaultReadStrategy", manifestRow.SelectedStrategy);
    Assert.Null(manifestRow.PlannedStrategy);
    Assert.Empty(manifestRow.FallbackCauses);

    Assert.Contains(
        "06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/benchmark-summary.md",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `latest-satellite-read` | MySQL external provider | `dvault-adddvaultmysql-optimized` | `mysql-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| P0.03 | Closed evidence row | MySQL external provider | `latest-satellite-read` | `completed-timing`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "The 2026-06-23 closure bundle completed retained multi-row at `15.827` ms, bounded staged bulk at `26.055` ms, and the deliberate large mixed provider-neutral fallback at `145.601` ms.",
        performanceProfiles,
        StringComparison.Ordinal);
    Assert.Contains(
        "without claiming a provider-neutral fallback improvement comparator",
        releaseNotes,
        StringComparison.Ordinal);
  }

  [Fact]
  public void ProviderPitBridgeAuditClosesDb2WithProviderOptimizationClosureBundle() {
    var gapMatrix = ReadRepositoryText(Path.Combine(
        "docs",
        "plans",
        "provider-optimization-gap-matrix.md"));
    var evidenceMatrix = ReadRepositoryText(Path.Combine(
        "docs",
        "plans",
        "provider-optimization-evidence-matrix.md"));
    var pitBridgeBoundary = ReadRepositoryText(Path.Combine(
        "docs",
        "architecture",
        "dvault-v1-pit-bridge-boundary.md"));
    var rootArtifacts = VerifyBenchmarkArtifactTriplet(
        ReadRepositoryText("benchmark-summary.md"),
        ReadRepositoryText("benchmark-summary.csv"),
        ReadRepositoryText("benchmark-summary.json"));
    var artifactDirectory = Path.Combine(
        "artifacts",
        "benchmarks",
        "06FF0000000000000000000000-provider-optimization-closure-20260623",
        "db2-rowcap-1000");
    var markdown = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.md"));
    var csv = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.csv"));
    var json = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.json"));

    var db2Artifacts = VerifyBenchmarkArtifactTriplet(markdown, csv, json);

    Assert.Equal("db2", db2Artifacts.Context.ProviderFilter);
    Assert.Equal(5, db2Artifacts.Context.Iterations);
    Assert.Contains("DB2 external provider: completed", markdown, StringComparison.Ordinal);

    Assert.Contains(
        "06FF0000000000000000000000-provider-optimization-closure-20260623/db2-rowcap-1000/benchmark-summary.md",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `provider-native-bulk-ingestion` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `latest-satellite-read` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `pit-as-of-read` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `bridge-traversal-read` | DB2 external provider | `dvault-adddvaultdb2-optimized` | `db2-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "DB2 registers diagnostics-gated latest-satellite/PIT/bridge read dispatch",
        evidenceMatrix,
        StringComparison.Ordinal);

    Assert.Contains(
        "PostgreSQL, MySQL, Oracle, SQL Server, and DB2 `provider-native-bulk-ingestion` rows are closed by provider-configured timing rows in the closure bundle",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| P0.05 | Closed evidence row | DB2 external provider | `latest-satellite-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| P1.05 | Closed evidence row | DB2 external provider | `provider-native-bulk-ingestion`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| P2.05 | Closed evidence row | DB2 external provider | `pit-as-of-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| P3.05 | Closed evidence row | DB2 external provider | `bridge-traversal-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.DoesNotContain(
        "| P2.05 | Evidence gap | DB2 external provider | `pit-as-of-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.DoesNotContain(
        "| P3.05 | Evidence gap | DB2 external provider | `bridge-traversal-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.Contains("AddDVaultDb2()", pitBridgeBoundary, StringComparison.Ordinal);

    var fallbackSaveRow = FindArtifactRow(
        db2Artifacts,
        Db2ProviderName,
        "provider-native-bulk-ingestion",
        "dvault-adddvault-fallback");
    Assert.Equal("completed", fallbackSaveRow.ExecutionStatus);
    Assert.Equal(5, fallbackSaveRow.Iterations);
    AssertCompletedMetricsPresent(fallbackSaveRow);
    Assert.Contains("selectedStrategy=<none>", fallbackSaveRow.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("fallbackCauses=NoProviderSpecificStrategyRegistered", fallbackSaveRow.ExecutionDetail, StringComparison.Ordinal);

    var optimizedSaveRow = FindArtifactRow(
        db2Artifacts,
        Db2ProviderName,
        "provider-native-bulk-ingestion",
        "dvault-adddvaultdb2-optimized");
    Assert.Equal("completed", optimizedSaveRow.ExecutionStatus);
    Assert.Equal(5, optimizedSaveRow.Iterations);
    AssertCompletedMetricsPresent(optimizedSaveRow);
    Assert.Contains("selectedStrategy=Db2DataVaultSaveStrategy", optimizedSaveRow.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("db2SaveBoundary=clean-context-set-based", optimizedSaveRow.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("stagedBulkBoundary=not-supported", optimizedSaveRow.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("fallbackCauses=none", optimizedSaveRow.ExecutionDetail, StringComparison.Ordinal);

    AssertCompletedProviderReadRow(
        FindArtifactRow(db2Artifacts, Db2ProviderName, "latest-satellite-read", "dvault-adddvaultdb2-optimized"),
        "Db2DataVaultReadStrategy",
        "LatestSatellite",
        "DB2");
    AssertCompletedProviderReadRow(
        FindArtifactRow(db2Artifacts, Db2ProviderName, "pit-as-of-read", "dvault-adddvaultdb2-optimized"),
        "Db2DataVaultReadStrategy",
        "PitAsOf",
        "DB2");
    AssertCompletedProviderReadRow(
        FindArtifactRow(db2Artifacts, Db2ProviderName, "bridge-traversal-read", "dvault-adddvaultdb2-optimized"),
        "Db2DataVaultReadStrategy",
        "Bridge",
        "DB2");

    AssertProviderReadPlaceholder(
        FindArtifactRow(rootArtifacts, Db2ProviderName, "pit-as-of-read", "dvault-adddvaultdb2-optimized"),
        "Db2DataVaultReadStrategy",
        "PitAsOf",
        BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable);
    AssertProviderReadPlaceholder(
        FindArtifactRow(rootArtifacts, Db2ProviderName, "bridge-traversal-read", "dvault-adddvaultdb2-optimized"),
        "Db2DataVaultReadStrategy",
        "Bridge",
        BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable);
  }

  [Fact]
  public void OracleHighVolumeThresholdArtifactRecordsNoChangeDecision() {
    var artifactDirectory = Path.Combine(
        "artifacts",
        "benchmarks",
        "v0.32.0-06F9XD2TGEYEG6S0AK86YF295M-oracle-high-volume-threshold-20260607");
    var markdown = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.md"));
    var csv = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.csv"));
    var json = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.json"));

    var artifacts = VerifyBenchmarkArtifactTriplet(markdown, csv, json);

    Assert.Equal(10, artifacts.RowsByKey.Count);
    Assert.Contains(
        "- Oracle threshold decision: keep OracleMaximumSatelliteOperationThreshold at 10000 satellite operations.",
        markdown,
        StringComparison.Ordinal);
    Assert.Contains(
        "- Decision: keep the Oracle direct optimized batching safety cap at 10000 satellite operations",
        markdown,
        StringComparison.Ordinal);
    Assert.Contains("stagedOracleBulk=not-selected-no-measured-win", markdown, StringComparison.Ordinal);

    var belowMinimum = FindArtifactRow(
        artifacts,
        OracleProviderName,
        "customer-profile-scale-10x1",
        "dvault-adddvaultoracle-optimized");
    Assert.Contains("OracleMinimumOperationThreshold", belowMinimum.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("saveStrategyStatus=ProviderNeutralFallback", belowMinimum.ExecutionDetail, StringComparison.Ordinal);

    var zeroChangeBoundary = FindArtifactRow(
        artifacts,
        OracleProviderName,
        "customer-profile-scale-10000x1",
        "dvault-adddvaultoracle-optimized");
    Assert.Equal("1176.560", zeroChangeBoundary.MeanMilliseconds);
    Assert.Contains("saveStrategyStatus=ProviderStrategySelected", zeroChangeBoundary.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("selectedStrategy=OracleDataVaultSaveStrategy", zeroChangeBoundary.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("satelliteOperations=10000", zeroChangeBoundary.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("fallbackCauses=none", zeroChangeBoundary.ExecutionDetail, StringComparison.Ordinal);

    var historyBoundary = FindArtifactRow(
        artifacts,
        OracleProviderName,
        "customer-profile-scale-1000x10",
        "dvault-adddvaultoracle-optimized");
    Assert.Equal("849.163", historyBoundary.MeanMilliseconds);
    Assert.Contains("saveStrategyStatus=ProviderStrategySelected", historyBoundary.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("satelliteOperations=10000", historyBoundary.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("fallbackCauses=none", historyBoundary.ExecutionDetail, StringComparison.Ordinal);

    var highVolumeFallback = FindArtifactRow(
        artifacts,
        OracleProviderName,
        "customer-profile-scale-10000x10",
        "dvault-adddvaultoracle-optimized");
    Assert.Equal("10689.765", highVolumeFallback.MeanMilliseconds);
    Assert.Contains("saveStrategyStatus=ProviderNeutralFallback", highVolumeFallback.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("OracleMaximumSatelliteOperationThreshold", highVolumeFallback.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("satelliteOperations=100000", highVolumeFallback.ExecutionDetail, StringComparison.Ordinal);

    var highVolumeConventional = FindArtifactRow(
        artifacts,
        OracleProviderName,
        "customer-profile-scale-10000x10",
        "conventional-ef-bulk");
    Assert.Equal("5500.134", highVolumeConventional.MeanMilliseconds);
  }

  [Fact]
  public void OracleConfiguredReadClosureArtifactRecordsCompletedLatestPitAndBridgeTiming() {
    var artifactDirectory = Path.Combine(
        "artifacts",
        "benchmarks",
        "06FF0000000000000000000000-provider-optimization-closure-20260623",
        "oracle-lob-prefetch");
    var markdown = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.md"));
    var csv = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.csv"));
    var json = ReadRepositoryText(Path.Combine(artifactDirectory, "benchmark-summary.json"));
    var evidenceMatrix = ReadRepositoryText(Path.Combine(
        "docs",
        "plans",
        "provider-optimization-evidence-matrix.md"));
    var gapMatrix = ReadRepositoryText(Path.Combine(
        "docs",
        "plans",
        "provider-optimization-gap-matrix.md"));
    var performanceProfiles = ReadRepositoryText(Path.Combine("docs", "performance-profiles.md"));

    var artifacts = VerifyBenchmarkArtifactTriplet(markdown, csv, json);

    Assert.True(
        artifacts.Context.OptionalProviders.TryGetValue(OracleProviderName, out var oracleProvider),
        "benchmark-summary.json context is missing optional provider '" + OracleProviderName + "'.");
    Assert.Equal("completed", oracleProvider.ExecutionStatus);
    Assert.Equal(string.Empty, oracleProvider.SkipReason);

    AssertCompletedOracleReadArtifactRow(
        artifacts,
        "latest-satellite-read",
        "18.783",
        "DVault Oracle optimized latest satellite read path",
        "LatestSatellite");
    AssertCompletedOracleReadArtifactRow(
        artifacts,
        "pit-as-of-read",
        "26.857",
        "DVault Oracle optimized PIT read path",
        "PitAsOf");
    AssertCompletedOracleReadArtifactRow(
        artifacts,
        "bridge-traversal-read",
        "3.922",
        "DVault Oracle optimized bridge read path",
        "Bridge");

    Assert.Contains("06FF0000000000000000000000-provider-optimization-closure-20260623/oracle-lob-prefetch/benchmark-summary.md", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("| `latest-satellite-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("| `pit-as-of-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("| `bridge-traversal-read` | Oracle external provider | `dvault-adddvaultoracle-optimized` | `oracle-optimized-dvault` | `completed-timing` | 2026-06-23 closure bundle |", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("| P0.04 | Closed evidence row | Oracle external provider | `latest-satellite-read`", gapMatrix, StringComparison.Ordinal);
    Assert.DoesNotContain("| P2.04 | Evidence gap | Oracle external provider | `pit-as-of-read`", gapMatrix, StringComparison.Ordinal);
    Assert.DoesNotContain("| P3.04 | Evidence gap | Oracle external provider | `bridge-traversal-read`", gapMatrix, StringComparison.Ordinal);
    Assert.Contains("Oracle latest/PIT timings include the ODP.NET LOB-prefetch read-command tuning.", performanceProfiles, StringComparison.Ordinal);
  }

  [Fact]
  public async Task ProviderNativeBulkBenchmarkProvesSelectedProviderStrategyBeforeTimingNativeRow() {
    var benchmark = new ProviderNativeBulkIngestionBenchmark(
        BenchmarkDatabaseProviders.Sqlite,
        DataVaultBenchmarkStrategy.SqliteOptimized,
        DataVaultLoadTimestampStorage.ProviderDefault);

    var result = await benchmark.ExecuteAsync(CancellationToken.None).ConfigureAwait(false);

    Assert.Contains("300 order hubs, 300 product hubs, 300 order-product links, and 2 fulfillment satellite rows", result.PersistedOutcome);
    Assert.Contains("saveStrategyStatus=ProviderStrategySelected", result.ExecutionDetail);
    Assert.Contains("selectedStrategy=SqliteDataVaultSaveStrategy", result.ExecutionDetail);
    Assert.Contains("requestCount=5", result.ExecutionDetail);
    Assert.Contains("hubOperations=600", result.ExecutionDetail);
    Assert.Contains("linkOperations=300", result.ExecutionDetail);
    Assert.Contains("satelliteOperations=3", result.ExecutionDetail);
    Assert.True(result.Elapsed > TimeSpan.Zero);
  }

  [Fact]
  public void SaveStrategyExecutionDetailUsesFallbackPathWhenSqlServerCandidateDeclines() {
    var fallbackCauses = new[]
    {
        new DataVaultSaveStrategyFallbackCause(
            DataVaultSaveStrategyFallbackCauseKind.SqlServerMaximumSatelliteOperationThreshold,
            "SQL Server optimized dispatch accepts at most 500 satellite operations; the request batch contains 1000."),
    };
    var diagnostics = CreateSaveStrategyDiagnostics(
        new DataVaultSaveStrategyDiagnostics(
            DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback,
            KnownProviderNames.SqlServer,
            SelectedStrategyName: null,
            SelectedStrategyPriority: null,
            Candidates:
            [
                new DataVaultSaveStrategyCandidateDiagnostics(
                    0,
                    "SqlServerDataVaultSaveStrategy",
                    100,
                    CanSave: false,
                    fallbackCauses) {
                  SupportedProviderNames = [KnownProviderNames.SqlServer],
                  GateRequirements =
                  [
                      new DataVaultSaveStrategyGateRequirement(
                          DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold,
                          MinimumTotalOperationCount: 100),
                      new DataVaultSaveStrategyGateRequirement(
                          DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold,
                          MinimumTotalOperationCount: 900),
                      new DataVaultSaveStrategyGateRequirement(
                          DataVaultSaveStrategyFallbackCauseKind.SqlServerMaximumSatelliteOperationThreshold,
                          MaximumSatelliteOperationCount: 500),
                  ],
                },
            ],
            fallbackCauses));
    var benchmark = new DiagnosticExecutionDetailBenchmark(
        "customer-profile-scale-1000x1",
        SqlServerProviderName,
        "dvault-adddvaultsqlserver-optimized",
        DataVaultBenchmarkHelpers.SqlServerOptimizedStrategyFamily,
        "1000 customers, 1 profile state each",
        "0% repeat-change history");

    var executionDetail = BenchmarkExecutionDetails.CreateSaveStrategyDetail(
        benchmark,
        diagnostics,
        requestCount: 1,
        hubOperationCount: 0,
        linkOperationCount: 0,
        satelliteOperationCount: 1000);

    Assert.Contains("strategyFamily=sqlserver-optimized-dvault", executionDetail);
    Assert.Contains("executionPath=DVault provider-neutral fallback path", executionDetail);
    Assert.DoesNotContain("DVault SQL Server staged native bulk save path", executionDetail);
    Assert.Contains("saveStrategyStatus=ProviderNeutralFallback", executionDetail);
    Assert.Contains("selectedStrategy=<none>", executionDetail);
    Assert.Contains("candidateStrategies=SqlServerDataVaultSaveStrategy", executionDetail);
    Assert.Contains("fallbackCauses=SqlServerMaximumSatelliteOperationThreshold", executionDetail);
    Assert.Contains("satelliteOperations=1000", executionDetail);
  }

  [Fact]
  public void PostgresLatestSatelliteBenchmarkDetailsRecordRetainedWindowedSqlShapeAndStrategyGate() {
    var benchmark = new BenchmarkExecutionDetailTestBenchmark(
        "latest-satellite-read",
        PostgresProviderName,
        "dvault-adddvaultpostgres-optimized",
        DataVaultBenchmarkHelpers.PostgresOptimizedStrategyFamily);

    var executionDetail = BenchmarkExecutionDetails.CreatePlanned(benchmark);

    Assert.Equal(
        "PostgresDataVaultReadStrategy",
        DataVaultBenchmarkHelpers.GetProviderReadStrategyName(
            DataVaultBenchmarkStrategy.PostgresOptimized,
            "latest-satellite-read"));
    Assert.Contains("selectedStrategy=PostgresDataVaultReadStrategy", executionDetail, StringComparison.Ordinal);
    Assert.Contains("plannedReadStrategy=PostgresDataVaultReadStrategy", executionDetail, StringComparison.Ordinal);
    Assert.Contains("readShape=LatestSatellite", executionDetail, StringComparison.Ordinal);
    Assert.Contains("latestSatelliteSqlShape=windowed-row-number", executionDetail, StringComparison.Ordinal);
  }

  [Fact]
  public async Task LocalBenchmarkRunnerCanRunLatestSatelliteIndexMatrixForSqlite() {
    var text = await RunBenchmarkAndCaptureOutputAsync(new BenchmarkOptions(
        1,
        0,
        LatestIndexMatrix: true,
        ProviderFilter: BenchmarkProviderFilters.Sqlite)).ConfigureAwait(false);

    Assert.Contains("latest-satellite-lookup-replay", text);
    Assert.Contains("latest-satellite-lookup-change", text);
    Assert.Contains("dvault-adddvaultsqlite-optimized/latest-index-default", text);
    Assert.Contains("dvault-adddvaultsqlite-optimized/latest-index-parent-desc", text);
    Assert.Contains("dvault-adddvaultsqlite-optimized/latest-index-covering", text);
    Assert.Contains("2000 profile satellite rows after unchanged replay latest lookup", text);
    Assert.Contains("2100 profile satellite rows after changed replay latest lookup", text);
    Assert.Contains("Recorded 6 benchmark report rows.", text);
    Assert.Contains("Executed 6 benchmark report rows.", text);
    Assert.DoesNotContain("customer-profile-history", text);
  }

  [Fact]
  public async Task LocalBenchmarkRunnerCanRunAllocationHotspotProfileForSqlite() {
    var artifactDirectory = Path.Combine(
        Path.GetTempPath(),
        "DVaultAllocationHotspotArtifacts-" + Guid.NewGuid().ToString("N"));

    try {
      var text = await RunBenchmarkAndCaptureOutputAsync(new BenchmarkOptions(
          1,
          0,
          artifactDirectory,
          ProviderFilter: BenchmarkProviderFilters.Sqlite,
          AllocationHotspots: true))
          .ConfigureAwait(false);

      Assert.Contains("DVault allocation hotspot profile", text, StringComparison.Ordinal);
      Assert.Contains("stable-hash-canonicalization", text, StringComparison.Ordinal);
      Assert.Contains("stable-hash-digest-generation", text, StringComparison.Ordinal);
      Assert.Contains("customer-profile-hub-only-save-prep", text, StringComparison.Ordinal);
      Assert.Contains("order-product-link-bearing-save-prep", text, StringComparison.Ordinal);
      Assert.Contains("satellite-unchanged-replay-filter", text, StringComparison.Ordinal);
      Assert.Contains("satellite-changed-replay-filter", text, StringComparison.Ordinal);
      Assert.Contains("Recorded 6 allocation hotspot benchmark rows.", text, StringComparison.Ordinal);

      var artifacts = VerifyBenchmarkArtifactTriplet(
          await File.ReadAllTextAsync(Path.Combine(artifactDirectory, "benchmark-summary.md")).ConfigureAwait(false),
          await File.ReadAllTextAsync(Path.Combine(artifactDirectory, "benchmark-summary.csv")).ConfigureAwait(false),
          await File.ReadAllTextAsync(Path.Combine(artifactDirectory, "benchmark-summary.json")).ConfigureAwait(false));
      Assert.Equal(BenchmarkProviderFilters.Sqlite, artifacts.Context.ProviderFilter);
      Assert.Equal(6, artifacts.RowsByKey.Count);
      Assert.Contains(
          artifacts.RowsByKey.Values,
          row => row.ScenarioName == "satellite-changed-replay-filter" &&
              row.ExecutionDetail.Contains("callerHashDiffGeneration=outside-profile", StringComparison.Ordinal));

      var hotspotMarkdown = await File
          .ReadAllTextAsync(Path.Combine(artifactDirectory, "allocation-hotspots.md"))
          .ConfigureAwait(false);
      Assert.Contains("## Ranked Hotspots", hotspotMarkdown, StringComparison.Ordinal);
      Assert.Contains("DefaultStableHashNormalizer.NormalizeFields", hotspotMarkdown, StringComparison.Ordinal);
      Assert.Contains("BuiltInStableHashService.ComputeHash", hotspotMarkdown, StringComparison.Ordinal);
      Assert.Contains("DefaultDataVaultSaveService.LoadLatestSatelliteHashDiffsAsync", hotspotMarkdown, StringComparison.Ordinal);
      Assert.Contains("Caller-owned satellite `HashDiff` generation is outside the measured operation", hotspotMarkdown, StringComparison.Ordinal);

      using var hotspotDocument = JsonDocument.Parse(
          await File.ReadAllTextAsync(Path.Combine(artifactDirectory, "allocation-hotspots.json")).ConfigureAwait(false));
      Assert.Equal("dvault.allocation-hotspots.v1", GetRequiredString(hotspotDocument.RootElement, "schemaVersion"));
      var rankedHotspots = hotspotDocument.RootElement.GetProperty("rankedHotspots").EnumerateArray().ToArray();
      Assert.NotEmpty(rankedHotspots);
      Assert.DoesNotContain(
          rankedHotspots,
          row => string.Equals(GetRequiredString(row, "surface"), "database write boundary", StringComparison.Ordinal));
      Assert.Contains(
          rankedHotspots,
          row => string.Equals(GetRequiredString(row, "surface"), "satellite latest-hash-diff replay filtering", StringComparison.Ordinal));
    }
    finally {
      if (Directory.Exists(artifactDirectory)) {
        Directory.Delete(artifactDirectory, recursive: true);
      }
    }
  }

  [Fact]
  public void BenchmarkOptionsCanSelectBoundedHashKeyStorageMatrix() {
    var options = BenchmarkOptions.Parse(["--hash-key-storage-matrix"]);

    Assert.Equal(
        ["sha256-v1-hex", "sha256-v1-binary", "sha256-128-v1-hex", "sha256-128-v1-binary"],
        options.EffectiveHashKeyVariants.Select(variant => variant.Label));
    Assert.Equal(
        [DataVaultHashKeyStorageProfile.HexString, DataVaultHashKeyStorageProfile.Binary, DataVaultHashKeyStorageProfile.HexString, DataVaultHashKeyStorageProfile.Binary],
        options.EffectiveHashKeyVariants.Select(variant => variant.StorageProfile));
    Assert.Equal([32, 32, 16, 16], options.EffectiveHashKeyVariants.Select(variant => variant.DigestByteLength));
  }

  [Fact]
  public void BenchmarkOptionsCanSelectAllocationHotspotMode() {
    var options = BenchmarkOptions.Parse(["--allocation-hotspots", "--provider", "sqlite"]);

    Assert.True(options.AllocationHotspots);
    Assert.Equal(BenchmarkProviderFilters.Sqlite, options.ProviderFilter);
    Assert.Equal(["sha256-v1-hex"], options.EffectiveHashKeyVariants.Select(variant => variant.Label));
  }

  [Fact]
  public async Task ProviderFilteredHashKeyStorageMatrixEmitsSelectedProviderPlaceholders() {
    var artifactDirectory = Path.Combine(
        Path.GetTempPath(),
        "DVaultBenchmarkProviderMatrixArtifacts-" + Guid.NewGuid().ToString("N"));
    var expectedVariantLabels = string.Join(", ", BenchmarkHashKeyVariant.BoundedStorageMatrix.Select(variant => variant.Label));

    try {
      var text = await RunBenchmarkAndCaptureOutputAsync(new BenchmarkOptions(
          1,
          0,
          artifactDirectory,
          ProviderFilter: BenchmarkProviderFilters.Postgres,
          HashKeyVariants: BenchmarkHashKeyVariant.BoundedStorageMatrix))
          .ConfigureAwait(false);

      Assert.Contains("Hash key variants: " + expectedVariantLabels, text);
      Assert.Contains(PostgresProviderName + ": skipped - " + NotConfiguredSkipReason, text);
      Assert.Contains("Recorded 24 benchmark report rows.", text);
      Assert.Contains("Skipped 24 benchmark report rows.", text);
      Assert.DoesNotContain("Running " + SqliteProviderName, text);
      Assert.DoesNotContain(SqlServerProviderName + ": skipped", text);
      Assert.DoesNotContain(MySqlProviderName + ": skipped", text);
      Assert.DoesNotContain(OracleProviderName + ": skipped", text);

      var artifacts = VerifyBenchmarkArtifactTriplet(
          await File.ReadAllTextAsync(Path.Combine(artifactDirectory, "benchmark-summary.md")).ConfigureAwait(false),
          await File.ReadAllTextAsync(Path.Combine(artifactDirectory, "benchmark-summary.csv")).ConfigureAwait(false),
          await File.ReadAllTextAsync(Path.Combine(artifactDirectory, "benchmark-summary.json")).ConfigureAwait(false));

      Assert.Equal(BenchmarkProviderFilters.Postgres, artifacts.Context.ProviderFilter);
      Assert.Equal(24, artifacts.RowsByKey.Count);
      AssertHashKeyVariantContext(artifacts.Context.HashKeyVariants, BenchmarkHashKeyVariant.BoundedStorageMatrix);
      Assert.Single(artifacts.Context.OptionalProviders);
      AssertOptionalProviderContext(
          artifacts.Context.OptionalProviders,
          PostgresProviderName,
          BenchmarkExternalProviderDefinitions.Postgres.ConnectionStringEnvironmentVariable,
          NotConfiguredSkipReason);

      foreach (var row in artifacts.RowsByKey.Values) {
        Assert.Equal(PostgresProviderName, row.ProviderName);
        Assert.Equal("skipped", row.ExecutionStatus);
        Assert.Equal(0, row.Iterations);
        Assert.Equal("not executed", row.PersistedOutcome);
        Assert.Contains("hashKeyVariant=", row.ExecutionDetail, StringComparison.Ordinal);
      }

      foreach (var variant in BenchmarkHashKeyVariant.BoundedStorageMatrix) {
        Assert.Contains(
            artifacts.RowsByKey.Values,
            row => row.ExecutionDetail.Contains("hashKeyVariant=" + variant.Label, StringComparison.Ordinal));
      }
    }
    finally {
      if (Directory.Exists(artifactDirectory)) {
        Directory.Delete(artifactDirectory, recursive: true);
      }
    }
  }

  [Fact]
  public async Task CustomerProfileDataVaultBenchmarkSupportsShortBinaryHashKeyVariant() {
    var cancellationToken = TestContext.Current.CancellationToken;
    var variant = BenchmarkHashKeyVariant.BoundedStorageMatrix.Single(candidate =>
        candidate.Label == "sha256-128-v1-binary");
    var benchmark = new CustomerProfileDataVaultBenchmark(
        BenchmarkDatabaseProviders.Sqlite,
        DataVaultBenchmarkStrategy.SqliteOptimized,
        DataVaultLoadTimestampStorage.ProviderDefault,
        variant);

    var result = await benchmark.ExecuteAsync(cancellationToken).ConfigureAwait(false);

    Assert.Contains("1 customer hub row and 2 profile satellite rows for C-100", result.PersistedOutcome);
    Assert.True(result.Elapsed > TimeSpan.Zero);
  }

  [Fact]
  public void HashKeyFootprintSidecarRowsDescribeBoundedMatrixPayloads() {
    var options = new BenchmarkOptions(
        1,
        0,
        ProviderFilter: BenchmarkProviderFilters.Sqlite,
        HashKeyVariants: BenchmarkHashKeyVariant.BoundedStorageMatrix);
    var postgresAvailability = PostgresBenchmarkAvailability.Skipped(BenchmarkSkipReason.NotConfigured());
    var context = BenchmarkRunContext.Create(
        options,
        postgresAvailability,
        [BenchmarkProviderAvailability.FromPostgres(postgresAvailability)]);
    var summaries = BenchmarkHashKeyVariant.BoundedStorageMatrix
        .Select(variant => new BenchmarkSummary(
            "customer-profile-history",
            SqliteProviderName,
            "dvault-adddvaultsqlite-optimized/" + variant.Label,
            "sqlite-optimized-dvault",
            "1 customer, 2 profile states",
            "50% repeat-change history",
            BenchmarkExecutionStatus.Completed,
            string.Empty,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            "scenario=customer-profile-history; hashKeyVariant=" + variant.Label,
            "1 customer hub row and 2 profile satellite rows for C-100"))
        .ToArray();

    var rows = BenchmarkHashKeyFootprintArtifacts.CreateRows(context, summaries);

    Assert.Equal(4, rows.Count);
    AssertFootprintRow(rows, "sha256-v1-hex", "TEXT", "LowercaseHexText", 64);
    AssertFootprintRow(rows, "sha256-v1-binary", "BLOB", "LowercaseHexBinary", 32);
    AssertFootprintRow(rows, "sha256-128-v1-hex", "TEXT", "LowercaseHexText", 32);
    AssertFootprintRow(rows, "sha256-128-v1-binary", "BLOB", "LowercaseHexBinary", 16);
  }

  [Fact]
  public async Task PostgresDiscoveryTreatsMissingEnvironmentVariableAsNotConfiguredSkip() {
    var availability = await PostgresBenchmarkAvailability
        .DiscoverAsync(
            _ => "  ",
            () => throw new InvalidOperationException("Provider dependency probe should not run."),
            (_, _) => throw new InvalidOperationException("Connection probe should not run."),
            CancellationToken.None)
        .ConfigureAwait(false);

    Assert.False(availability.IsAvailable);
    Assert.Equal("skipped", availability.ExecutionStatus);
    Assert.Equal("not configured", availability.SkipReason?.Category);
    Assert.Equal(NotConfiguredSkipReason, availability.SkipReason?.DisplayText);
  }

  [Fact]
  public async Task PostgresDiscoveryReportsUnavailableProviderDependencyBeforeConnecting() {
    var connectionProbeCalled = false;

    var availability = await PostgresBenchmarkAvailability
        .DiscoverAsync(
            _ => "Host=localhost;Database=dvault",
            () => false,
            (_, _) => {
              connectionProbeCalled = true;
              return Task.FromResult<string?>(null);
            },
            CancellationToken.None)
        .ConfigureAwait(false);

    Assert.False(connectionProbeCalled);
    Assert.False(availability.IsAvailable);
    Assert.Equal("provider dependency unavailable", availability.SkipReason?.Category);
    Assert.Contains("Npgsql.EntityFrameworkCore.PostgreSQL", availability.SkipReason?.DisplayText);
  }

  [Fact]
  public async Task PostgresDiscoveryReportsUnreachableConnectionAsSkippedProvider() {
    var availability = await PostgresBenchmarkAvailability
        .DiscoverAsync(
            _ => "Host=localhost;Database=dvault",
            () => true,
            (_, _) => Task.FromResult<string?>("simulated connection failure"),
            CancellationToken.None)
        .ConfigureAwait(false);

    Assert.False(availability.IsAvailable);
    Assert.Equal("connection unreachable", availability.SkipReason?.Category);
    Assert.Contains("simulated connection failure", availability.SkipReason?.DisplayText);
  }

  [Fact]
  public async Task PostgresDiscoveryReportsTimedOutConnectionProbeAsSkippedProvider() {
    var availability = await PostgresBenchmarkAvailability
        .DiscoverAsync(
            _ => "Host=localhost;Database=dvault",
            () => true,
            async (_, cancellationToken) => {
              await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken).ConfigureAwait(false);

              return null;
            },
            CancellationToken.None,
            TimeSpan.FromMilliseconds(10))
        .ConfigureAwait(false);

    Assert.False(availability.IsAvailable);
    Assert.Equal("connection unreachable", availability.SkipReason?.Category);
    Assert.Contains("Timed out after", availability.SkipReason?.DisplayText);
  }

  [Fact]
  public async Task PostgresDiscoveryReportsConfiguredConnectionAsAvailable() {
    var availability = await PostgresBenchmarkAvailability
        .DiscoverAsync(
            _ => "Host=localhost;Database=dvault",
            () => true,
            (_, _) => Task.FromResult<string?>(null),
            CancellationToken.None)
        .ConfigureAwait(false);

    Assert.True(availability.IsAvailable);
    Assert.Equal("completed", availability.ExecutionStatus);
    Assert.Null(availability.SkipReason);
    Assert.Equal(PostgresProviderName, availability.Provider.ProviderName);
  }

  private static async Task<string> RunBenchmarkAndCaptureOutputAsync(BenchmarkOptions options) {
    var originalOutput = Console.Out;
    using var output = new StringWriter(CultureInfo.InvariantCulture);
    var postgresAvailability = PostgresBenchmarkAvailability.Skipped(BenchmarkSkipReason.NotConfigured());
    var optionalProviders = new[]
    {
        BenchmarkProviderAvailability.FromPostgres(postgresAvailability),
        BenchmarkProviderAvailability.Skipped(
            BenchmarkExternalProviderDefinitions.SqlServer,
            BenchmarkSkipReason.NotConfigured(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable)),
        BenchmarkProviderAvailability.Skipped(
            BenchmarkExternalProviderDefinitions.MySql,
            BenchmarkSkipReason.NotConfigured(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),
        BenchmarkProviderAvailability.Skipped(
            BenchmarkExternalProviderDefinitions.Oracle,
            BenchmarkSkipReason.NotConfigured(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable)),
        BenchmarkProviderAvailability.Skipped(
            BenchmarkExternalProviderDefinitions.Db2,
            BenchmarkSkipReason.NotConfigured(BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable)),
    };

    try {
      Console.SetOut(output);

      await BenchmarkRunner
          .RunAsync(options, postgresAvailability, optionalProviders, CancellationToken.None)
          .ConfigureAwait(false);
    }
    finally {
      Console.SetOut(originalOutput);
    }

    return output.ToString();
  }

  private static VerifiedBenchmarkArtifacts VerifyBenchmarkArtifactTriplet(
      string markdown,
      string csv,
      string json) {
    using var document = JsonDocument.Parse(json);
    var context = ParseBenchmarkContext(document.RootElement.GetProperty("context"));
    var jsonRows = ParseBenchmarkJsonRows(document.RootElement.GetProperty("results"), context);
    var csvRows = ParseBenchmarkCsv(csv);
    var markdownRows = ParseBenchmarkMarkdown(markdown);

    AssertMarkdownContextMatchesJson(markdown, context, jsonRows.Count);
    AssertTripletRowsMatch(jsonRows, csvRows, "benchmark-summary.csv");
    AssertTripletRowsMatch(jsonRows, markdownRows, "benchmark-summary.md");

    return new VerifiedBenchmarkArtifacts(context, jsonRows);
  }

  private static BenchmarkArtifactContext ParseBenchmarkContext(JsonElement context) {
    var hashKeyVariants = context.TryGetProperty("hashKeyVariants", out var hashKeyVariantsProperty)
        ? ParseHashKeyVariantContext(hashKeyVariantsProperty)
        : [];
    var optionalProviders = context.GetProperty("optionalProviders")
        .EnumerateArray()
        .Select(provider => new BenchmarkOptionalProviderContext(
            GetRequiredString(provider, "providerName"),
            GetRequiredString(provider, "connectionStringEnvironmentVariable"),
            GetRequiredString(provider, "executionStatus"),
            GetRequiredString(provider, "skipReason")))
        .ToArray();

    var optionalProvidersByName = new Dictionary<string, BenchmarkOptionalProviderContext>(StringComparer.Ordinal);
    foreach (var provider in optionalProviders) {
      Assert.False(string.IsNullOrWhiteSpace(provider.ProviderName), "benchmark-summary.json context has an optional provider with a blank providerName.");
      Assert.False(
          string.IsNullOrWhiteSpace(provider.ConnectionStringEnvironmentVariable),
          "benchmark-summary.json context has an optional provider with a blank connectionStringEnvironmentVariable.");
      Assert.True(
          optionalProvidersByName.TryAdd(provider.ProviderName, provider),
          "benchmark-summary.json context has a duplicate optional provider '" + provider.ProviderName + "'.");
    }

    return new BenchmarkArtifactContext(
        GetRequiredString(context, "provider"),
        GetRequiredInt32(context, "iterations"),
        GetRequiredInt32(context, "warmupIterations"),
        GetRequiredString(context, "loadTimestampStorage"),
        GetRequiredString(context, "providerFilter"),
        GetRequiredString(context, "osDescription"),
        GetRequiredString(context, "osArchitecture"),
        GetRequiredString(context, "processArchitecture"),
        GetRequiredInt32(context, "processorCount"),
        GetRequiredString(context, "dotNetRuntimeDescription"),
        GetRequiredString(context, "dotNetRuntimeVersion"),
        hashKeyVariants,
        optionalProvidersByName);
  }

  private static IReadOnlyDictionary<string, BenchmarkArtifactRow> ParseBenchmarkJsonRows(
      JsonElement results,
      BenchmarkArtifactContext context) {
    var rows = results
        .EnumerateArray()
        .Select(result => ParseBenchmarkJsonRow(result, context))
        .ToArray();

    return ToRowDictionary(rows, "benchmark-summary.json");
  }

  private static BenchmarkArtifactRow ParseBenchmarkJsonRow(
      JsonElement result,
      BenchmarkArtifactContext context) {
    var executionStatus = GetRequiredString(result, "executionStatus");
    var row = new BenchmarkArtifactRow(
        GetRequiredString(result, "scenarioName"),
        GetRequiredString(result, "provider"),
        GetRequiredString(result, "baselineName"),
        GetRequiredString(result, "strategyFamily"),
        GetRequiredString(result, "datasetSize"),
        GetRequiredString(result, "changeRatio"),
        executionStatus,
        GetRequiredString(result, "skipReason"),
        GetRequiredInt32(result, "iterations"),
        FormatJsonNumber(result, "meanMilliseconds", "F3"),
        FormatJsonNumber(result, "minMilliseconds", "F3"),
        FormatJsonNumber(result, "maxMilliseconds", "F3"),
        FormatJsonNumber(result, "meanAllocatedBytes", "F0"),
        FormatJsonNumber(result, "minAllocatedBytes", "F0"),
        FormatJsonNumber(result, "maxAllocatedBytes", "F0"),
        GetRequiredString(result, "executionDetail"),
        GetRequiredString(result, "persistedOutcome"));

    Assert.False(string.IsNullOrWhiteSpace(row.ScenarioName), "benchmark-summary.json has a row with a blank scenarioName.");
    Assert.False(string.IsNullOrWhiteSpace(row.ProviderName), "benchmark-summary.json row '" + row.Key + "' has a blank provider.");
    Assert.False(string.IsNullOrWhiteSpace(row.BaselineName), "benchmark-summary.json has a row with a blank baselineName.");
    Assert.False(string.IsNullOrWhiteSpace(row.StrategyFamily), "benchmark-summary.json row '" + row.Key + "' has a blank strategyFamily.");
    Assert.False(string.IsNullOrWhiteSpace(row.DatasetSize), "benchmark-summary.json row '" + row.Key + "' has a blank datasetSize.");
    Assert.False(string.IsNullOrWhiteSpace(row.ChangeRatio), "benchmark-summary.json row '" + row.Key + "' has a blank changeRatio.");
    Assert.False(string.IsNullOrWhiteSpace(row.ExecutionDetail), "benchmark-summary.json row '" + row.Key + "' has a blank executionDetail.");
    Assert.False(string.IsNullOrWhiteSpace(row.PersistedOutcome), "benchmark-summary.json row '" + row.Key + "' has a blank persistedOutcome.");

    if (row.ExecutionStatus == "completed") {
      Assert.Equal(context.Iterations, row.Iterations);
      Assert.Equal(string.Empty, row.SkipReason);
      AssertCompletedMetricsPresent(row);
    }
    else {
      Assert.Contains(row.ExecutionStatus, new[] { "skipped", "failed" });
      Assert.Equal(0, row.Iterations);
      Assert.False(string.IsNullOrWhiteSpace(row.SkipReason), "benchmark-summary.json row '" + row.Key + "' has no skip/failure reason.");
      Assert.Equal("not executed", row.PersistedOutcome);
      AssertSkippedMetricsBlank(row);
      AssertJsonMetricNull(result, "meanMilliseconds");
      AssertJsonMetricNull(result, "minMilliseconds");
      AssertJsonMetricNull(result, "maxMilliseconds");
      AssertJsonMetricNull(result, "meanAllocatedBytes");
      AssertJsonMetricNull(result, "minAllocatedBytes");
      AssertJsonMetricNull(result, "maxAllocatedBytes");
    }

    return row;
  }

  private static IReadOnlyDictionary<string, BenchmarkArtifactRow> ParseBenchmarkCsv(string csv) {
    var lines = NormalizeLineEndings(csv).Split('\n', StringSplitOptions.RemoveEmptyEntries);
    Assert.True(lines.Length > 1, "benchmark-summary.csv must contain a header and at least one result row.");
    Assert.Equal(BenchmarkCsvHeader, lines[0]);

    var rows = lines
        .Skip(1)
        .Select((line, index) => ParseBenchmarkDelimitedRow(ParseCsvLine(line), "benchmark-summary.csv line " + (index + 2).ToString(CultureInfo.InvariantCulture)))
        .ToArray();

    return ToRowDictionary(rows, "benchmark-summary.csv");
  }

  private static IReadOnlyDictionary<string, BenchmarkArtifactRow> ParseBenchmarkMarkdown(string markdown) {
    var lines = NormalizeLineEndings(markdown).Split('\n');
    var headerIndex = Array.FindIndex(lines, line => string.Equals(line, BenchmarkMarkdownHeader, StringComparison.Ordinal));
    Assert.True(headerIndex >= 0, "benchmark-summary.md is missing the benchmark result table header.");
    Assert.True(headerIndex + 1 < lines.Length, "benchmark-summary.md is missing the benchmark result table separator.");
    Assert.Equal(BenchmarkMarkdownSeparator, lines[headerIndex + 1]);

    var rows = lines
        .Skip(headerIndex + 2)
        .Where(line => line.StartsWith("| ", StringComparison.Ordinal))
        .Select((line, index) => ParseBenchmarkDelimitedRow(ParseMarkdownTableLine(line), "benchmark-summary.md result row " + (index + 1).ToString(CultureInfo.InvariantCulture)))
        .ToArray();

    return ToRowDictionary(rows, "benchmark-summary.md");
  }

  private static BenchmarkArtifactRow ParseBenchmarkDelimitedRow(string[] fields, string source) {
    Assert.Equal(17, fields.Length);

    var row = new BenchmarkArtifactRow(
        fields[0],
        fields[1],
        fields[2],
        fields[3],
        fields[4],
        fields[5],
        fields[6],
        fields[7],
        int.Parse(fields[8], CultureInfo.InvariantCulture),
        fields[9],
        fields[10],
        fields[11],
        fields[12],
        fields[13],
        fields[14],
        fields[15],
        fields[16]);

    Assert.False(string.IsNullOrWhiteSpace(row.ExecutionDetail), source + " row '" + row.Key + "' has a blank execution detail.");
    Assert.False(string.IsNullOrWhiteSpace(row.PersistedOutcome), source + " row '" + row.Key + "' has a blank persisted outcome.");

    if (row.ExecutionStatus == "skipped" || row.ExecutionStatus == "failed") {
      Assert.Equal(0, row.Iterations);
      Assert.False(string.IsNullOrWhiteSpace(row.SkipReason), source + " row '" + row.Key + "' has no skip/failure reason.");
      Assert.Equal("not executed", row.PersistedOutcome);
      AssertSkippedMetricsBlank(row);
    }
    else {
      Assert.Equal("completed", row.ExecutionStatus);
      Assert.Equal(string.Empty, row.SkipReason);
      AssertCompletedMetricsPresent(row);
    }

    return row;
  }

  private static IReadOnlyDictionary<string, BenchmarkArtifactRow> ToRowDictionary(
      IEnumerable<BenchmarkArtifactRow> rows,
      string source) {
    var rowsByKey = new Dictionary<string, BenchmarkArtifactRow>(StringComparer.Ordinal);
    foreach (var row in rows) {
      Assert.True(
          rowsByKey.TryAdd(row.Key, row),
          source + " contains a duplicate benchmark row for '" + row.Key + "'.");
    }

    return rowsByKey;
  }

  private static void AssertTripletRowsMatch(
      IReadOnlyDictionary<string, BenchmarkArtifactRow> expectedRows,
      IReadOnlyDictionary<string, BenchmarkArtifactRow> actualRows,
      string source) {
    var expectedKeys = expectedRows.Keys.Order(StringComparer.Ordinal).ToArray();
    var actualKeys = actualRows.Keys.Order(StringComparer.Ordinal).ToArray();
    Assert.Equal(expectedKeys, actualKeys);

    foreach (var key in expectedKeys) {
      Assert.Equal(expectedRows[key].ToArtifactFields(), actualRows[key].ToArtifactFields());
    }
  }

  private static void AssertMarkdownContextMatchesJson(
      string markdown,
      BenchmarkArtifactContext context,
      int resultRowCount) {
    Assert.Contains("# DVault Benchmark Summary", markdown, StringComparison.Ordinal);
    Assert.Contains(
        "- Benchmark baselines: " + resultRowCount.ToString(CultureInfo.InvariantCulture),
        markdown,
        StringComparison.Ordinal);
    Assert.Contains("- Required provider: " + context.Provider, markdown, StringComparison.Ordinal);
    Assert.Contains("- Iterations: " + context.Iterations.ToString(CultureInfo.InvariantCulture), markdown, StringComparison.Ordinal);
    Assert.Contains("- Warmup iterations: " + context.WarmupIterations.ToString(CultureInfo.InvariantCulture), markdown, StringComparison.Ordinal);
    Assert.Contains("- Load timestamp storage: " + context.LoadTimestampStorage, markdown, StringComparison.Ordinal);
    Assert.Contains("- Provider filter: " + context.ProviderFilter, markdown, StringComparison.Ordinal);
    if (context.HashKeyVariants.Count > 0) {
      Assert.Contains(
          "- Hash key variants: " + string.Join(", ", context.HashKeyVariants.Select(variant => variant.Label)),
          markdown,
          StringComparison.Ordinal);
    }

    Assert.Contains("- OS description: " + context.OsDescription, markdown, StringComparison.Ordinal);
    Assert.Contains("- OS architecture: " + context.OsArchitecture, markdown, StringComparison.Ordinal);
    Assert.Contains("- Process architecture: " + context.ProcessArchitecture, markdown, StringComparison.Ordinal);
    Assert.Contains("- Processor count: " + context.ProcessorCount.ToString(CultureInfo.InvariantCulture), markdown, StringComparison.Ordinal);
    Assert.Contains("- .NET runtime description: " + context.DotNetRuntimeDescription, markdown, StringComparison.Ordinal);
    Assert.Contains("- .NET runtime version: " + context.DotNetRuntimeVersion, markdown, StringComparison.Ordinal);

    foreach (var provider in context.OptionalProviders.Values) {
      var expectedLine = "  - " + provider.ProviderName + ": " + provider.ExecutionStatus;
      if (!string.IsNullOrEmpty(provider.SkipReason)) {
        expectedLine += " - " + provider.SkipReason;
      }

      Assert.Contains(expectedLine, markdown, StringComparison.Ordinal);
    }
  }

  private static void AssertExpectedRootBenchmarkRows(VerifiedBenchmarkArtifacts artifacts) {
    Assert.Equal(SqliteProviderName, artifacts.Context.Provider);
    Assert.Equal(ExpectedRows.Length, artifacts.RowsByKey.Count);
    AssertHashKeyVariantContext(artifacts.Context.HashKeyVariants, [BenchmarkHashKeyVariant.Default]);
    AssertOptionalProviderContext(
        artifacts.Context.OptionalProviders,
        PostgresProviderName,
        BenchmarkExternalProviderDefinitions.Postgres.ConnectionStringEnvironmentVariable,
        NotConfiguredSkipReason);
    AssertOptionalProviderContext(
        artifacts.Context.OptionalProviders,
        SqlServerProviderName,
        BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable,
        NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable));
    AssertOptionalProviderContext(
        artifacts.Context.OptionalProviders,
        MySqlProviderName,
        BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable,
        NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable));
    AssertOptionalProviderContext(
        artifacts.Context.OptionalProviders,
        OracleProviderName,
        BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable,
        NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable));
    AssertOptionalProviderContext(
        artifacts.Context.OptionalProviders,
        Db2ProviderName,
        BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable,
        NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Db2.ConnectionStringEnvironmentVariable));

    foreach (var expectedRow in ExpectedRows) {
      var row = FindArtifactRow(
          artifacts,
          expectedRow.ProviderName,
          expectedRow.ScenarioName,
          expectedRow.BaselineName);

      Assert.Equal(expectedRow.StrategyFamily, row.StrategyFamily);
      Assert.Equal(expectedRow.DatasetSize, row.DatasetSize);
      Assert.Equal(expectedRow.ChangeRatio, row.ChangeRatio);
      Assert.Equal(expectedRow.ExecutionStatus, row.ExecutionStatus);
      Assert.Equal(expectedRow.SkipReason, row.SkipReason);
      Assert.Equal(
          expectedRow.ExecutionStatus == "completed" ? artifacts.Context.Iterations : 0,
          row.Iterations);
    }

    AssertProviderReadRowsStayVisibleAsSkipped(artifacts);
    AssertRootHashKeyVariantRowsIncludeExecutionDetail(artifacts);
  }

  private static void AssertRootHashKeyVariantRowsIncludeExecutionDetail(VerifiedBenchmarkArtifacts artifacts) {
    var variant = Assert.Single(artifacts.Context.HashKeyVariants);
    var expectedFragments = new[] {
        "hashKeyVariant=" + variant.Label,
        "stableHashAlgorithm=" + variant.StableHashAlgorithmId,
        "digestBytes=" + variant.DigestByteLength.ToString(CultureInfo.InvariantCulture),
        "hashKeyStorage=" + variant.StorageProfile,
        "hashKeyPayloadBytes=" + variant.HashKeyPayloadBytes.ToString(CultureInfo.InvariantCulture),
    };

    foreach (var row in artifacts.RowsByKey.Values.Where(IsHashKeyVariantBenchmarkRow)) {
      foreach (var fragment in expectedFragments) {
        Assert.Contains(fragment, row.ExecutionDetail, StringComparison.Ordinal);
      }
    }
  }

  private static bool IsHashKeyVariantBenchmarkRow(BenchmarkArtifactRow row) {
    return row.BaselineName.StartsWith("dvault-adddvault", StringComparison.Ordinal);
  }

  private static void AssertProviderReadRowsStayVisibleAsSkipped(VerifiedBenchmarkArtifacts artifacts) {
    foreach (var expectedRow in ExpectedProviderReadRows) {
      var row = FindArtifactRow(
          artifacts,
          expectedRow.ProviderName,
          expectedRow.ScenarioName,
          expectedRow.BaselineName);

      Assert.Equal("skipped", row.ExecutionStatus);
      Assert.Equal(0, row.Iterations);
      Assert.Equal("not executed", row.PersistedOutcome);
      Assert.False(string.IsNullOrWhiteSpace(row.SkipReason), "Provider read row '" + row.Key + "' has no skip reason.");
      AssertSkippedMetricsBlank(row);
      foreach (var fragment in expectedRow.RequiredExecutionDetailFragments) {
        Assert.Contains(fragment, row.ExecutionDetail, StringComparison.Ordinal);
      }
    }
  }

  private static void AssertProviderPitBridgeAuditRow(
      ProviderPitBridgeAuditRow provider,
      string gapMatrix,
      string evidenceMatrix,
      string pitBridgeBoundary,
      VerifiedBenchmarkArtifacts artifacts) {
    var bridgePriority = "P3" + provider.PitPriority[2..];
    Assert.Contains(
        "| " + provider.PitPriority + " | Closed evidence row | " + provider.ProviderName + " | `pit-as-of-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| " + bridgePriority + " | Closed evidence row | " + provider.ProviderName + " | `bridge-traversal-read`",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "`" + provider.StrategyName + "` for supported maintained PIT shapes.",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "`" + provider.StrategyName + "` for supported maintained bridge shapes.",
        gapMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `pit-as-of-read` | " + provider.ProviderName + " | `" + provider.BaselineName + "`",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "| `bridge-traversal-read` | " + provider.ProviderName + " | `" + provider.BaselineName + "`",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "Guidance row records planned `" + provider.StrategyName + "` for diagnostics-gated PIT reads.",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(
        "Guidance row records planned `" + provider.StrategyName + "` for diagnostics-gated bridge reads.",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains(provider.ExtensionName, pitBridgeBoundary, StringComparison.Ordinal);

    AssertProviderReadPlaceholder(
        FindArtifactRow(artifacts, provider.ProviderName, "pit-as-of-read", provider.BaselineName),
        provider.StrategyName,
        "PitAsOf",
        provider.ConnectionStringEnvironmentVariable);
    AssertProviderReadPlaceholder(
        FindArtifactRow(artifacts, provider.ProviderName, "bridge-traversal-read", provider.BaselineName),
        provider.StrategyName,
        "Bridge",
        provider.ConnectionStringEnvironmentVariable);
  }

  private static void AssertProviderReadPlaceholder(
      BenchmarkArtifactRow row,
      string strategyName,
      string readShape,
      string connectionStringEnvironmentVariable) {
    Assert.Equal("skipped", row.ExecutionStatus);
    Assert.Equal(0, row.Iterations);
    Assert.Equal(NotConfiguredSkipReasonFor(connectionStringEnvironmentVariable), row.SkipReason);
    Assert.Equal("not executed", row.PersistedOutcome);
    AssertSkippedMetricsBlank(row);
    Assert.Contains("readShape=" + readShape, row.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("selectedStrategy=" + strategyName, row.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("plannedReadStrategy=" + strategyName, row.ExecutionDetail, StringComparison.Ordinal);
  }

  private static void AssertCompletedProviderReadRow(
      BenchmarkArtifactRow row,
      string expectedStrategyName,
      string expectedReadShape,
      string providerDisplayName) {
    Assert.Equal("completed", row.ExecutionStatus);
    Assert.True(
        row.Iterations > 0,
        "Completed " + providerDisplayName + " read row '" + row.Key + "' has no iterations.");
    Assert.True(
        string.IsNullOrEmpty(row.SkipReason),
        "Completed " + providerDisplayName + " read row '" + row.Key + "' has a skip reason.");
    Assert.Contains("selectedStrategy=" + expectedStrategyName, row.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("plannedReadStrategy=" + expectedStrategyName, row.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("readShape=" + expectedReadShape, row.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("readStrategyStatus=ProviderStrategySelected", row.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("readShapeProviderStatus=ProviderStrategySelected", row.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("fallbackCauses=none", row.ExecutionDetail, StringComparison.Ordinal);
    Assert.Contains("readShapeFallbackCauses=none", row.ExecutionDetail, StringComparison.Ordinal);
  }

  private static void AssertCompletedOracleReadArtifactRow(
      VerifiedBenchmarkArtifacts artifacts,
      string scenarioName,
      string expectedMeanMilliseconds,
      string expectedExecutionPath,
      string expectedReadShape) {
    var row = FindArtifactRow(
        artifacts,
        OracleProviderName,
        scenarioName,
        "dvault-adddvaultoracle-optimized");
    var manifestRow = CreateBenchmarkBackedProviderEvidenceManifestRow(row, "completed-timing");

    AssertCompletedProviderReadRow(row, "OracleDataVaultReadStrategy", expectedReadShape, "Oracle");
    Assert.Equal(artifacts.Context.Iterations, row.Iterations);
    Assert.Equal(expectedMeanMilliseconds, row.MeanMilliseconds);
    Assert.Equal("oracle-optimized-dvault", row.StrategyFamily);
    Assert.Equal("completed-timing", manifestRow.EvidencePosture);
    Assert.Equal("present", manifestRow.ResultSummary.MetricState);
    Assert.Equal(expectedReadShape, manifestRow.ReadShape);
    Assert.Equal(expectedExecutionPath, manifestRow.SelectedPath);
    Assert.Null(manifestRow.PlannedPath);
    Assert.Equal("OracleDataVaultReadStrategy", manifestRow.SelectedStrategy);
    Assert.Null(manifestRow.PlannedStrategy);
    Assert.Empty(manifestRow.FallbackCauses);
  }
  private static void AssertPerformanceGuidanceMatchesArtifacts(
      string guidance,
      VerifiedBenchmarkArtifacts artifacts) {
    Assert.Contains("- [benchmark-summary.md](../benchmark-summary.md)", guidance, StringComparison.Ordinal);
    Assert.Contains("- [benchmark-summary.csv](../benchmark-summary.csv)", guidance, StringComparison.Ordinal);
    Assert.Contains("- [benchmark-summary.json](../benchmark-summary.json)", guidance, StringComparison.Ordinal);

    Assert.Contains(
        "- " +
        FormatCount(artifacts.Context.Iterations, "iteration", "iterations") +
        " and " +
        FormatCount(artifacts.Context.WarmupIterations, "warmup iteration", "warmup iterations") +
        ".",
        guidance,
        StringComparison.Ordinal);
    Assert.Contains("- Load timestamp storage `" + artifacts.Context.LoadTimestampStorage + "`.", guidance, StringComparison.Ordinal);
    Assert.Contains("- Provider filter `" + artifacts.Context.ProviderFilter + "`.", guidance, StringComparison.Ordinal);
    Assert.Contains(
        "- " +
        artifacts.Context.OsDescription +
        ", " +
        artifacts.Context.OsArchitecture +
        " OS and process architecture, " +
        artifacts.Context.ProcessorCount.ToString(CultureInfo.InvariantCulture) +
        " processors.",
        guidance,
        StringComparison.Ordinal);
    Assert.Contains("- " + artifacts.Context.DotNetRuntimeDescription + ".", guidance, StringComparison.Ordinal);
    Assert.Contains("- Required provider `" + artifacts.Context.Provider + "`.", guidance, StringComparison.Ordinal);
    Assert.Contains(
        "Optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 rows emitted as `executionStatus=skipped`",
        guidance,
        StringComparison.Ordinal);
    foreach (var provider in artifacts.Context.OptionalProviders.Values) {
      Assert.Contains("`" + provider.ConnectionStringEnvironmentVariable + "`", guidance, StringComparison.Ordinal);
    }

    foreach (var profile in ExpectedPerformanceProfiles) {
      Assert.Contains("| " + profile.ProfileName + " |", guidance, StringComparison.Ordinal);
      Assert.Contains("## " + profile.DocumentHeading, guidance, StringComparison.Ordinal);
    }

    foreach (var expectedRow in ExpectedPerformanceGuidanceRows) {
      var row = FindArtifactRow(artifacts, SqliteProviderName, expectedRow.ScenarioName, expectedRow.BaselineName);
      Assert.Equal("completed", row.ExecutionStatus);
      Assert.Contains(
          "| `" + row.ScenarioName + "` | `" + row.BaselineName + "` | " + row.MeanMilliseconds + " |",
          guidance,
          StringComparison.Ordinal);
    }

    foreach (var expectedRow in ExpectedProviderGuidanceRows) {
      var row = FindArtifactRow(
          artifacts,
          expectedRow.ProviderName,
          "provider-native-bulk-ingestion",
          expectedRow.BaselineName);

      Assert.Equal("skipped", row.ExecutionStatus);
      Assert.Equal(0, row.Iterations);
      Assert.Equal("not executed", row.PersistedOutcome);
      Assert.Contains("`" + row.BaselineName + "`", guidance, StringComparison.Ordinal);
      foreach (var fragment in expectedRow.RequiredExecutionDetailFragments) {
        Assert.Contains(fragment, row.ExecutionDetail, StringComparison.Ordinal);
      }
    }
  }

  private static void AssertProviderTuningProfileCategoriesMatchGuidance(string guidance) {
    var expectedCategories = ExpectedPerformanceProfiles
        .Select(profile => profile.Category)
        .ToArray();
    var actualCategories = Enum.GetValues<DataVaultPerformanceProfileCategory>();
    Assert.Equal(expectedCategories, actualCategories);

    foreach (var profile in ExpectedPerformanceProfiles) {
      Assert.Contains(profile.ProfileName, guidance, StringComparison.Ordinal);
    }
  }

  private static void AssertFootprintRow(
      IReadOnlyList<BenchmarkHashKeyFootprintRow> rows,
      string variant,
      string expectedHashKeyStoreType,
      string expectedValueFormat,
      int expectedPayloadBytes) {
    var row = Assert.Single(rows, candidate => candidate.Variant == variant);

    Assert.Equal(SqliteProviderName, row.Provider);
    Assert.Equal(expectedHashKeyStoreType, row.HashKeyStoreType);
    Assert.Equal(expectedHashKeyStoreType, row.ParticipantReferenceStoreType);
    Assert.Equal(expectedValueFormat, row.HashKeyValueFormat);
    Assert.Equal(expectedValueFormat, row.ParticipantReferenceValueFormat);
    Assert.Equal(expectedPayloadBytes, row.HashKeyPayloadBytes);
    Assert.Equal(expectedPayloadBytes, row.ParentHashReferencePayloadBytes);
    Assert.Equal(expectedPayloadBytes * 2, row.TwoColumnHashReferenceIndexPayloadBytes);
    Assert.Equal(1, row.CompletedRows);
    Assert.Equal(0, row.SkippedRows);
    Assert.Equal(0, row.FailedRows);
  }

  private static void AssertRegressionBudgetDefaultsAreDocumented(string benchmarkContract) {
    Assert.Contains("## Regression Budget", benchmarkContract, StringComparison.Ordinal);
    foreach (var rule in RegressionBudgetRules) {
      Assert.Contains(rule, benchmarkContract, StringComparison.Ordinal);
    }
  }

  private static void AssertProviderEvidenceManifestContractIsDocumented(
      string evidenceMatrix,
      string benchmarkContract,
      string benchmarkReadme,
      string performanceProfiles) {
    Assert.Contains("## Provider Evidence Manifest V1", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains(ProviderEvidenceManifestSchemaVersion, evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`schemaVersion` | string | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`rows` | array | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`scenario` | string | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`provider` | string | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`baseline` | string | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`strategyFamily` | string or null | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`datasetSize` | string or null | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`changeRatio` | string or null | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`sourceArtifacts` | array of strings | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`evidencePosture` | string | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`executionStatus` | string or null | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`skipReason` | string or null | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`workloadShape` | string or null | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`readShape` | string or null | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`selectedPath` | string or null | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`plannedPath` | string or null | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`selectedStrategy` | string or null | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`plannedStrategy` | string or null | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`fallbackCauses` | array of strings | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`resultSummary` | object | Required.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`metricState`: `present`, `not-executed`, `not-applicable`.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`readShape`: `LatestSatellite`, `PitAsOf`, `Bridge`, or `null`.", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains(
        "Provider facts map from deterministic `executionDetail` tokens emitted by `BenchmarkExecutionDetails`, not arbitrary prose.",
        evidenceMatrix,
        StringComparison.Ordinal);
    Assert.Contains("`executionPath` supplies `selectedPath`", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("`plannedReadStrategy` supplies `plannedStrategy`", evidenceMatrix, StringComparison.Ordinal);
    Assert.Contains("treat `none` as `[]`", evidenceMatrix, StringComparison.Ordinal);

    Assert.Contains("## Provider Evidence Manifest Alignment", benchmarkContract, StringComparison.Ordinal);
    Assert.Contains(ProviderEvidenceManifestSchemaVersion, benchmarkContract, StringComparison.Ordinal);
    Assert.Contains("deterministic `executionDetail` tokens", benchmarkContract, StringComparison.Ordinal);
    Assert.Contains(ProviderEvidenceManifestSchemaVersion, benchmarkReadme, StringComparison.Ordinal);
    Assert.Contains("provider-optimization-evidence-matrix.md", benchmarkReadme, StringComparison.Ordinal);
    Assert.Contains(ProviderEvidenceManifestSchemaVersion, performanceProfiles, StringComparison.Ordinal);
  }

  private static ProviderEvidenceManifestRow CreateBenchmarkBackedProviderEvidenceManifestRow(
      BenchmarkArtifactRow row,
      string evidencePosture) {
    var detailTokens = ParseExecutionDetailTokens(row.ExecutionDetail);
    var executionPath = GetRequiredExecutionDetailToken(detailTokens, "executionPath", row);
    var readShape = ToManifestNullableValue(GetOptionalExecutionDetailToken(detailTokens, "readShape"));
    var isCompleted = string.Equals(row.ExecutionStatus, "completed", StringComparison.Ordinal);
    var selectedStrategy = isCompleted
        ? ToManifestNullableValue(GetOptionalExecutionDetailToken(detailTokens, "selectedStrategy"))
        : null;
    var plannedStrategy = isCompleted
        ? null
        : ToManifestNullableValue(
            GetOptionalExecutionDetailToken(detailTokens, "plannedReadStrategy") ??
            GetOptionalExecutionDetailToken(detailTokens, "selectedStrategy"));

    return new ProviderEvidenceManifestRow(
        row.ScenarioName,
        row.ProviderName,
        row.BaselineName,
        row.StrategyFamily,
        row.DatasetSize,
        row.ChangeRatio,
        new[] { "benchmark-summary.md", "benchmark-summary.csv", "benchmark-summary.json" },
        evidencePosture,
        row.ExecutionStatus,
        string.IsNullOrEmpty(row.SkipReason) ? null : row.SkipReason,
        readShape is null ? row.ScenarioName : null,
        readShape,
        isCompleted ? executionPath : null,
        isCompleted ? null : executionPath,
        selectedStrategy,
        plannedStrategy,
        CollectFallbackCauses(detailTokens),
        new ProviderEvidenceManifestResultSummary(
            row.Iterations,
            isCompleted ? "present" : "not-executed",
            row.PersistedOutcome,
            CreateProviderEvidenceSummary(row, detailTokens)));
  }

  private static ProviderEvidenceManifestRow CreateDocsOnlyProviderEvidenceManifestRow() {
    return new ProviderEvidenceManifestRow(
        "pit-as-of-read",
        "DB2 external provider",
        "AddDVaultDb2() / Db2DataVaultReadStrategy",
        "db2-optimized-dvault",
        null,
        null,
        new[]
        {
            "docs/releases/v0.34.0.md",
            "tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs",
        },
        "diagnostics-only",
        null,
        null,
        null,
        "PitAsOf",
        null,
        "diagnostics-gated DB2 PIT read candidate",
        null,
        "Db2DataVaultReadStrategy",
        Array.Empty<string>(),
        new ProviderEvidenceManifestResultSummary(
            null,
            "not-applicable",
            null,
            "docs-owned diagnostics posture; no DB2 benchmark timing row claimed"));
  }

  private static IReadOnlyDictionary<string, string> ParseExecutionDetailTokens(string executionDetail) {
    var tokens = new Dictionary<string, string>(StringComparer.Ordinal);
    foreach (var segment in executionDetail.Split("; ", StringSplitOptions.None)) {
      var separatorIndex = segment.IndexOf('=', StringComparison.Ordinal);
      if (separatorIndex <= 0) {
        continue;
      }

      tokens[segment[..separatorIndex]] = segment[(separatorIndex + 1)..];
    }

    return tokens;
  }

  private static string GetRequiredExecutionDetailToken(
      IReadOnlyDictionary<string, string> detailTokens,
      string name,
      BenchmarkArtifactRow row) {
    var value = GetOptionalExecutionDetailToken(detailTokens, name);
    Assert.False(
        string.IsNullOrWhiteSpace(value),
        "Benchmark row '" + row.Key + "' is missing executionDetail token '" + name + "'.");

    return value!;
  }

  private static string? GetOptionalExecutionDetailToken(
      IReadOnlyDictionary<string, string> detailTokens,
      string name) {
    return detailTokens.TryGetValue(name, out var value) ? value : null;
  }

  private static string? ToManifestNullableValue(string? value) {
    if (string.IsNullOrWhiteSpace(value) ||
        string.Equals(value, "<none>", StringComparison.Ordinal) ||
        string.Equals(value, "none", StringComparison.Ordinal)) {
      return null;
    }

    return value;
  }

  private static string[] CollectFallbackCauses(IReadOnlyDictionary<string, string> detailTokens) {
    var causes = new List<string>();
    AddFallbackCauses(detailTokens, "fallbackCauses", causes);
    AddFallbackCauses(detailTokens, "readShapeFallbackCauses", causes);
    AddFallbackCauses(detailTokens, "stagedProviderBulkFallbackCauses", causes);

    return causes
        .Distinct(StringComparer.Ordinal)
        .ToArray();
  }

  private static void AddFallbackCauses(
      IReadOnlyDictionary<string, string> detailTokens,
      string name,
      List<string> causes) {
    if (!detailTokens.TryGetValue(name, out var value)) {
      return;
    }

    foreach (var cause in value.Split('|', StringSplitOptions.RemoveEmptyEntries)) {
      var manifestCause = ToManifestNullableValue(cause);
      if (manifestCause is not null) {
        causes.Add(manifestCause);
      }
    }
  }

  private static string CreateProviderEvidenceSummary(
      BenchmarkArtifactRow row,
      IReadOnlyDictionary<string, string> detailTokens) {
    var boundaryTokens = new[]
    {
        "transfer",
        "nativeBulkBoundary",
        "stagedBulkBoundary",
        "smallBatchBoundary",
        "oracleBulkBoundary",
        "stagedOracleBulk",
        "db2SaveBoundary",
        "cleanupBoundary",
        "providerSpecificReadStrategy",
    };
    var boundaries = boundaryTokens
        .Select(name => detailTokens.TryGetValue(name, out var value) ? name + "=" + value : null)
        .Where(value => value is not null)
        .ToArray();

    if (boundaries.Length == 0) {
      return row.ExecutionStatus + " provider evidence row";
    }

    return string.Join("; ", boundaries!);
  }

  private static BenchmarkArtifactRow FindArtifactRow(
      VerifiedBenchmarkArtifacts artifacts,
      string providerName,
      string scenarioName,
      string baselineName) {
    var key = CreateBenchmarkRowKey(scenarioName, providerName, baselineName);
    Assert.True(
        artifacts.RowsByKey.TryGetValue(key, out var row),
        "benchmark-summary artifact triplet is missing row '" + key + "'.");

    return row;
  }

  private static void AssertOptionalProviderContext(
      IReadOnlyDictionary<string, BenchmarkOptionalProviderContext> optionalProviders,
      string providerName,
      string connectionStringEnvironmentVariable,
      string skipReason) {
    Assert.True(
        optionalProviders.TryGetValue(providerName, out var provider),
        "benchmark-summary.json context is missing optional provider '" + providerName + "'.");
    Assert.Equal(connectionStringEnvironmentVariable, provider.ConnectionStringEnvironmentVariable);
    Assert.Equal("skipped", provider.ExecutionStatus);
    Assert.Equal(skipReason, provider.SkipReason);
  }

  private static void AssertHashKeyVariantContext(
      IReadOnlyList<BenchmarkHashKeyVariantArtifactContext> actualVariants,
      IReadOnlyList<BenchmarkHashKeyVariant> expectedVariants) {
    Assert.Equal(expectedVariants.Select(variant => variant.Label), actualVariants.Select(variant => variant.Label));

    foreach (var expectedVariant in expectedVariants) {
      var actualVariant = Assert.Single(actualVariants, variant => variant.Label == expectedVariant.Label);
      Assert.Equal(expectedVariant.StableHashAlgorithmId, actualVariant.StableHashAlgorithmId);
      Assert.Equal(expectedVariant.DigestByteLength, actualVariant.DigestByteLength);
      Assert.Equal(expectedVariant.HexCharacterLength, actualVariant.HexCharacterLength);
      Assert.Equal(expectedVariant.StorageProfile.ToString(), actualVariant.StorageProfile);
      Assert.Equal(expectedVariant.HashKeyPayloadBytes, actualVariant.HashKeyPayloadBytes);
    }
  }

  private static void AssertCompletedMetricsPresent(BenchmarkArtifactRow row) {
    Assert.False(string.IsNullOrWhiteSpace(row.MeanMilliseconds), "Completed row '" + row.Key + "' has no meanMilliseconds.");
    Assert.False(string.IsNullOrWhiteSpace(row.MinMilliseconds), "Completed row '" + row.Key + "' has no minMilliseconds.");
    Assert.False(string.IsNullOrWhiteSpace(row.MaxMilliseconds), "Completed row '" + row.Key + "' has no maxMilliseconds.");
    Assert.False(string.IsNullOrWhiteSpace(row.MeanAllocatedBytes), "Completed row '" + row.Key + "' has no meanAllocatedBytes.");
    Assert.False(string.IsNullOrWhiteSpace(row.MinAllocatedBytes), "Completed row '" + row.Key + "' has no minAllocatedBytes.");
    Assert.False(string.IsNullOrWhiteSpace(row.MaxAllocatedBytes), "Completed row '" + row.Key + "' has no maxAllocatedBytes.");
  }

  private static void AssertSkippedMetricsBlank(BenchmarkArtifactRow row) {
    Assert.Equal(string.Empty, row.MeanMilliseconds);
    Assert.Equal(string.Empty, row.MinMilliseconds);
    Assert.Equal(string.Empty, row.MaxMilliseconds);
    Assert.Equal(string.Empty, row.MeanAllocatedBytes);
    Assert.Equal(string.Empty, row.MinAllocatedBytes);
    Assert.Equal(string.Empty, row.MaxAllocatedBytes);
  }

  private static void AssertJsonMetricNull(JsonElement result, string propertyName) {
    Assert.Equal(JsonValueKind.Null, result.GetProperty(propertyName).ValueKind);
  }

  private static BenchmarkHashKeyVariantArtifactContext[] ParseHashKeyVariantContext(JsonElement hashKeyVariants) {
    Assert.Equal(JsonValueKind.Array, hashKeyVariants.ValueKind);

    return hashKeyVariants
        .EnumerateArray()
        .Select(variant => new BenchmarkHashKeyVariantArtifactContext(
            GetRequiredString(variant, "label"),
            GetRequiredString(variant, "stableHashAlgorithmId"),
            GetRequiredInt32(variant, "digestByteLength"),
            GetRequiredInt32(variant, "hexCharacterLength"),
            GetRequiredString(variant, "storageProfile"),
            GetRequiredInt32(variant, "hashKeyPayloadBytes")))
        .ToArray();
  }

  private static string[] ParseCsvLine(string line) {
    var values = new List<string>();
    var builder = new StringBuilder();
    var inQuotes = false;

    for (var index = 0; index < line.Length; index++) {
      var value = line[index];
      if (inQuotes) {
        if (value == '"' && index + 1 < line.Length && line[index + 1] == '"') {
          builder.Append('"');
          index++;
        }
        else if (value == '"') {
          inQuotes = false;
        }
        else {
          builder.Append(value);
        }
      }
      else if (value == ',') {
        values.Add(builder.ToString());
        builder.Clear();
      }
      else if (value == '"') {
        inQuotes = true;
      }
      else {
        builder.Append(value);
      }
    }

    Assert.False(inQuotes, "CSV row has an unterminated quoted value.");
    values.Add(builder.ToString());

    return [.. values];
  }

  private static string[] ParseMarkdownTableLine(string line) {
    Assert.StartsWith("| ", line);
    Assert.EndsWith(" |", line);

    return line[2..^2]
        .Split(" | ", StringSplitOptions.None)
        .Select(value => value.Replace("\\|", "|", StringComparison.Ordinal))
        .ToArray();
  }

  private static string GetRequiredString(JsonElement element, string propertyName) {
    var property = element.GetProperty(propertyName);
    Assert.Equal(JsonValueKind.String, property.ValueKind);

    return property.GetString() ?? string.Empty;
  }

  private static int GetRequiredInt32(JsonElement element, string propertyName) {
    var property = element.GetProperty(propertyName);
    Assert.Equal(JsonValueKind.Number, property.ValueKind);

    return property.GetInt32();
  }

  private static string FormatJsonNumber(JsonElement element, string propertyName, string format) {
    var property = element.GetProperty(propertyName);
    if (property.ValueKind == JsonValueKind.Null) {
      return string.Empty;
    }

    Assert.Equal(JsonValueKind.Number, property.ValueKind);

    return property.GetDouble().ToString(format, CultureInfo.InvariantCulture);
  }

  private static string CreateBenchmarkRowKey(
      string scenarioName,
      string providerName,
      string baselineName) {
    return scenarioName + "\u001f" + providerName + "\u001f" + baselineName;
  }

  private static string ReadRepositoryText(string relativePath) {
    var repositoryPath = Path.Combine(FindRepositoryRoot(), relativePath);
    Assert.True(File.Exists(repositoryPath), relativePath + " is missing from the repository root.");

    return File.ReadAllText(repositoryPath);
  }

  private static string FindRepositoryRoot() {
    var directory = new DirectoryInfo(AppContext.BaseDirectory);

    while (directory is not null) {
      if (File.Exists(Path.Combine(directory.FullName, "DVault.slnx"))) {
        return directory.FullName;
      }

      directory = directory.Parent;
    }

    throw new InvalidOperationException("Unable to locate the DVault repository root.");
  }

  private static string NormalizeLineEndings(string value) {
    return value.Replace("\r\n", "\n", StringComparison.Ordinal);
  }

  private static string FormatCount(int count, string singular, string plural) {
    return count.ToString(CultureInfo.InvariantCulture) + " " + (count == 1 ? singular : plural);
  }

  private static string CreateMarkdownRowPrefix(ExpectedBenchmarkRow expectedRow) {
    return "| " +
        expectedRow.ScenarioName +
        " | " +
        expectedRow.ProviderName +
        " | " +
        expectedRow.BaselineName +
        " | " +
        expectedRow.StrategyFamily +
        " | " +
        expectedRow.DatasetSize +
        " | " +
        expectedRow.ChangeRatio +
        " | " +
        expectedRow.ExecutionStatus +
        " | " +
        expectedRow.SkipReason +
        " | " +
        expectedRow.Iterations.ToString(CultureInfo.InvariantCulture) +
        " |";
  }

  private static string CreateCsvRowPrefix(ExpectedBenchmarkRow expectedRow) {
    return string.Join(
        ',',
        expectedRow.ScenarioName,
        expectedRow.ProviderName,
        expectedRow.BaselineName,
        expectedRow.StrategyFamily,
        EscapeCsv(expectedRow.DatasetSize),
        EscapeCsv(expectedRow.ChangeRatio),
        expectedRow.ExecutionStatus,
        EscapeCsv(expectedRow.SkipReason),
        expectedRow.Iterations.ToString(CultureInfo.InvariantCulture)) + ",";
  }

  private static string NotConfiguredSkipReason => BenchmarkSkipReason.NotConfigured().DisplayText;

  private static int ExpectedCompletedRowCount => ExpectedRows.Count(row => row.ExecutionStatus == "completed");

  private static int ExpectedSkippedRowCount => ExpectedRows.Count(row => row.ExecutionStatus == "skipped");

  private static string NotConfiguredSkipReasonFor(string connectionStringEnvironmentVariable) {
    return BenchmarkSkipReason.NotConfigured(connectionStringEnvironmentVariable).DisplayText;
  }

  private static void AssertOptionalProviderContext(
      JsonElement[] optionalProviders,
      string providerName,
      string connectionStringEnvironmentVariable,
      string skipReason) {
    var provider = Assert.Single(optionalProviders, candidate =>
        candidate.GetProperty("providerName").GetString() == providerName);

    Assert.Equal(connectionStringEnvironmentVariable, provider.GetProperty("connectionStringEnvironmentVariable").GetString());
    Assert.Equal("skipped", provider.GetProperty("executionStatus").GetString());
    Assert.Equal(skipReason, provider.GetProperty("skipReason").GetString());
  }

  private static ExpectedBenchmarkRow CompletedSqlite(
      string scenarioName,
      string baselineName,
      string strategyFamily,
      string datasetSize,
      string changeRatio) {
    return new ExpectedBenchmarkRow(
        scenarioName,
        SqliteProviderName,
        baselineName,
        strategyFamily,
        datasetSize,
        changeRatio,
        "completed",
        string.Empty,
        1);
  }

  private static ExpectedBenchmarkRow SkippedExternal(
      string providerName,
      string scenarioName,
      string baselineName,
      string strategyFamily,
      string datasetSize,
      string changeRatio,
      string skipReason) {
    return new ExpectedBenchmarkRow(
        scenarioName,
        providerName,
        baselineName,
        strategyFamily,
        datasetSize,
        changeRatio,
        "skipped",
        skipReason,
        0);
  }

  private static string EscapeCsv(string value) {
    if (!value.Contains('"') &&
        !value.Contains(',') &&
        !value.Contains('\r') &&
        !value.Contains('\n')) {
      return value;
    }

    return "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static DataVaultDiagnosticsResult CreateSaveStrategyDiagnostics(
      DataVaultSaveStrategyDiagnostics saveStrategy) {
    return CreateDiagnosticsResult(saveStrategy);
  }

  private static DataVaultDiagnosticsResult CreateDiagnosticsResult(
      DataVaultSaveStrategyDiagnostics saveStrategy) {
    return new DataVaultDiagnosticsResult(
        new DataVaultValidationDiagnostics(true, []),
        new DataVaultExplainDiagnostics(
            "unit-test",
            null,
            saveStrategy.ProviderName,
            "unit-test-profile",
            false,
            DataVaultProviderValueFormat.NativeDateTimeOffset,
            "datetime",
            "unit-test-behavior",
            false,
            []),
        saveStrategy,
        []);
  }

  private sealed record DiagnosticExecutionDetailBenchmark(
      string ScenarioName,
      string ProviderName,
      string BaselineName,
      string StrategyFamily,
      string DatasetSize,
      string ChangeRatio) : IScenarioBenchmark {
    public Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
      throw new NotSupportedException("This benchmark double is only used for execution detail formatting.");
    }
  }

  private sealed class BenchmarkExecutionDetailTestBenchmark : IScenarioBenchmark {
    public BenchmarkExecutionDetailTestBenchmark(
        string scenarioName,
        string providerName,
        string baselineName,
        string strategyFamily) {
      ScenarioName = scenarioName;
      ProviderName = providerName;
      BaselineName = baselineName;
      StrategyFamily = strategyFamily;
    }

    public string ScenarioName { get; }

    public string ProviderName { get; }

    public string BaselineName { get; }

    public string StrategyFamily { get; }

    public string DatasetSize => "unit-test dataset";

    public string ChangeRatio => "unit-test change ratio";

    public Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
      throw new NotSupportedException("The diagnostics detail test never executes the benchmark.");
    }
  }

  private sealed record ExpectedBenchmarkRow(
      string ScenarioName,
      string ProviderName,
      string BaselineName,
      string StrategyFamily,
      string DatasetSize,
      string ChangeRatio,
      string ExecutionStatus,
      string SkipReason,
      int Iterations);

  private sealed record ExpectedPerformanceProfile(
      DataVaultPerformanceProfileCategory Category,
      string ProfileName,
      string DocumentHeading);

  private sealed record ExpectedGuidanceRow(string ScenarioName, string BaselineName);

  private sealed record ExpectedProviderGuidanceRow(
      string ProviderName,
      string BaselineName,
      string[] RequiredExecutionDetailFragments);

  private sealed record ExpectedProviderReadRow(
      string ProviderName,
      string ScenarioName,
      string BaselineName,
      string[] RequiredExecutionDetailFragments);

  private sealed record ProviderPitBridgeAuditRow(
      string PitPriority,
      string ProviderName,
      string BaselineName,
      string StrategyName,
      string ExtensionName,
      string ConnectionStringEnvironmentVariable);

  private sealed record VerifiedBenchmarkArtifacts(
      BenchmarkArtifactContext Context,
      IReadOnlyDictionary<string, BenchmarkArtifactRow> RowsByKey);

  private sealed record BenchmarkArtifactContext(
      string Provider,
      int Iterations,
      int WarmupIterations,
      string LoadTimestampStorage,
      string ProviderFilter,
      string OsDescription,
      string OsArchitecture,
      string ProcessArchitecture,
      int ProcessorCount,
      string DotNetRuntimeDescription,
      string DotNetRuntimeVersion,
      IReadOnlyList<BenchmarkHashKeyVariantArtifactContext> HashKeyVariants,
      IReadOnlyDictionary<string, BenchmarkOptionalProviderContext> OptionalProviders);

  private sealed record BenchmarkHashKeyVariantArtifactContext(
      string Label,
      string StableHashAlgorithmId,
      int DigestByteLength,
      int HexCharacterLength,
      string StorageProfile,
      int HashKeyPayloadBytes);

  private sealed record BenchmarkOptionalProviderContext(
      string ProviderName,
      string ConnectionStringEnvironmentVariable,
      string ExecutionStatus,
      string SkipReason);

  private sealed record ProviderEvidenceManifestRow(
      string Scenario,
      string Provider,
      string Baseline,
      string? StrategyFamily,
      string? DatasetSize,
      string? ChangeRatio,
      IReadOnlyList<string> SourceArtifacts,
      string EvidencePosture,
      string? ExecutionStatus,
      string? SkipReason,
      string? WorkloadShape,
      string? ReadShape,
      string? SelectedPath,
      string? PlannedPath,
      string? SelectedStrategy,
      string? PlannedStrategy,
      IReadOnlyList<string> FallbackCauses,
      ProviderEvidenceManifestResultSummary ResultSummary);

  private sealed record ProviderEvidenceManifestResultSummary(
      int? Iterations,
      string MetricState,
      string? PersistedOutcome,
      string Summary);

  private sealed record BenchmarkArtifactRow(
      string ScenarioName,
      string ProviderName,
      string BaselineName,
      string StrategyFamily,
      string DatasetSize,
      string ChangeRatio,
      string ExecutionStatus,
      string SkipReason,
      int Iterations,
      string MeanMilliseconds,
      string MinMilliseconds,
      string MaxMilliseconds,
      string MeanAllocatedBytes,
      string MinAllocatedBytes,
      string MaxAllocatedBytes,
      string ExecutionDetail,
      string PersistedOutcome) {
    public string Key => CreateBenchmarkRowKey(ScenarioName, ProviderName, BaselineName);

    public string[] ToArtifactFields() {
      return
      [
          ScenarioName,
          ProviderName,
          BaselineName,
          StrategyFamily,
          DatasetSize,
          ChangeRatio,
          ExecutionStatus,
          SkipReason,
          Iterations.ToString(CultureInfo.InvariantCulture),
          MeanMilliseconds,
          MinMilliseconds,
          MaxMilliseconds,
          MeanAllocatedBytes,
          MinAllocatedBytes,
          MaxAllocatedBytes,
          ExecutionDetail,
          PersistedOutcome,
      ];
    }
  }
}
