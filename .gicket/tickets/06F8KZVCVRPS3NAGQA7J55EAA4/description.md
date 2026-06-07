<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the artifact evidence ticket against the shared benchmark artifact contract, the landed SQL Server dry-run prototype, and current relation state; the existing split remains sufficient and no persistent planning write was required.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Verified live context: epic 06F8KZTCEMNNFBFTVMFXEN268M parents this task, architecture story 06F8KZTNG44XDPMVTVCV4WJSHG is done, dry-run prototype story 06F8KZV18BQ0GN3CE4G02ATVA0 is done historical evidence, and this task still blocks docs ticket 06F8KZVRARQPG482YKCQ686PNM plus all-provider baseline task 06F9XD26D2MHVAKZ2GCZ67BEFC.
- The authoritative benchmark evidence contract is docs/plans/performance-evidence-benchmark-artifact-contract.md; this ticket does not invent ticket-specific benchmark filenames or schemas.
- Each provider-specific artifact proposal is assessed one exact provider and one representative workload at a time, using matched-input request diagnostics plus comparable benchmark artifacts.
- The semantic parity checklist is ratified from current landed contracts and prototype code: ordering, load timestamp, record source, hash key, hash diff, latest-state behavior, cancellation, cleanup boundary, and caller-owned transaction behavior are required, with PIT/bridge maintenance added when the workload exercises those surfaces.
- No child-ticket creation, relation cleanup, description update, attachment, or planning-document write was materialized in this pass.

### Scope In
- Define the mandatory benchmark artifact set and before/after comparison rules for provider-specific artifact proposals.
- Ratify the current required local SQLite baseline rows and optional external-provider visibility rules as the shared performance evidence floor.
- Define the required semantic parity checklist and the repository evidence anchors that future prototype or implementation tickets must cite.
- Distinguish prototype or documentation evidence from implementation-ready provider claims.

### Scope Out
- Running new benchmarks or completing the all-provider Podman baseline itself.
- Generating or updating SQL artifact manifests, sidecar SQL payloads, or runtime dispatch code.
- Changing benchmark harness schemas, adding new providers, or widening the workload matrix beyond one provider/workload proposal at a time.
- Automatic deployment, invocation, migration synchronization, or operational ownership.

## Acceptance Criteria
- The refined ticket states that every persisted benchmark evidence set uses the shared benchmark-summary.md / benchmark-summary.csv / benchmark-summary.json triplet from one execution, and before/after claims use comparable labeled triplet sets with matched scenario mode, provider filter, iteration count, warmup count, load-timestamp storage, and provider configuration.
- The refined ticket ratifies the existing benchmark row contract and visibility rules: rows keep scenario, provider, baseline, strategy family, dataset/change context, execution status, skip reason, timing/allocation fields, deterministic executionDetail, and persistedOutcome, with skipped or failed optional-provider rows remaining visible instead of disappearing.
- The refined ticket identifies the current baseline scenario set that artifact evidence may reuse from the shared contract: customer profile history, customer profile bulk insert-only, customer profile bulk history, customer profile streaming save, order-product fulfillment history, latest satellite read, PIT as-of read, and bridge traversal read; provider-native artifact proposals additionally compare against the provider-native-bulk-ingestion workload when that is the selected workload.
- The refined ticket requires request-bound diagnostics for the exact provider/workload and requires any artifact proposal to bind one provider/workload pair only, using the existing selected-strategy and fallback evidence rather than broad provider-family claims.
- The refined ticket requires a semantic parity checklist covering ordering, load timestamp, record source, hash key, hash diff, latest-state behavior, cancellation, cleanup, and caller-owned transaction behavior, with PIT or bridge maintenance added when relevant to the workload.
- The refined ticket explicitly distinguishes prototype or documentation evidence from implementation-ready claims: skipped external-provider rows and dry-run manifests are acceptable historical/prototype evidence, but production-readiness claims need configured exact-provider diagnostics and benchmark proof for the same workload.
- The refined ticket keeps current non-goals intact: no runtime dispatch, no automatic execution, no automatic deployment, no EF migration synchronization, and no new benchmark artifact schema.

## Definition of Done
- The accepted refinement clearly points implementers to the shared benchmark artifact contract, docs/performance-profiles.md, and the explicit-save/service artifact boundary as the authoritative sources.
- Future artifact-lane tickets can cite one bounded provider/workload evidence gate without reopening whether skipped optional-provider rows stay visible or whether semantic parity includes transaction and cleanup behavior.
- The refinement preserves the existing split: this ticket owns evidence requirements, 06F8KZV18BQ0GN3CE4G02ATVA0 owns the dry-run manifest prototype, 06F8KZVRARQPG482YKCQ686PNM owns documentation alignment, and 06F9XD26D2MHVAKZ2GCZ67BEFC owns the all-provider benchmark capture follow-up.
- No part of this refinement widens scope into new provider support, runtime product code, or operational rollout ownership.

## Implementation Notes
- Use docs/plans/performance-evidence-benchmark-artifact-contract.md as the source of truth for triplet files, run-context fields, row fields, skipped-row visibility, and regression-budget language.
- Use docs/performance-profiles.md and docs/architecture/dvault-v1-explicit-save-service.md as the admission gate for provider-specific artifact proposals and their non-goals.
- Use the landed SQL Server dry-run prototype in src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs as the concrete v1 example of benchmark row references plus semantic parity field names.
- Use tests/DCoding.Data.DVault.Tests/Unit/StreamingExplicitSaveContractSnapshotTests.cs, tests/DCoding.Data.DVault.Tests/Shared/ProviderSqlExecutionContract.cs, and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs as existing evidence anchors for ordering, hash-diff/latest-state continuity, cancellation, cleanup, and caller-owned transaction behavior.
- Use tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs as the verifier that the checked-in benchmark triplet, streaming-save metadata, and optional-provider provider-native row identities stay synchronized.
- No persistent ticket mutation was applied in this pass; the refinement is based on verified repository and .gicket state only.

## Open Questions
- none

## Follow-Up Questions
- After 06F9XD26D2MHVAKZ2GCZ67BEFC lands, should the docs task cite exact completed all-provider rows or keep the current prototype-era skipped-row examples in v0.32 guidance?
- If the next artifact prototype is not SQL Server, should the team prefer PostgreSQL staged COPY or MySQL staged bulk as the next contrasting provider boundary after the current dry-run example?
- Should a later ticket define provider-specific parity additions for PIT/bridge-focused artifact workloads beyond the save-oriented checklist ratified here?

## Risks
- The checked-in root benchmark triplet still shows optional external-provider rows as skipped when connection strings are unset, so this refinement must not be misread as completed live all-provider evidence.
- If future tickets omit matched-input diagnostics or hide skipped optional-provider rows, they will undermine the comparability rules this ticket is supposed to lock down.
- If provider-specific artifact work substitutes provider-side hashing, changes request ordering, or suppresses caller transaction ownership, it will violate the parity boundary already documented in current contracts and tests.
- Because the landed prototype is SQL Server-specific, teams may overgeneralize its workload facts unless this ticket keeps the one-provider/one-workload rule explicit.

## Split Recommendations
- No new split is justified; the current evidence/prototype/documentation/all-provider-baseline separation is sufficient.
- Create separate follow-up tickets instead of widening this task if the team wants deployable sidecar SQL payloads, runtime invocation helpers, provider-specific cleanup validators, or multi-workload/provider-matrix parity suites.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define benchmark and equivalence requirements for artifact proposals, including ordering, load timestamps, record source, hash keys, hash diffs, fallback, cancellation, cleanup, and transaction parity.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Decision: `already_satisfied_on_branch`

Summary:
- No repository diff is required for this developer pass.
- The current branch already carries the benchmark artifact evidence contract and semantic parity evidence anchors required by the ticket.
- This supplemental block documents the developer handoff while preserving the existing authoritative delivery contract above.

Repository evidence:
- `docs/plans/performance-evidence-benchmark-artifact-contract.md` defines the required `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` artifact set plus comparable before/after input rules.
- `docs/performance-profiles.md` points adopters to the root benchmark triplet and preserves skipped optional-provider rows as visible evidence rather than hiding them.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` verifies generated and checked-in benchmark triplets stay synchronized with row fields, optional-provider context, execution detail, and persisted outcome.
- `src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs` and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs` keep the SQL Server dry-run artifact scoped to the `provider-native-bulk-ingestion` workload with benchmark artifact and semantic parity metadata.
- `tests/DCoding.Data.DVault.Tests/Shared/ProviderSqlExecutionContract.cs`, `tests/DCoding.Data.DVault.Tests/Unit/StreamingExplicitSaveContractSnapshotTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` provide the transaction, cancellation, ordering, hash-diff continuity, latest-state, cleanup, and caller-owned transaction evidence anchors future artifact-lane tickets must cite.

Verification performed:
- Confirmed the checked-out branch is `ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari`.
- Confirmed all expected repository paths are tracked with `git ls-files`, including `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, and `tests/DCoding.Data.DVault.Tests/Shared/ProviderSqlExecutionContract.cs`.
- Ran bounded grep checks for benchmark triplet references, provider-native workload metadata, benchmark artifact metadata, and transaction/cancellation parity anchors.
- `git status --short` returned no changed paths.
- Build, test, and format commands were not run because no repository files changed.

Risks carried forward:
- Skipped optional-provider rows and dry-run manifests remain visibility or prototype evidence, not production-readiness proof.
- Future exact-provider claims still need configured diagnostics and completed comparable benchmark evidence for the same provider/workload pair.

<!-- gicket-bot:developer-delivery:v1:end -->