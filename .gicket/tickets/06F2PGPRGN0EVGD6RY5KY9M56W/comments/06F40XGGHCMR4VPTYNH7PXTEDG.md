[gicket-bot] PO refinement contract

Summary
- Refinement removes unsupported inferred PIT/bridge API-dispatch claims, narrows the contract to source-backed evidence plus additive work if needed, keeps SQLite as the required local proof point, and creates no child tickets, relations, attachments, or planning documents.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is restated to rely only on current-branch source-backed surface and repository docs: DataVaultPitAsOfReadRequest, DataVaultBridgeReadRequest, IDataVaultReadService.ReadPitRowsAsync(...), and the documented provider-neutral PIT/bridge baseline. Unsupported claims about already-existing PIT/bridge helper APIs or provider dispatch were removed or converted into explicit additive implementation work if needed.
- critic-item-2: `answered` - The revised contract no longer infers a pre-existing public PIT/bridge helper surface beyond what is visibly source-backed in this refinement pass. It does not depend on pre-existing ReadPitAsync(...), ReadBridgeRowsAsync(...), or ReadBridgeAsync(...); if implementation needs helper or entry-point APIs, that work is now explicit, additive, and must preserve request-shape compatibility.
- critic-item-3: `answered` - The contract no longer states that internal or provider-package PIT dispatch already exists. It now scopes PIT provider-aware dispatch or equivalent plumbing as work this ticket may introduce while preserving current provider-neutral semantics and fallback behavior.
- critic-item-4: `answered` - The contract no longer states that internal or provider-package bridge dispatch already exists. It now scopes bridge provider-aware dispatch or equivalent plumbing as work this ticket may introduce while preserving current bridge request semantics and provider-neutral fallback behavior.

Clarifications
- Visible source evidence in this refinement pass confirms DataVaultPitAsOfReadRequest, DataVaultBridgeReadRequest, and IDataVaultReadService.ReadPitRowsAsync(...); the contract no longer treats ReadPitAsync(...), ReadBridgeRowsAsync(...), or ReadBridgeAsync(...) as pre-verified existing public APIs.
- Repository docs currently describe PIT-backed reads and bridge reads as provider-neutral baselines and explicitly say provider-specific PIT/bridge optimization is not yet delivered; this story is the bounded ticket that may introduce the first optimized path.
- SQLite remains the required local optimized proof point because repository guidance keeps SQLite as the required local validation baseline.
- No child tickets, relation edits, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Provider-aware internal dispatch or equivalent plumbing for PIT-backed reads using the existing visible request semantics and preserving provider-neutral fallback.
- Provider-aware internal dispatch or equivalent plumbing for bridge reads using the existing visible request semantics and preserving provider-neutral fallback.
- Additive helper or entry-point surface only if implementation needs it to expose PIT or bridge reads without inventing new request shapes or breaking existing caller contracts.
- Tests, benchmarks, diagnostics, API snapshot updates, and documentation needed to prove optimized-path selection, compatibility, and fallback behavior for the SQLite proof provider.

Scope Out
- Breaking changes to existing PIT or bridge request semantics, new alternate request-shape families, or changes that make provider-specific optimization mandatory for callers.
- Implicit PIT refresh, automatic bridge maintenance, scheduler or background behavior, or read-time maintenance side effects.
- Broader graph traversal redesign, effectivity or path-payload features, delete-aware hierarchy policy changes, link-based PITs, or multi-active PIT semantics.
- Bundling non-SQLite provider implementations into this story unless they fit after the SQLite proof path without expanding scope.

Open questions
- none

Follow-up questions
- After the SQLite optimized baseline is proven, which external provider package should be prioritized next: PostgreSQL, SQL Server, MySQL, or Oracle?
- Should external-provider PIT and bridge benchmark artifacts remain opt-in after SQLite lands, or become part of the default archived evidence set?
- If implementation naturally separates common dispatch plumbing from PIT and bridge execution work, should those slices become distinct follow-up tickets before adding more providers?

Risks
- Current visible source evidence does not confirm every helper API named in the prior contract, so additive public surface or snapshot churn may be needed while keeping request contracts compatible.
- PIT and bridge reads depend on correctness-sensitive ordering and snapshot semantics; provider-specific SQL that changes tie-breaking, filtering, or depth handling would create subtle regressions.
- Repository benchmark and documentation baselines currently distinguish only provider-neutral PIT and bridge reads; over-claiming optimization before artifact-backed evidence lands would regress release-note accuracy.
- Non-SQLite provider proof remains optional and consumer-managed, so expansion beyond SQLite may lag.

Split recommendations
- If implementation grows, split first into one common dispatch and diagnostics slice and two execution slices: PIT optimization and bridge optimization.
- If non-SQLite provider-specific PIT or bridge SQL is needed in the same release, track each external provider package in its own child ticket so SQLite proof and fallback safety are not blocked.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment