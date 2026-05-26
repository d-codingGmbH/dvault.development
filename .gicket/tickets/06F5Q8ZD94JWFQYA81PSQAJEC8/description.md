<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined as an additive AddDVaultPostgres optimization story: stage large eligible PostgreSQL explicit save batches with native transfer while preserving existing save semantics, fallback behavior, and opt-in Postgres evidence lanes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This story is an internal optimization of the existing AddDVaultPostgres()/PostgresDataVaultSaveStrategy boundary, not a new public IDataVaultSaveService API.
- The current repository baseline already has a PostgreSQL optimized path for clean Npgsql contexts using set-based insert/reuse plus latest-state satellite checks; this story adds a staged bulk branch for larger eligible batches instead of reopening that baseline.
- The proof surfaces already exist: Postgres opt-in integration coverage is gated by DVAULT_TEST_POSTGRES_CONNECTION_STRING, and the existing provider-native-bulk-ingestion benchmark row for dvault-adddvaultpostgres-optimized is the benchmark evidence target.
- The core repository already exposes staged-provider bulk diagnostics vocabulary, so PostgreSQL staged-path eligibility, decline, fallback, or cleanup reporting should reuse that existing bounded taxonomy instead of inventing a new one.

### Scope In
- Add a PostgreSQL staged bulk branch inside the existing Postgres provider strategy for large eligible explicit save batches on clean Npgsql-backed DbContext instances.
- Create and populate PostgreSQL transient staging structures, use COPY or another demonstrably provider-native bulk transfer into the stage, and apply staged rows to hub, link, and ordinary satellite targets.
- Preserve current latest-state satellite hash-diff filtering, idempotent hub/link reuse behavior, saved-record ordering, and caller-visible row-count semantics.
- Extend PostgreSQL-gated integration coverage and the existing provider-native benchmark evidence so the staged path is exercised when Postgres is configured.
- Update current documentation and benchmark execution-detail wording needed to describe the landed PostgreSQL optimized boundary accurately.

### Scope Out
- Any new public save-service overloads or changes to DataVaultSaveRequest, DataVaultBulkSaveRequest, or DataVaultChunkedSaveRequest.
- Provider-native chunk execution or any change to the current chunked-save public contract documented for v0.19.0.
- New benchmark artifact schemas or new benchmark scenario families; the existing provider-native-bulk-ingestion PostgreSQL baseline remains the evidence surface.
- Cross-provider optimization redesign for SQLite, SQL Server, MySQL, or Oracle beyond small shared refactors strictly required to support the PostgreSQL implementation.
- Database provisioning, CI secret management, Docker setup, or always-on live Postgres infrastructure.

## Acceptance Criteria
- AddDVaultPostgres selects a PostgreSQL staged bulk path for an internally defined eligible large ordered explicit save batch on a clean Npgsql.EntityFrameworkCore.PostgreSQL context without changing the caller-facing explicit save contract.
- The staged path uses PostgreSQL staging plus COPY or another provider-native bulk transfer into the stage, then applies staged rows to target hub, link, and satellite tables with the same caller-visible persistence outcome as the existing PostgreSQL optimized path.
- Hub and link writes remain idempotent and deterministic, and unchanged latest-state satellite replays continue to be skipped by the existing hash-diff/latest-state rules.
- The staged path preserves caller-owned transaction and cancellation semantics: it participates in the current DbContext transaction when supported, does not silently escape that boundary, and cleans up transient staging objects on success, failure, and cancellation.
- If a batch is ineligible for the staged path or a staged-specific provider limitation is detected before writes, the strategy declines or falls back through existing DVault semantics with bounded diagnostics or telemetry rather than changing persisted results.
- Postgres opt-in integration coverage proves representative single-save and ordered bulk hub/link/satellite persistence through AddDVaultPostgres, including at least one provider-eligible ordered bulk batch that exercises the staged path and leaves no tracked DVault entities behind.
- When PostgreSQL is configured, the existing provider-native-bulk-ingestion benchmark row for dvault-adddvaultpostgres-optimized completes and its executionDetail makes the staged PostgreSQL boundary visible; when PostgreSQL is not configured, the skipped-row contract remains unchanged.

## Definition of Done
- Unit and integration test coverage exists for PostgreSQL staged-bulk eligibility, staging/apply behavior, rollback or cleanup behavior, and unchanged latest-state satellite replay behavior.
- The existing Postgres provider smoke and external opt-in test lanes continue to pass, and default local test execution remains runnable without a live PostgreSQL database.
- Benchmark contract tests and checked-in benchmark artifact expectations are updated so the PostgreSQL optimized provider-native row remains part of the documented artifact matrix.
- Current README and architecture surfaces that describe the PostgreSQL optimized write path are updated to match the landed staged-bulk boundary and fallback posture.
- The implementation lands without changing the public explicit-save API surface or benchmark artifact schema.

## Implementation Notes
- Keep the work centered on src/DCoding.Data.DVault.Postgres and the existing PostgresDataVaultSaveStrategy registration path; do not introduce a new public strategy entry point or new save request type.
- Treat the current PostgreSQL UNNEST or set-based path as the small-batch baseline and add the staged branch as an eligibility-gated larger-batch optimization rather than replacing every PostgreSQL optimized save path indiscriminately.
- Reuse the existing resolved-request pipeline, hash computation, saved-record ordering, and latest-state satellite filtering; the staged work should change the physical write path, not the explicit save contract.
- If the strategy reports staged-path eligibility or fallback details, reuse DataVaultStagedProviderBulkDiagnostics, DataVaultStagedProviderBulkLifecyclePhase, DataVaultStagedProviderBulkProviderCaveatKind, and the existing staged-provider fallback cause kinds.
- Preserve the existing benchmark identity for the PostgreSQL optimized path: baseline dvault-adddvaultpostgres-optimized, strategy family postgres-optimized-dvault, and the existing provider-native-bulk-ingestion scenario row.
- Keep v0.19.0 release notes historical; document the landed behavior in current docs and the next coordinated release note rather than rewriting the historical v0.19.0 public claim set.

## Open Questions
- none

## Follow-Up Questions
- Which upcoming coordinated release note should become the first public claim set that advertises PostgreSQL staged bulk as a documented provider optimization beyond the historical v0.19.0 baseline?

## Risks
- A PostgreSQL staged path may need provider-specific runtime hooks not used by the current Postgres package path, which increases implementation complexity and can create packaging or dependency tradeoffs if not kept bounded.
- Temporary staging cleanup and transaction participation are the main regression areas; failures there could leave transient artifacts or cause unintended fallback unless explicitly covered by tests.
- The current PostgreSQL optimized path already has a non-staged set-based implementation, so applying staging too broadly can regress small or medium batches if eligibility remains poorly tuned.
- PostgreSQL performance and live integration proof are external-opt-in, so benchmark and integration evidence can remain skipped on machines without DVAULT_TEST_POSTGRES_CONNECTION_STRING; the skipped-row contract must stay visible instead of silently dropping evidence.

## Split Recommendations
- If provider-specific runtime dependency or packaging-policy work grows beyond the PostgreSQL write path itself, split that dependency-policy decision from the staged-bulk behavior and evidence work.
- If broader multi-provider staged-bulk diagnostics alignment becomes necessary, keep this story focused on PostgreSQL behavior and proof and move cross-provider diagnostics symmetry into a separate follow-up ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Implement a PostgreSQL staged bulk path for eligible DVault save batches.

Acceptance criteria:
- Uses PostgreSQL-appropriate staging and COPY or equivalent native transfer.
- Preserves idempotency, hash-diff latest-state checks, transactions, cancellation, and cleanup.
- Adds PostgreSQL-gated integration tests and benchmark rows when configured.