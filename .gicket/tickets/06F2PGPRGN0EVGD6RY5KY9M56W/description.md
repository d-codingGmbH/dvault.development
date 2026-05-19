<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement corrected the delivery contract with current-branch source evidence for the existing PIT and bridge public read APIs, kept SQLite as the required first optimized proof provider, and made no child-ticket, relation, attachment, or planning-document changes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current branch source confirms the existing public PIT surface: `DataVaultPitAsOfReadRequest` in `src/DCoding.Data.DVault/DataVaultPitAsOfReadRequest.cs`, `IDataVaultReadService.ReadPitRowsAsync(...)` in `src/DCoding.Data.DVault/IDataVaultReadService.cs`, and `ReadPitAsync(...)` in `src/DCoding.Data.DVault/DataVaultReadServicePitExtensions.cs`.
- Current branch source confirms the existing public bridge surface: `DataVaultBridgeReadRequest` in `src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs`, `ReadBridgeRowsAsync(...)` and `ReadBridgeAsync(...)` in `src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs`, and registry-backed bridge adapters in `src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs`.
- Provider-aware read dispatch is currently limited to latest/as-of satellite reads via `IDataVaultProviderReadStrategy`, `DefaultDataVaultReadService`, and `SqliteDataVaultReadStrategy`; PIT and bridge reads remain provider-neutral in source today.
- README, `docs/releases/v0.7.0.md`, and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` still describe PIT and bridge reads as provider-neutral baselines and limit provider-specific read optimization evidence to latest-satellite reads, so SQLite remains the required first optimized proof point for this story.
- No child tickets, relation edits, attachments, or planning documents were materialized in this refinement pass.

### Scope In
- Add provider-aware dispatch or strategy selection for PIT reads beneath the existing `DataVaultPitAsOfReadRequest`, `ReadPitRowsAsync(...)`, and `ReadPitAsync(...)` compatibility surface.
- Add provider-aware dispatch or strategy selection for bridge reads beneath the existing `DataVaultBridgeReadRequest`, `ReadBridgeRowsAsync(...)`, and `ReadBridgeAsync(...)` compatibility surface.
- Keep provider-neutral PIT and bridge pipelines as supported fallback paths when a provider or request shape is not accepted for optimization.
- Add the SQLite tests, benchmarks, diagnostics, README, and release-note updates needed to prove optimized-path selection and semantic parity.

### Scope Out
- Breaking changes to existing PIT or bridge request types, method names, or caller-visible semantics.
- Implicit PIT refresh, automatic bridge maintenance, schedulers, interceptors, or read-time maintenance side effects.
- Full graph traversal redesign, path-payload or effectivity features, link-based PITs, multi-active PITs, or delete-aware hierarchy policy changes.
- Bundling non-SQLite provider-specific PIT or bridge optimizations into this story unless they fit after the SQLite proof path without expanding scope.

## Acceptance Criteria
- For SQLite, calls through the existing `DataVaultPitAsOfReadRequest` plus `IDataVaultReadService.ReadPitRowsAsync(...)` and `ReadPitAsync(...)` surface select an optimized PIT path when supported and preserve parent selection, matched PIT row choice, satellite snapshot binding, ordering, and failure semantics relative to the current provider-neutral PIT pipeline.
- For SQLite, calls through the existing `DataVaultBridgeReadRequest` plus `ReadBridgeRowsAsync(...)` and `ReadBridgeAsync(...)` surface select an optimized bridge path when supported and preserve endpoint filtering, hierarchy `maximumDepth` handling, `TraversalDepth` semantics, ordering, and failure semantics relative to the current provider-neutral bridge pipeline.
- Unsupported providers, unsupported request shapes, or declined optimization paths fall back cleanly to the current provider-neutral PIT or bridge pipelines with no implicit PIT refresh or bridge maintenance side effects.
- Automated coverage proves optimized-path selection, correctness, and fallback behavior for SQLite PIT and bridge reads against maintained PIT and bridge tables.
- Benchmark, diagnostics, README, and current release-note evidence clearly distinguish provider-neutral PIT/bridge reads from any new optimized path and do not claim provider-specific PIT/bridge optimization for unproven providers.

## Definition of Done
- SQLite unit and integration coverage preserves the existing PIT and bridge semantic baseline while adding optimized-path and fallback assertions.
- Repository benchmark scenarios for PIT as-of and bridge traversal reads are updated or reused so SQLite optimized evidence is reproducible from repository tooling.
- Diagnostics and any required public API snapshot or approval-fixture updates make optimized-path selection or any additive public surface visible rather than silent.
- README, current release-note content, and benchmark guidance are consistent with the delivered optimization boundary and do not over-claim providers not proven in-repository.

## Implementation Notes
- The PIT public boundary is source-backed today: `DataVaultPitAsOfReadRequest` and `IDataVaultReadService.ReadPitRowsAsync(...)` are public core types, and `ReadPitAsync(...)` is a public extension method that already delegates to `ReadPitRowsAsync(...)`.
- The bridge public boundary is source-backed today: `DataVaultBridgeReadRequest`, `ReadBridgeRowsAsync(...)`, and `ReadBridgeAsync(...)` already exist, and bridge callers must keep those request/helper contracts stable.
- Current provider-aware read dispatch infrastructure is latest-satellite-specific. `IDataVaultProviderReadStrategy` only exposes latest/as-of satellite hooks, and `DefaultDataVaultReadService` only dispatches those requests today.
- Bridge reads currently bypass `DefaultDataVaultReadService`: `DataVaultReadServiceBridgeExtensions` calls `DataVaultBridgeReadPipeline` directly, so bridge optimization may require additive internal dispatch plumbing behind the existing extension methods while preserving their public signatures.
- Compatibility baselines already exist in `tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs` and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs`; PIT behavior must also stay aligned with `docs/plans/pit-maintenance-service-v1-contract.md`.

## Open Questions
- none

## Follow-Up Questions
- After SQLite proof lands, which external provider package should be prioritized next for PIT and bridge read optimization: PostgreSQL, SQL Server, MySQL, or Oracle?
- Should external-provider PIT and bridge benchmark artifacts remain opt-in after SQLite lands or become part of the default archived evidence set?
- If provider-aware read dispatch grows beyond PIT and bridge, should a follow-up ticket unify the latest-satellite, PIT, and bridge optimization plumbing under one broader read-strategy contract?

## Risks
- Because `IDataVaultProviderReadStrategy` is currently latest-satellite-specific and bridge reads bypass `DefaultDataVaultReadService`, extending provider-aware optimization may require additive dispatch abstractions or public API snapshot churn.
- PIT snapshot selection and bridge depth and ordering rules are correctness-sensitive; provider-specific SQL can introduce subtle regressions even when happy-path results look similar.
- Repository docs and benchmark guidance currently describe PIT and bridge reads as provider-neutral baselines; shipping optimization without matching evidence would create documentation and release-note drift.
- SQLite is the only required local proof lane for this ticket, so non-SQLite expansion may surface new dispatch or SQL-shape constraints later.

## Split Recommendations
- No split is required from the current evidence; the story can stay whole if the work remains bounded to SQLite proof plus provider-neutral fallback safety.
- If implementation grows materially, split first into a shared provider-aware read-dispatch slice and two execution slices: PIT optimization and bridge optimization.
- If same-release proof is needed for non-SQLite providers, track each external provider package in its own child ticket instead of expanding this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Use provider-specific read strategies for maintained PIT and bridge structures.

## Scope
- Refine and complete the work for "Add provider-aware PIT and bridge read optimization" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.