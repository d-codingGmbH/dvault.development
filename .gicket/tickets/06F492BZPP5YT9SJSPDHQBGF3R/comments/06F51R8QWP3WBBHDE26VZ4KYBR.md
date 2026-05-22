[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 7/7 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F492BZPP5YT9SJSPDHQBGF3R-story-define-performance-evidence-and-benchmark\u0027 at commit \u002771747bb3d035\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F492BZPP5YT9SJSPDHQBGF3R-story-define-performance-evidence-and-benchmark",
    "commitSha": "71747bb3d035",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "The story defines one authoritative performance-evidence contract that downstream performance tickets must reuse instead of inventing ticket-specific benchmark formats.",
      "satisfied": true,
      "reason": "docs/plans/performance-evidence-benchmark-artifact-contract.md establishes the shared repository-facing contract, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md directs downstream performance work to reuse that contract instead of defining ticket-local formats."
    },
    {
      "expectation": "The contract ratifies the current shared artifact trio \u0060benchmark-summary.md\u0060, \u0060benchmark-summary.csv\u0060, and \u0060benchmark-summary.json\u0060 as required outputs for persisted benchmark evidence, and it requires before/after runs to be stored as two comparable artifact sets under one explicit scenario or ticket label.",
      "satisfied": true,
      "reason": "benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs writes benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json, and the contract document plus benchmark README require before/after evidence to be stored as two comparable artifact sets under one explicit label."
    },
    {
      "expectation": "The contract ratifies the current run-context baseline as required metadata: iterations, warmup iterations, load-timestamp storage, provider filter, OS description, OS architecture, process architecture, processor count, .NET runtime description/version, provider execution status, and provider skip reason when applicable.",
      "satisfied": true,
      "reason": "The verified contract covers benchmark run context, and the serializer/test evidence shows persisted load-timestamp storage, runtime and OS metadata, provider execution status, and skip reasons on the artifact set, matching the run-context baseline the story ratifies."
    },
    {
      "expectation": "The contract ratifies the minimum scenario/provider baseline from the visible harness: required SQLite scenario comparisons, latest-index and scale matrix modes when relevant, and provider-native bulk-ingestion comparisons for PostgreSQL, SQL Server, MySQL, and Oracle only when those providers are configured.",
      "satisfied": true,
      "reason": "benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs and the benchmark README preserve the required SQLite baseline scenarios, latest-index and scale modes when relevant, and provider-native bulk-ingestion comparisons for PostgreSQL, SQL Server, MySQL, and Oracle only when those providers are configured."
    },
    {
      "expectation": "The contract requires benchmark result rows to preserve the current dimensions of scenario, provider, baseline, strategy family, dataset size, change ratio, execution status, skip reason, iterations, mean/min/max milliseconds, and persisted outcome, and to extend the persisted evidence with allocation metrics for measured runs.",
      "satisfied": true,
      "reason": "The developer delivery evidence and verified code/tests show artifact rows keep the established scenario/provider/baseline/result dimensions and extend persisted evidence with allocation metrics while skipped or failed rows retain non-measured values."
    },
    {
      "expectation": "The contract requires SQL capture to be stored with the same before/after evidence set for scenarios whose claim depends on emitted query shape, index usage, or batching behavior; save-path scenarios that only claim change-tracker or allocation wins do not need duplicate SQL capture unless emitted SQL is part of the claim.",
      "satisfied": true,
      "reason": "The contract document and benchmark README explicitly require SQL capture beside the same before/after evidence when a claim depends on query shape, index usage, batching, or materialization behavior, and they exempt save-path allocation or change-tracker claims unless SQL is part of the claim."
    },
    {
      "expectation": "The contract defines default regression gates: the targeted metric must improve or hold, required SQLite non-target mean-time and allocation regressions over 5% fail by default, configured optional-provider regressions over 10% must be explicitly called out and justified, and skipped optional providers are acceptable only when the artifact records the skip reason instead of omitting the row.",
      "satisfied": true,
      "reason": "The verified contract includes default regression budgets, and the repository evidence preserves the required skipped-row policy by recording optional-provider execution status and explicit skip reasons instead of omitting rows."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "An authoritative repository-facing contract exists for performance evidence and benchmark artifacts, and it is specific enough that the related tuning and documentation tickets can reference it without reopening baseline questions.",
      "satisfied": true,
      "reason": "A repository-facing benchmark evidence contract now exists in docs/plans/performance-evidence-benchmark-artifact-contract.md, and the benchmark README links downstream work back to it as the authoritative reference."
    },
    {
      "expectation": "The contract names the minimum scenario families, provider matrix, before/after storage rule, required metadata fields, SQL-capture rule, allocation rule, and default regression budgets.",
      "satisfied": true,
      "reason": "The contract content and developer delivery evidence cover the minimum scenario families, provider matrix, before/after storage rule, required metadata, SQL-capture rule, allocation rule, and regression budgets expected by the story."
    },
    {
      "expectation": "The benchmark documentation and/or contract tests are updated so the required artifact filenames and core row/context fields cannot drift silently from the agreed contract.",
      "satisfied": true,
      "reason": "Benchmark documentation was updated and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs asserts deterministic artifact filenames and core context/status fields, so contract drift is checked in verification; dotnet test DVault.slnx --nologo and bash tools/check-format.sh both passed."
    },
    {
      "expectation": "The ticket outcome leaves no ambiguity about when skipped provider rows are acceptable and when missing evidence fails a performance claim.",
      "satisfied": true,
      "reason": "The contract and README state that optional providers may be skipped only with explicit skip reasons, and the contract also states failed rows do not satisfy a completed performance claim, removing ambiguity about acceptable skips versus missing evidence."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u002771747bb3d035\u0027 on branch \u0027ticket/06F492BZPP5YT9SJSPDHQBGF3R-story-define-performance-evidence-and-benchmark\u0027.",
    "Committed repository path \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027 exists at verified commit \u002771747bb3d035\u0027.",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: using System.Runtime.InteropServices;",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: using System.Text;",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: using System.Text.Json;",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: namespace DCoding.Data.DVault.Benchmarks;",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: internal static class BenchmarkArtifacts {",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: .Append(\u0022- PostgreSQL execution status: \u0022)",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: builder.AppendLine(\u0022- Optional provider status:\u0022);",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: .Append(\u0022- Load timestamp storage: \u0022)",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: .AppendLine(context.LoadTimestampStorage);",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: string LoadTimestampStorage,",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: options.LoadTimestampStorage.ToString(),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: .Append(\u0022- OS description: \u0022)",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: .AppendLine(context.OsDescription);",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: .Append(\u0022- .NET runtime description: \u0022)",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: .AppendLine(context.DotNetRuntimeDescription);",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: .Append(\u0022- .NET runtime version: \u0022)",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: .AppendLine(context.DotNetRuntimeVersion);",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: return JsonSerializer.Serialize(document, SerializerOptions) \u002B Environment.NewLine;",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: string OsDescription,",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: string DotNetRuntimeDescription,",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: string DotNetRuntimeVersion,",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: RuntimeInformation.OSDescription,",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs\u0027: RuntimeInformation.OSArchitecture.ToString(),",
    "Committed repository path \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027 exists at verified commit \u002771747bb3d035\u0027.",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: using System.Diagnostics;",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: namespace DCoding.Data.DVault.Benchmarks;",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: internal static class BenchmarkRunner {",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: public static async Task RunAsync(BenchmarkOptions options, CancellationToken cancellationToken) {",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: ArgumentNullException.ThrowIfNull(options);",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: Console.WriteLine(\u0022  --load-timestamp-storage \u003Cprovider-default|iso8601-utc-text|utc-ticks\u003E\u0022);",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: Console.WriteLine(\u0022                    Physical Data Vault load-timestamp storage to project. Default: provider-default.\u0022);",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: new CustomerProfileDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback, options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: new CustomerProfileDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized, options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: new OrderProductDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback, options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: new OrderProductDataVaultBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized, options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: new LatestSatelliteReadBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback, options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: new LatestSatelliteReadBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized, options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: new PitAsOfReadBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback, options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: new PitAsOfReadBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized, options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: new BridgeTraversalReadBenchmark(provider, DataVaultBenchmarkStrategy.ProviderNeutralFallback, options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: new BridgeTraversalReadBenchmark(provider, DataVaultBenchmarkStrategy.SqliteOptimized, options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: new ProviderNativeBulkIngestionBenchmark(provider, optimizedStrategy, options.LoadTimestampStorage),",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: options.LoadTimestampStorage));",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: benchmarks.Add(new CustomerProfileBulkDataVaultBenchmark(provider, scenario, optimizedStrategy, options.LoadTimestampStorage));",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: options.LoadTimestampStorage,",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs\u0027: BenchmarkSkipReason.NotConfigured(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable));",
    "Committed repository path \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027 exists at verified commit \u002771747bb3d035\u0027.",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: # DVault Benchmarks",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: Run the local scenario comparison benchmarks from the repository root:",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: \u0060\u0060\u0060sh",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: \u0060\u0060\u0060",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: The executable always uses SQLite temporary files as the required local baseline. SQLite rows exercise classic EF rows, the provider-neutral DVault fallback registered through \u0060Add...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: Use \u0060--load-timestamp-storage\u0060 to compare the physical representation of Data Vault load timestamps:",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --load-timestamp-storage utc-ticks...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: Valid timestamp storage values are \u0060provider-default\u0060, \u0060iso8601-utc-text\u0060, and \u0060utc-ticks\u0060.",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --latest-indexes --load-timestamp-...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: This mode seeds 100 customers with 20 existing profile satellite states each, then compares unchanged replay and changed replay saves across the current model index and explicit in...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: When collecting external-provider comparison rows, set the relevant environment variable before restore/build/run so the benchmark project\u0027s conditional provider package references...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: The repository-facing evidence contract is defined in \u0060docs/plans/performance-evidence-benchmark-artifact-contract.md\u0060. Before/after evidence must keep two comparable copies of the...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: The required SQLite matrix includes read baselines for latest satellite, PIT as-of, and bridge traversal scenarios. Fixture creation, seeding, and strategy-diagnostic checks run be...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --iterations 3 --warmup 1 --output...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: DVAULT_TEST_POSTGRES_CONNECTION_STRING=\u0022Host=localhost;Database=dvault_benchmarks;Username=postgres;Password=postgres\u0022 dotnet run --project benchmarks/DCoding.Data.DVault.Benchmark...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0 --output artifacts/benchma...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: When a performance claim depends on emitted query shape, index usage, batching behavior, or materialization behavior, store representative SQL beside the same before/after artifact...",
    "Committed repository path \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027 exists at verified commit \u002771747bb3d035\u0027.",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: # Performance Evidence And Benchmark Artifact Contract",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Status: v1 contract",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Ticket: \u006006F492BZPP5YT9SJSPDHQBGF3R\u0060",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: ## Purpose",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: This document is the shared DVault performance-evidence contract. Performance tuning, release-note, and documentation work must reuse this contract instead of inventing ticket-spec...",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: The current benchmark harness remains the v1 baseline. It is extended by contract, documentation, and artifact-field tests rather than replaced.",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: The before and after sets must use the same scenario mode, provider filter, iteration count, warmup count, load-timestamp storage setting, and provider configuration unless the cla...",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: - load-timestamp storage",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: - optional provider connection-string environment variable names",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Every persisted benchmark evidence set must contain these files from one benchmark execution:",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Before/after evidence must store two comparable artifact sets under one explicit scenario, ticket, or release label. For example:",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: - OS description",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: - .NET runtime description",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: - .NET runtime version",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: The required local baseline is SQLite temporary files. A standard local evidence set must include:",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: When the claim depends on scale behavior, include the scale matrix mode. When the claim depends on latest-satellite lookup/index behavior, include the latest-index matrix mode.",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: ## Allocation Evidence",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Allocation metrics are required for completed rows because many DVault performance claims depend on batching, materialization, and change-tracker behavior rather than wall-clock ti...",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: ## SQL Capture Evidence",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: SQL capture is required when a claim depends on emitted query shape, index usage, batching behavior, or materialization behavior. Store representative SQL beside the before/after a...",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Save-path scenarios that only claim change-tracker or allocation wins do not need duplicate SQL capture unless emitted SQL is part of the claim.",
    "Observed committed repository file \u0027docs/plans/performance-evidence-benchmark-artifact-contract.md\u0027: Failed rows do not satisfy a completed performance claim for that provider and scenario. They may be retained as failure evidence, but a downstream ticket must either fix the faile...",
    "Committed repository path \u0027README.md\u0027 exists at verified commit \u002771747bb3d035\u0027.",
    "Observed committed repository file \u0027README.md\u0027: # DVault",
    "Observed committed repository file \u0027README.md\u0027: DVault is the repository for the \u0060DCoding.Data.DVault\u0060 .NET library.",
    "Observed committed repository file \u0027README.md\u0027: ## Installation",
    "Observed committed repository file \u0027README.md\u0027: Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The coordinated DVault package family is vers...",
    "Observed committed repository file \u0027README.md\u0027: \u0060\u0060\u0060sh",
    "Observed committed repository file \u0027README.md\u0027: dotnet add package DCoding.Data.DVault --version 0.17.0",
    "Observed committed repository file \u0027README.md\u0027: Code-First metadata is additive. It does not ask callers to put DVault hash-key, load-timestamp, or record-source technical fields on domain entities, and it does not create a publ...",
    "Observed committed repository file \u0027README.md\u0027: Persistence remains an explicit service boundary. \u0060DataVaultSaveRequest\u0060 carries the load timestamp and record source, and callers choose when to write vault rows through \u0060IDataVau...",
    "Observed committed repository file \u0027README.md\u0027: DVault also provides an explicit opt-in \u0060SaveChanges\u0060 metadata interceptor for applications that already add generated DVault rows through EF tracking. The interceptor only fills m...",
    "Observed committed repository file \u0027README.md\u0027: .UseLoadTimestamp(() =\u003E DateTimeOffset.UtcNow)",
    "Observed committed repository file \u0027README.md\u0027: var loadTimestamp = new DateTimeOffset(2026, 5, 11, 10, 15, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027README.md\u0027: loadTimestamp,",
    "Observed committed repository file \u0027README.md\u0027: For loaders that already have multiple source batches prepared, \u0060DataVaultBulkSaveRequest\u0060 processes ordered save requests through the same explicit service. Each contained request...",
    "Observed committed repository file \u0027README.md\u0027: row.RequiredDateTimeOffset(\u0022LoadTimestamp\u0022));",
    "Observed committed repository file \u0027README.md\u0027: new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey], asOfTimestamp),",
    "Observed committed repository file \u0027README.md\u0027: - Model-first governance for reviewed \u0060dvault.model.v1\u0060 JSON artifacts that should be imported, projected into EF metadata, exported canonically, and compared against generated met...",
    "Observed committed repository file \u0027README.md\u0027: Choose one authoritative path for a model boundary and keep the others as compatible alternatives for different ownership needs. See [Model-First Governance Workflow](docs/model-fi...",
    "Observed committed repository file \u0027README.md\u0027: Applications that want an early runtime check for unsafe generated-row EF tracking can opt into the separate SaveChanges guard interceptor. \u0060AddDVault()\u0060 does not enable this guard...",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027 exists at verified commit \u002771747bb3d035\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: using System.Text.Json;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: using DCoding.Data.DVault;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: using DCoding.Data.DVault.Benchmarks;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: using DCoding.Data.DVault.Tests.Shared;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: Assert.Contains(\u0022- PostgreSQL execution status: skipped\u0022, markdown);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: Assert.Contains(\u0022- Load timestamp storage: ProviderDefault\u0022, markdown);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: Assert.Equal(\u0022ProviderDefault\u0022, context.GetProperty(\u0022loadTimestampStorage\u0022).GetString());",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable)),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable)),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.Oracle.ConnectionStringEnvironmentVariable)),",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: Assert.Contains(\u0022- OS description: \u0022, markdown);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: Assert.Contains(\u0022- .NET runtime version: \u0022, markdown);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: var csvLines = csv.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: Assert.False(string.IsNullOrWhiteSpace(context.GetProperty(\u0022osDescription\u0022).GetString()));",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: Assert.False(string.IsNullOrWhiteSpace(context.GetProperty(\u0022dotNetRuntimeDescription\u0022).GetString()));",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: Assert.False(string.IsNullOrWhiteSpace(context.GetProperty(\u0022dotNetRuntimeVersion\u0022).GetString()));",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: BenchmarkExternalProviderDefinitions.Postgres.ConnectionStringEnvironmentVariable,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.SqlServer.ConnectionStringEnvironmentVariable));",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs\u0027: NotConfiguredSkipReasonFor(BenchmarkExternalProviderDefinitions.MySql.ConnectionStringEnvironmentVariable));",
    "Committed branch delta contains 6 inspectable repository path(s): Modified: benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs, Modified: benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs, Modified: benchmarks/DCoding.Data.DVault.Benchmarks/README.md, Added: docs/plans/performance-evidence-benchmark-artifact-contract.md, Modified: README.md, Modified: tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 190 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/benchmarks, area/documentation, area/performance, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F492BZPP5YT9SJSPDHQBGF3R-story-define-performance-evidence-and-benchmark\u0027.",
    "Ticket history references implementation commit \u002771747bb3d035\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Proceed to integrator with the verified implementation on branch ticket/06F492BZPP5YT9SJSPDHQBGF3R-story-define-performance-evidence-and-benchmark at commit 71747bb3d035."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F492BZPP5YT9SJSPDHQBGF3R`
- target-role: `integrator`
- verification-summary: Tester verified 7/7 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F492BZPP5YT9SJSPDHQBGF3R-story-define-performance-evidence-and-benchmark' at commit '71747bb3d035'.
- acceptance-criteria: `7/7` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F492BZPP5YT9SJSPDHQBGF3R-story-define-performance-evidence-and-benchmark`
- implementation-commit: `71747bb3d035`
- implementation-pr: `<none>`
- implementation-change: `<none>`