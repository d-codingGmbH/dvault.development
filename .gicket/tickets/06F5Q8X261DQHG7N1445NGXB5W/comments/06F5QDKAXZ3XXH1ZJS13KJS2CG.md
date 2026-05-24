[gicket-bot] PO refinement contract

Summary
- Refined the contract ticket against the existing explicit save service, provider-strategy boundary, epic scope, and already-split implementation/diagnostic child stories; no additional materialized planning writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already fixes the v1 write boundary at `IDataVaultSaveService` with existing explicit request shapes `DataVaultSaveRequest` and ordered `DataVaultBulkSaveRequest`; this ticket should define the streaming/chunked contract as an additive boundary that stays compatible with those request types rather than replacing them.
- Existing architecture notes already fix key non-goals: no `SaveChanges` interception as the default write path, no background ingestion, and no scheduler or queue integration.
- Current child-story context already separates concerns: ticket `06F5Q8X8Q72TQ5B7F2JSAJWPR8` owns provider-neutral chunked execution, and ticket `06F5Q8XF9DPKFW9VY0F3Y32BH4` owns bounded hash-state and diagnostics. This contract ticket should stay focused on the public API/behavior definition and compatibility rules.
- Repository tests already establish baseline semantics that the streaming contract must preserve for v1: deterministic caller order for bulk requests, explicit cancellation propagation, participation in the caller's current transaction, record-source/load-timestamp resolution hooks, hub/link idempotent reuse, and satellite hash-diff replay behavior per parent.

Scope In
- Define the public streaming or chunked explicit-save contract as an explicit `IDataVaultSaveService` boundary that remains compatible with current single-request and ordered bulk-save usage.
- Define caller-visible rules for chunk input shape, per-chunk ordering, cancellation, transaction ownership, and load-timestamp/record-source behavior.
- Define v1 compatibility expectations between streaming/chunked saves and existing hub/link/satellite save semantics, including deterministic saved-record ordering and idempotent reuse behavior.
- Define the contract-level rules for carrying enough hash-key/hash-diff continuity across chunk boundaries without requiring full logical-load materialization, while leaving concrete bounded-state implementation and diagnostics to the dedicated child ticket.

Scope Out
- Implementing the provider-neutral chunked execution path.
- Implementing bounded state retention, memory diagnostics, or diagnostic event shapes beyond the contract-level requirement that such behavior remain bounded and deterministic.
- Background ingestion, schedulers, queues, file/CDC pipelines, or any automatic runtime orchestration.
- Changing the default write boundary to `SaveChanges` interception or making DVault persistence implicit.

Open questions
- none

Follow-up questions
- Should the eventual public streaming surface be exposed as a new request type, a new overload, or an adapter over existing ordered bulk-save requests, as long as it preserves the explicit `IDataVaultSaveService` boundary?
- If some satellite or multi-active shapes cannot guarantee bounded retained state in v1, should the implementation prefer deterministic rejection or a documented provider-neutral fallback path?
- After the provider-neutral contract lands, do any provider packages need additional optimized strategy-specific chunk execution tickets beyond the already-split provider-neutral implementation story?

Risks
- If the implementation ticket introduces a streaming surface that weakens the current deterministic ordering or current-transaction semantics, it will conflict with existing explicit-save and provider-strategy evidence already present in the repository.
- If the contract does not explicitly bound unsupported memory-sensitive shapes, later implementation may accidentally promise full logical-load streaming for cases that still require retained per-parent state across chunks.

Split recommendations
- No further split is recommended from this PO pass; the epic already has separate child stories for provider-neutral chunked execution (`06F5Q8X8Q72TQ5B7F2JSAJWPR8`) and bounded hash-state/diagnostics (`06F5Q8XF9DPKFW9VY0F3Y32BH4`).

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment