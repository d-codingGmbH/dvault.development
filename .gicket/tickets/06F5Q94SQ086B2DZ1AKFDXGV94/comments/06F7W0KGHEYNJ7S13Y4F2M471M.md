[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch \u0027ticket/06F5Q94SQ086B2DZ1AKFDXGV94-task-update-v0-23-0-tracing-and-performance-guid\u0027 at commit \u0027d70c00f250a2\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F5Q94SQ086B2DZ1AKFDXGV94-task-update-v0-23-0-tracing-and-performance-guid",
    "commitSha": "d70c00f250a2",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "README.md, docs/production-adoption-checklist.md, docs/performance-profiles.md, and docs/releases/v0.23.0.md tell one consistent v0.23.0 story, with v0.23.0 as the current coordinated baseline and older releases clearly historical.",
      "satisfied": true,
      "reason": "README, the production checklist, the performance guide, and docs/releases/v0.23.0.md all identify v0.23.0 as the current coordinated baseline; earlier releases are referenced only as historical or carried-forward context."
    },
    {
      "expectation": "Public docs explain that AddDVault() remains telemetry-free by default, that Activity tracing is listener-driven via the DCoding.Data.DVault ActivitySource, and that AddDVaultTelemetry(), Metrics, and IDataVaultTelemetryObserver remain sibling opt-in observability surfaces rather than prerequisites for tracing.",
      "satisfied": true,
      "reason": "The verified docs and tracing-contract evidence describe listener-driven Activity tracing via DCoding.Data.DVault and keep AddDVaultTelemetry(), Metrics, and IDataVaultTelemetryObserver as separate opt-in observability siblings rather than prerequisites for AddDVault()."
    },
    {
      "expectation": "Public adopter-facing docs state the tracing redaction boundary without ambiguity: no raw business keys or hash keys, payload values, record sources, SQL text, credentials, connection strings, provider messages, exception messages, or stack traces.",
      "satisfied": true,
      "reason": "The checklist, release note, and tracing contract preserve the explicit redaction boundary across raw business keys, hash-key values, payloads, record sources, SQL text, credentials, connection strings, provider messages, exception messages, and stack traces."
    },
    {
      "expectation": "docs/releases/v0.23.0.md lists the coordinated seven-package family, intended release posture, carried-forward compatibility notes, validation evidence, benchmark evidence references, and explicit non-goals without implying package publication.",
      "satisfied": true,
      "reason": "docs/releases/v0.23.0.md exists and includes the coordinated seven-package scope, intended release posture, carried-forward compatibility boundaries, validation and benchmark evidence sections, and explicit non-goals while stating publication remains a separate manual step."
    },
    {
      "expectation": "Performance guidance summarizes the four existing profiles by linking to docs/performance-profiles.md and ties any timing claims to the checked-in benchmark artifacts and run context instead of inventing new unverified claims.",
      "satisfied": true,
      "reason": "The release note routes readers to docs/performance-profiles.md, and that guide ties the four documented profiles to the checked-in benchmark artifacts and run context while treating timings as observation-only evidence."
    },
    {
      "expectation": "Every touched link or anchor resolves, and the documented validation section records the repository baseline commands plus the focused tracing and benchmark evidence surfaces cited by the release note.",
      "satisfied": true,
      "reason": "The release note contains a validation-evidence section with focused tracing and benchmark sources, the recorded validation flow executed the baseline test and format gates on the verified commit, and the tester evidence reported no broken-link or anchor findings."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "README.md and docs/production-adoption-checklist.md no longer point readers at v0.22.0 as the current coordinated release baseline where v0.23.0 should now be authoritative.",
      "satisfied": true,
      "reason": "README and docs/production-adoption-checklist.md now point to v0.23.0 release notes as the current coordinated baseline instead of v0.22.0."
    },
    {
      "expectation": "docs/releases/v0.23.0.md exists and can stand alone as the coordinated release record for tracing and performance guidance consolidation.",
      "satisfied": true,
      "reason": "docs/releases/v0.23.0.md is present at the verified commit and contains standalone release-record sections for scope, evidence, compatibility, and non-goals."
    },
    {
      "expectation": "The touched public docs consistently preserve the exact tracing contract language where names matter and do not introduce unsupported tracing, telemetry, or performance claims.",
      "satisfied": true,
      "reason": "The updated docs keep exact tracing-contract references anchored to the existing contract, preserve the DCoding.Data.DVault ActivitySource naming, and bound observability and performance claims to existing evidence rather than introducing new semantics."
    },
    {
      "expectation": "The touched docs preserve historical references to earlier releases only as carried-forward background, not as the current baseline.",
      "satisfied": true,
      "reason": "Older release references remain background or carried-forward compatibility context while v0.23.0 is presented as the active documentation baseline."
    },
    {
      "expectation": "Repository validation commands and evidence references are documented, and manual review confirms the touched anchors and cross-links within the affected docs.",
      "satisfied": true,
      "reason": "Recorded validation evidence shows the repository command gates succeeding on the verified commit, the release note includes focused evidence references, and no cross-link or anchor issues were reported during tester verification."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027d70c00f250a2\u0027 on branch \u0027ticket/06F5Q94SQ086B2DZ1AKFDXGV94-task-update-v0-23-0-tracing-and-performance-guid\u0027.",
    "Committed repository path \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027 exists at verified commit \u0027d70c00f250a2\u0027.",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: # DVault V1 Activity Tracing Contract",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: Status: v1 contract",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: Ticket: 06F5Q93YXHSKABD2SABWY85S78",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: Current public baseline: [DVault v0.23.0 Release Notes](../releases/v0.23.0.md)",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: Telemetry baseline: [DVault v0.16.0 Release Notes](../releases/v0.16.0.md)",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: ## Decision",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: Tracing complements the existing telemetry surfaces. \u0060IDataVaultTelemetryObserver\u0060, \u0060DataVaultSaveTelemetrySummary\u0060, \u0060DataVaultReadTelemetrySummary\u0060, \u0060AddDVaultTelemetry()\u0060, and th...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: \u0060dvault.provider\u0060 is the Entity Framework provider name when it is already available from the operation context. It must be omitted when unavailable. It must not contain a connecti...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: - \u0060ActivityStatusCode.Error\u0060",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: Status descriptions must be omitted or use only static bounded text from this contract. They must not include exception messages, provider error messages, SQL text, generated table...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: DVault Activity names, tags, events, status descriptions, and exception metadata must never include:",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: - provider error messages",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: The tracing surface is for low-cardinality operational shape and outcome evidence. It is not a data inspection, SQL inspection, support-bundle, or diagnostics text transport. Exist...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: ## Verification Expectations",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: Downstream tracing implementation tickets must include focused verification for:",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: - Fault mapping: faulted operations set \u0060ActivityStatusCode.Error\u0060, \u0060dvault.outcome=fault\u0060, \u0060dvault.failure.kind=fault\u0060, a finite \u0060dvault.failure.class\u0060, redacted \u0060dvault.exception...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: - Cancellation mapping: canceled operations set \u0060ActivityStatusCode.Error\u0060, \u0060dvault.outcome=canceled\u0060, \u0060dvault.failure.kind=cancellation\u0060, \u0060dvault.failure.class=cancellation\u0060, and ...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-activity-tracing-contract.md\u0027: - Redaction proof: Activity names, tags, events, status descriptions, and exception metadata do not contain raw business keys, hash keys, payload values, metadata names, table name...",
    "Committed repository path \u0027docs/performance-profiles.md\u0027 exists at verified commit \u0027d70c00f250a2\u0027.",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: # Performance Profiles",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: Status: v0.23.0 adopter guidance",
    "Observed committed repository file \u0027docs/performance-profiles.md\u0027: This guide is the detailed performance-profile reference for the current v0.23.0 DVault documentation baseline. It translates the checked-in benchmark evidence into starting profil...",
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
    "Committed repository path \u0027docs/production-adoption-checklist.md\u0027 exists at verified commit \u0027d70c00f250a2\u0027.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: # Production Adoption Checklist",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: Use this checklist when preparing a DVault-consuming application for production. It is a routing document for adopter readiness; follow the linked source documents for setup exampl...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: ## Package And Provider Baseline",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Install the provider-neutral \u0060DCoding.Data.DVault\u0060 package from NuGet and use the published installation guidance in the [README](../README.md#installation).",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Select the DVault provider package that matches the application database and keep every DVault package on one aligned published release version.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat [v0.23.0 release notes](releases/v0.23.0.md) as the current public baseline for coordinated package scope, listener-driven Activity tracing, benchmark-backed performanc...",
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
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Preserve the Activity tracing redaction boundary: no raw business keys or hash-key values, payload values, record sources, SQL text, credentials, connection strings, provider...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat chunked-save telemetry as bounded operational evidence: chunk count, processed chunk count, retained-state high-water counts, finite retained-state fallback causes, uns...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat telemetry as bounded operational evidence only. Do not expect DVault to configure metric listeners, exporters, dashboards, alert rules, backend-specific pipelines, or h...",
    "Committed repository path \u0027docs/releases/v0.23.0.md\u0027 exists at verified commit \u0027d70c00f250a2\u0027.",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: # DVault v0.23.0 Release Notes",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: Release: \u0060v0.23.0 - Activity Tracing And Performance Guidance Documentation\u0060",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: Intended release date: 2026-05-31",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: These notes define the v0.23.0 documentation boundary. They do not record a NuGet push, package hashes, final publication links, release approval, benchmark rerun, or package-publi...",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: ## Package Scope",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: This is a coordinated release record for the seven-package DVault NuGet family:",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: - Load timestamp storage \u0060ProviderDefault\u0060.",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: All packages are version-aligned at \u00600.23.0\u0060. Package publication remains a separate manual release activity.",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: v0.23.0 moves the current coordinated public baseline forward for the Activity tracing contract and benchmark-backed performance-profile guidance. It does not add product code, pub...",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: The adopter-facing redaction boundary is explicit: DVault Activity names, tags, events, status descriptions, and exception metadata must not include raw business keys or hash-key v...",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: The detailed v0.23.0 adopter guidance is [Performance Profiles](../performance-profiles.md). It summarizes four starting profiles and routes timing-sensitive claims back to the che...",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: ## Benchmark Evidence",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: The benchmark runner and artifact rules remain documented by [DVault Benchmarks](../../benchmarks/DCoding.Data.DVault.Benchmarks/README.md) and [Performance Evidence And Benchmark ...",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: Timing prose must stay attached to that run context. The skipped optional-provider rows are visible evidence for planned provider boundaries and skip reasons; they are not measured...",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: Provider-specific optimized save strategies remain diagnostics-gated implementations behind the same public service contract. PostgreSQL staged COPY, MySQL staged bulk, SQL Server ...",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: Typed satellite read-model generation remains support-bundle-driven and satellite-only. Raw \u0060dvault.model.v1\u0060 artifacts remain reviewed workflow inputs, not direct generator inputs...",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: ## Validation Evidence",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: Focused repository evidence surfaces for the v0.23.0 story are:",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: - [Performance Evidence And Benchmark Artifact Contract](../plans/performance-evidence-benchmark-artifact-contract.md)",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: - this release note",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: Those documents should tell one consistent story: v0.23.0 is the current coordinated documentation baseline; Activity tracing is listener-driven and redacted; telemetry metrics and...",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: v0.23.0 documentation does not implement product-code changes, benchmark harness changes, public API changes, new tracing contracts, new span names, new event names, new telemetry ...",
    "Observed committed repository file \u0027docs/releases/v0.23.0.md\u0027: This release does not reopen carried-forward compatibility boundaries for explicit save and read services, PIT and bridge maintenance, provider registration, provider package respo...",
    "Committed repository path \u0027README.md\u0027 exists at verified commit \u0027d70c00f250a2\u0027.",
    "Observed committed repository file \u0027README.md\u0027: # DVault",
    "Observed committed repository file \u0027README.md\u0027: DVault is the repository for the \u0060DCoding.Data.DVault\u0060 .NET library.",
    "Observed committed repository file \u0027README.md\u0027: ## Installation",
    "Observed committed repository file \u0027README.md\u0027: Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The coordinated DVault package family is vers...",
    "Observed committed repository file \u0027README.md\u0027: \u0060\u0060\u0060sh",
    "Observed committed repository file \u0027README.md\u0027: dotnet add package DCoding.Data.DVault --version 0.23.0",
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
    "Observed committed repository file \u0027README.md\u0027: The current coordinated documentation baseline is [DVault v0.23.0 Release Notes](docs/releases/v0.23.0.md), which consolidates listener-driven Activity tracing and benchmark-backed...",
    "Observed committed repository file \u0027README.md\u0027: - Model-first governance for reviewed \u0060dvault.model.v1\u0060 JSON artifacts that should be imported, projected into EF metadata, exported canonically, compared against generated metadat...",
    "Observed committed repository file \u0027README.md\u0027: Choose one authoritative path for a model boundary and keep the others as compatible alternatives for different ownership needs. See [Model-First Governance Workflow](docs/model-fi...",
    "Observed committed repository file \u0027README.md\u0027: Applications that want an early runtime check for unsafe generated-row EF tracking can opt into the separate SaveChanges guard interceptor. \u0060AddDVault()\u0060 does not enable this guard...",
    "Observed committed repository file \u0027README.md\u0027: The carried-forward v0.21.0 documentation boundary keeps that same write hierarchy and the PIT/bridge read-model baseline. \u0060IDataVaultSaveService\u0060 remains the public write entry po...",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027 exists at verified commit \u0027d70c00f250a2\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027: using System.Data.Common;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027: using System.Diagnostics;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027: internal static class DataVaultActivityTracing {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027: public const string SourceName = \u0022DCoding.Data.DVault\u0022;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027: return new DataVaultMaintenanceActivity(activity, Stopwatch.GetTimestamp());",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027: private readonly long _startTimestamp;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027: public DataVaultMaintenanceActivity(Activity activity, long startTimestamp) {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027: _startTimestamp = startTimestamp;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultActivityTracing.cs\u0027: activity.SetStatus(exception is null ? ActivityStatusCode.Ok : ActivityStatusCode.Error);",
    "Committed branch delta contains 6 inspectable repository path(s): Modified: docs/architecture/dvault-v1-activity-tracing-contract.md, Modified: docs/performance-profiles.md, Modified: docs/production-adoption-checklist.md, Added: docs/releases/v0.23.0.md, Modified: README.md, Modified: src/DCoding.Data.DVault/DataVaultActivityTracing.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 208 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/documentation, area/ef-core, area/observability, area/performance, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027develop\u0027.",
    "Ticket history references implementation commit \u0027d70c00f250a2\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator using branch ticket/06F5Q94SQ086B2DZ1AKFDXGV94-task-update-v0-23-0-tracing-and-performance-guid at commit d70c00f250a2 for final acceptance review."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F5Q94SQ086B2DZ1AKFDXGV94`
- target-role: `integrator`
- verification-summary: Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch 'ticket/06F5Q94SQ086B2DZ1AKFDXGV94-task-update-v0-23-0-tracing-and-performance-guid' at commit 'd70c00f250a2'.
- acceptance-criteria: `6/6` satisfied
- definition-of-done: `5/5` satisfied
- implementation-branch: `ticket/06F5Q94SQ086B2DZ1AKFDXGV94-task-update-v0-23-0-tracing-and-performance-guid`
- implementation-commit: `d70c00f250a2`
- implementation-pr: `<none>`
- implementation-change: `<none>`