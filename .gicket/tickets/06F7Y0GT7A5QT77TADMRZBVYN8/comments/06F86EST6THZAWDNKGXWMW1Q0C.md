[gicket-bot] PO refinement contract

Summary
- Refined the story to a bounded additive contract for support-bundle explain-driven typed PIT and bridge helpers over the existing `IDataVaultReadService` boundary; no bounded planning writes were materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence keeps v0.22.0 as satellite-only typed helper shipment; this story is additive and must not restate PIT or bridge helpers as already shipped.
- Typed PIT and bridge helpers consume exactly one authoritative `dvault.support-bundle.v1` input carrying reviewed PIT/bridge explain metadata plus the existing optional `DVaultTypedReadModelMetadataSourceFingerprint`; no raw `dvault.model.v1`, source-visible Code-First callbacks, or literal metadata-first declarations are generator inputs.
- Helpers stay ergonomic extensions over existing provider-neutral PIT and bridge reads and do not generate provider-specific SQL, perform PIT/bridge maintenance, schedule refresh, or widen runtime read semantics.
- No ticket description update, relation change, child-ticket creation, attachment, or planning-document write was applied during this refinement pass.

Scope In
- Define generated PIT and bridge helper naming from the existing typed satellite pattern: `{ProducedName}ReadModel` plus `Read{ProducedName}...Async` extension methods over `IDataVaultReadService`.
- Support PIT helpers only for repository-proven runtime PIT shapes: hub-parent ordinary PITs, hub-parent PITs whose referenced multi-active satellites share one canonical driving-key family, and bounded link-parent PITs with unique non-multi-active satellites on the same declared link parent.
- Support bridge helpers only for repository-proven runtime bridge shapes: many-to-many endpoint traversal and hierarchy traversal with required bounded `maximumDepth`.
- Define generated projection members, nullability, and public constants for produced or mapped column names, metadata source kind, and metadata fingerprint so PIT and bridge helpers follow the existing typed satellite compatibility pattern.
- Define explicit unsupported-shape diagnostics and helper-skip behavior for PIT and bridge shapes that remain outside the bounded contract.

Scope Out
- Any raw model parsing, source inspection, or fallback inference outside reviewed support-bundle metadata.
- Provider-specific SQL generation, provider physical-plan promises, custom LINQ or query providers, or dynamic request compilation.
- Automatic PIT or bridge maintenance, read-time refresh, scheduling, or `SaveChanges` orchestration.
- Graph or path APIs beyond the existing many-to-many and hierarchy bridge traversal boundary.
- Unbounded PIT tuple filters, cross-product multi-active semantics, delete-aware bridge maintenance behavior, or new runtime read primitives.

Open questions
- none

Follow-up questions
- When implementation is scheduled, should PIT and bridge helper emission ship in one release or stage PIT first and bridge second?
- After implementation lands, should public examples live only in analyzer README and tests or also in release notes and adoption guidance?

Risks
- Live relation state still shows incoming blocker tickets `06F7Y0F650KM61BQXMEQPZ86DR` and `06F7Y0FZXX5J0G7G15681HVEBR`; implementation should verify whether those blockers remain semantically active or need relation cleanup before development starts.
- PIT helper support is only safe when the support-bundle export path can carry the bounded PIT read-shape facts needed for parent identity, canonical driving-key families, and segment snapshot references without reintroducing raw-model parsing.
- Bridge helper ergonomics must not drift into broader graph traversal or provider-specific behavior; the contract needs to stay constrained to the existing many-to-many and hierarchy read-service semantics.

Split recommendations
- No bounded child-ticket split was materialized in this pass; keep the contract-definition story unified and split later engineering delivery into PIT and bridge implementation tickets only if development capacity or test volume warrants it.

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