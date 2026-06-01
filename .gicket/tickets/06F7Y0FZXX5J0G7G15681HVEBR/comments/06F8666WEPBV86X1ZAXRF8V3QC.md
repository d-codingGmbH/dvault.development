[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 8/8 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F7Y0FZXX5J0G7G15681HVEBR-story-define-redacted-read-plan-explain-v2-contr\u0027 at commit \u00273877df37bcd2\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F7Y0FZXX5J0G7G15681HVEBR-story-define-redacted-read-plan-explain-v2-contr",
    "commitSha": "3877df37bcd2",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "The refined contract names IDataVaultReadDiagnosticsService.Analyze(...) and DataVaultDiagnosticsResult.ReadShape as the authoritative request-bound surface and states that support-bundle export serializes the same bounded data under readShape.",
      "satisfied": true,
      "reason": "docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md is present at the verified commit and defines IDataVaultReadDiagnosticsService.Analyze(...) and the existing DataVaultDiagnosticsResult.ReadShape surface as authoritative; the verified implementation evidence keeps support-bundle readShape output aligned with that same bounded surface."
    },
    {
      "expectation": "The contract preserves the existing closed vocabularies: DataVaultReadShapeKind values LatestSatellite, PitAsOf, and Bridge; read-strategy status values NotEvaluated, ProviderStrategySelected, and ProviderNeutralFallback; and the finite DataVaultReadStrategyFallbackCauseKind set already used by diagnostics.",
      "satisfied": true,
      "reason": "The contract is backed by src/DCoding.Data.DVault/DataVaultDiagnostics.cs, which defines DataVaultReadShapeKind values LatestSatellite, PitAsOf, and Bridge, read-strategy statuses NotEvaluated, ProviderStrategySelected, and ProviderNeutralFallback, and the existing finite DataVaultReadStrategyFallbackCauseKind vocabulary referenced by the contract."
    },
    {
      "expectation": "Latest/current and as-of satellite diagnostics are specified to include translated satellite identity, parent reference, filter columns, Current vs AsOf semantics, series-selection rule, cutoff rule, deterministic ordering, projected column groups, and expected index baseline.",
      "satisfied": true,
      "reason": "The added v2 contract document defines the latest/current and as-of satellite payload members, and the updated DataVaultDiagnosticsTests provide representative satellite serialization/assertion coverage for filter columns, projected columns, ordering, and expected index baseline on the existing read-shape surface."
    },
    {
      "expectation": "PIT diagnostics are specified to include translated PIT identity, parent reference, referenced satellite snapshot bindings, filter columns, PIT row-identity columns, PIT row-selection rule, snapshot lookup behavior, no-latest-fallback behavior, maintained-PIT prerequisite, projected column groups, referenced-satellite lookup count, and expected index baseline.",
      "satisfied": true,
      "reason": "The v2 contract document defines the PIT payload members, and the verified test evidence covers PIT read-shape behavior including row-identity columns, referenced satellite bindings, filter columns, provider strategy exposure, and expected PIT index baselines."
    },
    {
      "expectation": "Bridge diagnostics are specified to include bridge kind, translated bridge identity, endpoint descriptors, selected filter endpoint, endpoint filter, optional maximum-depth predicate, deterministic ordering, supported endpoint rules, projected column groups, and expected traversal index baseline.",
      "satisfied": true,
      "reason": "The v2 contract document defines the bridge payload members, and the verified bridge diagnostics evidence covers bridge kind, endpoint/filter details, deterministic traversal-related output, provider behavior, and expected traversal index baseline on the documented read-shape surface."
    },
    {
      "expectation": "Provider facts are limited to provider name, capability and behavior profile names plus defaulting flags, selected strategy name when present, read-strategy status, and finite fallback causes; non-applicable optional fields are omitted rather than filled with sentinels.",
      "satisfied": true,
      "reason": "The implementation adds SelectedStrategyName to DataVaultReadShapeProviderDiagnostics, while the contract limits provider facts to provider/profile/defaulting/status/selected-strategy/fallback data and requires omission of non-applicable optional fields; passing tests confirm the documented provider-selected and provider-neutral fallback exposure."
    },
    {
      "expectation": "The contract explicitly forbids raw request keys, raw hash-key values, as-of or timestamp values, SQL text, query plans, credentials, connection strings, provider error text, exception text, and other secret-bearing output.",
      "satisfied": true,
      "reason": "The contract explicitly forbids raw request keys, raw hash-key values, request timestamp values, SQL text, query plans, credentials, connection strings, provider error text, exception text, and similar secret-bearing output, and the updated automated coverage verifies representative serialization without leaking supplied request-key values."
    },
    {
      "expectation": "The contract explicitly says this surface is diagnostics and tuning guidance, not a raw-SQL advisor, automatic-index advisor, or provider-specific physical-plan promise.",
      "satisfied": true,
      "reason": "The contract and updated guidance documents state that read-plan/read-shape explainability is bounded diagnostics and tuning guidance only, and they explicitly route raw-SQL or physical execution-plan needs to separate workflows rather than promising an advisor or provider-specific physical-plan output."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "An authoritative ticket handoff or public-facing contract document enumerates the reused vocabularies, per-shape payload members, redaction rules, omission rules, and non-goals.",
      "satisfied": true,
      "reason": "An authoritative public-facing contract document, docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md, exists at the verified commit and enumerates reused vocabularies, per-shape payload members, redaction rules, omission rules, and non-goals."
    },
    {
      "expectation": "Any implementation or documentation updates keep IDataVaultReadDiagnosticsService, DataVaultDiagnosticsResult.ReadShape, and support-bundle serialization aligned with the documented contract instead of introducing a second competing shape.",
      "satisfied": true,
      "reason": "The branch delta updates the existing diagnostics source, tests, API snapshot, and guidance docs without introducing a competing shape, and the documented contract remains anchored on IDataVaultReadDiagnosticsService, DataVaultDiagnosticsResult.ReadShape, and support-bundle readShape serialization."
    },
    {
      "expectation": "Automated coverage proves representative satellite, PIT, and bridge read-shape payloads plus provider-selected and provider-neutral fallback exposure remain serialized as documented and do not leak supplied request-key values.",
      "satisfied": true,
      "reason": "dotnet test DVault.slnx --nologo succeeded, DataVaultDiagnosticsTests was expanded, and the verification evidence states representative satellite, PIT, and bridge payloads plus provider-selected/provider-neutral fallback exposure are serialized as documented without leaking supplied request-key values."
    },
    {
      "expectation": "Release or guidance text that references read-plan or read-shape explainability is updated to keep the public message aligned with the bounded redacted diagnostics surface and to avoid raw-SQL or query-plan promises.",
      "satisfied": true,
      "reason": "README.md, docs/architecture/dvault-v1-pit-bridge-boundary.md, docs/performance-profiles.md, and docs/production-adoption-checklist.md were updated, and the observed guidance keeps the public message aligned with the bounded redacted diagnostics surface while avoiding raw-SQL or query-plan promises."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u00273877df37bcd2\u0027 on branch \u0027ticket/06F7Y0FZXX5J0G7G15681HVEBR-story-define-redacted-read-plan-explain-v2-contr\u0027.",
    "Committed repository path \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027 exists at verified commit \u00273877df37bcd2\u0027.",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: # DVault V1 PIT And Bridge Boundary",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Status: v1 implementation note",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Ticket: 06F5Q91M0PM17RP43ZQRPBDXP0",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Current public baseline: [DVault v0.21.0 Release Notes](../releases/v0.21.0.md)",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: ## Decision",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: DVault v1 treats PIT and bridge tables as explicit read models. Application code owns when those read models are maintained, and \u0060IDataVaultReadService\u0060 consumes the already-mainta...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: PIT reads target one \u0060DataVaultPitMetadata\u0060 declaration, explicit parent hash keys, and an \u0060asOf\u0060 timestamp. \u0060ReadPitRowsAsync(...)\u0060 returns raw \u0060DataVaultPitReadRecord\u0060 rows. \u0060Rea...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: The runtime metadata path supports:",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: The public \u0060dvault.model.v1\u0060 PIT artifact shape remains hub-parent-only and continues to use the \u0060hub\u0060 field. Runtime link-parent PIT maintenance and reads do not imply model-first...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: \u0060IDataVaultReadDiagnosticsService\u0060 is the diagnostics boundary for read strategy and read-shape evidence. Request-bound diagnostics keep provider strategy selection in \u0060ReadStrateg...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: ## Evidence",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: - [DataVaultPitMaintenanceServiceSqliteTests.cs](../../tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs) covers PIT rebuild, parent maintena...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Benchmark evidence:",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: The shared benchmark artifact contract is [Performance Evidence And Benchmark Artifact Contract](../plans/performance-evidence-benchmark-artifact-contract.md). The relevant rows ar...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: Migration and drift guidance remains centralized in [DVault Dotnet EF Design-Time Workflow](dvault-dotnet-ef-design-time-workflow.md) and [Model-First Governance Workflow](../model...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: - Non-SQLite optimized PIT or bridge read claims.",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-pit-bridge-boundary.md\u0027: - Provider-specific physical-design tuning, automatic index creation, raw SQL evidence, or provider query-plan advice.",
    "Committed repository path \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027 exists at verified commit \u00273877df37bcd2\u0027.",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: # DVault V2 Redacted Read-Plan Explain Contract",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: Status: v2 contract",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: Ticket: 06F7Y0FZXX5J0G7G15681HVEBR",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: Current public baseline: [DVault v0.24.0 Release Notes](../releases/v0.24.0.md)",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: ## Decision",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: DVault v2 read-plan explainability is the request-bound diagnostics surface exposed by \u0060IDataVaultReadDiagnosticsService.Analyze(...)\u0060. The service returns the existing \u0060DataVaultD...",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: The shape may include generated metadata identifiers, table names, column names, index names, and enum values. It must not include raw parent hash-key request values, raw business ...",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: - \u0060referencedSatellites\u0060: referenced satellite metadata names, table names, PIT snapshot reference columns, satellite parent hash-key columns, satellite load-timestamp columns, and...",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: - raw as-of, cutoff, load-timestamp, or other timestamp values supplied by a request.",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: - provider error text.",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: - new runtime read execution APIs.",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: Consumers that need raw SQL or physical execution-plan evidence should capture that in a separate consumer-owned workflow with its own redaction, storage, and review rules.",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: ## Evidence",
    "Observed committed repository file \u0027docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md\u0027: Focused repository evidence:",
    "Committed repository path \u0027docs/performance-profiles.md\u0027 exists at verified commit \u00273877df37bcd2\u0027.",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: # Performance Profiles",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Status: v0.24.0 adopter guidance",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: This guide is the detailed performance-profile reference for the current v0.24.0 DVault documentation baseline. It translates the checked-in benchmark evidence into starting profil...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: ## Evidence Baseline",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use the root benchmark artifact triplet as the source for the row names and timing values in this guide:",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: - [benchmark-summary.md](../benchmark-summary.md)",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: - Load timestamp storage \u0060ProviderDefault\u0060.",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Treat all millisecond values below as observations from that run only. Rerun the benchmarks when provider, hardware, runtime, load-timestamp storage, iteration count, warmup count,...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: | Medium chunked ingestion | The loader has an ordered source stream and must bound memory without changing load timestamps, record sources, or request order. | Keep \u0060DataVaultBulk...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Keep the first production proof on the explicit \u0060IDataVaultSaveService\u0060 boundary with caller-supplied load timestamp, record source, hub/link/satellite intent, and caller-owned tra...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Keep \u0060DataVaultBulkSaveRequest\u0060 when the loader already has the complete ordered request set materialized. Choose \u0060DataVaultChunkedSaveRequest\u0060 when the loader has already formed b...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Before claiming provider-native behavior, run request-bound \u0060IDataVaultDiagnosticsService\u0060 analysis for the exact batch and verify strategy status, selected strategy name, candidat...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: The benchmark runner and artifact rules are documented in [DVault Benchmarks](../benchmarks/DCoding.Data.DVault.Benchmarks/README.md) and [Performance Evidence And Benchmark Artifa...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: | Staged provider ingestion | The application has clean provider-specific contexts and larger eligible ordered bulk batches for PostgreSQL, SQL Server, MySQL, or Oracle. | Register...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use this profile for small application-local vaults, early local proofs, and services that first need ordinary explicit saves to be correct and observable. The checked-in SQLite ev...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: All values in this section are from the evidence baseline above:",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: | Scenario | Baseline | Mean ms | Evidence posture |",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Stop treating the root SQLite rows as enough evidence when the application uses a non-SQLite database, provider diagnostics report fallback, the request shape includes unsupported ...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: The v0.24 async streaming contract uses the same \u0060DataVaultSaveChunk\u0060 payload model through an additive \u0060IAsyncEnumerable\u003CDataVaultSaveChunk\u003E\u0060 save overload. That overload is for c...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use the same explicit \u0060IDataVaultSaveService\u0060 boundary as ordinary saves. \u0060DataVaultChunkedSaveRequest\u0060 is the materialized input shape for bounded provider-neutral chunking, and t...",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Use these provider boundaries as starting gates, not timing claims from the checked-in run:",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: | Provider | Starting gate | Evidence posture |",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: The checked-in provider-native bulk rows are evidence for visibility and boundaries, not measured wins. \u0060benchmark-summary.csv\u0060 and \u0060benchmark-summary.json\u0060 keep the skipped rows v...",
    "Committed repository path \u0027docs/production-adoption-checklist.md\u0027 exists at verified commit \u00273877df37bcd2\u0027.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: # Production Adoption Checklist",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: Use this checklist when preparing a DVault-consuming application for production. It is a routing document for adopter readiness; follow the linked source documents for setup exampl...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: ## Package And Provider Baseline",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Install the provider-neutral \u0060DCoding.Data.DVault\u0060 package from NuGet and use the published installation guidance in the [README](../README.md#installation).",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Select the DVault provider package that matches the application database and keep every DVault package on one aligned published release version.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat [v0.24.0 release notes](releases/v0.24.0.md) as the current public baseline for coordinated package scope, async chunk-source saves, async save helper convenience, guid...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use \u0060IDataVaultSaveService\u0060 as the default write boundary. Each save request should carry an explicit UTC load timestamp and record source.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Keep \u0060DataVaultBulkSaveRequest\u0060 for already-materialized ordered batches. Use \u0060DataVaultChunkedSaveRequest\u0060 with bounded \u0060DataVaultSaveChunk\u0060 values when the loader needs mat...",
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
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Preserve the Activity tracing redaction boundary: no raw business keys or hash-key values, payload values, record sources, SQL text, credentials, connection strings, provider...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat chunked-save telemetry as bounded operational evidence: chunk count, processed chunk count, retained-state high-water counts, finite retained-state fallback causes, uns...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat telemetry as bounded operational evidence only. Do not expect DVault to configure metric listeners, exporters, dashboards, alert rules, backend-specific pipelines, or h...",
    "Committed repository path \u0027README.md\u0027 exists at verified commit \u00273877df37bcd2\u0027.",
    "Observed committed repository file \u0027README.md\u0027: # DVault",
    "Observed committed repository file \u0027README.md\u0027: DVault is the repository for the \u0060DCoding.Data.DVault\u0060 .NET library.",
    "Observed committed repository file \u0027README.md\u0027: ## Installation",
    "Observed committed repository file \u0027README.md\u0027: Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The coordinated DVault package family is vers...",
    "Observed committed repository file \u0027README.md\u0027: \u0060\u0060\u0060sh",
    "Observed committed repository file \u0027README.md\u0027: dotnet add package DCoding.Data.DVault --version 0.24.0",
    "Observed committed repository file \u0027README.md\u0027: Code-First metadata is additive. It does not ask callers to put DVault hash-key, load-timestamp, or record-source technical fields on domain entities, and it does not create a publ...",
    "Observed committed repository file \u0027README.md\u0027: Persistence remains an explicit service boundary. \u0060DataVaultSaveRequest\u0060 carries the load timestamp and record source, and callers choose when to write vault rows through \u0060IDataVau...",
    "Observed committed repository file \u0027README.md\u0027: DVault also provides an explicit opt-in \u0060SaveChanges\u0060 metadata interceptor for applications that already add generated DVault rows through EF tracking. The interceptor only fills m...",
    "Observed committed repository file \u0027README.md\u0027: .UseLoadTimestamp(() =\u003E DateTimeOffset.UtcNow)",
    "Observed committed repository file \u0027README.md\u0027: var loadTimestamp = new DateTimeOffset(2026, 5, 11, 10, 15, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027README.md\u0027: loadTimestamp,",
    "Observed committed repository file \u0027README.md\u0027: For loaders that already have multiple source batches prepared, \u0060DataVaultBulkSaveRequest\u0060 processes ordered save requests through the same explicit service. Each contained request...",
    "Observed committed repository file \u0027README.md\u0027: For bounded loaders that should not materialize the complete ordered request set before saving, \u0060DataVaultChunkedSaveRequest\u0060 and \u0060DataVaultSaveChunk\u0060 are additive explicit-save in...",
    "Observed committed repository file \u0027README.md\u0027: BuildOrderedRequests(loadTimestamp, \u0022crm-import\u0022);",
    "Observed committed repository file \u0027README.md\u0027: StreamRequestChunksAsync(loadTimestamp, \u0022crm-import\u0022, cancellationToken);",
    "Observed committed repository file \u0027README.md\u0027: customer =\u003E BuildCustomerProfileRequest(customer, loadTimestamp, \u0022cr",
    "Observed committed repository file \u0027README.md\u0027: The current coordinated release baseline is [DVault v0.24.0 Release Notes](docs/releases/v0.24.0.md), which adds the async chunk-source save overload and async helper convenience o...",
    "Observed committed repository file \u0027README.md\u0027: - Model-first governance for reviewed \u0060dvault.model.v1\u0060 JSON artifacts that should be imported, projected into EF metadata, exported canonically, compared against generated metadat...",
    "Observed committed repository file \u0027README.md\u0027: Choose one authoritative path for a model boundary and keep the others as compatible alternatives for different ownership needs. See [Model-First Governance Workflow](docs/model-fi...",
    "Observed committed repository file \u0027README.md\u0027: Applications that want an early runtime check for unsafe generated-row EF tracking can opt into the separate SaveChanges guard interceptor. \u0060AddDVault()\u0060 does not enable this guard...",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027 exists at verified commit \u00273877df37bcd2\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using System.Globalization;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using System.Text;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using Microsoft.EntityFrameworkCore.Infrastructure;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: using Microsoft.EntityFrameworkCore.Metadata;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: DataVaultProviderValueFormat LoadTimestampValueFormat,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: string LoadTimestampStoreType,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: /// Gets the value format used when PIT rows persist satellite snapshot load-timestamp references.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: /// Gets the provider store type used when PIT rows persist satellite snapshot load-timestamp references.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0027: Error,",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027 exists at verified commit \u00273877df37bcd2\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: using System.Text.Json;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: DataVaultLogicalPropertyKind.LoadTimestamp,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: .Single(property =\u003E property.TechnicalRole == TechnicalMetadataColumnRole.LoadTimestamp)",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: Assert.Equal([\u0022LoadTimestamp\u0022], latestSatelliteShape.FilterColumns[1].ColumnNames);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: [\u0022CustomerHashKey\u0022, \u0022HashDiff\u0022, \u0022LoadTimestamp\u0022, \u0022RecordSource\u0022]);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: index =\u003E index.Kind == \u0022secondary-index\u0022 \u0026\u0026 index.DescendingColumnNames.Contains(\u0022LoadTimestamp\u0022));",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: [\u0022CustomerHashKey\u0022, \u0022LoadTimestamp\u0022]);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: [\u0022ProfileLoadTimestamp\u0022, \u0022StatusLoadTimestamp\u0022]);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: Assert.Equal([\u0022CustomerOrderHashKey\u0022, \u0022LoadTimestamp\u0022], pitReadShape.RowIdentityColumns.Single().ColumnNames);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: [\u0022StateLoadTimestamp\u0022, \u0022FulfillmentLoadTimestamp\u0022],",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs\u0027: index.ColumnNames.SequenceEqual([\u0022CustomerOrderHashKey\u0022, \u0022LoadTimestamp\u0022]));",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027 exists at verified commit \u00273877df37bcd2\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # DVault public API snapshot",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Package: DCoding.Data.DVault",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Assembly: DCoding.Data.DVault",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Generated from built assembly output.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Update intentionally with: DVAULT_UPDATE_API_SNAPSHOTS=1 dotnet test DVault.slnx --nologo --filter FullyQualifiedName~ApiSurfaceSnapshotTests",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: type public static class DCoding.Data.DVault.DVaultServiceCollectionExtensions",
    "Committed branch delta contains 8 inspectable repository path(s): Modified: docs/architecture/dvault-v1-pit-bridge-boundary.md, Added: docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md, Modified: docs/performance-profiles.md, Modified: docs/production-adoption-checklist.md, Modified: README.md, Modified: src/DCoding.Data.DVault/DataVaultDiagnostics.cs, Modified: tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, Modified: tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 209 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/architecture, area/diagnostics, area/ef-core, area/read-models, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F7Y0FZXX5J0G7G15681HVEBR-story-define-redacted-read-plan-explain-v2-contr\u0027.",
    "Ticket history references implementation commit \u00273877df37bcd2\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator using branch ticket/06F7Y0FZXX5J0G7G15681HVEBR-story-define-redacted-read-plan-explain-v2-contr at verified commit 3877df37bcd2.",
    "Use the passing deterministic verification evidence from dotnet test DVault.slnx --nologo and bash tools/check-format.sh in the integrator decision."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F7Y0FZXX5J0G7G15681HVEBR`
- target-role: `integrator`
- verification-summary: Tester verified 8/8 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F7Y0FZXX5J0G7G15681HVEBR-story-define-redacted-read-plan-explain-v2-contr' at commit '3877df37bcd2'.
- acceptance-criteria: `8/8` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F7Y0FZXX5J0G7G15681HVEBR-story-define-redacted-read-plan-explain-v2-contr`
- implementation-commit: `3877df37bcd2`
- implementation-pr: `<none>`
- implementation-change: `<none>`