[gicket-bot] PO refinement contract

Summary
- Refined the PIT helper implementation story against the shipped satellite-only generator baseline and the done additive PIT-helper contract; no bounded planning writes were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence keeps the current implemented generator baseline support-bundle-driven and satellite-only (`docs/releases/v0.24.0.md`, analyzer README); this ticket is additive PIT-helper implementation, not a restatement of shipped behavior.
- Done ticket `06F7Y0GT7A5QT77TADMRZBVYN8` is the authoritative contract for PIT helper naming, support-bundle input, fingerprint gating, and runtime-boundary rules; bridge generation stays separate.
- The runtime PIT boundary is already bounded in `docs/architecture/dvault-v1-pit-bridge-boundary.md`: hub-parent ordinary PITs, hub-parent multi-active PITs with one canonical driving-key family, and bounded link-parent PITs with unique non-multi-active satellites on one declared link parent.
- Current analyzer tests already exercise PIT diagnostic-only behavior (`DMV1963`, `DMV1967`, `DMV1968`, `DMV1969`); this story should replace the supported PIT skip path with real helper emission while preserving deterministic unsupported-shape diagnostics.
- No child-ticket creation, relation mutation, attachment, planning-document write, or manual ticket-description update was applied in this refinement pass.

Scope In
- Generate PIT read-model records and `Read{ProducedName}AsOfAsync(...)` extensions from one authoritative `dvault.support-bundle.v1` input carrying reviewed `readShape.pit` explain facts.
- Support only repository-proven PIT runtime shapes: hub-parent ordinary PITs, hub-parent PITs with one canonical multi-active driving-key family, and bounded link-parent PITs with unique non-multi-active satellites on one declared link parent.
- Emit deterministic PIT projection members and compatibility constants for `ParentHashKey`, `LoadTimestamp`, optional canonical driving keys, nullable snapshot-reference timestamps, produced column names, mapped names, metadata source kind, and metadata fingerprint.
- Construct `DataVaultPitAsOfReadRequest` values and delegate to the existing `IDataVaultReadService` PIT read path without widening runtime semantics.
- Add generator snapshot, public API/approval, and runtime parity coverage for supported helper emission and unsupported PIT diagnostics.

Scope Out
- Bridge helper generation; that remains the sibling story `06F7Y0HJ1ZPY7ND9N8RVS92H4C`.
- Raw `dvault.model.v1` parsing, source-visible Code-First inspection, literal metadata-first inference, or fallback to unreviewed metadata sources.
- PIT maintenance/rebuild, read-time refresh, provider-specific SQL, payload joins, new runtime read primitives, or dynamic query compilation.
- Broader documentation or release-note sweep beyond minimal code-adjacent updates; the downstream docs task `06F7Y0HZKHBHMYX9EYDYFRYXZ0` remains the main documentation vehicle.

Open questions
- none

Follow-up questions
- After implementation lands, should the historical `06F7Y0GT7A5QT77TADMRZBVYN8 -> 06F7Y0H83H29E1D9K5RK3K7Y9W` `blocks` relation be cleaned up by a relation-audit pass, or is that dependency history intentionally preserved?
- When the downstream docs task runs, should PIT helper examples focus only on supported shared-driving-key multi-active cases, or also include explicit rejected-shape examples?

Risks
- If support-bundle export does not actually carry the required request-bound `readShape.pit` facts for parent identity, segment snapshot references, deterministic ordering, and column bindings, supported runtime PIT shapes will still collapse to diagnostics instead of helper emission.
- Shared-driving-key multi-active PIT support is only safe when the support bundle proves one canonical driving-key name or order family; mismatches must keep diagnostic-only behavior.
- Link-parent PIT helper emission must stay constrained to unique non-multi-active satellites on one declared link parent so the generator does not imply model-first link-parent PIT artifact support or broader runtime semantics.
- Live relation state still includes a historical `blocks` edge from done contract ticket `06F7Y0GT7A5QT77TADMRZBVYN8`; ticket metadata is currently `isBlocked: false`, but dependency-graph cleanup may still be needed later.

Split recommendations
- No further child split is justified from current evidence: PIT implementation is already separated from the bridge-helper story `06F7Y0HJ1ZPY7ND9N8RVS92H4C` and the downstream documentation task `06F7Y0HZKHBHMYX9EYDYFRYXZ0`.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment