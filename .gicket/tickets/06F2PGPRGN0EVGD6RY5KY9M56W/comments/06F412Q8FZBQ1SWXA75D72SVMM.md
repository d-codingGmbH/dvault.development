[gicket-bot] PO refinement contract

Summary
- Refinement corrected the delivery contract with current-branch source evidence for the existing PIT and bridge public read APIs, kept SQLite as the required first optimized proof provider, and made no child-ticket, relation, attachment, or planning-document changes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now cites source-backed PIT and bridge read surfaces instead of inference: `DataVaultPitAsOfReadRequest`, `DataVaultBridgeReadRequest`, `IDataVaultReadService.ReadPitRowsAsync(...)`, `DataVaultReadServicePitExtensions.ReadPitAsync(...)`, and `DataVaultReadServiceBridgeExtensions.ReadBridgeRowsAsync(...)`/`ReadBridgeAsync(...)` are all visible in the current branch, so the story scope is to add provider-aware optimization beneath those existing caller-facing surfaces.
- critic-item-2: `answered` - The persisted contract no longer infers missing public APIs. Existing PIT and bridge request/helper surfaces are now treated as verified source-backed compatibility boundaries, while provider-aware optimization itself remains the work to add because current dispatch infrastructure only proves provider-specific latest/as-of satellite reads.
- critic-item-3: `answered` - The earlier statement that `ReadPitAsync(...)`, `ReadBridgeRowsAsync(...)`, and `ReadBridgeAsync(...)` were not pre-verified is no longer accurate. Current-branch source and SQLite integration coverage confirm those helpers exist today, so the contract now preserves them as existing public surfaces and limits new work to provider-aware PIT/bridge optimization behind them.

Clarifications
- Current branch source confirms the existing public PIT surface: `DataVaultPitAsOfReadRequest` in `src/DCoding.Data.DVault/DataVaultPitAsOfReadRequest.cs`, `IDataVaultReadService.ReadPitRowsAsync(...)` in `src/DCoding.Data.DVault/IDataVaultReadService.cs`, and `ReadPitAsync(...)` in `src/DCoding.Data.DVault/DataVaultReadServicePitExtensions.cs`.
- Current branch source confirms the existing public bridge surface: `DataVaultBridgeReadRequest` in `src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs`, `ReadBridgeRowsAsync(...)` and `ReadBridgeAsync(...)` in `src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs`, and registry-backed bridge adapters in `src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs`.
- Provider-aware read dispatch is currently limited to latest/as-of satellite reads via `IDataVaultProviderReadStrategy`, `DefaultDataVaultReadService`, and `SqliteDataVaultReadStrategy`; PIT and bridge reads remain provider-neutral in source today.
- README, `docs/releases/v0.7.0.md`, and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` still describe PIT and bridge reads as provider-neutral baselines and limit provider-specific read optimization evidence to latest-satellite reads, so SQLite remains the required first optimized proof point for this story.
- No child tickets, relation edits, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Add provider-aware dispatch or strategy selection for PIT reads beneath the existing `DataVaultPitAsOfReadRequest`, `ReadPitRowsAsync(...)`, and `ReadPitAsync(...)` compatibility surface.
- Add provider-aware dispatch or strategy selection for bridge reads beneath the existing `DataVaultBridgeReadRequest`, `ReadBridgeRowsAsync(...)`, and `ReadBridgeAsync(...)` compatibility surface.
- Keep provider-neutral PIT and bridge pipelines as supported fallback paths when a provider or request shape is not accepted for optimization.
- Add the SQLite tests, benchmarks, diagnostics, README, and release-note updates needed to prove optimized-path selection and semantic parity.

Scope Out
- Breaking changes to existing PIT or bridge request types, method names, or caller-visible semantics.
- Implicit PIT refresh, automatic bridge maintenance, schedulers, interceptors, or read-time maintenance side effects.
- Full graph traversal redesign, path-payload or effectivity features, link-based PITs, multi-active PITs, or delete-aware hierarchy policy changes.
- Bundling non-SQLite provider-specific PIT or bridge optimizations into this story unless they fit after the SQLite proof path without expanding scope.

Open questions
- none

Follow-up questions
- After SQLite proof lands, which external provider package should be prioritized next for PIT and bridge read optimization: PostgreSQL, SQL Server, MySQL, or Oracle?
- Should external-provider PIT and bridge benchmark artifacts remain opt-in after SQLite lands or become part of the default archived evidence set?
- If provider-aware read dispatch grows beyond PIT and bridge, should a follow-up ticket unify the latest-satellite, PIT, and bridge optimization plumbing under one broader read-strategy contract?

Risks
- Because `IDataVaultProviderReadStrategy` is currently latest-satellite-specific and bridge reads bypass `DefaultDataVaultReadService`, extending provider-aware optimization may require additive dispatch abstractions or public API snapshot churn.
- PIT snapshot selection and bridge depth and ordering rules are correctness-sensitive; provider-specific SQL can introduce subtle regressions even when happy-path results look similar.
- Repository docs and benchmark guidance currently describe PIT and bridge reads as provider-neutral baselines; shipping optimization without matching evidence would create documentation and release-note drift.
- SQLite is the only required local proof lane for this ticket, so non-SQLite expansion may surface new dispatch or SQL-shape constraints later.

Split recommendations
- No split is required from the current evidence; the story can stay whole if the work remains bounded to SQLite proof plus provider-neutral fallback safety.
- If implementation grows materially, split first into a shared provider-aware read-dispatch slice and two execution slices: PIT optimization and bridge optimization.
- If same-release proof is needed for non-SQLite providers, track each external provider package in its own child ticket instead of expanding this story.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment