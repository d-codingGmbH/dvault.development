[gicket-bot] PO refinement contract

Summary
- Refined this to one bounded additive story: extend the existing PIT projection, maintenance, and PIT-backed read baseline from hub-parent-only to link-parent, non-multi-active, same-link satellites; provider-neutral correctness stays in scope and provider-specific optimization stays out. No child-ticket, relation, description, attachment, or planning-document writes were applied in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already supports link-parent satellite modeling and `DataVaultMetadataRegistry` accepts `DataVaultPitMetadata` whose parent is a `Link`, so this story extends the existing PIT baseline rather than inventing a new PIT metadata type or declaration path.
- Preserve the existing PIT row/read contract names: `ParentHashKey`, `LoadTimestamp`, and ordered `<Satellite>LoadTimestamp` snapshot columns remain authoritative; for link-parent PITs `ParentHashKey` carries the link hash key.
- Link-parent PIT support is bounded to one declared link parent plus ordered unique non-multi-active satellites attached to that same link; hub-attached, mixed-parent, bridge-driven, and multi-active PIT shapes remain unsupported here.
- PIT reads stay on the existing explicit-metadata `DataVaultPitAsOfReadRequest` / `ReadPitRowsAsync(...)` / `ReadPitAsync(...)` boundary; this story does not add registry-backed PIT read requests.
- The live incoming `blocks` relation from done ticket `06F5Q90KC6JGQPSP285XQYSPK8` is historical sequencing context rather than an active scope blocker; no relation cleanup was applied in this pass.

Scope In
- Extend PIT EF metadata translation to project one link-parent `DataVaultPitMetadata` with deterministic PIT table metadata and snapshot columns for attached link-parent satellites.
- Extend explicit and registry-backed PIT maintenance so rebuild and targeted parent maintenance accept the supported link-parent PIT shape and recompute history for explicit link hash keys.
- Extend provider-neutral PIT-backed reads and required diagnostics so explicit `DataVaultPitAsOfReadRequest` callers can read maintained link-parent PIT rows without changing hub-parent PIT behavior or projection semantics.
- Add unit, SQLite integration, public contract snapshot, and documentation coverage for the supported link-parent PIT baseline.

Scope Out
- Multi-active PIT semantics, driving-key PIT row generation, or link-parent PITs that reference multi-active satellites.
- PITs that mix hub-parent and link-parent satellites, traverse bridges, or introduce a new PIT metadata/declaration surface.
- Registry-backed PIT read request surfaces, automatic PIT refresh, background scheduling, `SaveChanges` hooks, or PIT/bridge orchestration.
- Provider-specific link-parent PIT read optimization, physical tuning promises, or broader benchmark/evidence work already deferred to downstream diagnostics tickets.

Open questions
- none

Follow-up questions
- Should a later ticket extend SQLite/provider-specific PIT read strategies to accept the new link-parent baseline instead of declining to provider-neutral fallback?
- After link-parent PIT support lands, should PIT reads remain explicit-metadata-only, or is a separate registry-backed PIT as-of read request worth planning?

Risks
- README, production-adoption guidance, deferred-capabilities planning text, and existing release notes currently describe link-parent PITs as unsupported; partial doc updates would create public contract drift.
- The current codebase has separate hub-only guards in PIT translation, maintenance validation, read validation, and strategy diagnostics, so updating only one path would leave inconsistent behavior or regress hub-parent compatibility.
- Downstream diagnostics/benchmark work already depends on this story, so incomplete link-parent validation or missing regression coverage would delay later PIT evidence tickets.

Split recommendations
- No additional split is recommended. Multi-active PIT work and broader diagnostics/benchmark evidence are already separated, and the remaining link-parent PIT work is one coherent baseline across projection, maintenance, provider-neutral reads, tests, and required docs.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment