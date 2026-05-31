<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined as one bounded docs story: create a dedicated adopter-facing performance profile guide grounded in the checked-in benchmark triplet, preserve the existing epic and blocking relations, and avoid broad repo-wide release-doc consolidation in this ticket.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository inspection showed no human clarification comments on the ticket; the only current comments are bot lease and claim markers.
- Persisted relation state already matches the intended flow: epic `06F5Q93R4633D41Z21WQW3SVGR` is the parent, and this story blocks documentation task `06F5Q94SQ086B2DZ1AKFDXGV94`; no relation write is needed.
- Treat this story as the detailed performance-profile guidance owner; the downstream documentation task should summarize and cross-link this work rather than duplicate the benchmark interpretation.
- Use only currently shipped observability surfaces in this story: `AddDVaultTelemetry()` plus existing save/read diagnostics and benchmark reruns. Do not couple the work to pending Activity tracing tickets.
- The checked-in root benchmark run is SQLite-required with all optional PostgreSQL, SQL Server, MySQL, and Oracle rows present as `executionStatus=skipped`, so provider-specific sections must describe eligibility boundaries and skip posture, not measured external-provider wins.

### Scope In
- Create or update one adopter-facing performance-profile guide under `docs/` as the canonical detailed guidance surface, with narrow benchmark-doc cross-links if needed.
- Document the small app-local vault profile using the SQLite local baseline and existing explicit save/read service posture.
- Document the medium chunked-ingestion profile using `customer-profile-streaming-save` evidence and `DataVaultChunkedSaveRequest` guidance.
- Document the staged provider ingestion profile using the visible provider-native bulk rows and existing provider gate and boundary docs for PostgreSQL, SQL Server, MySQL, and Oracle.
- Document the read-model-heavy profile using `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` evidence plus explicit PIT and bridge maintenance boundaries.
- For every profile, include workload shape, registration starting point, diagnostics or telemetry to inspect, benchmark rows cited, and stop conditions or rerun triggers.

### Scope Out
- No product code, benchmark harness changes, or new benchmark scenarios in this story.
- No dashboards, exporters, collectors, alerting, hosting, database or container provisioning, scheduler templates, or credential-management guidance.
- No tracing-contract or Activity instrumentation guidance beyond the already shipped `AddDVaultTelemetry()` and diagnostics surfaces.
- No absolute performance guarantees detached from artifact run context.
- No broad README, production-checklist, and release-note consolidation beyond the minimal cross-links needed for the detailed guide; coordinated public-doc rollup remains ticket `06F5Q94SQ086B2DZ1AKFDXGV94`.

