[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 6/6 acceptance criteria and 6/6 definition-of-done expectations on branch \u0027ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g\u0027 at commit \u0027dc3f09dc952a\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g",
    "commitSha": "dc3f09dc952a",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "The delivered guidance explicitly cites \u0060benchmark-summary.md\u0060, \u0060benchmark-summary.csv\u0060, \u0060benchmark-summary.json\u0060, \u0060benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0060, and \u0060docs/plans/performance-evidence-benchmark-artifact-contract.md\u0060, and keeps run-context details attached to any timing claim.",
      "satisfied": true,
      "reason": "docs/performance-profiles.md cites benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, the benchmark README, and the artifact contract, and its evidence baseline keeps timing claims attached to the checked-in run context."
    },
    {
      "expectation": "The small app-local profile explains when default \u0060AddDVault()\u0060 is the safe starting point, when \u0060AddDVaultSqlite()\u0060 changes the evidence-backed path, which rows support the recommendation, and what diagnostics to inspect before changing registration.",
      "satisfied": true,
      "reason": "The small app-local profile presents AddDVault() as the safe starting point, explains when AddDVaultSqlite() is appropriate, cites the SQLite customer-profile rows, and directs adopters to request-bound diagnostics before changing registration."
    },
    {
      "expectation": "The medium chunked-ingestion profile uses the \u0060customer-profile-streaming-save\u0060 rows, recommends \u0060DataVaultChunkedSaveRequest\u0060 only for bounded ordered loaders, gives evidence-backed starting chunk sizes, and states when to prefer materialized bulk or rerun local benchmarks.",
      "satisfied": true,
      "reason": "The medium chunked-ingestion profile uses the customer-profile-streaming-save rows, limits DataVaultChunkedSaveRequest to bounded ordered loaders, recommends chunk size 10 as the checked-in starting point, and explains when materialized bulk or local reruns should replace that guidance."
    },
    {
      "expectation": "The staged provider ingestion profile describes clean-context and provider-gate expectations for PostgreSQL, SQL Server, MySQL, and Oracle using visible benchmark rows and existing release-note posture, with skipped optional-provider rows described as skipped evidence rather than measured wins.",
      "satisfied": true,
      "reason": "The staged provider-ingestion profile documents clean-context and provider-gate boundaries for PostgreSQL, SQL Server, MySQL, and Oracle, cites the visible provider-native-bulk-ingestion rows, and treats skipped optional-provider rows as boundary evidence rather than measured wins."
    },
    {
      "expectation": "The read-model-heavy profile uses \u0060latest-satellite-read\u0060, \u0060pit-as-of-read\u0060, and \u0060bridge-traversal-read\u0060 rows, states that SQLite is the only repository-proven optimized PIT or bridge read provider path, and keeps PIT and bridge maintenance caller-owned and explicit.",
      "satisfied": true,
      "reason": "The read-model-heavy profile uses latest-satellite-read, pit-as-of-read, and bridge-traversal-read, states that SQLite via AddDVaultSqlite() is the only repository-proven optimized PIT/bridge read path, and keeps PIT/bridge maintenance explicit and caller-owned."
    },
    {
      "expectation": "The guidance stays within consumer-owned observability and infrastructure boundaries and references current telemetry or diagnostics surfaces without inventing new platform responsibilities.",
      "satisfied": true,
      "reason": "The guide stays within consumer-owned observability and infrastructure boundaries, references current telemetry and diagnostics surfaces, and does not invent hosted monitoring, provisioning, scheduler, or other new platform responsibilities."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "A dedicated adopter-facing performance-profile guidance document is added or updated under \u0060docs/\u0060 and serves as the authoritative detailed profile reference for v0.23.0.",
      "satisfied": true,
      "reason": "A dedicated docs/performance-profiles.md document exists at the verified commit and declares itself the detailed v0.23.0 adopter guidance/reference."
    },
    {
      "expectation": "Any benchmark README or narrow discovery links touched by this story point to the new guidance consistently and do not restate incompatible performance posture.",
      "satisfied": true,
      "reason": "The verified branch delta updates README.md, docs/production-adoption-checklist.md, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md to route readers to the new guide without changing the established benchmark-evidence posture."
    },
    {
      "expectation": "Each of the four profiles includes workload shape, registration guidance, starting point, diagnostics or telemetry to inspect, supporting benchmark rows, and explicit stop conditions.",
      "satisfied": true,
      "reason": "The guide covers all four profiles with workload shape, registration guidance, starting point, diagnostics or telemetry, supporting benchmark rows, and explicit stop conditions or rerun triggers."
    },
    {
      "expectation": "Links and anchors touched by the documentation change resolve.",
      "satisfied": true,
      "reason": "The touched documentation links point to committed targets, and the observed README Installation heading plus the inspected changed docs provide resolving anchor/link evidence with no link-related verification findings."
    },
    {
      "expectation": "Available docs or markdown validation is run if present, or the delivery notes explicitly state that no repository validator exists.",
      "satisfied": true,
      "reason": "The available repository validation gate bash tools/check-format.sh was run and passed, providing validation coverage for the touched documentation set."
    },
    {
      "expectation": "If benchmark artifacts are regenerated, the markdown/csv/json triplet remains together and the updated documentation explains the new run context.",
      "satisfied": true,
      "reason": "The verified branch delta changes only README.md, docs/production-adoption-checklist.md, benchmarks/DCoding.Data.DVault.Benchmarks/README.md, and docs/performance-profiles.md, so benchmark artifacts were not regenerated and the conditional triplet-regeneration requirement was not triggered."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027dc3f09dc952a\u0027 on branch \u0027ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g\u0027.",
    "Committed repository path \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027 exists at verified commit \u0027dc3f09dc952a\u0027.",
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
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: The default SQLite matrix also includes a streaming-save comparison for the existing chunked save boundary. The \u0060customer-profile-streaming-save\u0060 rows use the same 60 ordered expli...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: The required SQLite matrix also includes bounded EF Core compiled and pooled-context evidence:",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: - compiled-model startup compares ordinary DVault model building with a DVault-projected design model initialized into an EF runtime model and supplied through \u0060UseModel(runtimeMod...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: These rows are SQLite evidence only. They do not claim provider-specific compiled-model generation, dynamic \u0060IDataVaultReadService\u0060 request compilation, provider-specific SQL shape...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --iterations 3 --warmup 1 --output...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: DVAULT_TEST_POSTGRES_CONNECTION_STRING=\u0022Host=localhost;Database=dvault_benchmarks;Username=postgres;Password=postgres\u0022 dotnet run --project benchmarks/DCoding.Data.DVault.Benchmark...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0 --output artifacts/benchma...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: The v0.20.0 provider-optimized documentation boundary reuses the same root artifact triplet: \u0060benchmark-summary.md\u0060, \u0060benchmark-summary.csv\u0060, and \u0060benchmark-summary.json\u0060. Do not i...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: When a performance claim depends on emitted query shape, index usage, batching behavior, or materialization behavior, store representative SQL beside the same before/after artifact...",
    "Observed committed repository file \u0027benchmarks/DCoding.Data.DVault.Benchmarks/README.md\u0027: - compiled-model startup: one seeded generated order hub row, measured once through ordinary DVault model building and once through precomputed \u0060UseModel(runtimeModel)\u0060",
    "Committed repository path \u0027docs/performance-profiles.md\u0027 exists at verified commit \u0027dc3f09dc952a\u0027.",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: # Performance Profiles",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Status: v0.23.0 adopter guidance",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: This guide is the detailed performance-profile reference for DVault adopters. It translates the checked-in benchmark evidence into starting profiles, stop conditions, and rerun tri...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: ## Evidence Baseline",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use the root benchmark artifact triplet as the source for the row names and timing values in this guide:",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: - [benchmark-summary.md](../benchmark-summary.md)",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: - Load timestamp storage \u0060ProviderDefault\u0060.",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Treat all millisecond values below as observations from that run only. Rerun the benchmarks when provider, hardware, runtime, load-timestamp storage, iteration count, warmup count,...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: | Medium chunked ingestion | The loader has an ordered source stream and must bound memory without changing load timestamps, record sources, or request order. | Keep \u0060DataVaultBulk...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Keep the first production proof on the explicit \u0060IDataVaultSaveService\u0060 boundary with caller-supplied load timestamp, record source, hub/link/satellite intent, and caller-owned tra...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Keep \u0060DataVaultBulkSaveRequest\u0060 when the loader already has the complete ordered request set materialized. Choose \u0060DataVaultChunkedSaveRequest\u0060 only when the loader needs bounded c...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Before claiming provider-native behavior, run request-bound \u0060IDataVaultDiagnosticsService\u0060 analysis for the exact batch and verify strategy status, selected strategy name, candidat...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: The benchmark runner and artifact rules are documented in [DVault Benchmarks](../benchmarks/DCoding.Data.DVault.Benchmarks/README.md) and [Performance Evidence And Benchmark Artifa...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: | Staged provider ingestion | The application has clean provider-specific contexts and larger eligible ordered bulk batches for PostgreSQL, SQL Server, MySQL, or Oracle. | Register...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use this profile for small application-local vaults, early local proofs, and services that first need ordinary explicit saves to be correct and observable. The checked-in SQLite ev...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: All values in this section are from the evidence baseline above:",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: | Scenario | Baseline | Mean ms | Evidence posture |",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Stop treating the root SQLite rows as enough evidence when the application uses a non-SQLite database, provider diagnostics report fallback, the request shape includes unsupported ...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use the same explicit \u0060IDataVaultSaveService\u0060 boundary as ordinary saves. \u0060DataVaultChunkedSaveRequest\u0060 is an input shape for bounded provider-neutral chunking; it is not a provide...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use these provider boundaries as starting gates, not timing claims from the checked-in run:",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: | Provider | Starting gate | Evidence posture |",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: The checked-in provider-native bulk rows are evidence for visibility and boundaries, not measured wins. \u0060benchmark-summary.csv\u0060 and \u0060benchmark-summary.json\u0060 keep the skipped rows v...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Stop before making a measured provider-specific performance claim when optional provider rows are skipped, connection strings are unset, provider packages are not restored for the ...",
    "Committed repository path \u0027docs/production-adoption-checklist.md\u0027 exists at verified commit \u0027dc3f09dc952a\u0027.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: # Production Adoption Checklist",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: Use this checklist when preparing a DVault-consuming application for production. It is a routing document for adopter readiness; follow the linked source documents for setup exampl...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: ## Package And Provider Baseline",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Install the provider-neutral \u0060DCoding.Data.DVault\u0060 package from NuGet and use the published installation guidance in the [README](../README.md#installation).",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Select the DVault provider package that matches the application database and keep every DVault package on one aligned published release version.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat [v0.22.0 release notes](releases/v0.22.0.md) as the current public baseline for coordinated package scope, support-bundle-driven typed satellite helper generation, stab...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use \u0060IDataVaultSaveService\u0060 as the default write boundary. Each save request should carry an explicit UTC load timestamp and record source.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Keep \u0060DataVaultBulkSaveRequest\u0060 for already-materialized ordered batches. Use \u0060DataVaultChunkedSaveRequest\u0060 with bounded \u0060DataVaultSaveChunk\u0060 values only when the loader need...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat generated mapper helpers as compile-time ergonomics around the same explicit save boundary: they construct registry-backed operations but do not choose timestamps, reco...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat \u0060UseDataVaultSaveChangesMetadataInterceptor(...)\u0060 as optional and metadata-only. It fills missing \u0060LoadTimestamp\u0060 and \u0060RecordSource\u0060 values on already tracked generated...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use the runnable SQLite or PostgreSQL quickstarts as setup evidence when a small local proof is useful; see [examples/README.md](../examples/README.md).",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use model-first governance when source-controlled \u0060dvault.model.v1\u0060 JSON artifacts need review, strict import diagnostics, canonical export, projection into EF metadata, drif...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Run DVault diagnostics against the configured design-time model before applying migrations. Use [DVault Dotnet EF Design-Time Workflow](architecture/dvault-dotnet-ef-design-t...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use \u0060dotnet run --project \u003Cconsumer-project\u003E -- export --output \u003Cpath\u003E\u0060 only for artifact maintenance or reviewed refresh workflows, not as the default blocking CI gate.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Add \u0060dotnet run --project \u003Cconsumer-project\u003E -- support-bundle --output \u003Cpath\u003E\u0060 as a consumer-invoked troubleshooting artifact when configuration, provider-behavior evidence,...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Review migration guardrail output as operation-level \u0060Safe\u0060, \u0060Risky\u0060, or \u0060Incompatible\u0060 evidence from \u0060DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)\u0060; treat incom...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Do not expect DVault to ship a \u0060dotnet ef\u0060 command shim, intercept EF CLI commands, auto-run migrations, or apply schema repairs. Those behaviors are outside the current v1 w...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat \u0060UseDataVaultSaveChangesGuardInterceptor(...)\u0060 as a separate optional runtime guard. Choose blocking mode for hard failures or warning mode for caller-observed reports;...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Keep dynamic \u0060IDataVaultReadService\u0060 requests as the default for runtime-built shapes, PIT reads, bridge reads, and caller-selected projectors. Use consumer-owned EF compiled...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use \u0060IDataVaultPitMaintenanceService\u0060 after satellite ingestion when PIT declarations should be materialized explicitly; PIT-backed reads then consume those maintained rows. ...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: ## Telemetry, Explainability, And Support Evidence",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat chunked-save telemetry as bounded operational evidence: chunk count, processed chunk count, retained-state high-water counts, finite retained-state fallback causes, uns...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat telemetry as bounded operational evidence only. Do not expect DVault to configure metric listeners, exporters, dashboards, alert rules, backend-specific pipelines, or h...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use request-bound read-shape diagnostics and bounded benchmark evidence for query-shape tuning guidance. Do not expect DVault to emit raw SQL, create indexes automatically, i...",
    "Committed repository path \u0027README.md\u0027 exists at verified commit \u0027dc3f09dc952a\u0027.",
    "Observed committed repository file \u0027README.md\u0027: # DVault",
    "Observed committed repository file \u0027README.md\u0027: DVault is the repository for the \u0060DCoding.Data.DVault\u0060 .NET library.",
    "Observed committed repository file \u0027README.md\u0027: ## Installation",
    "Observed committed repository file \u0027README.md\u0027: Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The coordinated DVault package family is vers...",
    "Observed committed repository file \u0027README.md\u0027: \u0060\u0060\u0060sh",
    "Observed committed repository file \u0027README.md\u0027: dotnet add package DCoding.Data.DVault --version 0.22.0",
    "Observed committed repository file \u0027README.md\u0027: Code-First metadata is additive. It does not ask callers to put DVault hash-key, load-timestamp, or record-source technical fields on domain entities, and it does not create a publ...",
    "Observed committed repository file \u0027README.md\u0027: Persistence remains an explicit service boundary. \u0060DataVaultSaveRequest\u0060 carries the load timestamp and record source, and callers choose when to write vault rows through \u0060IDataVau...",
    "Observed committed repository file \u0027README.md\u0027: DVault also provides an explicit opt-in \u0060SaveChanges\u0060 metadata interceptor for applications that already add generated DVault rows through EF tracking. The interceptor only fills m...",
    "Observed committed repository file \u0027README.md\u0027: .UseLoadTimestamp(() =\u003E DateTimeOffset.UtcNow)",
    "Observed committed repository file \u0027README.md\u0027: var loadTimestamp = new DateTimeOffset(2026, 5, 11, 10, 15, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027README.md\u0027: loadTimestamp,",
    "Observed committed repository file \u0027README.md\u0027: For loaders that already have multiple source batches prepared, \u0060DataVaultBulkSaveRequest\u0060 processes ordered save requests through the same explicit service. Each contained request...",
    "Observed committed repository file \u0027README.md\u0027: For bounded loaders that should not materialize the complete ordered request set before saving, \u0060DataVaultChunkedSaveRequest\u0060 and \u0060DataVaultSaveChunk\u0060 are additive explicit-save in...",
    "Observed committed repository file \u0027README.md\u0027: BuildOrderedRequests(loadTimestamp, \u0022crm-import\u0022);",
    "Observed committed repository file \u0027README.md\u0027: Keep \u0060DataVaultBulkSaveRequest\u0060 when the loader already has the full ordered request set materialized. Switch to \u0060DataVaultChunkedSaveRequest\u0060 only when the caller needs bounded ch...",
    "Observed committed repository file \u0027README.md\u0027: - Model-first governance for reviewed \u0060dvault.model.v1\u0060 JSON artifacts that should be imported, projected into EF metadata, exported canonically, compared against generated metadat...",
    "Observed committed repository file \u0027README.md\u0027: Choose one authoritative path for a model boundary and keep the others as compatible alternatives for different ownership needs. See [Model-First Governance Workflow](docs/model-fi...",
    "Observed committed repository file \u0027README.md\u0027: Applications that want an early runtime check for unsafe generated-row EF tracking can opt into the separate SaveChanges guard interceptor. \u0060AddDVault()\u0060 does not enable this guard...",
    "Observed committed repository file \u0027README.md\u0027: The carried-forward v0.21.0 documentation boundary keeps that same write hierarchy and the PIT/bridge read-model baseline. \u0060IDataVaultSaveService\u0060 remains the public write entry po...",
    "Committed branch delta contains 4 inspectable repository path(s): Modified: benchmarks/DCoding.Data.DVault.Benchmarks/README.md, Added: docs/performance-profiles.md, Modified: docs/production-adoption-checklist.md, Modified: README.md.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 207 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/benchmarks, area/documentation, area/ef-core, area/performance, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g\u0027.",
    "Ticket history references implementation commit \u0027dc3f09dc952a\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator using branch ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g at verified commit dc3f09dc952a.",
    "Use the passing dotnet test DVault.slnx --nologo and bash tools/check-format.sh results as the tester verification evidence for integrator review."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F5Q94KX65TXQ8EC75FWSD01W`
- target-role: `integrator`
- verification-summary: Tester verified 6/6 acceptance criteria and 6/6 definition-of-done expectations on branch 'ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g' at commit 'dc3f09dc952a'.
- acceptance-criteria: `6/6` satisfied
- definition-of-done: `6/6` satisfied
- implementation-branch: `ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g`
- implementation-commit: `dc3f09dc952a`
- implementation-pr: `<none>`
- implementation-change: `<none>`