## Acceptance Criteria
- The delivered guidance explicitly cites `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, `benchmarks/DCoding.Data.DVault.Benchmarks/README.md`, and `docs/plans/performance-evidence-benchmark-artifact-contract.md`, and keeps run-context details attached to any timing claim.
- The small app-local profile explains when default `AddDVault()` is the safe starting point, when `AddDVaultSqlite()` changes the evidence-backed path, which rows support the recommendation, and what diagnostics to inspect before changing registration.
- The medium chunked-ingestion profile uses the `customer-profile-streaming-save` rows, recommends `DataVaultChunkedSaveRequest` only for bounded ordered loaders, gives evidence-backed starting chunk sizes, and states when to prefer materialized bulk or rerun local benchmarks.
- The staged provider ingestion profile describes clean-context and provider-gate expectations for PostgreSQL, SQL Server, MySQL, and Oracle using visible benchmark rows and existing release-note posture, with skipped optional-provider rows described as skipped evidence rather than measured wins.
- The read-model-heavy profile uses `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` rows, states that SQLite is the only repository-proven optimized PIT or bridge read provider path, and keeps PIT and bridge maintenance caller-owned and explicit.
- The guidance stays within consumer-owned observability and infrastructure boundaries and references current telemetry or diagnostics surfaces without inventing new platform responsibilities.

## Definition of Done
- A dedicated adopter-facing performance-profile guidance document is added or updated under `docs/` and serves as the authoritative detailed profile reference for v0.23.0.
- Any benchmark README or narrow discovery links touched by this story point to the new guidance consistently and do not restate incompatible performance posture.
- Each of the four profiles includes workload shape, registration guidance, starting point, diagnostics or telemetry to inspect, supporting benchmark rows, and explicit stop conditions.
- Links and anchors touched by the documentation change resolve.
- Available docs or markdown validation is run if present, or the delivery notes explicitly state that no repository validator exists.
- If benchmark artifacts are regenerated, the markdown/csv/json triplet remains together and the updated documentation explains the new run context.

## Implementation Notes
- Use one canonical detailed guide, preferably a new top-level adopter doc such as `docs/performance-profiles.md`, because `docs/` already holds the repository's other adopter-facing guidance and the downstream release-doc task can summarize or link it later.
- Anchor the small app-local profile to the checked-in SQLite rows for `customer-profile-history`, `customer-profile-bulk-insert-only`, and `customer-profile-bulk-history`; frame `AddDVault()` as the default safe baseline and `AddDVaultSqlite()` as the SQLite-optimized option rather than a universal requirement.
- Anchor the medium chunked-ingestion profile to `customer-profile-streaming-save`: the checked-in SQLite run shows materialized explicit bulk at 6.828 ms, chunk size 10 at 13.330 ms, and chunk size 5 at 19.313 ms, all with provider-neutral fallback; start guidance at chunk size 10 when bounded chunking is required and tell adopters to retune with their own workload.
- Anchor the staged provider ingestion profile to the `provider-native-bulk-ingestion` rows and the explicit save architecture note: PostgreSQL stages at 60-plus operations with retained direct or UNNEST below that boundary; MySQL keeps multi-row above the 50-operation native gate and stages at 60-plus; SQL Server uses native bulk at 50-plus total operations and up to 500 satellite operations; Oracle stays on direct optimized batching at 50-plus total operations and up to 10000 satellite operations with `stagedOracleBulk=not-selected-no-measured-win`.
- Anchor the read-model-heavy profile to `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read`; note that `SqliteDataVaultReadStrategy` is the only repository-proven optimized read path for latest-satellite, PIT, and bridge shapes, and that unsupported providers or shapes fall back through provider-neutral read pipelines.
- Keep observability guidance on existing shipped surfaces only: `AddDVaultTelemetry()`, `IDataVaultReadDiagnosticsService`, save strategy diagnostics, read strategy diagnostics, read-shape diagnostics, and benchmark reruns. Do not describe pending Activity tracing work as if it were already part of this story's deliverable.
- Capture the current checked-in run context whenever quoting times: 3 iterations, 1 warmup iteration, provider filter `all`, Debian GNU/Linux 13, .NET 10.0.8, 32 processors, SQLite required baseline, and all optional external providers skipped because their connection-string environment variables were unset.

## Open Questions
- none

## Follow-Up Questions
- When ticket `06F5Q94SQ086B2DZ1AKFDXGV94` performs the coordinated v0.23.0 documentation rollup, should the new profile guide be summarized from both `README.md` and `docs/production-adoption-checklist.md`, or should one of those stay link-only?
- If a later checked-in benchmark run includes completed external-provider rows, should a follow-up docs pass add measured provider-specific examples that go beyond the current skipped-row boundary guidance?

## Risks
- Because the checked-in optional-provider rows are all skipped, provider-specific sections can easily overclaim unless they stay disciplined about describing gates, fallback behavior, and skip reasons rather than measured wins.
- Timing values are machine-specific and must stay attached to the artifact run context; copying raw numbers without iterations, provider filter, and hardware/runtime context would violate the benchmark evidence contract.
- This story already blocks ticket `06F5Q94SQ086B2DZ1AKFDXGV94`, so expanding it into full coordinated README or release-note consolidation would create unnecessary schedule coupling.

## Split Recommendations
- No split recommended. Keep one detailed performance-guidance story here and leave the broader repo-wide documentation summary work to ticket `06F5Q94SQ086B2DZ1AKFDXGV94`.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Add practical, benchmark-backed performance profile guidance for DVault adopters in v0.23.0.

# Background
DVault already has benchmark artifacts and an evidence contract. This ticket turns that evidence into adopter guidance. The guidance must stay honest about machine-specific timings, required SQLite local baseline, optional external providers, and consumer-owned infrastructure.

# Scope In
- Update documentation that explains performance profiles and when to start from each profile.
- Use repository evidence from `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, `benchmarks/DCoding.Data.DVault.Benchmarks/README.md`, and `docs/plans/performance-evidence-benchmark-artifact-contract.md`.
- Cover these profiles: small app-local vault, medium chunked ingestion, staged provider ingestion, and read-model-heavy PIT/bridge usage.
- For each profile, document the workload shape, likely DVault path, registration guidance, batch or chunk-size starting points, diagnostics/telemetry to inspect, benchmark rows that support the guidance, and stop conditions.
- Preserve existing provider-specific boundaries for SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.

# Scope Out
- No product-code changes.
- No new benchmark scenarios unless the existing evidence is insufficient and the gap is explicitly documented.
- No dashboard, hosted monitoring, collector, container, database, credential, scheduler, or CI provisioning template.
- No absolute performance guarantee detached from benchmark run context.
- No claim that optional providers were executed when benchmark artifacts show skipped rows.

# Profile Content Requirements
- Small app-local vault: emphasize default `AddDVault()`, SQLite/local evidence, explicit save/read services, and when provider-neutral fallback is acceptable.
- Medium chunked ingestion: use the existing `customer-profile-streaming-save` evidence and `DataVaultChunkedSaveRequest` guidance; include starting chunk sizes from current evidence, then tell consumers to tune with their own workload.
- Staged provider ingestion: describe provider-eligible ordered bulk workloads, the documented provider thresholds/boundaries, clean-context expectations, and fallback diagnostics to inspect.
- Read-model-heavy PIT/bridge usage: use latest satellite, PIT as-of, and bridge traversal benchmark rows; point to explicit PIT/bridge maintenance boundaries and SQLite read-strategy evidence.

# Acceptance Criteria
- The guidance names the benchmark artifact files it relies on and keeps run context attached to timing claims.
- Each profile includes concrete starting guidance, diagnostics to inspect, and stop conditions.
- Provider guidance matches existing benchmark evidence and release-note posture. Skipped optional-provider rows remain described as skipped evidence, not as measured wins.
- The documentation tells adopters when to use `AddDVault()`, optional provider registrations, `AddDVaultTelemetry()`, existing diagnostics, and benchmark reruns.
- The documentation does not introduce new platform ownership for dashboards, hosting, container/database provisioning, scheduling, alerting, or credential management.
- Existing release/readme wording remains consistent with the performance-evidence artifact contract.

# Verification
- Run available docs/markdown validation if present.
- Inspect links and anchors touched by the change.
- If benchmark artifacts are regenerated, preserve the required markdown/csv/json triplet and explain the run context in the docs.